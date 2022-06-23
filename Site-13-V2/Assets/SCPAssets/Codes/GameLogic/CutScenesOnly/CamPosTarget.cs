using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel
{
    public class CamPosTarget : MonoBehaviour
    {
        public static CamPosTarget Instance;
        public Transform ThisTransform;
        void Start()
        {
            Instance = this;
            ThisTransform = transform;
        }

    }
}
