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
            markdownScrollViewer.Markdown = $"# Site-13 Tool Set\n\n## Language Tool\n\n- Version:{typeof(MainWindow).Assembly.GetName().Version}\n\n" +
                $"## Third-Party Libs\n\n- AvaloniaUI\n\n- Markdown.Avalonia";
            AboutButton.Click += (_, _) => {
                Dialog dialog=new Dialog();
                dialog.DialogTitle = "About";
                dialog.DialogContent = markdownScrollViewer;
                dialog.isCancelEnabled=false;
                dialog.ShowDialog(this);
            };
            Load();
        }
        void Load()
        {
            CentralEditor.Children.Add(new Field());
            CentralEditor.Children.Add(new Field());
            CentralEditor.Children.Add(new Field());
        }
        StackPanel CentralEditor;
        Button AboutButton;
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            this.ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.PreferSystemChrome;
            this.ExtendClientAreaToDecorationsHint = true;
            this.TransparencyLevelHint = WindowTransparencyLevel.Blur;
            Trace.WriteLine(this.ActualTransparencyLevel);
            this.Background = new SolidColorBrush(Colors.Transparent);
            CentralEditor=this.FindControl<StackPanel>("CentralEditor");
            AboutButton = this.FindControl<Button>("AboutButton");
            Init();
            //this.SystemDecorations = SystemDecorations.BorderOnly;
        }
    }
}
