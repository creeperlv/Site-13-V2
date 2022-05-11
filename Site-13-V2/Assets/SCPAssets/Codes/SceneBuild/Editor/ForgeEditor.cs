using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Site13Kernel.SceneBuild.Editors
{

    public class ForgeEditor : EditorWindow
    {

        [MenuItem("Site13/Forge Tool")]
        static void Init()
        {
            GetWindow<ForgeEditor>("Forge Scene Serializer").Show();
        }
        public string Result;
        public Transform Container;
        public SceneDescription SceneDescription=new SceneDescription();
        Vector2 SP;
        private void OnGUI()
        {
            GUILayout.Label("<color=#2288EE>FORGE</color>", new GUIStyle() { fontSize=36, richText=true});
            GUILayout.Label("<color=#2288EE>Scene Serializer</color>", new GUIStyle() { fontSize=28, richText=true});
            GUILayout.Label("Serialization Target:");
            Container = EditorGUILayout.ObjectField(Container, typeof(Transform), true) as Transform;
            GUILayout.Label("Scene Description", new GUIStyle() { fontSize = 24 });
            SceneDescription.SceneSkybox = EditorGUILayout.IntField("Skybox",0);
            SceneDescription.AmbientColor= EditorGUILayout.ColorField(new GUIContent("Ambient Color"),Color.black, true,false,true);
            SceneDescription.ForColor= EditorGUILayout.ColorField(new GUIContent("Fog Color"),Color.black);
            SceneDescription.Near= EditorGUILayout.FloatField("Fog Near",0);
            SceneDescription.Far= EditorGUILayout.FloatField("Fog Far",300);
            if (GUILayout.Button("Start Serialize"))
            {
                Serialize();
            }
            GUILayout.Label("Result:");
            //SP=EditorGUILayout.BeginScrollView(SP,false,true,GUILayout.Height(300));
            EditorGUILayout.SelectableLabel(Result, GUILayout.ExpandHeight(true));
            //EditorGUILayout.EndScrollView();
        }
        void Serialize()
        {
            if (Container == null)
                Result = "Please select a transform to serialize!";
            if (SceneDescription == null)
                Result = "Please create a scene description!";
            else
            {
                Result = SceneBuilder.Serialize(SceneDescription, Container);

            }
        }
    }

}