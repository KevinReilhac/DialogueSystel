/**
################################################################################
#          File: baseDialogueDisplayer.cs                                      #
#          File Created: Tuesday, 8th March 2022 5:00:37 pm                    #
#          Author: Kévin Reilhac (kevin.reilhac.pro@gmail.com)                 #
################################################################################
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Hilo.Utils;

namespace Hilo.DialogueSystem
{
	/// <summary>
	/// Base class for DialogueDisplayer
	/// </summary>
	/// <typeparam name="T"> UI Text type </typeparam>
	public abstract class baseDialogueDisplayer<T> : MonoBehaviour where T : MonoBehaviour
	{
		private const string CURRENT_PAGE_REPLACE_TEXT = "{CURRENT}";
		private const string MAX_PAGE_REPLACE_TEXT = "{MAX}";
		public enum OnEndDialogueType
		{
			Nothing,
			Disable,
			Destroy
		}

		[Header("Settings")]
		[SerializeField] private SO_Dialogue dialogue = null;
		[SerializeField] private OnEndDialogueType endDialogueType = OnEndDialogueType.Disable;
		[SerializeField] private string pageNumberTextFormat = CURRENT_PAGE_REPLACE_TEXT + '/' + MAX_PAGE_REPLACE_TEXT;
		[SerializeField] private int startPage = 0;
		[SerializeField] private GenericDictionary<string, string> replacers = new GenericDictionary<string, string>();
		[Header("Prefabs")]
		[SerializeField] private baseDialogueButton<T> buttonPrefab = null;
		[Header("Events")]
		[SerializeField] private UnityEvent onEndDialogue = null;
		[SerializeField, HideInInspector] private GenericDictionary<int, UnityEvent> pageEvents = null;
		[SerializeField, HideInInspector] private GenericDictionary<int, List<UnityEvent>> answerEvents = null;
		[Header("References")]
		[SerializeField] protected T text = null;
		[SerializeField] protected T pageNumberText = null;
		[SerializeField] private AudioSource audioSource = null;
		[SerializeField] private Transform buttonsParent = null;

		private int currentPage = 0;

#region Init
		/// <summary>
		/// Setting callbacks and create AudioSource if needed
		/// </summary>
		private void Awake()
		{
			if (audioSource == null && IsPagesHaveAudio())
				audioSource = gameObject.AddComponent<AudioSource>();

		}

		private void Start()
		{
			ShowPage(startPage);
		}


		/// <summary>
		/// Check if one of the pages have an AudioClip
		/// </summary>
		/// <returns></returns>
		private bool IsPagesHaveAudio()
		{
			foreach (Page page in dialogue.pages)
			{
				if (page.clip != null)
					return (true);
			}

			return (false);
		}
#endregion

#region PageDraw
		/// <summary>
		/// Jumping into an other page
		/// </summary>
		/// <param name="delta"> Page index difference with current page index </param>
		public void NextPage(int delta = 1)
		{
			if (currentPage + delta >= dialogue.pages.Count)
				EndDialogue();
			else
				ShowPage(Mathf.Clamp(currentPage + delta, 0, dialogue.pages.Count - 1));
		}

		/// <summary>
		/// Draw a page from index
		/// /// </summary>
		/// <param name="pageIndex"> page index to display </param>
		public void ShowPage(int pageIndex) => ShowPage(dialogue.pages[pageIndex]);

		/// <summary>
		/// Draw a page from a page reference
		/// </summary>
		/// <param name="page"> page to draw </param>
		public void ShowPage(Page page)
		{
			if (audioSource)
				audioSource.Stop();
			currentPage = dialogue.pages.IndexOf(page);

			SetText(ApplyReplacers(page.text));

			SetupAnswersButtons(page);
			UpdatePageNumberText();
			if (pageEvents.ContainsKey(currentPage))
				pageEvents[currentPage].Invoke();
			if (page.clip != null && audioSource != null)
			{
				audioSource.Stop();
				audioSource.PlayOneShot(page.clip);
			}
		}

		private void SetupAnswersButtons(Page page)
		{
			buttonsParent.ClearChilds();
			for (int i = 0; i < page.answers.Count; i++)
				CreateButton(page.answers[i], i, dialogue.pages.IndexOf(page));
		}

		private void CreateButton(Answer answer, int answerIndex, int pageIndex)
		{
			baseDialogueButton<T> buttonInstance = Instantiate(buttonPrefab, buttonsParent);
			bool interactable = true;

			if (answer.action == Answer.AnswerAction.Previous && pageIndex == 0)
				interactable = false;

			buttonInstance.Setup(answer);
			buttonInstance.SetInteractable(interactable);
			if (interactable == false)
				return;
			buttonInstance.OnClick.AddListener((a) => {
				AnswerHandler(a);
				if (answerEvents.ContainsKey(currentPage))
				{
					answerEvents[currentPage][answerIndex].Invoke();
				}
			});
		}

		private void UpdatePageNumberText()
		{
			if (pageNumberText == null)
				return;
			SetPageNumberText(GetPageNumberString(currentPage + 1));
		}

		public string GetPageNumberString(int pageNumber)
		{
			string replaces = pageNumberTextFormat.
				Replace(CURRENT_PAGE_REPLACE_TEXT, pageNumber.ToString()).
				Replace(MAX_PAGE_REPLACE_TEXT, dialogue.pages.Count.ToString());

			return (replaces);
		}
#endregion

#region Events
		/// <summary>
		/// Called when you press Next on the last page
		/// </summary>
		private void EndDialogue()
		{
			onEndDialogue.Invoke();
			switch (endDialogueType)
			{
				case (OnEndDialogueType.Destroy) :
					Destroy(gameObject);
					break;
				case (OnEndDialogueType.Disable) :
					gameObject.SetActive(false);
					break;
				case (OnEndDialogueType.Nothing) :
					break;
			}
		}

		/// <summary>
		/// Replace every words in replacers keys
		/// by replacers values
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public string ApplyReplacers(string text)
		{
			foreach (var item in replacers)
			{
				if (item.Key == null || item.Key == "")
					continue;
				text = text.Replace(item.Key, item.Value);
			}

			return (text);
		}


		private void AnswerHandler(Answer answer)
		{
			switch (answer.action)
			{
				case Answer.AnswerAction.Previous:
					NextPage(-1);
					break;
				case Answer.AnswerAction.Next:
					NextPage();
					break;
				case Answer.AnswerAction.SetPage:
					ShowPage(answer.setPageValue - 1);
					break;
				case Answer.AnswerAction.End:
					EndDialogue();
					break;
				default:
				case Answer.AnswerAction.Custom:
					break;
			}
		}
#endregion

#region AbstractMethod
		abstract protected void SetText(string text);
		abstract protected void SetPageNumberText(string text);
#endregion

#region Getters
		public int CurrentPage
		{
			get => currentPage;
		}

		public SO_Dialogue Dialogue
		{
			get => dialogue;
		}

		public GenericDictionary<int, UnityEvent> PageEvents
		{
			get => pageEvents;
		}

		public GenericDictionary<int, List<UnityEvent>> AnswerEvents
		{
			get => answerEvents;
		}
#endregion

#region Publics
		public void AddReplacer(string key, string value)
		{
			replacers.Add(key, value);
		}
#endregion
	}
}