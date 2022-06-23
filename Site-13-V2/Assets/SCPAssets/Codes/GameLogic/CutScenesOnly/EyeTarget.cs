using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel
{
    public class EyeTarget : MonoBehaviour
    {
        public static EyeTarget Instance;
        public Transform ThisTransform;
        private void Start()
        {
            Instance = this;
            ThisTransform = transform;
        }
    }
}
