using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Reflection;

namespace CampaignScriptEditor.Editors.Fields
{
    public partial class StringField : UserControl, IFieldEditor
    {
        public StringField()
        {
            InitializeComponent();
        }
        FieldInfo? fi;
        public void SetField(FieldInfo fi, object? initialValue = null)
        {
            this.fi = fi;
            Header.Text = fi.Name;
            if (initialValue is not null)
                ContentBox.Text = initialValue.ToString();

        }
        public object GetObject()
        {
            return ContentBox.Text;
        }

    }
}
