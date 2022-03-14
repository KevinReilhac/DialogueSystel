/**
################################################################################
#          File: SO_DialogueDisplayerCustomInspector.cs                        #
#          File Created: Wednesday, 9th March 2022 9:15:53 am                  #
#          Author: Kévin Reilhac (kevin.reilhac.pro@gmail.com)                 #
################################################################################
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using Hilo.Utils;

namespace Hilo.DialogueSystem
{
	/// <summary>
	/// baseDialogueDisplayer custom inspector
	/// </summary>
	/// <typeparam name="T"> UI text type for including UnityText and TextMeshPro </typeparam>
	public class baseDialogueDisplayerCustomInspector<T> : Editor where T : MonoBehaviour
	{
		private int currentPageIndex = 0;

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			baseDialogueDisplayer<T> dialogueDisplayer = target as baseDialogueDisplayer<T>;

			if (dialogueDisplayer.Dialogue != null && dialogueDisplayer.Dialogue.pages.Count > 0)
			{
				EditorExtentions.DrawUILine(Color.black);
				EditorExtentions.Header("Pages");
				PageSectionDrawer(dialogueDisplayer);
			}
		}

		private void PageSectionDrawer(baseDialogueDisplayer<T> dialogueDisplayer)
		{
			PageIndexSelectionDrawer(dialogueDisplayer);
			PageDrawer(dialogueDisplayer, dialogueDisplayer.Dialogue.pages[currentPageIndex]);


			EditorGUILayout.Space();
			if (GUILayout.Button("Open dialogue"))
				Selection.activeObject = dialogueDisplayer.Dialogue;
		}

		private void PageIndexSelectionDrawer(baseDialogueDisplayer<T> dialogueDisplayer)
		{
			currentPageIndex = EditorGUILayout.IntField("Page", currentPageIndex + 1) - 1;
			EditorGUILayout.BeginHorizontal();
			GUI.enabled = currentPageIndex > 0;
			if (GUILayout.Button("Previous"))
				currentPageIndex--;
			GUI.enabled = true;

			GUI.enabled = currentPageIndex < dialogueDisplayer.Dialogue.pages.Count - 1;
			if (GUILayout.Button("Next"))
				currentPageIndex++;
			EditorGUILayout.EndHorizontal();
			GUI.enabled = true;

			currentPageIndex = Mathf.Clamp(currentPageIndex, 0, dialogueDisplayer.Dialogue.pages.Count - 1);
		}

		private void PageDrawer(baseDialogueDisplayer<T> dialogueDisplayer, Page page)
		{

			GUI.enabled = false;
			EditorGUILayout.TextArea(page.text, GUILayout.MinHeight(100));
			DrawAnswers(page.answers);
			GUI.enabled = true;

			CustomEventsDrawer(dialogueDisplayer, page);
		}

		private void DrawAnswers(List<Answer> answers)
		{
			EditorGUILayout.BeginHorizontal();
			foreach (Answer answer in answers)
				EditorGUILayout.TextField(answer.text);
			EditorGUILayout.EndHorizontal();
		}

		private void CustomEventsDrawer(baseDialogueDisplayer<T> dialogueDisplayer, Page page)
		{
			DrawPageEvent(dialogueDisplayer, page);
			DrawAnswerEvents(dialogueDisplayer, page);
		}

		private bool showAnswerEvents = false;
		private void DrawAnswerEvents(baseDialogueDisplayer<T> dialogueDisplayer, Page page)
		{
			showAnswerEvents = EditorGUILayout.Foldout(showAnswerEvents, "Answers events");

			if (!showAnswerEvents)
				return;
			for (int i = 0; i < page.answers.Count; i++)
			{
				DrawAnswerEvent(dialogueDisplayer, page, i);
			}
		}

		private void DrawAnswerEvent(baseDialogueDisplayer<T> dialogueDisplayer, Page page, int index)
		{
			if (!dialogueDisplayer.AnswerEvents.ContainsKey(currentPageIndex))
			{
				dialogueDisplayer.AnswerEvents.Add(currentPageIndex, new List<UnityEvent>(page.answers.Count));
				return;
			}

			if (index >= dialogueDisplayer.AnswerEvents[currentPageIndex].Count)
			{
				while (dialogueDisplayer.AnswerEvents[currentPageIndex].Count < page.answers.Count)
					dialogueDisplayer.AnswerEvents[currentPageIndex].Add(new UnityEvent());
				return;
			}

			int elementIndex = dialogueDisplayer.AnswerEvents.IndexOfKey(currentPageIndex);

			SerializedProperty eventListProperty = serializedObject.FindProperty("answerEvents")
				.FindPropertyRelative("list")
				.GetArrayElementAtIndex(elementIndex)
				.FindPropertyRelative("Value");

			if (index >= eventListProperty.arraySize)
				return;

			SerializedProperty eventProperty = eventListProperty.GetArrayElementAtIndex(index); 

			EditorGUILayout.PropertyField(eventProperty, new GUIContent($" On answer \"{ page.answers[index].text }\" custom event"));
			serializedObject.ApplyModifiedProperties();
		}

		private void DrawPageEvent(baseDialogueDisplayer<T> dialogueDisplayer, Page page)
		{
			if (!dialogueDisplayer.PageEvents.ContainsKey(currentPageIndex))
			{
				dialogueDisplayer.PageEvents.Add(currentPageIndex, new UnityEvent());
				return;
			}
			int elementIndex = dialogueDisplayer.PageEvents.IndexOfKey(currentPageIndex);

			SerializedProperty eventProperty = serializedObject.FindProperty("pageEvents")
				.FindPropertyRelative("list")
				.GetArrayElementAtIndex(elementIndex)
				.FindPropertyRelative("Value");

			EditorGUILayout.PropertyField(eventProperty, new GUIContent($"On page {currentPageIndex + 1} show event"));
			serializedObject.ApplyModifiedProperties();
		}
	}
}