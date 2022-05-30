using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CampaignScriptEditor.Editors;
using CampaignScriptEditor.Editors.Fields;
using CommonTools;
using Newtonsoft.Json;
using Site13Kernel.Data.Serializables;
using Site13Kernel.GameLogic.Directors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CampaignScriptEditor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            ApplyStyles();
            Load();
            InitEvents();
        }
        public void InitEvents()
        {
            NewFile.Click += (_, _) =>
            {
                Events.Children.Clear();
                CurrentFile = null;
                GC.Collect();
            };
            Save.Click += async (_, _) => await __Save();
            SaveAs.Click += async (_, _) => await __SaveAs();
            Open.Click += async (_, _) => await __Open();
            ShowButton.Click += (_, _) => {
                foreach (var item in Events.Children)
                {
                    if (item is EventItem e)
                    {
                        e.EditorToggle.IsChecked = true;
                    }
                }
            }; 
            HideButton.Click += (_, _) => {
                foreach (var item in Events.Children)
                {
                    if (item is EventItem e)
                    {
                        e.EditorToggle.IsChecked = false;
                    }
                }
            };
            About.Click += async (_, _) =>
            {
                AboutDialog aboutDialog = new AboutDialog();
                aboutDialog.SetInfo("Script Editor", typeof(MainWindow).Assembly.GetName().Version);
                await aboutDialog.ShowDialog(this);
            };
        }
        async Task __Open()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            var L = await openFileDialog.ShowAsync(this);
            if (L is not null)
                if (L.Length > 0)
                {
                    CurrentFile = new FileInfo(L[0]);
                    var _L = JsonConvert.DeserializeObject<List<EventBase>>(await File.ReadAllTextAsync(CurrentFile.FullName), settings);
                    if (_L is not null)
                        foreach (var item in _L)
                        {
                            var e = new EventItem();
                            //e.InitType(item.GetType());
                            e.InitObject(item);
                            Events.Children.Add(e);
                        }
                    GC.Collect();
                }
        }
        async Task __SaveAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            var file = await saveFileDialog.ShowAsync(this);
            if (file is not null)
            {
                CurrentFile = new FileInfo(file);
                await __Save();
            }
        }
        async Task __Save()
        {
            if (CurrentFile is null)
            {
                await __SaveAs();
            }
            else
            {
                List<EventBase> l = new List<EventBase>();
                foreach (var item in Events.Children)
                {
                    if (item is EventItem e)
                    {
                        l.Add((EventBase)e.ObtainObject());
                    }
                }
                if (CurrentFile.Exists)
                {
                    CurrentFile.Delete();
                }
                await File.WriteAllTextAsync(CurrentFile.FullName, JsonConvert.SerializeObject(l, settings));
                GC.Collect();
            }
        }
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Formatting.Indented,
        };
        public FileInfo? CurrentFile;
        public void ApplyStyles()
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
        public static Type EventBaseType = typeof(EventBase);
        public void Load()
        {
            FieldEditorPool.FieldEditors.Add(typeof(string), typeof(StringField));
            FieldEditorPool.FieldEditors.Add(typeof(bool), typeof(BoolField));
            FieldEditorPool.FieldEditors.Add(typeof(float), typeof(FloatField));
            //FieldEditorPool.FieldEditors.Add(typeof(SerializableLocation), typeof(SerializableLocationField));
            FieldEditorPool.FieldEditors.Add(typeof(SerializableVector3), typeof(Vector3Field));
            FieldEditorPool.FieldEditors.Add(typeof(SerializableQuaternion), typeof(QuaternionField));
            foreach (var item in EventBaseType.Assembly.GetTypes())
            {
                if (item.IsAssignableTo(EventBaseType))
                {
                    if (item.FullName != EventBaseType.FullName)
                    {
                        Button button = new Button()
                        {
                            Content = item.Name
                        };
                        button.Click += (_, _) =>
                        {
                            var e = new EventItem();
                            e.InitType(item);
                            Events.Children.Add(e);
                        };
                        EventPrimitives.Children.Add(button);
                    }
                }
            }
        }
    }
}
