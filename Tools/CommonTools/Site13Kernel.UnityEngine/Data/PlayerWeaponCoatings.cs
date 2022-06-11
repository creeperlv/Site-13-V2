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
    public class PlayerWeaponCoatings
    {
        static string _File;
        public static PlayerWeaponCoatings CurrentPlayerWeaponCoatings;
        public Dictionary<string,string> useWeaponCoatings = new Dictionary<string, string>();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetCoating(string Name,string Value)
        {
            if(!useWeaponCoatings.TryAdd(Name, Value))
            {
                useWeaponCoatings[Name] = Value;
            }
            Save();
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string GetCoating(string name,string fallback)
        {
            if (CurrentPlayerWeaponCoatings == null) return fallback;
            if(CurrentPlayerWeaponCoatings.useWeaponCoatings.TryGetValue(name, out var value))
            {
                return value;
            }else return fallback;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Init()
        {
            _File = Path.Combine(GameEnv.DataPath, "PlayerWeaponCoating.json");
            if (File.Exists(_File))
            {
                Load();
            }
            else
            {
                CurrentPlayerWeaponCoatings = new PlayerWeaponCoatings();
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Save()
        {
            if (File.Exists(_File))
            {
                File.Delete(_File);
            }
            File.WriteAllText(_File, JsonConvert.SerializeObject(CurrentPlayerWeaponCoatings, Settings.settings));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Load()
        {
            CurrentPlayerWeaponCoatings = JsonConvert.DeserializeObject<PlayerWeaponCoatings>(File.ReadAllText(_File), Settings.settings);
        }
    }
}
