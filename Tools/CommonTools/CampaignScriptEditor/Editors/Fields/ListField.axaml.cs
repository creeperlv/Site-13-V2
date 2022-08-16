using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Site13Kernel.Data.Serializables;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace CampaignScriptEditor.Editors.Fields
{
    public partial class ListField : UserControl, IFieldEditor, IGenericField
    {
        public ListField()
        {
            InitializeComponent();
        }

        public object GetObject()
        {
            if (Primitive is not null)
            {
                var obj = Activator.CreateInstance(Primitive);
                Trace.WriteLine(Primitive);
                Trace.WriteLine(obj.GetType());
                foreach (var item in Items.Children)
                {
                    if (item is Grid g)
                    {
                        if (g.Children[0] is IFieldEditor fe)
                        {
                            var add = Primitive.GetMethod("Add");
                            if (add is not null)
                                add.Invoke(obj, new object[] { fe.GetObject() });
                        }
                    }
                }
                return obj;
            }
            return new object();
        }
        Type? Primitive;
        public void SetField(FieldInfo fi, object? initialValue = null)
        {
            Header.Text = fi.Name;
            Primitive = fi.FieldType;
            var t = fi.FieldType.GenericTypeArguments[0];
            AddButton.Click += (_, _) =>
            {
                var control = FieldEditorPool.CreateField(t) as IControl;
                if (control is not null)
                {
                    AddControl(control);
                }
            };
            {
                if(initialValue is not null)
                {
                    LoadList(initialValue);
                }
            }
            //fi.FieldType.IsAssignableTo(t);
        }
        static void PrintMethods(Type t)
        {
        }
        void LoadList(Object obj)
        {
            var P = Primitive!.
                GetRuntimeMethods();
            foreach (var item in P)
            {
                Trace.WriteLine(item.ToString());
            }
            var M = Primitive!.GetMethods();
            foreach (var item in M)
            {
                Trace.WriteLine(item.ToString());
            }
            var L=Primitive!.GetProperty("Count")!.GetValue(obj);
            Trace.WriteLine(L);
            if(L is int Count)
            {
                var E=typeof(System.Linq.Enumerable);
                List<SerializableRoutineStep> LS;
                //LS.ElementAt(9);
                var mths = E.GetMethods();// !.MakeGenericMethod(Primitive.GenericTypeArguments[0]);
                MethodInfo? ElementAt=null;
                foreach (var item in mths)
                {
                    if (item.Name.StartsWith("ElementAt"))
                    {
                        foreach (var __p in item.GetParameters())
                        {
                            if(__p.ParameterType == typeof(int))
                            {
                                ElementAt = item.MakeGenericMethod(Primitive.GenericTypeArguments[0]);
                            }
                        }
                    }
                }
                for(int i = 0; i < Count; i++)
                {
                    var _obj = ElementAt.Invoke(null, new object[] {obj, i });
                    if(_obj is not null)
                    {
                        var control = FieldEditorPool.CreateField(_obj.GetType(), _obj) as IControl;
                        if (control is not null)
                        {
                            AddControl(control);
                        }
                    }
                }
            }
        }
        public void SetType(Type t,object? value=null)
        {
            Header.Text = $"List<{t.GenericTypeArguments[0]}>";
            Primitive = t;
            var _t = t.GenericTypeArguments[0];
            AddButton.Click += (_, _) =>
            {
                var control = FieldEditorPool.CreateField(_t) as IControl;
                if (control is not null)
                {
                    AddControl(control);
                }
            };
            if (value is not null)
            {
                LoadList(value);
            }
        }
        void AddControl(IControl control)
        {
            Grid g = new Grid();
            g.ColumnDefinitions = new ColumnDefinitions { new ColumnDefinition(), new ColumnDefinition { Width = GridLength.Auto } };
            Button button = new Button();
            button.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            button.Foreground = new SolidColorBrush(Color.FromRgb(240, 100, 100));
            button.Content = "-";
            button.Click += (_, _) => { Items.Children.Remove(g); };
            g.Children.Add(control);
            g.Children.Add(button);
            Grid.SetColumn(button, 1);
            Items.Children.Add(g);
        }
    }

}
