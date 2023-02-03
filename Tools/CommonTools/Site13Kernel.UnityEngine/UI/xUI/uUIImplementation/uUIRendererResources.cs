using Site13Kernel.Core;
using Site13Kernel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.UI.xUI.uUIImplementation
{
    public class uUIRendererResources : ControlledBehavior
    {
        public static uUIRendererResources Instance=null;
        public KVList<string, KVList<string, GameObject>> Primitives = new KVList<string, KVList<string, GameObject>>();
        public Dictionary<string, Dictionary<string,GameObject>> PrimitiveDictionary;
        public KVList<string,Font> Fonts = new KVList<string, Font>();
        
        public Dictionary<string, Font> FontsDictionary;
        public override void Init()
        {
            var _d = Primitives.ObtainMap();
            PrimitiveDictionary = new Dictionary<string, Dictionary<string, GameObject>>();
            foreach (var item in _d)
            {
                PrimitiveDictionary.Add(item.Key,item.Value.ObtainMap());
            }
            Primitives.PrefabDefinitions.Clear();
            FontsDictionary=Fonts.ObtainMap();
            Fonts.PrefabDefinitions.Clear();
            Primitives = null;
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
        public static bool TryGet(string name, out GameObject gameObject)
        {
            if (Instance == null)
            {
                gameObject = null;
                return false;
            }
            return Instance.PrimitiveDictionary.TryGetValue(name, out gameObject);
        }
    }
}
