using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

namespace Hilo.DialogueSystem
{
	[CustomEditor(typeof(TMProDialogueDisplayer))]
	public class TMProDialogueDisplayerCustomInspector : baseDialogueDisplayerCustomInspector<TextMeshProUGUI>
	{
	}
}
