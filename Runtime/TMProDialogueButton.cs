/**
################################################################################
#          File: TMProDialogueButton.cs                                        #
#          File Created: Tuesday, 8th March 2022 5:14:50 pm                    #
#          Author: Kévin Reilhac (kevin.reilhac.pro@gmail.com)                 #
################################################################################
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Hilo.DialogueSystem
{
	[AddComponentMenu("Hilo/Dialogue/TMPro/TMProDialogueButton")]
	public class TMProDialogueButton : baseDialogueButton<TextMeshProUGUI>
	{
		public override void SetText(string textValue)
		{
			text.text = textValue;
		}
	}
}
