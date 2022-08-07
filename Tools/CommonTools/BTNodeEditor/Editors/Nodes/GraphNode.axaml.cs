using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using BTNodeEditor.Editors.Controls;
using CampaignScriptEditor;
using CampaignScriptEditor.Editors.Fields;
using Site13Kernel.GameLogic.BT.Nodes;
using Site13Kernel.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace BTNodeEditor.Editors.Nodes
{
    public partial class GraphNode : UserControl
    {
        public NodeGraphEditor? ParentEditor;
        public List<NodeConnection> L = new();
        public List<NodeConnection> R = new();
        bool isPressed;
        Point __p;
        internal SerializableNode? SN;
        public static GraphNode FromSerializableNode(SerializableNode SN, NodeGraphEditor editor)
        {
            GraphNode node = new GraphNode(editor);
            node.SN = SN;
            node.InitObject(SN.Contained!);
            if (SN.DisableDeletion) node.DeleteNode.IsEnabled = false;
            if (SN.DisableDuplicate) node.DuplicateNode.IsEnabled = false;
            if (!SN.HaveR) node.RRect.IsVisible = false;
            if (!SN.HaveL) node.LRect.IsVisible = false;
            return node;
        }
        public SerializableNode Serialize()
        {
            if (SN is null)
            {
                SN = new SerializableNode();
                SN.ID = Guid.NewGuid().ToString();
            }
            SN.X = this.Margin.Left;
            SN.Y = this.Margin.Top;
            SN.Contained = (BTBaseNode)ObtainObject();
            return SN;
        }
        public void ApplyEditor()
        {

        }
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
        Object? Event;
        Type? TargetT;
        public void InitObject(object obj)
        {
            Type t = obj.GetType();
            TargetT = t;
            if (t.IsAssignableTo(MainWindow.BTBaseNodeType))
            {
                TitleButton.Content = t.Name;
                Event = obj;
                List<string> ExistedFields = new List<string>();
                {
                    var fields = MainWindow.BTBaseNodeType.GetFields();
                    foreach (var item in fields)
                    {
                        if (ExistedFields.Contains(item.Name)) continue;
                        if (item.FieldType.IsAssignableFrom(MainWindow.BTBaseNodeType)) continue;
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
                        if (item.FieldType.IsAssignableFrom(MainWindow.BTBaseNodeType)) continue;
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
        public void InitType(Type t)
        {
            TargetT = t;
            if (t.IsAssignableTo(MainWindow.BTBaseNodeType))
            {
                Event = Activator.CreateInstance(t);
                List<string> ExistedFields = new List<string>();
                {
                    var fields = MainWindow.BTBaseNodeType.GetFields();
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
        public void AddL(NodeConnection connection)
        {
            L.Add(connection);
            connection.R = this;
        }
        public void RemoveL(NodeConnection connection)
        {
            if (L.Contains(connection))
                L.Remove(connection);
        }
        public void RemoveR(NodeConnection connection)
        {
            if (R.Contains(connection))
                R.Remove(connection);
        }
        public void AddR(NodeConnection connection)
        {
            R.Add(connection);
            connection.L = this;
        }
        int i = 0;
        static SolidColorBrush BG0 = new(Color.FromArgb(0xFF, 0x33, 0x33, 0x33));
        static SolidColorBrush BG1 = new(Color.FromArgb(0x88, 0x33, 0x33, 0x33));
        public GraphNode(NodeGraphEditor editor)
        {
            InitializeComponent();
            ParentEditor = editor;
            InitEvents();
        }
        void InitEvents()
        {
            Flyout flyout = new Flyout();
            FlyoutCodeViewer codeViewer = new FlyoutCodeViewer();
            flyout.Content = codeViewer;
            codeViewer.CloseBtn.Click += (_, _) => { flyout.Hide(); };
            ViewSource.Click += (_, _) =>
            {
                codeViewer.CodePresenter.Text = JsonUtilities.Serialize(Serialize());
                flyout.ShowAt(NodeBorder);
            };
            DuplicateNode.Click += (_, _) =>
            {
                if (ParentEditor is not null)
                {
                    var __SN = (SerializableNode)Serialize().Duplicate();
                    __SN.X += 50;
                    __SN.Y += 50;
                    __SN.ID = Guid.NewGuid().ToString();
                    ParentEditor.AddNode(__SN);
                }
            };
            DeleteNode.Click += (_, _) =>
            {
                if (ParentEditor is not null)
                    ParentEditor.DeleteNode(this);
            };
            NodeBorder.PropertyChanged += (_, _) => { CalcuateAll(); };
            NodeBorder.PointerPressed += (_, b) =>
            {
                if (b.GetCurrentPoint(NodeBorder).Properties.IsRightButtonPressed) return;
                isPressed = true;
                __p = b.GetPosition(NodeBorder);
                NodeBorder.Background = BG1;
            };
            NodeBorder.PointerMoved += (_, b) =>
            {
                if (isPressed)
                {
                    var ___p = b.GetPosition(this.Parent);
                    this.Margin = new Thickness(___p.X - __p.X, ___p.Y - __p.Y, 0, 0);
                    i++;
                    if (i % 3 == 0)
                    {
                        CalcuateAll();
                    }
                }
            };
            NodeBorder.PointerEnter += (_, b) =>
            {
                b.GetPosition(NodeBorder);
            };
            NodeBorder.PointerReleased += (_, b) =>
            {
                CalcuateAll();
                NodeBorder.Background = BG0;
                isPressed = false; i = 0;
            };
            LRect.Click += (_, _) =>
            {
                if (ParentEditor is not null)
                {
                    NodeBorder.Background = BG0;
                    isPressed = false;
                    i = 0;
                    ParentEditor.PressFromGraphNode(this, false);
                }
            };
            RRect.Click += (_, _) =>
            {
                if (ParentEditor is not null)
                {
                    NodeBorder.Background = BG0;
                    isPressed = false;
                    i = 0;
                    ParentEditor.PressFromGraphNode(this, true);
                }
            };
        }
        void CalcuateAll()
        {
            foreach (var item in L)
            {
                item.CalculatePath();
            }
            foreach (var item in R)
            {
                item.CalculatePath();
            }
        }
        public GraphNode()
        {
            InitializeComponent();
            InitEvents();
        }

    }
}
