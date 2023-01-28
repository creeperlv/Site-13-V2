using Site13Kernel.Attributes;
using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.UI.xUI.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Site13Kernel.UI.xUI.uUIImplementation
{
    public class uUIRendererResources : ControlledBehavior
    {
        public static uUIRendererResources Instance=null;
        public KVList<string, GameObject> Primitives = new KVList<string, GameObject>();
        public Dictionary<string, GameObject> PrimitiveDictionary;
        public override void Init()
        {
            PrimitiveDictionary = Primitives.ObtainMap();
            Primitives.PrefabDefinitions.Clear();
            Primitives = null;
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
    public class uUIRendererHolder : ControlledBehavior
    {
        
    }
    public class uUIRendererEngine : AbstractRenderEngine
    {
        IUIElement root;
        //Dictionary<Type,IUIElementImplementation>
        public Transform RootTransform;
        public override void CommitUITree(IUIElement element)
        {
            root = element;
            InstantiateRecursively(root, RootTransform);
        }
        public void InstantiateRecursively(IUIElement element,Transform ParentTransform)
        {
            string name = element.GetType().Name;
            if (uUIRendererResources.TryGet(name, out var obj))
            {
                var _obj=GameObject.Instantiate(obj, ParentTransform);
                var implementation=_obj.GetComponent<IUIElementImplementation>();
                element.SetIUIElementImplementation(implementation);
                if(element is IPosition pos)
                {
                    var posi = (IPositionImplementation)implementation;
                    pos.SetIPositionImplementation(posi);
                }
            }
        }

        [TODO]
        public override void AttachUITree(IUIElement Parent, IUIElement Children)
        {
        }
        [TODO]
        public override void RemoveUITree(IUIElement element)
        {
        }
    }

}
