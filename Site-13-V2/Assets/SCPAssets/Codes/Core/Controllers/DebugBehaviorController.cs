using Site13Kernel.Core.Profiles;
using Site13Kernel.Diagnostics;
using Site13Kernel.GameLogic.Character;
using Site13Kernel.GameLogic.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = Site13Kernel.Diagnostics.Debug;

namespace Site13Kernel.Core.Controllers
{
    public class DebugBehaviorController : BehaviorController
    {
#if DEBUG
        public bool GenerateProfile;
        void Start()
        {
            GameRuntime.CurrentLocals = new GameLocals();
            if (CrossScene)
            {
                DontDestroyOnLoad(this.gameObject);
            }
            StartCoroutine(InitProfile());
            SerializeAll();
            foreach (var item in _OnInit)
            {
                try
                {
                    InitBehavior(item);
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
            }
            foreach (var item in _OnInit)
            {
                try
                {
                    item.Init();

                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
        IEnumerator InitProfile()
        {
            yield return null;
            yield return null;
            if (GenerateProfile)
            {
                Profile profile = new Profile { PlayerID = Guid.NewGuid(), PlayerName = "DEBUG PLAYER" };
                ProfileSystem.AddProfile(profile.Duplicate());
                ProfileSystem.SetActiveProfile(profile);
                yield return null;
                yield return null;
                yield return null;
                BipedController.Instance.GetComponent<ActiveInteractor>().PlayerID = profile.PlayerID;
                BipedController.Instance.GetComponent<ActiveInteractor>().Profile = profile.Duplicate();
                BipedController.Instance.GetComponent<ActiveInteractor>().ID = profile.PlayerID.ToString();
            }
        }
        void Update()
        {
            float DeltaTime=Time.deltaTime;
            float UDeltaTime=Time.unscaledDeltaTime;
            foreach (var item in _OnRefresh)
            {
                try
                {
                    item.Refresh(DeltaTime,UDeltaTime);
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
        private void FixedUpdate()
        {

            float DeltaTime=Time.fixedDeltaTime;
            float UDeltaTime=Time.fixedUnscaledDeltaTime;
            foreach (var item in _OnFixedRefresh)
            {
                try
                {
                    item.FixedRefresh(DeltaTime,UDeltaTime);
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }
#else
    void Start(){
        GameObject.Destroy(this.gameObject);
    }
#endif
    }
}
