using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using xUI.Core;
using xUI.Core.Abstraction;
using xUI.Core.UIElements;

namespace Site13Kernel.xUIImpl
{
    public class xUIElementImplBase : UIBehaviour, IUIElementImplementation
    {
        public virtual void Bind(IUIElement element)
        {
            
        }

        public virtual void Repaint()
        {
            
        }

        public virtual void SetHit(bool IsEnabled)
        {
            
        }

        public virtual void SetIsEnable(bool State)
        {
            
        }

        public virtual void SetStyles(List<string> styles)
        {
            
        }
    }
    public class xUIBase : MonoBehaviour
    {
        public TextAsset xUIFile;
        public virtual void Start()
        {

        }
        UIElement _Root;
        public void Initialize()
        {
            _Root = UIComposer.Parse(xUIFile.text);
            AbstractRenderEngine.CurrentEngine.CommitUITree(_Root);
        }
        public UIElement FindControl(string Name)
        {
            return RecursiveFind(_Root, Name);
        }
        UIElement RecursiveFind(UIElement item, string Name)
        {
            if (item.Name == Name) return item;
            if (item is IContent ic)
            {
                if (ic.Content is UIElement element)
                {
                    var _i = RecursiveFind(element, Name);
                    if (_i != null) return _i;

                }
            }
            if (item is IxUIContainer)
            {
                foreach (UIElement _item in item.Children)
                {
                    var _i = RecursiveFind(_item, Name);
                    if (_i != null) return _i;
                }
            }
            return null;
        }
    }

}
