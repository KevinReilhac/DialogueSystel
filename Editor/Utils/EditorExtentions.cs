using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Hilo.Utils
{
	public static class EditorExtentions
	{
		public static void DrawUILine(Color color, int thickness = 2, int padding = 10)
		{
			Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
			r.height = thickness;
			r.y += padding / 2;
			r.x -= 2;
			r.width += 6;
			EditorGUI.DrawRect(r, color);
		}

		public static void Header(string label, bool space = true)
		{
			if (space)
				EditorGUILayout.Space();
			EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
		}
	}
}