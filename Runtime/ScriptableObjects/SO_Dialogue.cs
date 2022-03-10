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
	public class Page
	{
		[TextArea]
		public string text = null;
		public bool hasCustomNextButtonText = false;
		public string customNextButtonText = "next";
		public bool hasCustomPreviousButtonText = false;
		public string customPreviousButtonText = "previous";
		public AudioClip clip = null;
	}

	[CreateAssetMenu(fileName = "Dialogue", menuName = "Data/Dialogue")]
	public class SO_Dialogue : ScriptableObject
	{
		[SerializeField] public List<Page> pages = new List<Page>();
	}
}