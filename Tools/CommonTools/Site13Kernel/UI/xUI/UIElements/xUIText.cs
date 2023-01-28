using Site13Kernel.UI.xUI.Abstraction;

namespace Site13Kernel.UI.xUI.UIElements
{
    public class xUIText : UIElement, IContent
    {
        public string _Content;

        public object Content
        {
            get => _Content; 
            set
            {
                var text_content = value as string;
                if (IsInitialized())
                {
                    ICImpl.SetContent(text_content);
                }
                _Content = text_content;
            }
        }
        IContentImpl ICImpl=null;
        public void SetIContentImpl(IContentImpl impl)
        {
            if (ICImpl != null) return;
            ICImpl = impl;
        }
    }
}
