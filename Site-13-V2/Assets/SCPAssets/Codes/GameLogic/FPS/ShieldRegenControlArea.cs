using Site13Kernel.Core;
using Site13Kernel.Core.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.FPS
{
    public class ShieldRegenControlArea : MonoBehaviour
    {
        public bool TriggerOnExit;
        public float InsideValue;
        public float ExitValue;
        public bool OnlyWorkOnPlayer;
        public void OnTriggerStay(Collider other)
        {
            if (OnlyWorkOnPlayer)
            {

                var entity = other.gameObject.GetComponentInChildren<BioEntity>();
                if (entity != null&&other.gameObject.GetComponentInChildren<FPSController>()!=null)
                {
                    entity.ShieldRecoverSpeed = InsideValue;
                }
            }
            else
            {

                var entity = other.gameObject.GetComponentInChildren<BioEntity>();
                if (entity != null)
                {
                    entity.ShieldRecoverSpeed = InsideValue;
                }
            }
        }
        public void OnTriggerExit(Collider other)
        {
            if (OnlyWorkOnPlayer)
            {

                var entity = other.gameObject.GetComponentInChildren<BioEntity>();
                if (entity != null && other.gameObject.GetComponentInChildren<FPSController>() != null)
                {
                    entity.ShieldRecoverSpeed = ExitValue;
                }
            }
            else
            {

                if (TriggerOnExit)
                {

                    var entity = other.gameObject.GetComponent<BioEntity>();
                    if (entity != null)
                    {
                        entity.ShieldRecoverSpeed = ExitValue;
                    }
                }
            }
        }
    }
}
