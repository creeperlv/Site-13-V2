using Site13Kernel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.CutScenesOnly
{
    public class GenericControllableRenderer : MonoBehaviour
    {
        public Renderer ControlledRenderer;
        Material[] Materials;
        public void SetColor(string Name,Color Value)
        {
            foreach (var item in Materials)
            {
                item.SetColor(Name, Value);
            }
        }
        public void SetFloat(string Name,float Value)
        {
            foreach (var item in Materials)
            {
                item.SetFloat(Name, Value);
            }
        }
        void Start()
        {
            Materials = ControlledRenderer.materials;
        }

    }
}
