using Site13Kernel.GameLogic.Animations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

namespace Site13Kernel.Editor.UI
{
    public class BipedPseudoTransformCreator : EditorWindow
    {
        [MenuItem("Site13/Biped Tools/Biped Pseudo Transform Creator")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(BipedPseudoTransformCreator));
        }
        public class StashTransform
        {
            public Transform Trans;
            public List<StashTransform> Children;
        }
        Transform _obj0;
        Transform _obj1;
        public StashTransform BuildTree(Transform transform)
        {
            StashTransform stashTransform = new StashTransform();
            stashTransform.Trans = transform;

            int Count = transform.childCount;
            List<StashTransform> Children = new List<StashTransform>(Count);
            for (int i = 0 ; i < Count ; i++)
            {
                Children.Add(BuildTree(transform.GetChild(i)));
            }
            stashTransform.Children = Children;
            return stashTransform;
        }
        public void Disassemble(StashTransform stashTransform , Transform Cache)
        {
            var p = stashTransform.Trans.parent;
            if (p != Cache)
            {
                stashTransform.Trans.parent = Cache;
            }
            foreach (var item in stashTransform.Children)
            {
                Disassemble(item , Cache);
            }

        }
        public void SetRotation(StashTransform transform)
        {
            transform.Trans.rotation = Quaternion.identity;
            foreach (var item in transform.Children)
            {
                SetRotation(item);
            }
        }
        public void Reassemble(StashTransform stashTransform)
        {
            foreach (var item in stashTransform.Children)
            {
                item.Trans.parent = stashTransform.Trans;
                Reassemble(item);
            }
        }
        public void ResetRotation(StashTransform _ToReset , Transform Cache)
        {
            Disassemble(_ToReset , Cache);
            Debug.Log("Disassembled");
            SetRotation(_ToReset);
            Debug.Log("Reseting Rotation");
            Reassemble(_ToReset);
            Debug.Log("Reassemble.");
        }
        public void SyncTransform(StashTransform stashTransform , Transform transform , TransformSync sync)
        {
            foreach (var item in stashTransform.Children)
            {
                var _name = item.Trans.name;
                var _t = transform.Find(_name);
                if (_t != null)
                {
                    sync.transforms.Add(new TrackedTransforms { Source = item.Trans , Target = _t });
                    SyncTransform(item , _t , sync);
                }
            }
        }
        public void OnGUI()
        {
            GUILayout.Label("<color=#2288EE>Biped Tool</color>" , new GUIStyle() { fontSize = 36 , richText = true });
            GUILayout.Label("<color=#2288EE>Biped Pseudo Transform Creator</color>" , new GUIStyle() { fontSize = 24 , richText = true });
            _obj0 = (Transform)EditorGUILayout.ObjectField("Biped Root" , _obj0 , typeof(Transform) , true);
            _obj1 = (Transform)EditorGUILayout.ObjectField("Created Root" , _obj1 , typeof(Transform) , true);
            if (GUILayout.Button("Create"))
            {
                if (!(_obj0 is null))
                {
                    if (!(_obj1 is null))
                    {
                        var result = Object.Instantiate<Transform>(_obj0 , _obj1);
                        result.position = _obj0.position;
                        var _ToReset = BuildTree(result);
                        ResetRotation(_ToReset , _obj1);
                    }
                }
            }
            if (GUILayout.Button("Create And Attach Transform Sync"))
            {
                if (!(_obj0 is null))
                {
                    if (!(_obj1 is null))
                    {
                        var result = Object.Instantiate<Transform>(_obj0 , _obj1);
                        result.position = _obj0.position;
                        var ts = result.gameObject.AddComponent<TransformSync>();
                        var _ToReset = BuildTree(result);
                        SyncTransform(_ToReset , _obj0 , ts);
                        ResetRotation(_ToReset , _obj1);
                    }
                }
            }
        }
    }
}
