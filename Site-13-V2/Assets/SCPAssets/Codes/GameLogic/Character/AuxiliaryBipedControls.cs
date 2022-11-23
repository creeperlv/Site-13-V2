using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.GameLogic.Effects;
using Site13Kernel.GameLogic.FPS;
using Site13Kernel.GameLogic.Physic;
using Site13Kernel.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.Character
{
    public class AuxiliaryBipedControls : MonoBehaviour
    {
        public MeleeArea meleeArea;
        public BipedEntity BindedEntity;
        public BipedController BindedController;
        public CameraShakeEffect CSE;
        public float CSE_Run_Intensity = 1;
        public float CSE_Run_H = 1;
        public float CSE_Run_V = 1;
        public float CSE_Run_R = 25;
        public float CSE_Run_DimIntensity = 3;


        public float CSE_Melee_Intensity = 1;
        public float CSE_Melee_H = 1;
        public float CSE_Melee_V = 1;
        public float CSE_Melee_R = 25;
        public float CSE_Melee_DimIntensity = 3;
        public bool CSE_Melee_isR = false;

        public float CSE_Walk_Intensity = 1;
        public float CSE_Walk_H = 1;
        public float CSE_Walk_V = 1;
        public float CSE_Walk_R = 25;
        public float CSE_Walk_DimIntensity = 2;

        public float GrenadeThrowForce = 10;

        public List<AudioSource> MeleeSource;
        public List<AudioClip> MeleeClip;
        public void PlayMeleeSFX()
        {
            var source = ListOperations.ObtainOne(MeleeSource);
            source.clip = ListOperations.ObtainOne(MeleeClip);
            source.Play();
        }
        public void MeleeStart()
        {
            meleeArea.StartDetection();
        }
        public void PlayWeaponReloadSound()
        {
            BindedController.PlayWeaponReloadSound();
        }
        public void PlayWeaponReloadSoundWithEmpty()
        {
            BindedController.PlayWeaponReloadSoundWithEmpty();
        }
        public void PlayWeaponChamberingAnimation()
        {
            BindedController.PlayWeaponChamberingAnimation();
        }
        public void PlayWeaponChamberingFromEmptyAnimation()
        {
            BindedController.PlayWeaponChamberingFromEmptyAnimation();
        }
        public void ThrowOutHoldingObject()
        {
            Debug.Log("Trying throw object.");
            var ho = this.BindedEntity.EntityBag.HoldableObject;
            this.BindedEntity.EntityBag.DropHoldable(ho, true);
            //ho.transform.SetParent(ho.OriginalParent);
            //ho.transform.localScale = ho.OriginalScale;
            ho.transform.position = BindedEntity.GrenadeEmissionPoint.position;
            ho.transform.rotation = BindedEntity.GrenadeEmissionPoint.rotation;
            var PO = ho.GetComponent<PhysicsObject>();
            if (PO != null)
            {
                PO.Emitter = BindedEntity;
            }
            else
            {
                PO = ho.gameObject.AddComponent<PhysicsObject>();
                PO.Emitter = BindedEntity;
            }
            var rb=ho.GetComponent<Rigidbody>();
            rb.AddForce(BindedEntity.GrenadeEmissionPoint.forward * ho.ThrowOutForce, ForceMode.Impulse);
        }
        public void ThrowAGrenade()
        {
            {
                BindedController._Grenades[BindedEntity.EntityBag.CurrentGrenade].SetActive(false);
            }
            var obj=GrenadeController.CurrentController.Instantiate(
                GrenadePool.CurrentPool.GrenadeItemMap[BindedEntity.EntityBag.CurrentGrenade].GamePlayPrefab,
                BindedEntity.GrenadeEmissionPoint.position,
                BindedEntity.GrenadeEmissionPoint.rotation,
                BindedEntity.GrenadeEmissionPoint.forward * GrenadeThrowForce, ForceMode.Impulse,BindedEntity);
            BindedEntity.EntityBag.Grenades[BindedEntity.EntityBag.CurrentGrenade].RemainingCount--;
            var PO=obj.GetComponent<PhysicsObject>();
            if(PO != null)
            {
                PO.Emitter = BindedEntity;
            }
            else
            {
                PO=obj.AddComponent<PhysicsObject>();
                PO.Emitter = BindedEntity;
            }
        }
        public void ShakeCamRunStep()
        {

            CSE.SetShake(Intensity: CSE_Run_Intensity, willDiminish: true, DiminishIntensity: CSE_Run_DimIntensity, true,
                RotationShakeSpeed: CSE_Run_R, HorizontalBaseIntensity: CSE_Run_H, VerticalBaseIntensity: CSE_Run_V);
        }
        public void ShakeCamWalkStep()
        {
            CSE.SetShake(Intensity: CSE_Walk_Intensity, willDiminish: true, DiminishIntensity: CSE_Walk_DimIntensity, true,
                RotationShakeSpeed: CSE_Walk_R, HorizontalBaseIntensity: CSE_Walk_H, VerticalBaseIntensity: CSE_Walk_V);
        }
        public void ShakeCamMelee()
        {
            CSE.SetShake(Intensity: CSE_Melee_Intensity, willDiminish: true, DiminishIntensity: CSE_Melee_DimIntensity, CSE_Melee_isR,
                RotationShakeSpeed: CSE_Melee_R, HorizontalBaseIntensity: CSE_Melee_H, VerticalBaseIntensity: CSE_Melee_V);
        }
        public void MeleeStop()
        {
            meleeArea.StopDetection();
        }
    }
}
