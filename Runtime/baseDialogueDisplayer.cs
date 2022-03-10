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
		public enum OnEndDialogueType
		{
			Nothing,
			Disable,
			Destroy
		}

		[Header("Settings")]
		[SerializeField] private SO_Dialogue dialogue = null;
		[SerializeField] private string defaultNextText = "Suivant";
		[SerializeField] private string defaultPreviousText = "Précédent";
		[SerializeField] private OnEndDialogueType endDialogueType = OnEndDialogueType.Disable;
		[Header("Events")]
		[SerializeField] private UnityEvent onEndDialogue = null;
		[SerializeField, HideInInspector] private GenericDictionary<int, UnityEvent> pageEvents = null;
		[Header("References")]
		[SerializeField] protected T text = null;
		[SerializeField] private baseDialogueButton<T> previousButton = null;
		[SerializeField] private baseDialogueButton<T> nextButton = null;
		[SerializeField] private AudioSource audioSource = null;

		private int currentPage = 0;

		/// <summary>
		/// Setting callbacks and create AudioSource if needed
		/// </summary>
		private void Awake()
		{
			if (audioSource == null && IsPagesHaveAudio())
				audioSource = gameObject.AddComponent<AudioSource>();

			previousButton.SetCallback(() => NextPage(-1));
			nextButton.SetCallback(() => NextPage(1));
			ShowPage(0);
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
		/// Draw a page from index
		/// </summary>
		/// <param name="pageIndex"> page index to display </param>
		public void ShowPage(int pageIndex) => ShowPage(dialogue.pages[pageIndex]);


		/// <summary>
		/// Draw a page from a page reference
		/// </summary>
		/// <param name="page"> page to draw </param>
		public void ShowPage(Page page)
		{
			currentPage = dialogue.pages.IndexOf(page);

			SetText(page.text);
			nextButton.SetText(page.hasCustomNextButtonText ? page.customNextButtonText : defaultNextText);
			previousButton.SetText(page.hasCustomPreviousButtonText ? page.customPreviousButtonText : defaultPreviousText);
			previousButton.gameObject.SetActive(currentPage > 0);
			if (pageEvents.ContainsKey(currentPage))
				pageEvents[currentPage].Invoke();
			if (page.clip != null && audioSource != null)
			{
				audioSource.Stop();
				audioSource.PlayOneShot(page.clip);
			}
		}

#region AbstractMethod
		abstract protected void SetText(string text);
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

		public string DefaultPreviousButtonText
		{
			get => defaultPreviousText;
		}

		public string DefaultNextButtonText
		{
			get => defaultNextText;
		}

		public GenericDictionary<int, UnityEvent> CustomEvents
		{
			get => pageEvents;
		}
#endregion

	}
}