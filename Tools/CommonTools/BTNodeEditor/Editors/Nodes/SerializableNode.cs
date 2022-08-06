using System;
using System.Collections.Generic;

namespace BTNodeEditor.Editors.Nodes
{
    [Serializable]
    public class SerializableNode
    {
        public string ID;
        public double X;
        public double Y;
        public bool HaveL=true;
        public bool HaveR=true;
        public List<string> L=new();
        public List<string> R=new();
        public Site13Kernel.GameLogic.BT.Nodes.BTBaseNode? Contained;
    }
}
