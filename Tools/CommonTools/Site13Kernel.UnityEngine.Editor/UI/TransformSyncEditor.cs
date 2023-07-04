using Site13Kernel.GameLogic.Animations;
using UnityEditor;
using UnityEngine;

namespace Site13Kernel.Editor.UI
{
	[CustomEditor(typeof(TransformSync))]
	public class TransformSyncEditor : UnityEditor.Editor
	{
		bool SyncWithBase;
		TransformSync Target_Base;
		public override void OnInspectorGUI()
		{

			DrawDefaultInspector();
			Target_Base = (TransformSync)target;
			if (GUILayout.Button("Bake"))
			{
				Target_Base.CalcDelta();
			}
			if (GUILayout.Button("Live Preview"))
			{
				PreviewTransformSync.OpenWindow(Target_Base);
			}

		}
	}
}
