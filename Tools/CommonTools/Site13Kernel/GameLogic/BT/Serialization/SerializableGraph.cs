using Site13Kernel.Data;
using Site13Kernel.GameLogic.BT.Nodes;
using Site13Kernel.GameLogic.BT.Nodes.Generic;
using Site13Kernel.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                    int c = 0;
                    Root = Convert(item, ref c);
                    break;
                }
            }
            return Root;
        }
        BTBaseNode Convert(SerializableNode SN, ref int SequenceCount)
        {
            var _node = SN.Contained;
            int __C = -1;
            switch (_node)
            {
                case Sequence _:
                    __C= (SequenceCount * 1) + 0; ;
                    SequenceCount = SequenceCount + 1;
                    break;
                case End _:
                    SequenceCount = SequenceCount - 1;
                    break;
                default:
                    break;
            }
            _node.NextNode = null;
            if (SN.R.Count > 0)
            {
                if (SN.R.Count == 1)
                {
                    _node.NextNode = Convert(FindNode(SN.R.First()), ref SequenceCount);
                }
                else if (SN.R.Count > 1)
                {
                    BTBaseNode __n = new Selector();
                    _node.NextNode = __n;
                    BTBaseNode NextSelector = null;
                    foreach (var item in SN.R)
                    {
                        var __cn = Convert(FindNode(item), ref SequenceCount);
                        switch (__cn)
                        {
                            case EditorSelector _:
                            case Selector _:
                                NextSelector = __cn;
                                continue;
                            default:
                                break;
                        }
                        __n.NextNode = __cn;
                        __n = Deepest(__cn);
                    }
                    if (NextSelector != null)
                    {
                        var N = Deepest(_node);
                        N.NextNode = NextSelector;
                    }
                }
            }
            if (SequenceCount != __C&&__C!=-1)
            {
                Trace.WriteLine($"Not Equal:{SequenceCount}=={__C}");
                End end = new End();
                var N = Deepest(_node);
                N.NextNode = end;
                SequenceCount--;
            }
            return _node;
        }
        BTBaseNode Deepest(BTBaseNode __node)
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
