using Site13Kernel.Core;
using System;
using UnityEngine;

namespace Site13Kernel.GameLogic.Controls
{
    public class BasicController : ControlledBehavior
    {
        public static BasicController Instance;
        public ControllerFunctions ControllerFunctions;
        public bool InputControllable;
        public Transform HorizontalTransform;
        public Transform VerticalTransform;
        public Transform MovingTransform;
        public void OnEnable()
        {
            if (InputControllable)
            {
                Instance = this;
            }
        }
        public virtual void HorizontalRotation(float Angle)
        {

        }
        public virtual void VerticalRotation(float Angle)
        {

        }
        public virtual void FlashLight()
        {

        }
        public virtual void Melee()
        {

        }
        public virtual void Reload()
        {

        }
        public virtual void UseEquipment()
        {

        }
        public virtual void ThrowGrenade()
        {

        }
        public virtual void Run()
        {

        }
        public virtual void CancelRun()
        {

        }
        public virtual void Aim()
        {

        }
        public virtual void CancelAim()
        {

        }
        public virtual void Crouch()
        {

        }
        public virtual void CancelCrouch()
        {

        }
        public virtual void Zoom()
        {

        }
        public virtual void CancelZoom()
        {

        }
        public virtual void StartFire()
        {

        }
        public virtual void CancelFire()
        {

        }
        public virtual void Interact()
        {

        }
        public virtual void CancelInteract()
        {

        }
        public virtual void Jump()
        {

        }
        public virtual void SwitchWeapon() { }
        public virtual void SwitchGrenade() { }
        public virtual void SwitchEquipment() { }
        public virtual void Move(Vector2 Movement, float DeltaTime)
        {
            MovingTransform.Translate(Movement * DeltaTime);
        }
    }
}
