using Site13Kernel.Data;
using Site13Kernel.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.Rendering
{
    [Serializable]
    public class RemappableMaterial : MonoBehaviour
    {
        public Material ControlledMaterial;
        public List<KVPair<string, string>> TextureMap = new List<KVPair<string, string>>();
        public Dictionary<string, string> _TextureMap = new Dictionary<string, string>();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Map()
        {
            _TextureMap = CollectionUtilities.ToDictionary(TextureMap);
            foreach (var item in _TextureMap)
            {
                var t=TextureList.Get(item.Value);
                if (t != null)
                {
                    Debug.Log("Set A Texture");
                    ControlledMaterial.SetTexture(item.Key, t);
                }else 
                    Debug.Log("Texture Fail:"+item.Key+"<->"+item.Value);
            }
        }
    }
}
