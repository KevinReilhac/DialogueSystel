/**
################################################################################
#          File: SO_DialogueCustomInspector.cs                                 #
#          File Created: Tuesday, 8th March 2022 3:54:57 pm                    #
#          Author: Kévin Reilhac (kevin.reilhac.pro@gmail.com)                 #
################################################################################
**/

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Hilo.Utils;

namespace Hilo.DialogueSystem
{
	/// <summary>
	/// Dialogue scriptable object custom inspector
	/// </summary>
	[CustomEditor(typeof(SO_Dialogue))]
	public class SO_DialogueCustomInspector : Editor
	{
		private Dictionary<int, ReorderableList> answersLists = new Dictionary<int, ReorderableList>();
		SO_Dialogue dialogue;

		private void OnEnable()
		{
			dialogue = target as SO_Dialogue;
		}

		public override void OnInspectorGUI()
		{
			EditorUtility.SetDirty(dialogue);

			for (int i = 0; i < dialogue.pages.Count; i++)
			{
				EditorGUILayout.LabelField(string.Format("Page {0}/{1}", i + 1, dialogue.pages.Count));
				PageDrawer(dialogue.pages[i]);
				EditorExtentions.DrawUILine(Color.black);
			}

			if (GUILayout.Button("Add Page"))
				dialogue.pages.Add(new Page());
		}

		private void PageDrawer(Page page)
		{
			page.text = EditorGUILayout.TextArea(page.text, GUILayout.MinHeight(100));
			int pageIndex = dialogue.pages.IndexOf(page);

			DrawAnswers(page);
			EditorExtentions.Header("Audio");
			page.clip = EditorGUILayout.ObjectField("AudioClip", page.clip, typeof(AudioClip), false) as AudioClip;
			EditorGUILayout.Space();
			DrawMovePageButtons(page);
			if (GUILayout.Button("Delete") && DeleteConfirmation(page.text))
				dialogue.pages.Remove(page);
		}

		private void DrawMovePageButtons(Page page)
		{
			int pageIndex = dialogue.pages.IndexOf(page);
			EditorGUILayout.BeginHorizontal();
			GUI.enabled = pageIndex < dialogue.pages.Count - 1;
			if (GUILayout.Button("Move down"))
				PageMove(page, + 1);
			GUI.enabled = pageIndex > 0;
			if (GUILayout.Button("Move up"))
				PageMove(page, - 1);
			EditorGUILayout.EndHorizontal();
			GUI.enabled = true;
		}

		private void PageMove(Page page, int delta)
		{
			int pageIndex = dialogue.pages.IndexOf(page);

			if (pageIndex + delta < 0 || pageIndex + delta >= dialogue.pages.Count)
				return;

			Page tmpPage = dialogue.pages[pageIndex + delta];
			dialogue.pages[pageIndex + delta] = page;
			dialogue.pages[pageIndex] = tmpPage;
		}

		private void DrawAnswers(Page page)
		{
			int pageIndex = dialogue.pages.IndexOf(page);

			if (!answersLists.ContainsKey(pageIndex))
				answersLists.Add(pageIndex, CreateNewReorderableList(page.answers, pageIndex));

			serializedObject.Update();
			answersLists[pageIndex].DoLayoutList();
			serializedObject.ApplyModifiedProperties();
		}

		private ReorderableList CreateNewReorderableList(List<Answer> answers, int pageIndex)
		{
			ReorderableList list = new ReorderableList(serializedObject, GetAnswerListSerializeProperty(pageIndex), true, true, true, true);

			list.drawElementCallback = (r, i, a, f) => DrawAnswerListItem(r, i, a, f, pageIndex);
			list.drawHeaderCallback = (r) => EditorGUI.LabelField(r, "Answers");

			return (list);
		}

		private SerializedProperty GetAnswerListSerializeProperty(int pageIndex)
		{
			return serializedObject.FindProperty("pages")
				.GetArrayElementAtIndex(pageIndex)
				.FindPropertyRelative("answers");
		}

		private void DrawAnswerListItem(Rect rect, int index, bool isActive, bool isFocused, int pageIndex)
		{
			ReorderableList list = answersLists[pageIndex];
			SerializedProperty answer = list.serializedProperty.GetArrayElementAtIndex(index);
			SerializedProperty text = answer.FindPropertyRelative("text");
			SerializedProperty action = answer.FindPropertyRelative("action");
			SerializedProperty setPageValue = answer.FindPropertyRelative("setPageValue");

			EditorGUI.PropertyField(
				new Rect(rect.x, rect.y, 250, EditorGUIUtility.singleLineHeight),
				text,
				GUIContent.none
			);

			EditorGUI.PropertyField(
				new Rect(rect.x + 255, rect.y, 100, EditorGUIUtility.singleLineHeight),
				action,
				GUIContent.none
			);

			if (action.intValue == (int)Answer.AnswerAction.SetPage)
			{
				EditorGUI.PropertyField(
					new Rect(rect.x + 360, rect.y, 30, EditorGUIUtility.singleLineHeight),
					setPageValue,
					GUIContent.none
				);

				setPageValue.intValue = Mathf.Clamp(setPageValue.intValue, 0, dialogue.pages.Count);
			}
		}

		private bool DeleteConfirmation(string pageText)
		{
			return EditorUtility.DisplayDialog(
				"Are you sure ?",
				string.Format("Do you want to delete this page ? \n\n \"{0}\"", pageText),
				"Yes",
				"No"
			);
		}
	}
}
