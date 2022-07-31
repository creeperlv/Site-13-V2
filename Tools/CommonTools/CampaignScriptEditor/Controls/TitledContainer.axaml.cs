using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Metadata;

namespace CampaignScriptEditor.Controls
{
    public partial class TitledContainer : UserControl
    {
        public TitledContainer()
        {
            InitializeComponent();
        }
        private string _title = "";

        public Avalonia.Controls.Controls Children { get => ChildrenContainer.Children; }
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                TitleButton.Content = _title;
            }
        }
    }
}
