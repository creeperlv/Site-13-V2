using Site13Kernel.GameLogic.Animations;
using UnityEditor;
using UnityEngine;

namespace Site13Kernel.Editor.UI
{
	public class PreviewTransformSync : EditorWindow
	{
		[MenuItem("Site13/Biped Tools/Transform Sync Preview")]
		public static void ShowWindow()
		{
			GetWindow(typeof(PreviewTransformSync));
		}
		public static void OpenWindow(TransformSync sync)
		{
			var w = GetWindow<PreviewTransformSync>();
			w.target = sync;
		}
		bool IsPreviewing = true;
		public TransformSync target;
		public void OnGUI()
		{
			GUILayout.Label("<color=#2288EE>Biped Tool</color>" , new GUIStyle() { fontSize = 28 , richText = true });
			GUILayout.Label("<color=#2288EE>Transform Sync Preview</color>" , new GUIStyle() { fontSize = 18 , richText = true });

			target = (TransformSync)EditorGUILayout.ObjectField("Transform Sync To Preview" , target , typeof(TransformSync) , true);
			IsPreviewing = EditorGUILayout.Toggle("Is Previewing" , IsPreviewing);
		}
		public void Update()
		{
			if (IsPreviewing)
				if (target != null)
					target.Update();
		}
	}
}
