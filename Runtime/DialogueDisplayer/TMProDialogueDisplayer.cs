/**
################################################################################
#          File: TMProDialogueDisplayer.cs                                     #
#          File Created: Tuesday, 8th March 2022 5:30:59 pm                    #
#          Author: Kévin Reilhac (kevin.reilhac.pro@gmail.com)                 #
################################################################################
**/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Hilo.DialogueSystem
{
	[AddComponentMenu("Hilo/Dialogue/TMPro/TMProDialogueDisplayer")]
	public class TMProDialogueDisplayer : baseDialogueDisplayer<TextMeshProUGUI>
	{
		protected override void SetText(string value)
		{
			text.gameObject.SetActive(false);
			text.SetText(value);
			text.gameObject.SetActive(true);
		}

		protected override void SetPageNumberText(string value)
		{
			pageNumberText.gameObject.SetActive(false);
			pageNumberText.SetText(value);
			pageNumberText.gameObject.SetActive(true);
		}
	}
}
