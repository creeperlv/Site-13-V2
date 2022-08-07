using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Diagnostics;
using System.Reflection;

namespace CampaignScriptEditor.Editors.Fields
{
    public partial class IntField : UserControl, IFieldEditor
    {
        public IntField()
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
            ContentBox.KeyDown += (a, b) => {
                switch (b.Key)
                {
                    case Avalonia.Input.Key.NumPad0:
                    case Avalonia.Input.Key.NumPad1:
                    case Avalonia.Input.Key.NumPad2:
                    case Avalonia.Input.Key.NumPad3:
                    case Avalonia.Input.Key.NumPad4:
                    case Avalonia.Input.Key.NumPad5:
                    case Avalonia.Input.Key.NumPad6:
                    case Avalonia.Input.Key.NumPad7:
                    case Avalonia.Input.Key.NumPad8:
                    case Avalonia.Input.Key.NumPad9:
                    case Avalonia.Input.Key.D0:
                    case Avalonia.Input.Key.D1:
                    case Avalonia.Input.Key.D2:
                    case Avalonia.Input.Key.D3:
                    case Avalonia.Input.Key.D4:
                    case Avalonia.Input.Key.D5:
                    case Avalonia.Input.Key.D6:
                    case Avalonia.Input.Key.D7:
                    case Avalonia.Input.Key.D8:
                    case Avalonia.Input.Key.D9:
                    case Avalonia.Input.Key.Decimal:

                        break;
                    default:
                        b.Handled = true;
                        break;
                }
            };
        }
        public object GetObject()
        {
            if (int.TryParse(ContentBox.Text, out var r))
            {

                return r;
            }
            else return 0f;
        }
    }
}
