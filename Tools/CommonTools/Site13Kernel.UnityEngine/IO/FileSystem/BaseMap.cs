using System;
using System.Collections.Generic;

namespace Site13Kernel.IO.FileSystem
{
    [Serializable]
    public class BaseMap
    {
        //public Dictionary<string,UnityEngine.Object> Mapping=new Dictionary<string, UnityEngine.Object>();
        public List<VirtualFileNode> VirtualFileNodes=new List<VirtualFileNode>();
    }
    [Serializable]
    public class VirtualFileNode
    {
        public string Name;
        public bool isEndPoint;
        public UnityEngine.Object obj;
        public List<VirtualFileNode> Children=null;

    }
}
