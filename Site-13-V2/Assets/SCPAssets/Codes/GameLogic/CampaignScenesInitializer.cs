using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Site13Kernel.Core;
using UnityEngine.SceneManagement;

namespace Site13Kernel.GameLogic
{
    public class CampaignScenesInitializer : SyncInitializer
    {
        public List<string> PreloadScenes=new List<string>();
        public Dictionary<string,AsyncOperation> asyncOperations= new Dictionary<string,AsyncOperation>();
        public Dictionary<string,GameObject[]> LoadedScenes= new Dictionary<string,GameObject[]>();
        public override void Init()
        {
            foreach (var item in PreloadScenes)
            {
                var AO=SceneManager.LoadSceneAsync(item);
                AO.allowSceneActivation = false;
                asyncOperations.Add(item, AO);
            }
        }
        public override void Execute()
        {

            bool __DONE = true;
            foreach (var item in asyncOperations)
            {
                if (item.Value.progress >= .9f)
                {
                    item.Value.allowSceneActivation = true;
                    var RGOs=SceneManager.GetSceneByName(item.Key).GetRootGameObjects();
                    LoadedScenes.Add(item.Key, RGOs);
                    foreach (var OBJ in RGOs)
                    {
                        OBJ.SetActive(false);
                    }
                    __DONE &= true;
                }
                else
                {
                    __DONE &= false;
                }
            }
            isDone = __DONE;
        }
    }
}
