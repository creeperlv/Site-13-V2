using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace CampaignScriptEditor.Editors.Fields
{
    public partial class GenericField : UserControl, IFieldEditor, IGenericField
    {
        public GenericField()
        {
            InitializeComponent();
        }

        public object GetObject()
        {
            if (TargetType is not null)
            {
                var obj=Activator.CreateInstance(TargetType);
                if(obj is not null)
                {
                    foreach (var item in fields)
                    {
                        item.Key.SetValue(obj,item.Value.GetObject());
                    }
                    return obj;
                }
            }
            return new object();
        }
        Type? TargetType=null;
        Dictionary<FieldInfo, IFieldEditor> fields = new Dictionary<FieldInfo, IFieldEditor>();
        public void SetField(FieldInfo fi, object? initialValue = null)
        {
            var t = fi.FieldType;
            TargetType = t;
            Header.Content = fi.Name;
            var f = t.GetFields();
            foreach (var item in f)
            {
                //var obj = item.GetValue(initialValue);
                var _f = FieldEditorPool.CreateField(item, initialValue);
                if (_f is not null)
                {
                    var c = _f as IControl;

                    if (c is not null)
                    {
                        fields.Add(item, _f);
                        SubFields.Children.Add(c);
                    }
                }
            }
        }

        public void SetType(Type t,object? obj=null)
        {
            Header.Content = t.Name;
            TargetType = t;
            var f = t.GetFields();
            if(obj != null)
            {

                foreach (var item in f)
                {
                    Trace.WriteLine($"Matching:{item.FieldType} to {obj.GetType()}");
                    var _f = FieldEditorPool.CreateField(item, obj);
                    if (_f is not null)
                    {
                        var c = _f as IControl;

                        if (c is not null)
                        {
                            fields.Add(item, _f);
                            SubFields.Children.Add(c);
                        }
                    }
                }
            }
            else
            {

                foreach (var item in f)
                {
                    var _f = FieldEditorPool.CreateField(item);
                    if (_f is not null)
                    {
                        var c = _f as IControl;

                        if (c is not null)
                        {
                            fields.Add(item, _f);
                            SubFields.Children.Add(c);
                        }
                    }
                }
            }
        }
    }
}
