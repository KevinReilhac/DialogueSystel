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
				PageDrawer(dialogueObject.pages[i], dialogueObject);
				EditorExtentions.DrawUILine(Color.black);
			}

			if (GUILayout.Button("Add Page"))
				dialogueObject.pages.Add(new Page());
		}

		private void PageDrawer(Page page, SO_Dialogue dialogue)
		{
			page.text = EditorGUILayout.TextArea(page.text, GUILayout.MinHeight(100));
			int pageIndex = dialogue.pages.IndexOf(page);

			DrawCustomButtonToggles(page);
			DrawCustomButtonTexts(page);

			page.clip = EditorGUILayout.ObjectField("AudioClip", page.clip, typeof(AudioClip), false) as AudioClip;
			if (GUILayout.Button("Remove") && DeleteConfirmation(page.text))
				dialogue.pages.Remove(page);
		}

		private void DrawCustomButtonToggles(Page page)
		{
			GUILayout.BeginHorizontal();
			page.hasCustomPreviousButtonText = EditorGUILayout.Toggle("Custom previous button", page.hasCustomPreviousButtonText);
			page.hasCustomNextButtonText = EditorGUILayout.Toggle("Custom next button", page.hasCustomNextButtonText);
			GUILayout.EndHorizontal();
		}

		private void DrawCustomButtonTexts(Page page)
		{
			GUILayout.BeginHorizontal();
			if (page.hasCustomPreviousButtonText)
				page.customPreviousButtonText = EditorGUILayout.TextField(page.customPreviousButtonText);
			if (page.hasCustomNextButtonText)
				page.customNextButtonText = EditorGUILayout.TextField(page.customNextButtonText);
			GUILayout.EndHorizontal();
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
