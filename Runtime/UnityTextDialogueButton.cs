/**
################################################################################
#          File: UnityTextDialogueButton.cs                                    #
#          File Created: Tuesday, 8th March 2022 5:16:20 pm                    #
#          Author: Kévin Reilhac (kevin.reilhac.pro@gmail.com)                 #
################################################################################
**/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hilo.DialogueSystem
{
	[AddComponentMenu("Hilo/Dialogue/UnityText/UnityTextDialogueButton")]
	public class UnityTextDialogueButton : baseDialogueButton<Text>
	{
		public override void SetText(string textValue)
		{
			text.text = textValue;
		}
	}
}
