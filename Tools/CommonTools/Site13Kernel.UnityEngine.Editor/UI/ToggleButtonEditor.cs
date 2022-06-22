using Site13Kernel.UI.Elements;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine.UI;

namespace Site13Kernel.Editor.UI
{
    [CustomEditor(typeof(ToggleButton), true)]
    [CanEditMultipleObjects]
    public class ToggleButtonEditor : TextButtonEditor
    {

        SerializedProperty AnimatedMark;
        SerializedProperty CheckedTrigger;
        SerializedProperty CheckedMouseOnTrigger;
        SerializedProperty UnCheckedTrigger;
        SerializedProperty UnCheckedMouseOnTrigger;
        SerializedProperty PreventUncheckOnClick;
        protected override void OnEnable()
        {
            base.OnEnable();
            AnimatedMark = serializedObject.FindProperty("AnimatedMark");
            CheckedTrigger = serializedObject.FindProperty("CheckedTrigger");
            UnCheckedTrigger = serializedObject.FindProperty("UnCheckedTrigger");
            CheckedMouseOnTrigger = serializedObject.FindProperty("CheckedMouseOnTrigger");
            UnCheckedMouseOnTrigger = serializedObject.FindProperty("UnCheckedMouseOnTrigger");
            PreventUncheckOnClick = serializedObject.FindProperty("PreventUncheckOnClick");
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
            EditorGUILayout.PropertyField(PreventUncheckOnClick);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
