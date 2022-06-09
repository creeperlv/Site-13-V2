using CLUNL.Localization;
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
}
