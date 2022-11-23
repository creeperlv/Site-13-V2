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
        public Transform OriginalParent;
        public Vector3 OriginalScale;
        //public LocalizedString ObjectName;
        public string TargetAnimationSetID;
        public int OriginalLayer;
        public GameObject ContolledObject;
        public bool CanThrowOut;
        public bool AllowRun;
        public float ThrowOutForce = 10;
        public float MeleeDamage;
        public BipedEntity Holder;
        [Header("Pickupable Definition")]
        public Pickupable Pickup;
        public List<Collider> AttachedColliders;
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
                Debug.Log("Trying to set layer.");
                ObjectGenerator.SetLayerForChildrenWithZero(this.gameObject, OriginalLayer);
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
