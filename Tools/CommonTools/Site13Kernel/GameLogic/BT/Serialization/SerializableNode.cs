using Site13Kernel.Data;
using Site13Kernel.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.GameLogic.BT.Serialization
{
    [Serializable]
    public class SerializableNode : IDuplicatable
    {
        public string ID = "";
        public double X;
        public double Y;
        public bool HaveL = true;
        public bool HaveR = true;
        public bool DisableDeletion = false;
        public bool DisableDuplicate = false;
        public List<string> L = new List<string>();
        public List<string> R = new List<string>();
        public Site13Kernel.GameLogic.BT.Nodes.BTBaseNode? Contained;

        public IDuplicatable Duplicate()
        {
            return JsonUtilities.Deserialize<SerializableNode>(JsonUtilities.Serialize(this));
        }
    }
}
