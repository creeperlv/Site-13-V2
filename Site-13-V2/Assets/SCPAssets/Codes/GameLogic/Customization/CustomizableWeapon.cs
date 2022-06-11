using Site13Kernel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.Customization
{
    public class CustomizableWeapon : MonoBehaviour
    {
        public string TargetWeaponCoating = "DEFAULT";
        public string WeaponID;
        public KVList<int, CustomizableMeshRenderer> ControlledCustomizableMeshRenders;
        Dictionary<int, CustomizableMeshRenderer> customizableMeshRenderers = null;
        public void ApplyCoating()
        {
            if (customizableMeshRenderers == null)
            {
                customizableMeshRenderers = ControlledCustomizableMeshRenders.ObtainMap();
            }
            var palette = WeaponCoatingPalettes.Find(WeaponID, TargetWeaponCoating);
            if (palette != null)
            {
                foreach (var item in customizableMeshRenderers)
                {
                    var M = item.Value.ControlledRenderer.materials;
                    foreach (var mat in item.Value.MaterialMap.ObtainMap())
                    {
                        if (palette._MaterialMap.TryGetValue(mat.Value, out var v))
                        {
                            M[mat.Key] = v;
                        }
                    }
                    item.Value.ControlledRenderer.materials = M;
                }
            }
            else
            {
            }
        }
    }
}
