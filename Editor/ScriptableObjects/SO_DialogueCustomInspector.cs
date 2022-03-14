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
using Hilo.Utils;

namespace Hilo.DialogueSystem
{
	/// <summary>
	/// Dialogue scriptable object custom inspector
	/// </summary>
	[CustomEditor(typeof(SO_Dialogue))]
	public class SO_DialogueCustomInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			SO_Dialogue dialogueObject = target as SO_Dialogue;
			EditorUtility.SetDirty(dialogueObject);

			for (int i = 0; i < dialogueObject.pages.Count; i++)
			{
				EditorGUILayout.LabelField(string.Format("Page {0}/{1}", i + 1, dialogueObject.pages.Count));
				PageDrawer(dialogueObject, dialogueObject.pages[i]);
				EditorExtentions.DrawUILine(Color.black);
			}

			if (GUILayout.Button("Add Page"))
				dialogueObject.pages.Add(new Page());
		}

		private void PageDrawer(SO_Dialogue dialogue, Page page)
		{
			page.text = EditorGUILayout.TextArea(page.text, GUILayout.MinHeight(100));
			int pageIndex = dialogue.pages.IndexOf(page);

			EditorExtentions.Header("Answers");
			DrawAnswers(dialogue, page);

			page.clip = EditorGUILayout.ObjectField("AudioClip", page.clip, typeof(AudioClip), false) as AudioClip;
			if (GUILayout.Button("Remove") && DeleteConfirmation(page.text))
				dialogue.pages.Remove(page);
		}

		private void DrawAnswers(SO_Dialogue dialogue, Page page)
		{
			List<Answer> answers = new List<Answer>(page.answers);
			foreach (Answer answer in answers)
				DrawAnswer(dialogue, page, answer);
			EditorGUILayout.Space();
			if (GUILayout.Button("Add answer"))
				page.answers.Add(new Answer("custom", Answer.AnswerAction.None));
		}

		private void DrawAnswer(SO_Dialogue dialogue, Page page, Answer answer)
		{
			EditorGUILayout.BeginHorizontal();
			answer.text = GUILayout.TextField(answer.text, GUILayout.Width(300f));
			answer.action = (Answer.AnswerAction)EditorGUILayout.EnumPopup(answer.action);
			if (answer.action == Answer.AnswerAction.SetPage)
				answer.setPageValue = Mathf.Clamp(EditorGUILayout.IntField(answer.setPageValue, GUILayout.Width(50f)), 0, dialogue.pages.Count - 1);
			if (GUILayout.Button("Delete", GUILayout.Width(100f)))
				page.answers.Remove(answer);
			EditorGUILayout.EndHorizontal();
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
