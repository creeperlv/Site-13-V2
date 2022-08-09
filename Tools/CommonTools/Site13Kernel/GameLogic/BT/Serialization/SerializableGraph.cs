using Site13Kernel.Data;
using Site13Kernel.GameLogic.BT.Nodes;
using Site13Kernel.GameLogic.BT.Nodes.Generic;
using Site13Kernel.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

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
                if (item.Contained is Start s)
                {
                    Root = Convert(item);
                    break;
                }
            }
            return Root;
        }
        BTBaseNode Convert(SerializableNode SN)
        {
            var _node = SN.Contained;
            _node.NextNode = null;
            if (SN.R.Count > 0)
            {
                if (SN.R.Count == 1)
                {
                    _node.NextNode = Convert(FindNode(SN.R.First()));
                }
                else if (SN.R.Count > 1)
                {
                    BTBaseNode __n = new Selector();
                    _node.NextNode = __n;
                    foreach (var item in SN.R)
                    {
                        var __cn = Convert(FindNode(item));
                        __n.NextNode = __cn;
                        __n = Newest(__cn);
                    }
                }
            }
            return _node;
        }
        BTBaseNode Newest(BTBaseNode __node)
        {
            var _node = __node;
            int d = 0;
            while (_node.NextNode != null)
            {
                _node = _node.NextNode;
                d++;
            }
            return _node;
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
