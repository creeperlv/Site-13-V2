using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Data;
using Site13Kernel.Diagnostics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Site13Kernel.GameLogic
{
    public class SceneLoader : ControlledBehavior
    {
        public static SceneLoader Instance = null;
        public Dictionary<int, List<GameObject>> Scenes = new Dictionary<int, List<GameObject>>();
        public Dictionary<int, bool> SceneMap = new Dictionary<int, bool>();
        public Dictionary<int, bool> SceneStatusMap = new Dictionary<int, bool>();
        public Dictionary<int, bool> SceneMapStickFlag = new Dictionary<int, bool>();
        public List<SceneLoaderLoadItem> Preloads;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void Start()
        {
            if (Instance == null)
            {
                Instance = this;

                foreach (var item in Preloads)
                {
                    if (!item.isRef)
                        Instance.LoadScene(item.SceneID, item.isShow, item.isAddictive, item.isStick);
                }
            }

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EndLevel()
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
            try
            {
                AIController.CurrentController.DestoryAllCharacters();
            }
            catch (System.Exception)
            {
            }
            try
            {
                GlobalBioController.CurrentGlobalBioController.DestoryAll(true);
            }
            catch (System.Exception)
            {
            }
            try
            {
                GlobalBioController.CurrentGlobalBioController.DestoryAll(true);
            }
            catch (Exception)
            {
            }
            try
            {
                for (int i = WeaponPool.CurrentPool.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(WeaponPool.CurrentPool.transform.GetChild(i).gameObject);
                }
            }
            catch (Exception)
            {
            }
            GameRuntime.CurrentGlobals.isInLevel = false;
            Instance.ShowScene(4);
            try
            {
                Instance.Unload("LEVELBASE");
            }
            catch (Exception)
            {
            }
            Instance.LoadScene(GameRuntime.CurrentGlobals.MainMenuSceneID, true, false, false);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Init()
        {
            Start();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetStick(int SceneID, bool isStick)
        {
            SceneMapStickFlag[SceneID] = isStick;
            //return SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(SceneID));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SetActive(int SceneID)
        {
            return SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(SceneID));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SetActive(string SceneID)
        {
            var SID = Utilities.SceneUtility.LookUp(SceneID);
            if (SID == -1)
            {
                return false;
            }
            else
                return SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(SID));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetActiveSceneID(int SceneID)
        {
            return SceneManager.GetActiveScene().buildIndex;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool isOn(int ID, out bool isOn)
        {
            if (SceneStatusMap.ContainsKey(ID))
            {
                isOn = SceneStatusMap[ID];
                return true;
            }
            else
            {
                isOn = false;
                return false;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddSceneLog(int SceneID, bool Additive, bool Stick)
        {
            Debugger.CurrentDebugger.Log($"[SceneLoader]Added \"{SceneID}\", Additive={Additive}, Stick={Stick}");
            if (!SceneMap.TryAdd(SceneID, Additive))
            {
                SceneMap[SceneID] = Additive;
            }
            if (!SceneMapStickFlag.TryAdd(SceneID, Stick))
            {
                SceneMapStickFlag[SceneID] = Stick;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SceneKey"></param>
        /// <param name="ShowImmediately"></param>
        /// <param name="Additive"></param>
        /// <param name="stick"></param>
        /// <returns>false for Lookup failure. true for lookup succeed</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool LoadScene(string SceneKey, bool ShowImmediately, bool Additive, bool stick)
        {
            var SID = Utilities.SceneUtility.LookUp(SceneKey);
            if (SID == -1)
            {
                return false;
            }
            else
                LoadScene(SID, ShowImmediately, Additive, stick);
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void LoadScene(int SceneID, bool ShowImmediately, bool Additive, bool Stick)
        {
            Debugger.CurrentDebugger.Log($"[SceneLoader]Loading \"{SceneID}\", Additive={Additive}, Stick={Stick}");
            if (SceneMap.ContainsKey(SceneID))
            {
                Debugger.CurrentDebugger.Log($"[SceneLoader]SceneID: \"{SceneID}\" has already loaded, operation aborted.");
                var _On = false;
                isOn(SceneID, out _On);
                if (ShowImmediately)
                {
                    if (!_On)
                    {
                        ShowScene(SceneID);
                    }
                }
                else
                {
                    if (_On)
                    {
                        HideScene(SceneID);
                    }
                }
                {
                    for (int i = SceneMap.Count - 1; i > 0; i--)
                    {
                        if (SceneMapStickFlag.ContainsKey(SceneMap.Keys.ElementAt(i)))
                        {
                            if (SceneMapStickFlag[SceneMap.Keys.ElementAt(i)] == false)
                            {
                                Unload(SceneMap.Keys.ElementAt(i));
                            }
                        }
                        else
                        {
                            Unload(SceneMap.Keys.ElementAt(i));
                        }
                    }
                }
                return;
            }
            new LoadOperation(SceneID, ShowImmediately, Additive).StartLoadAsync();
            if (!SceneMap.TryAdd(SceneID, Additive))
            {
                SceneMap[SceneID] = Additive;
            }
            if (!SceneMapStickFlag.TryAdd(SceneID, Stick))
            {
                SceneMapStickFlag[SceneID] = Stick;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void HideScene(int ID)
        {
            Debugger.CurrentDebugger.Log($"[SceneLoader]Hiding \"{ID}\"");
            SceneLoader.Instance.SceneStatusMap[ID] = false;
            if (Instance.Scenes.ContainsKey(ID))
            {
                foreach (var item in Instance.Scenes[ID])
                {
                    item.SetActive(false);
                }
            }
            else
            {
                Debugger.CurrentDebugger.Log($"[SceneLoader]No Loaded Scene Matches \"{ID}\"");
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ShowScene(int ID)
        {
            Debugger.CurrentDebugger.Log($"[SceneLoader]Showing \"{ID}\"");
            SceneLoader.Instance.SceneStatusMap[ID] = true;
            if (Instance.Scenes.ContainsKey(ID))
            {
                foreach (var item in Instance.Scenes[ID])
                {
                    item.SetActive(true);
                }
            }
            else
            {
                Debugger.CurrentDebugger.Log($"[SceneLoader]No Loaded Scene Matches \"{ID}\"");
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAllUnstick()
        {
            Debugger.CurrentDebugger.Log($"[SceneLoader]Removing all unstick scenes");
            for (int i = SceneMapStickFlag.Count - 1; i >= 0; i--)
            {
                if (SceneMapStickFlag.Values.ElementAt(i) == false)
                {
                    Unload(SceneMapStickFlag.Keys.ElementAt(i));
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Unload(int SceneID)
        {
            Debugger.CurrentDebugger.Log($"[SceneLoader]Unloading \"{SceneID}\"");
            SceneManager.UnloadSceneAsync(SceneID);
            if (SceneMap.ContainsKey(SceneID))
            {
                SceneLoader.Instance.SceneStatusMap.Remove(SceneID);
                Scenes.Remove(SceneID);
                SceneMap.Remove(SceneID);
                SceneMapStickFlag.Remove(SceneID);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Unload(string sceneName)
        {
            var SceneID = Site13Kernel.Utilities.SceneUtility.LookUp(sceneName);
            Debugger.CurrentDebugger.Log($"[SceneLoader]Unloading \"{SceneID}\"");
            if (SceneManager.GetSceneByBuildIndex(SceneID).isLoaded)
                SceneManager.UnloadSceneAsync(SceneID);
            if (SceneMap.ContainsKey(SceneID))
            {
                SceneLoader.Instance.SceneStatusMap.Remove(SceneID);
                Scenes.Remove(SceneID);
                SceneMap.Remove(SceneID);
                SceneMapStickFlag.Remove(SceneID);
            }
        }
        public int LoadingOperationCount = 0;
        private class LoadOperation
        {
            int BGSceneID;
            bool open;
            bool add;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal LoadOperation(int ID, bool open, bool add)
            {
                this.BGSceneID = ID;
                this.open = open;
                this.add = add;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal void StartLoadAsync()
            {
                Instance.LoadingOperationCount++;
                if (add)
                    SceneManager.LoadSceneAsync(BGSceneID, LoadSceneMode.Additive).completed += OP_completed;
                else
                {
                    SceneManager.LoadSceneAsync(BGSceneID, LoadSceneMode.Additive).completed += OP_completed;
                    SceneLoader.Instance.RemoveAllUnstick();
                }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void OP_completed(AsyncOperation obj)
            {
                Instance.LoadingOperationCount--;
                var GO = SceneManager.GetSceneByBuildIndex(BGSceneID).GetRootGameObjects();
                List<GameObject> list = new List<GameObject>(GO);
                SceneLoader.Instance.Scenes.TryAdd(BGSceneID, list);
                SceneLoader.Instance.SceneStatusMap.TryAdd(BGSceneID, true);
                if (!open)
                    SceneLoader.Instance.HideScene(BGSceneID);
            }
        }
    }
    [Serializable]
    public class SceneLoaderLoadItem
    {
        public int SceneID;
        public bool isAddictive;
        public bool isStick;
        public bool isShow;
        public bool isRef;
    }
}