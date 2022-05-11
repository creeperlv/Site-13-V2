using System;

namespace Site13Kernel.Data
{
    [Serializable]
    public class ForgeBaseMap
    {
        public string MapID;
        public int SceneID;
        public override int GetHashCode()
        {
            return MapID.GetHashCode();
        }
    }
}
