using Site13Kernel.Data;
using Site13Kernel.GameLogic.BT.Nodes;
using Site13Kernel.GameLogic.BT.Nodes.Generic;
using Site13Kernel.Utilities;
using System;
using System.Collections.Generic;

namespace Site13Kernel.GameLogic.BT.Serialization
{
    [Serializable]
    public class SerializableGraph : IDuplicatable
    {
        public List<SerializableNode> nodes = new List<SerializableNode>();
        public IDuplicatable Duplicate()
        {
            return JsonUtilities.Deserialize<SerializableGraph>(JsonUtilities.Serialize(this));
        }
        public BTBaseNode Build()
        {
            BTBaseNode Root = new BTBaseNode();
            foreach (var item in nodes)
            {
                if(item.Contained is Start s)
                {
                    Root = s;
                    
                }
            }
            return Root;
        }
        SerializableNode FindNode(string ID)
        {
            foreach (var item in nodes)
            {
                if (item.ID == ID) return item;
            }
            return null;
        }
    }
}
