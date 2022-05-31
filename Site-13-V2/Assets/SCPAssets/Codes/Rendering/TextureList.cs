using Site13Kernel.Data;
using Site13Kernel.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel
{
    [Serializable]
    public class TextureList
    {
        public static TextureList CurrentList = null;
        public List<KVPair<string, Texture2D>> SerializableMap = new List<KVPair<string, Texture2D>>();
        public Dictionary<string, Texture2D> TextureMap = new Dictionary<string, Texture2D>();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void __init()
        {
            TextureMap = CollectionUtilities.ToDictionary(SerializableMap);
        }
        public static Texture2D Get(string Name)
        {
            if (CurrentList != null)
            {
                if(CurrentList.TextureMap.TryGetValue(Name, out Texture2D tex))
                {
                    return tex;
                }
            }
            return null;
        }
    }
}
