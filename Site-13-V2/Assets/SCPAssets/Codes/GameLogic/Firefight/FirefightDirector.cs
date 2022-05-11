using CLUNL.Localization;
using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Data;
using Site13Kernel.GameLogic.AI;
using Site13Kernel.GameLogic.Level;
using Site13Kernel.UI;
using Site13Kernel.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.Firefight
{
    public class FirefightDirector : MonoBehaviour
    {
        public static FirefightDirector Instance;
        public int MaxAICount = 45;
        public AudioSource BroadCaster;
        public GameObject EndGameArea;
        public GameObject EndGameAnimationPart0;
        public GameObject EndGameAnimationPart1;
        public EventTrigger EndGameTrigger;
        public float EndGameAnimationLength;
        public List<KVPair<FirefightMode, List<AudioClip>>> InitVoices;
        public List<KVPair<FirefightMode, ResourceBuilder>> Resources;
        public List<KVPair<FirefightMode, LocalizedString>> Missions;
        public List<FirefightSpawner> Spawners;
        public List<Transform> SpawnPoints;
        public List<Transform> PlayerSpawnPoints;
        public Transform InitialSpawnPoint;
        public List<GameObject> ActiveOnStart;
        public List<WeaponSpawner> WeaponSpawners;
        public List<Routine> AvailableRoutines;
        public FirefightDefinition DefaultFirefight;
        public string PlayerID = "PLAYER";
        public BehaviorController BehaviorController;
        public KVPair<Weapon, Weapon> InitialWeaponSet;
        public GameObject DeathAnimationObject;
        public float DeathAnimationLength;
        public float RespawnSpan = 5;
        FirefightDefinition def;
        int WaveCont;
        public void Start()
        {
            Instance = this;
            if (FirefightLocals.Instance != null)
            {
                def = FirefightLocals.Instance.CurrentDef;
            }
            else
            {
                def = DefaultFirefight;
                StartFirefight();
            }
        }
        public void StartFirefight()
        {
            ScoreBoard.ClearScore();
            foreach (var item in ActiveOnStart)
            {
                item.SetActive(true);
            }
            if (def.AllowWeaponSpawn)
            {
                foreach (WeaponSpawner item in WeaponSpawners)
                {
                    item.StartSpawnProcess();
                }
            }
            switch (def.GameMode)
            {
                case FirefightMode.LimitedTime:
                    StartCoroutine(LimitedTimeFlow());
                    break;
                case FirefightMode.UnlimitedFirefight:
                    StartCoroutine(UnlimitedFlow());
                    break;
                default:
                    break;
            }
        }
        IEnumerator LimitedTimeFlow()
        {
            yield return null;
            foreach (var item in Resources)
            {
                if (item.Key == FirefightMode.LimitedTime)
                {
                    item.Value.Init();
                    break;
                }
            }
            yield return null;
            SpawnPlayer(InitialSpawnPoint);
            yield return null;
            yield return GiveWeapon();
            yield return null;
            IssueMission();
            StartCoroutine(CountDown());
            while (true)
            {
                yield return ARound();
            }
        }
        IEnumerator CountDown()
        {
            yield return new WaitForSeconds(def.TimeLength);
            EndGameArea.SetActive(true);
            EndGameAnimationPart0.SetActive(true);
            EndGameTrigger.AddCallback(() =>
            {
                FPSController.Instance.gameObject.SetActive(false);
                EndGameAnimationPart0.SetActive(false);
                EndGameAnimationPart1.SetActive(true);
                StartCoroutine(EndGameAnimationCountDown());
            });
        }
        IEnumerator EndGameAnimationCountDown()
        {
            yield return new WaitForSeconds(EndGameAnimationLength);
            ToPGCB();
        }
        IEnumerator DeathAnimationCountDown()
        {
            yield return new WaitForSeconds(DeathAnimationLength);
            ToPGCB();
        }
        void ToPGCB()
        {
            AIController.CurrentController.DestoryAllCharacters();
            try
            {
                GlobalBioController.CurrentGlobalBioController.DestoryAll(true);
            }
            catch (System.Exception)
            {
            }
            SceneLoader.Instance.SetStick(GameRuntime.CurrentGlobals.Scene_LevelBase, false);
            SceneLoader.Instance.LoadScene("FIREFIGHT_WIN", true, false, false);
        }
        IEnumerator ARound()
        {
            float WaitTime = 0;
            while (AIController.CurrentController._OnRefresh.Count > MaxAICount)
            {
                yield return null;
            }
            var i = 0;
            foreach (var item in SpawnPoints)
            {
                i++;
                if (i > WaveCont)
                {
                    if (WaveCont > SpawnPoints.Count)
                    {

                    }
                    else
                        WaveCont++;
                    break;
                }
                var a = Maths.ObtainOne(Spawners);
                WaitTime = Mathf.Max(WaitTime, a.SelfDestruction);
                var g = GameObject.Instantiate(a, item);
                var _g = g.GetComponent<FirefightSpawner>();
                _g.StartSpawn();
                _g.SetRoute(AvailableRoutines);
                g.transform.localPosition = Vector3.zero;
                g.transform.localRotation = Quaternion.identity;
            }
            yield return new WaitForSeconds(WaitTime);
        }
        IEnumerator UnlimitedFlow()
        {
            yield return null;
            foreach (var item in Resources)
            {
                if (item.Key == FirefightMode.UnlimitedFirefight)
                {
                    item.Value.Init();
                    break;
                }
            }
            yield return null;
            SpawnPlayer(InitialSpawnPoint);
            yield return null;
            yield return GiveWeapon();
            yield return null;
            IssueMission();
            while (true)
            {
                yield return ARound();
            }
        }
        void SetDeath()
        {
            switch (def.GameMode)
            {
                case FirefightMode.LimitedTime:
                    FPSController.Instance.OnDeath = () => { StartCoroutine(Respawn()); };
                    break;
                case FirefightMode.UnlimitedFirefight:
                    FPSController.Instance.OnDeath = () =>
                    {
                        DeathAnimationObject.SetActive(true);
                        StartCoroutine(DeathAnimationCountDown());
                    };
                    break;
                default:
                    break;
            }
        }
        void Announce()
        {
            if (BroadCaster != null)
                foreach (var item in InitVoices)
                {
                    if (item.Key == def.GameMode)
                    {
                        BroadCaster.clip = Maths.ObtainOne(item.Value);
                        BroadCaster.Play();
                    }
                }
        }
        void IssueMission()
        {
            foreach (var item in Missions)
            {
                if (item.Key == def.GameMode)
                {
                    FPSController.Instance.IssueMission(item.Value);
                    break;
                }
            }
            try
            {
                Announce();
            }
            catch (System.Exception)
            {
            }
        }
        IEnumerator GiveWeapon()
        {
            FPSController.Instance.GiveWeapon(InitialWeaponSet.Key);
            yield return null;
            FPSController.Instance.GiveWeapon(InitialWeaponSet.Value);
        }
        IEnumerator Respawn()
        {
            yield return new WaitForSeconds(RespawnSpan);
            SpawnPlayer(Maths.ObtainOne(PlayerSpawnPoints));
            yield return null;
            yield return GiveWeapon();
        }
        void SpawnPlayer(Transform SpawnPoint)
        {

            var PLAYER = GlobalBioController.CurrentGlobalBioController.Spawn(PlayerID, Vector3.zero, Vector3.zero);
            PLAYER.transform.GetChild(1).position = SpawnPoint.position;
            PLAYER.transform.GetChild(1).rotation = SpawnPoint.rotation;
            var FPSC = PLAYER.GetComponentInChildren<FPSController>();
            BehaviorController.RegisterRefresh(FPSC);
            FPSC.Parent = BehaviorController;
            FPSC.Init();
            SetDeath();
        }

    }
}
