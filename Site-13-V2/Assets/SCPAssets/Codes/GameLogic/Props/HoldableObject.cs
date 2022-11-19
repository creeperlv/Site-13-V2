using CLUNL.Localization;
using Site13Kernel.Core;
using Site13Kernel.Core.Interactives;
using Site13Kernel.GameLogic.FPS;
using Site13Kernel.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.Props
{
    public class HoldableObject : ControlledBehavior
    {
        public int IconID;
        public int NameID;
        public bool isHolded;
        //public LocalizedString ObjectName;
        public string TargetAnimationSetID;
        public GameObject ContolledObject;
        public bool CanThrowOut;
        public float MeleeDamage;
        public BipedEntity Holder;
        [Header("Pickupable Definition")]
        public Pickupable Pickup;
        public List<BoxCollider> AttachedColliders;
        public void OnHold()
        {
            ApplyObjectStatus(false);
        }
        public void OnDrop()
        {
            ApplyObjectStatus(true);
        }
        public void ApplyObjectStatus(bool isPickupable = false)
        {
            Pickup.enabled = isPickupable;
            if (isPickupable)
            {
                foreach (var item in AttachedColliders)
                {
                    item.enabled = true;
                }
                this.gameObject.AddComponent<Rigidbody>();
                ObjectGenerator.SetLayerForChildren(this.gameObject, GameRuntime.CurrentGlobals.PickupableLayer);
                this.Pickup.gameObject.layer = GameRuntime.CurrentGlobals.PickupableTriggerLayer;
            }
            else
            {

                foreach (var item in AttachedColliders)
                {
                    item.enabled = false;
                }
                GameObject.Destroy(this.GetComponent<Rigidbody>());
            }
        }
    }
}
