using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hilo.Utils
{
	public static class TransformExtention
	{
		public static void ClearChilds(this Transform transform)
		{
			if (!Application.isPlaying)
				ClearChildsImmediate(transform);

			int childs = transform.childCount;

			for (int i = childs - 1; i >= 0; i--)
				GameObject.Destroy(transform.GetChild(i).gameObject);
		}

		public static void ClearChildsImmediate(this Transform transform)
		{
			int childs = transform.childCount;

			for (int i = childs - 1; i >= 0; i--)
				GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
		}
	}
}
