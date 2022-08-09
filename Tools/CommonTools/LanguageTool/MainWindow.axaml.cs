using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using CommonTools;
using Markdown.Avalonia;
using System.Diagnostics;

namespace LanguageTool
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }
        void Init()
        {
            MarkdownScrollViewer markdownScrollViewer=new MarkdownScrollViewer();
            markdownScrollViewer.Markdown =LibInfo.GetAboutString("Language Tool");
            AboutButton.Click += (_, _) => {
                Dialog dialog=new Dialog();
                dialog.Height = 400;
                dialog._DialogTitle = "About";
                dialog.DialogContent = markdownScrollViewer;
                dialog.isCancelEnabled=false;
                dialog.ShowDialog(this);
            };
            AddFieldButton.Click += (_, _) => {
                CentralEditor.Children.Add(new Field());
            };
            Load();
        }
        void Load()
        {
        }
        //StackPanel CentralEditor;
        //Button AboutButton;
        //Button AddFieldButton;
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            this.ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.PreferSystemChrome;
            this.ExtendClientAreaToDecorationsHint = true;
            this.TransparencyLevelHint = WindowTransparencyLevel.Blur;
            this.TransparencyBackgroundFallback=new SolidColorBrush(Color.FromArgb(255,0,0,0));
            Trace.WriteLine(this.ActualTransparencyLevel);
            this.Background = new SolidColorBrush(Colors.Transparent);
            //CentralEditor=this.FindControl<StackPanel>("CentralEditor");
            //AboutButton = this.FindControl<Button>("AboutButton");
            //AddFieldButton = this.FindControl<Button>("AddFieldButton");
            Init();
            //this.SystemDecorations = SystemDecorations.BorderOnly;
        }
    }
}
