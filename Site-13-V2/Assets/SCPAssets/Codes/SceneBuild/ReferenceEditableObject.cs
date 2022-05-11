using System;
using System.Collections;
using System.Runtime.Serialization;
using UnityEngine;
namespace Site13Kernel.SceneBuild
{
    [Serializable]
    public class ReferenceEditableObject : MonoBehaviour
    {
        public EditableObject ReferedObject;
    }
}