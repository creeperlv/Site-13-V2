using Site13Kernel.Core;
using Site13Kernel.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Site13Kernel.UI.xUI.uUIImplementation
{
    public class uUIRendererResources : ControlledBehavior
    {
        public static uUIRendererResources Instance = null;
        public KVList<string, KVList<string, GameObject>> Primitives = new KVList<string, KVList<string, GameObject>>();
        public Dictionary<string, Dictionary<string, GameObject>> PrimitiveDictionary;
        public KVList<string, Font> Fonts = new KVList<string, Font>();
        public ResourceKey RootKey;
        public Dictionary<string, Font> FontsDictionary;
        public override void Init()
        {
            var _d = Primitives.ObtainMap();
            PrimitiveDictionary = new Dictionary<string, Dictionary<string, GameObject>>();
            foreach (var item in _d)
            {
                PrimitiveDictionary.Add(item.Key, item.Value.ObtainMap());
            }
            Primitives.PrefabDefinitions.Clear();
            FontsDictionary = Fonts.ObtainMap();
            Fonts.PrefabDefinitions.Clear();
            Primitives = null;
        }
        public ResourceItem QueryResource(string url)
        {
            Uri uri = new Uri(url);

            switch (uri.Scheme)
            {
                case "site13":
                    {
                        var path=uri.AbsolutePath;
                        
                    }
                    break;
                default:
                    break;
            }
            return null;
        }
        public static bool TryGetFont(string name, out Font font)
        {
            if (Instance == null)
            {
                font = null;
                return false;
            }
            return Instance.FontsDictionary.TryGetValue(name, out font);
        }
        public static bool TryGet(string name, string variant, out GameObject gameObject)
        {
            if (Instance == null)
            {
                gameObject = null;
                return false;
            }
            if (Instance.PrimitiveDictionary.TryGetValue(name, out var L))
            {
                if (L.Count > 0)
                {
                    if (variant == null)
                    {
                        gameObject = L.Values.ElementAt(0);
                        return true;
                    }
                    if (L.TryGetValue(variant, out gameObject))
                    {
                        return true;
                    }

                    gameObject = L.Values.ElementAt(0);
                    return true;
                }
            }
            gameObject = null;
            return false;
        }
    }
    [Serializable]
    public class ResourceItem
    {
        public ResourceItemType ResourceType;
        public Sprite Image;
    }
    public enum ResourceItemType
    {
        Sprite,
    }
    [Serializable]
    public class ResourceKey
    {
        public KVList<string, ResourceKey> ResKeys;
        public KVList<string, ResourceItem> ResItems;
        public Dictionary<string, ResourceKey> SubKeys;
        public Dictionary<string, ResourceItem> Items;
        public void Init()
        {
            SubKeys = ResKeys.ObtainMap();
            Items = ResItems.ObtainMap();
        }
    }
}
