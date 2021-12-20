using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic.AI
{
    public class BioCollector : MonoBehaviour
    {
        public BioEntity BioEntity;
        public float LastClosestFoeDistance;
        public BioEntity LastClosestFoe;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnTriggerStay(Collider other)
        {
            if (GlobalBioController.CurrentGlobalBioController == null) return;
            var BIO = other.GetComponentInChildren<BioEntity>();
            if (BIO != null)
            {
                float Dis = (transform.position - BIO.transform.position).magnitude;
                if (GlobalBioController.CurrentGlobalBioController.isFoe(BIO.CombatRelationGroup, BioEntity.CombatRelationGroup))
                {
                    if (LastClosestFoeDistance > Dis || LastClosestFoe == null)
                    {
                        LastClosestFoeDistance = Dis;
                        LastClosestFoe = BIO;
                    }
                }
            }
        }
        //private void OnCollisionStay(Collision other)
        //{

        //    if (GlobalBioController.CurrentGlobalBioController == null) return;
        //    var BIO = other.gameObject.GetComponentInChildren<BioEntity>();
        //    if (BIO != null)
        //    {
        //        float Dis = (transform.position - BIO.transform.position).magnitude;
        //        if (GlobalBioController.CurrentGlobalBioController.isFoe(BIO.CombatRelationGroup, BioEntity.CombatRelationGroup))
        //        {
        //            if (LastClosestFoeDistance > Dis)
        //            {
        //                LastClosestFoeDistance = Dis;
        //                LastClosestFoe = BIO;
        //            }
        //        }
        //    }
        //}
    }
}
