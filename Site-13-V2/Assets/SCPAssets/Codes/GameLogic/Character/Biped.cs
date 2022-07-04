using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.Character
{
    public class Biped : MonoBehaviour
    {
        public Transform Root;
        public float MaxHorizontalRootTolerance;
        public Transform HorizontalLevel0;
        public Transform HorizontalLevel1;
        public Transform VerticalLevel0;
        public Transform VerticalLevel1;
        /// <summary>
        /// _I for `immediately`.
        /// </summary>
        /// <param name="Rotation"></param>
        public void HeadTo_I(Vector3 Rotation)
        {

        }
    }
}
