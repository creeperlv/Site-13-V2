using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Site13Kernel.Data;
using System;
using System.Runtime.InteropServices;

namespace CommonTools
{
    public partial class AboutDialog : Window
    {
        public AboutDialog()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            InitStyles();
        }
        public void SetInfo(string ToolTitle, Version ToolVersion, params string[] addiationalLibs)
        {
            MainVersion.Text = typeof(Weapon).Assembly.GetName().Version + "";
            this.ToolVersion.Text = ToolVersion + "";
            this.ToolName.Text = ToolTitle + "";
            AdditionalLibs.Children.Clear();
            foreach (var item in addiationalLibs)
            {
                AdditionalLibs.Children.Add(new TextBlock() { Text = item });
            }
        }
        public void InitStyles()
        {
            ExtendClientAreaToDecorationsHint = true;
            ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.PreferSystemChrome;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (Environment.OSVersion.Version.Build >= 22000)
                {
                    TransparencyLevelHint = WindowTransparencyLevel.Mica;
                }
                else
                if (Environment.OSVersion.Version.Build >= 14393)
                {
                    TransparencyLevelHint = WindowTransparencyLevel.AcrylicBlur;
                }
            }
        }
    }
}
