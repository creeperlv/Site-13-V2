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
using System.IO;
using Site13Kernel.Utilities;
using System.Threading.Tasks;
using Site13Kernel.GameLogic.BT.Serialization;
using BTNodeEditor.Editors.Nodes;
using Site13Kernel.GameLogic.BT.Attributes;
using System.Diagnostics;

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
            CentralEditor.AddStartNode();
        }
        public BTNodeEditor.Editors.NodeGraphEditor CentralEditor = new Editors.NodeGraphEditor();
        void InitEvents()
        {
            NewBtn.Click += (_, _) =>
            {
                EmptyEditor();
                CentralEditor.AddStartNode();
                __file = null;
                GC.Collect();
            };
            HideButton.Click += (_, _) =>
            {
                CentralEditor.HideAll();
            };
            ShowButton.Click += (_, _) =>
            {
                CentralEditor.ShowAll();
            };
            Site13KernelOnlyBtn.Click += async (_, _) =>
            {
                await BuildSite13KernelJson();
            };
            Site13KernelBinBtn.Click += async (_, _) =>
            {
                await BuildSite13KernelBin();
            };
            AboutMenuItem.Click += async (_, _) =>
            {
                AboutDialog aboutDialog = new AboutDialog();
                aboutDialog.SetInfo("BehaviorTree Node Editor", typeof(MainWindow).Assembly.GetName().Version, "wieslawsoltes/PanAndZoom", "rikimaru0345/Ceras");
                await aboutDialog.ShowDialog(this);
            };
            SaveAsBtn.Click += async (_, _) =>
            {
                await __save_as();
            };
            SaveBtn.Click += async (_, _) =>
            {
                if (__file is null)
                {
                    await __save_as();
                }
                else
                {
                    __save();
                }
            };
            OpenBtn.Click += async (_, _) =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.AllowMultiple = false;
                var l = await openFileDialog.ShowAsync(this);
                if (l.Length > 0)
                {
                    __file = l[0];
                    EmptyEditor();
                    CentralEditor.LoadSerializableGraph(JsonUtilities.Deserialize<SerializableGraph>(File.ReadAllText(__file)));
                    GC.Collect();
                }
            };
        }
        async Task BuildSite13KernelJson()
        {
            var __n = CentralEditor.ToSerializableGraph().Build();
            CheckNode(__n);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filters = new List<FileDialogFilter>();
            saveFileDialog.Filters.Add(new FileDialogFilter() { Extensions = new List<string>() { "json" }, Name = "Json" });
            var f = await saveFileDialog.ShowAsync(this);
            if (f is not null)
            {
                if (File.Exists(f))
                {
                    File.Delete(f);
                }
                Trace.WriteLine(__n.NextNode == null);
                File.WriteAllText(f, JsonUtilities.Serialize(__n));
            }
        }
        async Task BuildSite13KernelBin()
        {
            var __n = CentralEditor.ToSerializableGraph().Build();
            CheckNode(__n);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filters = new List<FileDialogFilter>();
            saveFileDialog.Filters.Add(new FileDialogFilter() { Extensions = new List<string>() { "bytes" }, Name = "Binary" });
            var f = await saveFileDialog.ShowAsync(this);
            if (f is not null)
            {
                if (File.Exists(f))
                {
                    File.Delete(f);
                }
                Trace.WriteLine(__n.NextNode == null);
                File.WriteAllBytes(f, BinaryUtilities.Serialize(__n));
            }
        }
        void CheckNode(BTBaseNode Node)
        {
            Trace.WriteLine(Node.GetType().Name);
            if (Node.NextNode is not null) CheckNode(Node.NextNode);
        }
        void EmptyEditor()
        {
            CentralArea.Children.Remove(CentralEditor);
            CentralEditor = new Editors.NodeGraphEditor();
            CentralArea.Children.Add(CentralEditor);
            GC.Collect();
        }
        async Task __save_as()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filters = new List<FileDialogFilter>();
            saveFileDialog.Filters.Add(new FileDialogFilter() { Extensions = new List<string>() { "json" }, Name = "Json" });
            saveFileDialog.Filters.Add(new FileDialogFilter() { Extensions = new List<string>() { "bt-graph" }, Name = "Behavior Tree Graph" });
            var f = await saveFileDialog.ShowAsync(this);
            if (f is not null)
            {
                __file = f;
                __save();
            }
        }
        void __save()
        {
            if (__file is not null)
            {
                if (File.Exists(__file))
                {
                    File.Delete(__file);
                }
                File.WriteAllText(__file, JsonUtilities.Serialize(CentralEditor.ToSerializableGraph()));
            }
        }
        string? __file = null;
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
                        {
                            //Check HideInEditor
                            var attr = item.GetCustomAttributes(typeof(HideInEditorAttribute), false);
                            if (attr.Length > 0) continue;
                        }
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
                            Content = item.Name,
                            FontSize = 11
                        };
                        button.Click += (_, _) =>
                        {
                            CentralEditor.AddNode(new SerializableNode()
                            {
                                X = 150,
                                Y = 150,
                                ID = Guid.NewGuid().ToString(),
                                Contained = (BTBaseNode)Activator.CreateInstance(item)!
                            }, true); ;
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
                else
                {
                    TransparencyLevelHint = WindowTransparencyLevel.Blur;
                }
            }
            else
            {
                TransparencyLevelHint = WindowTransparencyLevel.Blur;
            }
        }
    }
}
