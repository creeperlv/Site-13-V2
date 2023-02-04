using Site13Kernel.Attributes;
using System;
using System.Text;
using UnityEngine;
using xUI.Core.Abstraction;

namespace Site13Kernel.UI.xUI.uUIImplementation
{
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
        public void InstantiateRecursively(IUIElement element, Transform ParentTransform)
        {
            string name = element.GetType().Name;
            string variant = element.Variant;
            if (uUIRendererResources.TryGet(name, variant, out var obj))
            {
                var _obj = GameObject.Instantiate(obj, ParentTransform);
                var implementation = _obj.GetComponent<IUIElementImplementation>();
                element.SetIUIElementImplementation(implementation);
                if (element is IPosition pos)
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
