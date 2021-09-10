using Newtonsoft.Json;
using Site13Kernel.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Site13Kernel.Data
{
    [Serializable]
    public class Settings
    {
        static string _File;
        public static Settings CurrentSettings;
        public float RenderScale = 1;
        public bool FullScreen = true;
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
        public static void Load()
        {
            CurrentSettings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(_File));
        }
        public static void Save() {
            if (File.Exists(_File))
            {
                File.Delete(_File);
            }
            File.WriteAllText(_File, JsonConvert.SerializeObject(CurrentSettings));
        }
    }
}
