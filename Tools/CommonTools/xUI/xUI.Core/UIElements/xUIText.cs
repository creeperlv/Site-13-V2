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
                _Content = text_content;
                if (IsInitialized())
                {
                    ICImpl.SetContent(text_content);
                }
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
        public override void SetProperty(string name, object value)
        {
            switch (name)
            {
                case "Text":
                    {
                        Content = value;
                    }
                    break;
                case "FontSize":
                    {
                        FontSize = int.Parse(value.ToString());
                    }
                    break;
                case "FontFamily":
                    {
                        FontFamily = value as string;
                    }
                    break;
                default:
                    break;
            }
            base.SetProperty(name, value);
        }
        IContentImpl ICImpl = null;
        public override void Initialize()
        {
            var text_content = _Content as string;
            if (textableImpl != null)
            {
                textableImpl.SetFontSize(_FS);
                textableImpl.SetFontFamily(_FontFamily);
            }
            else
            {

            }
            if (ICImpl != null)
            {
                ICImpl.SetContent(text_content);
            }
            else
            {

            }
        }
        public void SetIContentImpl(IContentImpl impl)
        {
            if (ICImpl != null) return;
            ICImpl = impl;
        }
        IxUITextableImpl textableImpl = null;
        public void SetIxUITextableImpl(IxUITextableImpl impl)
        {
            if (textableImpl != null) return;
            textableImpl = impl;
        }
    }
}
