using Site13Kernel.UI.Elements;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;

namespace Site13Kernel.UnityEngine.Editor.UI
{
    [CustomEditor(typeof(ToggleButton), true)]
    [CanEditMultipleObjects]
    public class ToggleButtonEditor: ButtonEditor
    {

        SerializedProperty AnimatedMark;
        SerializedProperty CheckedTrigger;
        SerializedProperty CheckedMouseOnTrigger;
        SerializedProperty UnCheckedTrigger;
        SerializedProperty UnCheckedMouseOnTrigger;
        protected override void OnEnable()
        {
            base.OnEnable();
            AnimatedMark = serializedObject.FindProperty("AnimatedMark");
            CheckedTrigger = serializedObject.FindProperty("CheckedTrigger");
            UnCheckedTrigger = serializedObject.FindProperty("UnCheckedTrigger");
            CheckedMouseOnTrigger = serializedObject.FindProperty("CheckedMouseOnTrigger");
            UnCheckedMouseOnTrigger = serializedObject.FindProperty("UnCheckedMouseOnTrigger");
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(AnimatedMark);
            EditorGUILayout.PropertyField(CheckedTrigger);
            EditorGUILayout.PropertyField(CheckedMouseOnTrigger);
            EditorGUILayout.PropertyField(UnCheckedTrigger);
            EditorGUILayout.PropertyField(UnCheckedMouseOnTrigger);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
