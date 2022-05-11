using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site13Kernel.Data
{
    [Serializable]
    public class ForgeMap
    {
        public string MapID;//Prefer GUID
        public string DisplayName;
        public string Description;
        public string BaseMapID;
        public string SceneDescriptionFile;
        public ForgeMap Duplicate()
        {
            return new ForgeMap { BaseMapID = new string(BaseMapID), 
                Description = new string(Description), 
                DisplayName = new string(DisplayName), 
                MapID = new string(MapID),
                SceneDescriptionFile = new string(SceneDescriptionFile)
            };
        }
    }
}
