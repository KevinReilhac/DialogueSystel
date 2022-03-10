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

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.TextField(page.hasCustomPreviousButtonText ? page.customPreviousButtonText : dialogueDisplayer.DefaultPreviousButtonText);
			EditorGUILayout.TextField(page.hasCustomNextButtonText ? page.customNextButtonText : dialogueDisplayer.DefaultNextButtonText);
			EditorGUILayout.EndHorizontal();
			GUI.enabled = true;

			CustomEventsDrawer(dialogueDisplayer, page);
			if (GUILayout.Button("ShowPage"))
				dialogueDisplayer.ShowPage(currentPageIndex);
		}

		private void CustomEventsDrawer(baseDialogueDisplayer<T> dialogueDisplayer, Page page)
		{
			if (!dialogueDisplayer.CustomEvents.ContainsKey(currentPageIndex))
			{
				dialogueDisplayer.CustomEvents.Add(currentPageIndex, new UnityEvent());
				return;
			}
			int elementIndex = dialogueDisplayer.CustomEvents.IndexOfKey(currentPageIndex);

			SerializedProperty eventProperty = serializedObject.FindProperty("pageEvents")
				.FindPropertyRelative("list")
				.GetArrayElementAtIndex(elementIndex)
				.FindPropertyRelative("Value");

			EditorGUILayout.PropertyField(eventProperty, new GUIContent($"On page {currentPageIndex + 1} show event"));
			serializedObject.ApplyModifiedProperties();
		}
	}
}