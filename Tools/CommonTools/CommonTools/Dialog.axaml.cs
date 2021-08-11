using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;
using System;

namespace CommonTools
{
    public partial class Dialog : Window
    {
        public Dialog()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }
        public Action OKAction;
        public Action CancelAction;
        public IControl DialogContent
        {
            get
            {
                return ContentPresenter.Children[0];
            }
            set
            {
                if (ContentPresenter.Children.Count == 1)
                    ContentPresenter.Children[0] = value;
                else
                    ContentPresenter.Children.Add(value);
            }
        }
        public string DialogTitle
        {
            get
            {
                return _DialogTitle.Text;
            }
            set
            {
                _DialogTitle.Text = value;
            }
        }
        public bool isOKEnabled
        {
            get
            {
                return OKButton.IsVisible;
            }
            set
            {
                OKButton.IsVisible = value;
            }
        }
        public bool isCancelEnabled
        {
            get
            {
                return CancelButton.IsVisible;
            }
            set
            {
                CancelButton.IsVisible = value;
            }
        }
        Button CancelButton;
        Button OKButton;
        Grid ContentPresenter;
        TextBlock _DialogTitle;
        void Init()
        {
            CancelButton.Click += (_, _) =>
            {
                this.Close();
                if (CancelAction is not null)
                    CancelAction();
            };
            OKButton.Click += (_, _) =>
            {
                this.Close();
                if (OKAction is not null)
                    OKAction();
            };
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            this.ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.PreferSystemChrome;
            this.ExtendClientAreaToDecorationsHint = true;
            //this.TransparencyLevelHint = WindowTransparencyLevel.Blur;
            this.SystemDecorations = SystemDecorations.BorderOnly;
            CancelButton = this.FindControl<Button>("CancelButton");
            OKButton = this.FindControl<Button>("OKButton");
            ContentPresenter = this.FindControl<Grid>("ContentPresenter");
            _DialogTitle = this.FindControl<TextBlock>("DialogTitle");
            this.Closing += (_,_) => {
                ContentPresenter.Children.Clear();
            };
            Init();
        }
    }
}
