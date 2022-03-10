using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace Hilo.DialogueSystem
{
	[CustomEditor(typeof(UnityTextDialogueDisplayer))]
	public class UnityTextDialogueDisplayerCustomInspector : baseDialogueDisplayerCustomInspector<Text>
	{
	}
}
