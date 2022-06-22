using Site13Kernel.UI.Elements;
using UnityEditor;
using UnityEditor.UI;

namespace Site13Kernel.Editor.UI
{
    [CustomEditor(typeof(TextButton), true)]
    [CanEditMultipleObjects]
    public class TextButtonEditor : ButtonEditor
    {

        SerializedProperty ControlledText;
        protected override void OnEnable()
        {
            base.OnEnable();
            ControlledText = serializedObject.FindProperty("ControlledText");
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(ControlledText);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
