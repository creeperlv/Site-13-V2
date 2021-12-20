using Newtonsoft.Json;
using Site13Kernel.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Site13Kernel.Data
{
    [Serializable]
    public class Settings
    {
        static string _File;
        public static Settings CurrentSettings;
        public float RenderScale = 100;
        public bool FullScreen = true;
        public float MouseSensibly = 1;
        public float SFX = 1;
        public float UI_SFX = 1;
        public float BGM = 1;
        public float UI_BGM = 1;
        public int WINDOW_W=-1;
        public int WINDOW_H=-1;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Init()
        {
            _File = Path.Combine(GameEnv.DataPath, "Configuration.json");
            if (File.Exists(_File))
            {
                Load();
            }
            else
            {
                CurrentSettings = new Settings();
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Load()
        {
            CurrentSettings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(_File));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Save()
        {
            if (File.Exists(_File))
            {
                File.Delete(_File);
            }
            File.WriteAllText(_File, JsonConvert.SerializeObject(CurrentSettings));
        }
    }
}
