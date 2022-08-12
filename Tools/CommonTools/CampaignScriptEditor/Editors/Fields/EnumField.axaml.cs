using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Reflection;

namespace CampaignScriptEditor.Editors.Fields
{
    public partial class EnumField : UserControl, IFieldEditor,IGenericField
    {
        public EnumField()
        {
            InitializeComponent();
            EnumBox.PointerPressed += (a, b) => { b.Handled = true; };
        }
        public object GetObject()
        {
            return Enum.Parse(tt!, EnumBox.SelectedItem!.ToString()!);
        }
        Type? tt;
        public void SetField(FieldInfo fi, object? initialValue = null)
        {
            tt = fi.FieldType;
            Header.Text = fi.Name;
            var n = fi.FieldType.GetEnumNames();
            EnumBox.Items = n;
            if (initialValue is not null)
            {
                var str = initialValue.ToString();
                bool Hit = false;
                for (int i = 0; i < n.Length; i++)
                {
                    if (n[i] == str)
                    {
                        EnumBox.SelectedIndex = i;
                        Hit = true;
                        break;
                    }
                }
                if (!Hit)
                {
                    EnumBox.SelectedIndex = 0;
                }
            }
            else
            {
                EnumBox.SelectedIndex = 0;
            }
        }

        public void SetType(Type t,object? initialValue = null)
        {
            tt = t;
            Header.Text = t.Name;
            var n = t.GetEnumNames();
            EnumBox.Items = n; 
            if (initialValue is not null)
            {
                var str = initialValue.ToString();
                bool Hit = false;
                for (int i = 0; i < n.Length; i++)
                {
                    if (n[i] == str)
                    {
                        EnumBox.SelectedIndex = i;
                        Hit = true;
                        break;
                    }
                }
                if (!Hit)
                {
                    EnumBox.SelectedIndex = 0;
                }
            }
            else
            {
                EnumBox.SelectedIndex = 0;
            }
        }
    }
}
