using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Core.TagSystem
{
    public class ConvertToTagEntity : MonoBehaviour
    {
        void Start()
        {
            TagSystem.TagSystemManager.Instance.AddObject(this.gameObject);
        }

    }
}
