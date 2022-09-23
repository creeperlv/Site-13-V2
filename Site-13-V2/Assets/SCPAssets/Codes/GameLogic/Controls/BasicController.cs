using Site13Kernel.Core;
using UnityEngine;

namespace Site13Kernel.GameLogic.Controls
{
    public class BasicController : ControlledBehavior
    {

        public Transform HorizontalTransform;
        public Transform VerticalTransform;
        public Transform MovingTransform;
        public virtual void HorizontalRotation(float Angle)
        {

        }
        public virtual void VerticalRotation(float Angle)
        {

        }
        public virtual void Move(Vector2 Movement,float DeltaTime)
        {
            MovingTransform.Translate(Movement * DeltaTime);
        }
    }
}
