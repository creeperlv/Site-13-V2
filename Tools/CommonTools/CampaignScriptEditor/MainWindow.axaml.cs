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
            ShowButton.Click += (_, _) =>
            {
                foreach (var item in Events.Children)
                {
                    if (item is EventItem e)
                    {
                        e.EditorToggle.IsChecked = true;
                    }
                }
            };
            HideButton.Click += (_, _) =>
            {
                foreach (var item in Events.Children)
                {
                    if (item is EventItem e)
                    {
                        e.EditorToggle.IsChecked = false;
                    }
                }
            };
            CollectVariable.Click += (_, _) => CollectReferences();
            About.Click += async (_, _) =>
            {
                AboutDialog aboutDialog = new AboutDialog();
                aboutDialog.SetInfo("Script Editor", typeof(MainWindow).Assembly.GetName().Version);
                await aboutDialog.ShowDialog(this);
            };
        }
        void CollectReferences()
        {
            {
                List<string> Triggers = new List<string>();
                List<string> Locations = new List<string>();
                List<string> Objects = new List<string>();
                List<string> Components = new List<string>();
                foreach (var item in Events.Children)
                {
                    if (item is EventItem e)
                    {
                        var __e = (EventBase)e.ObtainObject();
                        if (__e.EventTriggerID != "")
                        {
                            if (!Triggers.Contains(__e.EventTriggerID))
                            {
                                Triggers.Add(__e.EventTriggerID);
                            }
                        }
                        switch (__e)
                        {
                            case ToggleObject o:
                                if (!Objects.Contains(o.ObjectID))
                                {
                                    Objects.Add(o.ObjectID);
                                }
                                break;
                            case ToggleComponent c:
                                {
                                    if (!Components.Contains(c.ComponentID)){
                                        Components.Add(c.ComponentID);
                                    }
                                }
                                break;
                            default:
                                {
                                    __e.GetType();
                                    var SL = FindLocation(__e.GetType(), __e);
                                    if(SL is not null)
                                    {
                                        if (SL.UseSceneLookUp)
                                        {
                                            if (!Locations.Contains(SL.LookUpName))
                                            {
                                                Locations.Add(SL.LookUpName);
                                            }
                                        }
                                    }
                                }
                                break;
                        }

                    }
                }
                TriggerIDContainer.Children.Clear();
                ObjectsIDContainer.Children.Clear();
                LocationsIDContainer.Children.Clear();
                ComponentsIDContainer.Children.Clear();
                foreach (var item in Triggers)
                {
                    if (item is not null)
                        TriggerIDContainer.Children.Add(MakeTB(item));
                }
                foreach (var item in Objects)
                {
                    if (item is not null)
                        ObjectsIDContainer.Children.Add(MakeTB(item));
                }
                foreach (var item in Locations)
                {
                    if (item is not null)
                        LocationsIDContainer.Children.Add(MakeTB(item));
                }
                foreach (var item in Components)
                {
                    if (item is not null)
                        ComponentsIDContainer.Children.Add(MakeTB(item));
                }
            }
        }
        SerializableLocation? FindLocation(Type t, object obj)
        {
            var fs = t.GetFields();
            foreach (var item in fs)
            {
                if (!item.FieldType.IsSealed)
                {
                    if (item.FieldType == typeof(SerializableLocation))
                    {
                        return item.GetValue(obj) as SerializableLocation;
                    }
                    else
                    {
                        var v = item.GetValue(obj);
                        if (v is not null)
                        {
                            var L = FindLocation(item.FieldType, v);
                            if (L is not null) return L;

                        }
                    }
                }
            }
            return null;
        }
        Thickness Zero = new Thickness(0);
        TextBox MakeTB(string content)
        {
            TextBox textBlock = new TextBox();
            textBlock.Text = content;
            textBlock.IsReadOnly = true;
            textBlock.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
            textBlock.FontSize = 11;
            //textBlock.BorderThickness = Zero;
            //textBlock.Padding = Zero;
            return textBlock;
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
                    {
                        Events.Children.Clear();
                        foreach (var item in _L)
                        {
                            var e = new EventItem();
                            //e.InitType(item.GetType());
                            e.InitObject(item);
                            Events.Children.Add(e);
                        }
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
            TypeNameHandling = TypeNameHandling.Objects,TypeNameAssemblyFormatHandling= TypeNameAssemblyFormatHandling.Simple,
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
