/**
################################################################################
#          File: SO_Dialogue.cs                                                #
#          File Created: Tuesday, 8th March 2022 3:43:31 pm                    #
#          Author: Kévin Reilhac (kevin.reilhac.pro@gmail.com)                 #
################################################################################
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hilo.DialogueSystem
{
	[System.Serializable]
	public class Answer
	{
		public enum AnswerAction
		{
			None,
			Next,
			Previous,
			SetPage,
			End,
		}

		public Answer(string text, AnswerAction action)
		{
			this.text = text;
			this.action = action;
		}

		public string text = "";
		public AnswerAction action = AnswerAction.None;
		public int setPageValue = 0;
	}

	[System.Serializable]
	public class Page
	{
		[TextArea]
		public string text = "";
		public List<Answer> answers = new List<Answer>() {
				new Answer("Précedent", Answer.AnswerAction.Previous),
				new Answer("Suivant", Answer.AnswerAction.Next)
		};
		public AudioClip clip = null;
	}

	[CreateAssetMenu(fileName = "Dialogue", menuName = "DialogueSystem/Dialogue")]
	public class SO_Dialogue : ScriptableObject
	{
		[SerializeField] public List<Page> pages = new List<Page>();
	}
}