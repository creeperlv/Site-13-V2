using System.Collections.Generic;
using xUI.Core.Abstraction;
using xUI.Core.Attributes;

namespace xUI.Core.UIElements
{
    public class xUIGrid : UIElement, IxUIContainer, IBackground
    {
        UIElement _Background;
        List<IUIElement> _Children = new List<IUIElement>();
        public override List<IUIElement> Children { get => _Children; set => _Children = value; }
        public UIElement Background
        {
            get => _Background;
            set
            {
                if (value == null)
                {
                    if (IsInitialized())
                    {
                        AbstractRenderEngine.CurrentEngine.RemoveUITree(_Background);
                    }
                }
                _Background = value;
            }
        }

        public void Add(object content)
        {
            if (_Children == null) _Children = new List<IUIElement>();
            if (content is IUIElement element)
            {
                _Children.Add(element);
            }
        }

        public void Remove(object content)
        {
            if (IsInitialized())
            {
                // Remove From Original Tree.
            }
            _Children.Remove((UIElement)content);
        }
        IBackgroundImpl bgimpl = null;
        public void SetIBackgroundImpl(IBackgroundImpl impl)
        {
            if (bgimpl != null) return;
            bgimpl = impl;
        }
        [TODO]
        public void OrganizeLayout()
        {

        }
    }
}
