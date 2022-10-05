using Site13Kernel.Core;
using Site13Kernel.Data;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.GameLogic.FPS
{
    [Serializable]
    public class GenericWeapon:ControlledBehavior
    {
        public Weapon WeaponData;
        public Animator WeaponAnimation;
        public string Trigger_Idle;
        public string Trigger_Fire;
        public string Trigger_Overheat;
        public string Trigger_Vent;
        public AmmoDisp AmmoDispType = AmmoDisp.None;
        public List<Renderer> AmmoRenderers;
        public List<Text> AmmoDispTexts;
        public enum AmmoDisp
        {
            None, TwoDig, ThreeDig, Liner, Text
        }
        [Header("Bullet")]
        public PrefabReference Bullet;
        public PrefabReference FireEffect;
        [Header("Effects")]
        public Transform FirePoint;
        public Transform EffectPoint;
        public AudioSource FireSFX;
        public List<AudioClip> FireSounds;
        [Header("Effects")]
        public float FireInterval;

        public Site13Event OnSingleFire = new Site13Event();
        public Site13Event OnOverheat = new Site13Event();
        bool FIRE0;
        int FIRE1;
        bool FIRE2;
        bool FIRE3;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Fire()
        {
            FIRE0 = true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Press()
        {
            FIRE3 = true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Unfire()
        {
            FIRE0 = false;
            FIRE3 = false;
            FIRE1 = 0;
        }

        private void OnEnable()
        {
            NotifyWeaponAmmo();
        }
        public void SingleFire()
        {

        }
        public void NotifyWeaponAmmo()
        {
            switch (AmmoDispType)
            {
                case AmmoDisp.None:
                    break;
                case AmmoDisp.TwoDig:
                    {
                        AmmoRenderers[0].material.SetFloat("_DigitNum", WeaponData.CurrentMagazine % 10);
                        AmmoRenderers[1].material.SetFloat("_DigitNum", Mathf.FloorToInt(WeaponData.CurrentMagazine / 10));
                    }
                    break;
                case AmmoDisp.ThreeDig:
                    {

                        AmmoRenderers[0].material.SetFloat("_DigitNum", WeaponData.CurrentMagazine % 10);
                        AmmoRenderers[1].material.SetFloat("_DigitNum", Mathf.FloorToInt(WeaponData.CurrentMagazine / 10) % 10);
                        AmmoRenderers[2].material.SetFloat("_DigitNum", Mathf.FloorToInt(WeaponData.CurrentMagazine / 100));
                    }
                    break;
                case AmmoDisp.Liner:
                    break;
                case AmmoDisp.Text:
                    break;
                default:
                    break;
            }
        }
    }
}
