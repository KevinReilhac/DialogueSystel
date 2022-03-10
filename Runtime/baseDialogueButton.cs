/**
################################################################################
#          File: baseDialogueButton.cs                                         #
#          File Created: Tuesday, 8th March 2022 5:08:30 pm                    #
#          Author: Kévin Reilhac (kevin.reilhac.pro@gmail.com)                 #
################################################################################
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Hilo.DialogueSystem
{
	/// <summary>
	/// Base class for dialogue button (previous and next)
	/// </summary>
	/// <typeparam name="T"> UI Text type </typeparam>
	[RequireComponent(typeof(Button))]
	[System.Serializable]
	public abstract class baseDialogueButton<T> : MonoBehaviour where T : MonoBehaviour
	{
		[SerializeField] protected T text = null;
		[SerializeField] private Button button = null;

		public void SetCallback(UnityAction onClickCallback)
		{
			button.onClick.AddListener(onClickCallback);
		}

		abstract public void SetText(string textValue);
	}
}
