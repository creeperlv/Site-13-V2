using Site13Kernel.GameLogic.Character;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.GameLogic.AI.V2
{
    public class MapArea : MonoBehaviour
    {
        public MapAreaType areaType;
        public BoxCollider Collider;
        public Vector3 Size => Collider.size;
        public Vector3 Center => Collider.center;
        private void OnCollisionEnter(Collision collision)
        {
            var biped = collision.gameObject.GetComponent<Biped>();
            if (biped != null)
            {
                ApplyToBiped(biped);
            }
        }
        private void OnCollisionExit(Collision collision)
        {
            var biped = collision.gameObject.GetComponent<Biped>();
            if (biped != null)
            {
                CancelFromBiped(biped);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ApplyToBiped(Biped b)
        {
            switch (areaType)
            {
                case MapAreaType.Crouch:
                    if (!b.Crouch.Contains(this))
                        b.Crouch.Add(this);
                    break;
                case MapAreaType.Blindage:
                    if (!b.Blindage.Contains(this))
                        b.Blindage.Add(this);
                    break;
                default:
                    break;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CancelFromBiped(Biped b)
        {
            switch (areaType)
            {
                case MapAreaType.Crouch:
                    if (b.Crouch.Contains(this))
                        b.Crouch.Remove(this);
                    break;
                case MapAreaType.Blindage:
                    if (b.Blindage.Contains(this))
                        b.Blindage.Remove(this);
                    break;
                default:
                    break;
            }
        }
    }
    public enum MapAreaType
    {
        Crouch, Blindage
    }
}
