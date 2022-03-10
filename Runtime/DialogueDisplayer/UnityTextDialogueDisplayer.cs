/**
################################################################################
#          File: UnityTextDialogueDisplayer.cs                                 #
#          File Created: Tuesday, 8th March 2022 5:18:29 pm                    #
#          Author: Kévin Reilhac (kevin.reilhac.pro@gmail.com)                 #
################################################################################
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hilo.DialogueSystem
{
	[AddComponentMenu("Hilo/Dialogue/UnityText/UnityTextDialogueDisplayer")]
	public class UnityTextDialogueDisplayer : baseDialogueDisplayer<Text>
	{
		protected override void SetText(string value)
		{
			text.text = value;
		}
	}
}
