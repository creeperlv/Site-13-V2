using Site13Kernel.Attributes;
using Site13Kernel.UI;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using xUI.Core.Abstraction;

namespace Site13Kernel.xUIImpl
{
    public class uUIRendererEngine : AbstractRenderEngine
    {
        IUIElement root;
        //Dictionary<Type,IUIElementImplementation>
        public RectTransform RootTransform;
        public override void CommitUITree(IUIElement element)
        {
            root = element;
            if (RootTransform == null)
            {
                Debug.Log("RootTransform is null!");
            }
            InstantiateRecursively(root, RootTransform);
            RecursivelyInitialize(root);
        }
        void RecursivelyInitialize(IUIElement element)
        {
            element.Initialize();
            if (element.Children != null)
                if (element.Children.Count > 0)
                {
                    foreach (var item in element.Children)
                    {
                        RecursivelyInitialize(item);
                    }
                }
            if (element is IContent)
            {
                if (element.Children is IUIElement child)
                    RecursivelyInitialize(child);
            }
        }
        public void InstantiateRecursively(IUIElement element, RectTransform ParentTransform)
        {
            string name = element.GetType().Name;
            string variant = element.Variant;
            if (uUIRendererResources.TryGet(name, variant, out var obj))
            {
                var _obj = GameObject.Instantiate(obj, ParentTransform);
                Debug.Log("Why?");
                uUIRendererResources.Instance.UpdateParent(_obj.transform, ParentTransform);
                var implementation = _obj.GetComponent<IUIElementImplementation>();
                element.SetIUIElementImplementation(implementation);
                if (element is IPosition pos)
                {
                    var posi = (IPositionImplementation)implementation;
                    pos.SetIPositionImplementation(posi);
                }
                if (element is ISize size)
                {
                    var sizei = (ISizeImplementation)implementation;
                    size.SetISizeImplementation(sizei);
                }
                if (element is IContent content)
                {
                    var contenti = (IContentImpl)implementation;
                    content.SetIContentImpl(contenti);
                }
                if (element is IxUITextable textable)
                {
                    var xUITextableImpl = (IxUITextableImpl)implementation;
                    textable.SetIxUITextableImpl(xUITextableImpl);
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
