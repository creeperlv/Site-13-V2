using Site13Kernel.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.FPS
{
    [Serializable]
    public class BasicWeapon : MonoBehaviour
    {
        public Weapon Base;
        
        public GameObject BulletPrefab;
        
        public float FireInterval = 0;
        public float Recoil = 0;
        public float MaxRecoil = 2;
        public float RecoilRecoverSpeed = 5;

        public WeaponFireType FireType = WeaponFireType.FullAuto;
        
        public bool FIRE0 = false;
        public bool FIRE1 = false;
        public void OnFrame(float DeltaT, float UnscaledDeltaT)
        {
            if (FIRE0)
            {

            }
        }
    }
    public enum WeaponFireType
    {
        FullAuto, 
        SemiAuto,
        SingleShot
    }
}
