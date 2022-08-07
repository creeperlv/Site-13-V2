using Avalonia.Controls;
using System.Runtime.InteropServices;
using System;
using CampaignScriptEditor.Controls;
using CampaignScriptEditor.Editors.Fields;
using CampaignScriptEditor;
using Site13Kernel.Data.Serializables;
using System.Collections.Generic;
using Site13Kernel.Data.Attributes;
using Site13Kernel.GameLogic.BT.Nodes;
using Site13Kernel.GameLogic.Directors;
using CommonTools;

namespace BTNodeEditor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CentralArea.Children.Add(CentralEditor);
            ApplyVisual();
            InitEvents();
            Load();
        }
        public BTNodeEditor.Editors.NodeGraphEditor CentralEditor = new Editors.NodeGraphEditor();
        void InitEvents()
        {
            AboutMenuItem.Click += async (_, _) => {

                AboutDialog aboutDialog = new AboutDialog();
                aboutDialog.SetInfo("BehaviorTree Node Editor", typeof(MainWindow).Assembly.GetName().Version, "Avalonia.Controls.PanAndZoom");
                await aboutDialog.ShowDialog(this);
            };
        }
        public static Type BTBaseNodeType = typeof(BTBaseNode);
        void Load()
        {

            FieldEditorPool.FieldEditors.Add(typeof(string), typeof(StringField));
            FieldEditorPool.FieldEditors.Add(typeof(bool), typeof(BoolField));
            FieldEditorPool.FieldEditors.Add(typeof(float), typeof(FloatField));
            FieldEditorPool.FieldEditors.Add(typeof(int), typeof(IntField));
            FieldEditorPool.FieldEditors.Add(typeof(SerializableVector3), typeof(Vector3Field));
            FieldEditorPool.FieldEditors.Add(typeof(SerializableQuaternion), typeof(QuaternionField));
            Dictionary<string, TitledContainer> containers = new Dictionary<string, TitledContainer>();
            foreach (var item in BTBaseNodeType.Assembly.GetTypes())
            {
                if (item.IsAssignableTo(BTBaseNodeType))
                {
                    if (item.FullName != BTBaseNodeType.FullName)
                    {
                        var catas = item.GetCustomAttributes(typeof(CatalogAttribute), false);
                        string Cata = "General";
                        if (catas.Length > 0)
                        {
                            Cata = (catas[0] as CatalogAttribute)!.CatalogString;
                        }
                        else
                        {

                        }
                        TitledContainer? CurrentContainer = null;
                        if (!containers.TryGetValue(Cata, out CurrentContainer))
                        {
                            CurrentContainer = new TitledContainer();
                            CurrentContainer.Title = Cata;
                            containers.Add(Cata, CurrentContainer);
                        }
                        Button button = new Button()
                        {
                            Content = item.Name
                        };
                        button.Click += (_, _) =>
                        {
                            CentralEditor.AddNode(new Editors.Nodes.SerializableNode() { X = 150, Y = 150, ID = Guid.NewGuid().ToString(), Contained = (BTBaseNode)Activator.CreateInstance(item)! }); ;
                        };
                        CurrentContainer!.Children.Add(button);
                    }
                }
            }
            foreach (var item in containers)
            {
                Primitives.Children.Add(item.Value);
            }
        }
        void ApplyVisual()
        {

            ExtendClientAreaToDecorationsHint = true;
            ExtendClientAreaChromeHints = Avalonia.Platform.ExtendClientAreaChromeHints.PreferSystemChrome;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (Environment.OSVersion.Version.Build >= 22000)
                {
                    TransparencyLevelHint = WindowTransparencyLevel.Mica;
                }
                else
                if (Environment.OSVersion.Version.Build >= 14393)
                {
                    TransparencyLevelHint = WindowTransparencyLevel.AcrylicBlur;
                }
            }
        }
    }
}
