using Newtonsoft.Json.Linq;
using xUI.Core.Abstraction;

namespace xUI.Core.UIElements
{
    public class xUIText : UIElement, IContent, IxUITextable
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
        int _FS = 12;
        public int FontSize
        {
            get => _FS;
            set
            {
                if (IsInitialized())
                {
                    textableImpl.SetFontSize(value);
                }
                _FS = value;
            }
        }
        string _FontFamily;
        public string FontFamily
        {
            get => _FontFamily; set
            {
                if (IsInitialized())
                {
                    textableImpl.SetFontFamily(value);
                }
                _FontFamily = value;
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
        IxUITextableImpl textableImpl;
        public void SetIxUITextableImpl(IxUITextableImpl impl)
        {
            textableImpl = impl;
        }
    }
}
