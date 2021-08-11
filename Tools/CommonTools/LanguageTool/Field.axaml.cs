using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace LanguageTool
{
    public partial class Field : UserControl
    {
        public Field()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
