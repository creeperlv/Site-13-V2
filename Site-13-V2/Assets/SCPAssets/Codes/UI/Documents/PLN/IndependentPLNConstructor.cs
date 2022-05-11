using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Site13Kernel.UI.Documents.PLN
{
    public class IndependentPLNConstructor : MonoBehaviour
    {
        public GameObject TextTemplate;
        public GameObject ImageTemplate;
        public List<string> Documents = new List<string>();
        public Transform Container;
        public StylingConfiguration StylingConfiguration;
        public int BaseSize=28;
        void Start()
        {
            PLNEngineCore.SetStyle(StylingConfiguration);
            PLNEngineCore.Init(TextTemplate, ImageTemplate);
            PLNEngineCore.View(Container, Documents, Color.white, BaseSize);
        }

    }

}