using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Reflection;

namespace CampaignScriptEditor.Editors.Fields
{
    public partial class KVPairField : UserControl, IFieldEditor, IGenericField
    {
        public KVPairField()
        {
            InitializeComponent();
        }
        IFieldEditor? KFE;
        IFieldEditor? VFE;
        public object GetObject()
        {
            if (TargetT is not null)
            {
                var obj = Activator.CreateInstance(TargetT);
                if (obj is not null)
                {
                    return obj;
                }
            }
            return new object();
        }
        Type? TargetT;
        public void SetField(FieldInfo fi, object? initialValue = null)
        {
            TargetT = fi.FieldType;
            Header.Text = fi.Name;
            var GT = TargetT.GenericTypeArguments;
            {
                var __v = FindValue("Key", initialValue);
                var fe = FieldEditorPool.CreateField(GT[0],__v);
                if (fe is not null)
                {
                    if (fe is IControl c)
                    {
                        KFE = fe;
                        K.Children.Add(c);
                    }
                }
            }
            {
                var __v = FindValue("Value", initialValue);
                var fe = FieldEditorPool.CreateField(GT[1],__v);
                if (fe is not null)
                {
                    if (fe is IControl c)
                    {
                        VFE = fe;
                        V.Children.Add(c);
                    }
                }
            }
        }
        object? FindValue(string field,object? obj)
        {
            if (obj is not null)
                return TargetT!.GetField(field)!.GetValue(obj);
            else return null;
        }
        public void SetType(Type t,object? initValue=null)
        {
            TargetT = t;
            var GT = t.GenericTypeArguments;
            Header.Text = $"KVPair<{GT[0]},{GT[1]}>";
            {
                var __v = FindValue("Key",initValue);
                var fe = FieldEditorPool.CreateField(GT[0], __v);
                if (fe is not null)
                {
                    if (fe is IControl c)
                    {
                        KFE = fe;
                        K.Children.Add(c);
                    }
                }
            }
            {
                var __v = FindValue("Value", initValue);
                var fe = FieldEditorPool.CreateField(GT[1], __v);
                if (fe is not null)
                {
                    if (fe is IControl c)
                    {
                        VFE = fe;
                        V.Children.Add(c);
                    }
                }
            }
        }

    }
}
