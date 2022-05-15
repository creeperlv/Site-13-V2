using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Reflection;

namespace CampaignScriptEditor.Editors.Fields
{
    public partial class BoolField : UserControl,IFieldEditor
    {
        public BoolField()
        {
            InitializeComponent();
        }

        public object GetObject()
        {
            return BoolControl.IsChecked!;
        }

        public void SetField(FieldInfo fi, object? initialValue = null)
        {
            BoolControl.Content = fi.Name;
            if(initialValue is bool b)
            {
                BoolControl.IsChecked = b;
            }
        }
    }
}
