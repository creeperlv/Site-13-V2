using Site13Kernel.UI.Elements;
using UnityEditor;

namespace Site13Kernel.Editor.UI
{
    [CustomEditor(typeof(IconToggleButton), true)]
    [CanEditMultipleObjects]
    public class IconToggleButtonEditor : ToggleButtonEditor
    {

        SerializedProperty ControlledImage;
        protected override void OnEnable()
        {
            base.OnEnable();
            ControlledImage = serializedObject.FindProperty("ControlledImage");
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(ControlledImage);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
