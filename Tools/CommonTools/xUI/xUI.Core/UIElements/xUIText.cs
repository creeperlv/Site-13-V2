using Newtonsoft.Json.Linq;
using xUI.Core.Abstraction;

namespace xUI.Core.UIElements
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
        IContentImpl ICImpl = null;
        public override void Initialize()
        {
            var text_content = _Content as string;
            ICImpl.SetContent(text_content);
        }
        public void SetIContentImpl(IContentImpl impl)
        {
            if (ICImpl != null) return;
            ICImpl = impl;
        }
    }
}
