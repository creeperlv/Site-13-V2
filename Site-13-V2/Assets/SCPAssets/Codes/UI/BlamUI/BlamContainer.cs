using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Site13Kernel.UI.BlamUI
{
    public class BlamContainer : BlamBase
    {
        public List<BlamButton> Children = new List<BlamButton>();
        public List<BlamContainer> ChildrenContainers = new List<BlamContainer>();
        
    }
    public class BlamBase : UIBehaviour
    {
        public bool isEnabled;
        public virtual bool IsEnabled { get; set; }
    }
}
