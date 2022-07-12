using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using CampaignScriptEditor.Editors.Fields;
using Site13Kernel.GameLogic.Directors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace CampaignScriptEditor.Editors
{
    public partial class EventItem : UserControl
    {
        static Random r = new Random();
        public MainWindow? ParentContainer;
        public EventItem()
        {
            InitializeComponent();
            Up.Click += (_, _) =>
            {
                var sp = (this.Parent as StackPanel);
                if (sp is not null)
                {
                    var i = sp.Children.IndexOf(this);
                    if (i > 0)
                    {
                        sp.Children.RemoveAt(i);
                        sp.Children.Insert(i - 1, this);
                    }
                }
            };
            Copy.Click += (_, _) =>
            {
                if(ParentContainer is not null)
                {
                    ParentContainer.AddItem(ObtainObject());
                }
            };
            Down.Click += (_, _) =>
            {
                var sp = (this.Parent as StackPanel);
                if (sp is not null)
                {
                    var i = sp.Children.IndexOf(this);
                    if (i < sp.Children.Count - 1)
                    {
                        sp.Children.RemoveAt(i);
                        sp.Children.Insert(i + 1, this);
                    }
                }
            };
            Delete.Click += (_, _) => { (this.Parent as StackPanel)!.Children.Remove(this); };
        }
        public Object? Event;
        static Type StringType = typeof(string);
        static Type FloatType = typeof(float);
        static Type BoolType = typeof(bool);
        public object ObtainObject()
        {
            if (TargetT is not null)
            {
                var obj = Activator.CreateInstance(TargetT);
                if (obj is not null)
                {
                    foreach (var item in _fields)
                    {
                        item.Key.SetValue(obj, item.Value.GetObject());
                    }
                    return obj;
                }
            }
            return new object();
        }
        public Dictionary<FieldInfo, IFieldEditor> _fields = new Dictionary<FieldInfo, IFieldEditor>();
        public void InitObject(object obj)
        {
            Type t = obj.GetType();
            TargetT = t;
            if (t.IsAssignableTo(MainWindow.EventBaseType))
            {
                ColorIndicator.Background = new SolidColorBrush(Color.FromRgb((byte)r.Next(0, 255), (byte)r.Next(0, 255), (byte)r.Next(0, 255)));
                EditorToggle.Content = t.Name;
                Event = obj;
                List<string> ExistedFields = new List<string>();
                {
                    var fields = MainWindow.EventBaseType.GetFields();
                    foreach (var item in fields)
                    {
                        if (ExistedFields.Contains(item.Name)) continue;
                        var editor = FieldEditorPool.CreateField(item, Event);
                        if (editor is not null)
                        {
                            FieldContainer.Children.Add(editor as UserControl);
                            _fields.Add(item, editor);
                            ExistedFields.Add(item.Name);
                        }
                    }
                }
                {
                    var fields = t.GetFields();

                    foreach (var item in fields)
                    {
                        if (ExistedFields.Contains(item.Name)) continue;
                        var editor = FieldEditorPool.CreateField(item, Event);
                        if (editor is not null)
                        {
                            FieldContainer.Children.Add(editor as IControl);
                            _fields.Add(item, editor);
                            ExistedFields.Add(item.Name);
                        }
                        else
                        {
                        }
                    }
                }
            }
        }
        Type? TargetT;
        public void InitType(Type t)
        {
            TargetT = t;
            if (t.IsAssignableTo(MainWindow.EventBaseType))
            {
                ColorIndicator.Background = new SolidColorBrush(Color.FromRgb((byte)r.Next(0, 255), (byte)r.Next(0, 255), (byte)r.Next(0, 255)));
                EditorToggle.Content = t.Name;
                Event = Activator.CreateInstance(t);
                List<string> ExistedFields = new List<string>();
                {
                    var fields = MainWindow.EventBaseType.GetFields();
                    foreach (var item in fields)
                    {
                        if (ExistedFields.Contains(item.Name)) continue;
                        var editor = FieldEditorPool.CreateField(item, Event);
                        if (editor is not null)
                        {
                            FieldContainer.Children.Add(editor as UserControl);
                            _fields.Add(item, editor);
                            ExistedFields.Add(item.Name);
                        }
                    }
                }
                {
                    var fields = t.GetFields();

                    foreach (var item in fields)
                    {
                        if (ExistedFields.Contains(item.Name)) continue;
                        var editor = FieldEditorPool.CreateField(item, Event);
                        if (editor is not null)
                        {
                            FieldContainer.Children.Add(editor as IControl);
                            _fields.Add(item, editor);
                            ExistedFields.Add(item.Name);
                        }
                        else
                        {
                        }
                    }
                }
            }
        }
    }
}
