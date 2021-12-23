using Site13Kernel.Core;
using Site13Kernel.Core.Interactives;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic.Props
{
    public class BaseDoor : ControlledBehavior
    {
        public bool isDiscoverable;
        public bool isLocked;
        public float DoorRange = 5f;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Open()
        {

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Close()
        {

        }
        bool State;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void OnFrame(float DeltaTime, float UnscaledDeltaTime)
        {

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            if (isLocked) return;
            if (GameRuntime.CurrentGlobals.isPaused) return;
            {
                //Vector3 position = base.transform.position;
                //Collider[] array = Physics.OverlapSphere(position, DoorRange);
                //bool isHit = false;
                //foreach (Collider collider in array)
                //{
                //    if (collider.GetComponent<BioEntity>() != null)
                //    {
                //        isHit = true;
                //        break;
                //    }
                //}
                bool isHit = false;
                foreach (var item in InsideEntities)
                {
                    if (item != null)
                    {
                        isHit = true;
                        break;
                    }
                }
                if (State == isHit)
                {

                }
                else
                {
                    State = isHit;
                    if (State)
                    {
                        Open();
                    }
                    else
                    {
                        Close();
                    }
                }
            }
            OnFrame(DeltaTime, UnscaledDeltaTime);
        }
        public List<BioEntity> InsideEntities = new List<BioEntity>();
        private void OnTriggerEnter(Collider other)
        {
            var BE = other.gameObject.GetComponentInChildren<BioEntity>();
            if (BE != null)
                InsideEntities.Add(BE);
        }
        private void OnTriggerExit(Collider other)
        {
            var BE = other.gameObject.GetComponentInChildren<BioEntity>();
            if (BE != null)
                if (InsideEntities.Contains(BE))
                    InsideEntities.Remove(BE);
        }
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public override void Operate(float DeltaTime, float UnscaledDeltaTime, DamagableEntity Operator)
        //{
        //    Open();
        //}
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        //public override void UnOperate()
        //{
        //    Close();
        //}
    }
}
