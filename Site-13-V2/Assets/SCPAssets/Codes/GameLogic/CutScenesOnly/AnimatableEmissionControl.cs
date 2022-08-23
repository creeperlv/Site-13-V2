using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Site13Kernel.GameLogic.CutScenesOnly
{
    public class AnimatableEmissionControl : MonoBehaviour
    {
        public bool AutoSearchRenderers;
        public Transform SearchRoot;
        public List<GenericControllableRenderer> FoundRenderers = new List<GenericControllableRenderer>();
        public float Intensity;
        public float _Intensity;
        public Color EmissionColor;
        public Color _EmissionColor;
        public bool OnlyUseIntensity;
        public string KeyWord;
        private void Start()
        {
            if (AutoSearchRenderers)
            {
                FoundRenderers = SearchRoot.GetComponentsInChildren<GenericControllableRenderer>().ToList();
            }
        }
        private void Update()
        {
            if (_Intensity != Intensity || _EmissionColor != EmissionColor)
            {
                foreach (var item in FoundRenderers)
                {
                    if (OnlyUseIntensity)
                        item.SetFloat(KeyWord, Intensity);
                    else item.SetColor(KeyWord, EmissionColor * Intensity);
                }
                _Intensity = Intensity;
                _EmissionColor = EmissionColor;
            }
        }
    }
}
