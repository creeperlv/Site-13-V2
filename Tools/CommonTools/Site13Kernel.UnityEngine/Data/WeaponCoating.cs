using CLUNL.Localization;
using Site13Kernel.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.Data
{
    [Serializable]
    public class WeaponCoating
    {
        public string ID;
        public LocalizedString Name;
        public LocalizedString Description;
        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
    public class NameDefinition:ControlledBehavior {
        public static NameDefinition Instance;
        public KVList<int, LocalizedString> _Names;
        public Dictionary<int, LocalizedString> Names;
        public override void Init()
        {
            Instance = this;
            Names=_Names.ObtainMap();
            _Names.PrefabDefinitions.Clear();
        }
        public static LocalizedString Query(int ID,LocalizedString Fallback=null)
        {
            if(Instance==null) { return Fallback; }
            if(Instance.Names.ContainsKey(ID))
            {
                return Instance.Names[ID];
            }
            else
            {
                return Fallback;
            }
        }
    }

}
