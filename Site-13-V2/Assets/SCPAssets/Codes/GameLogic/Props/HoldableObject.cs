using CLUNL.Localization;
using Site13Kernel.Core.Interactives;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.Props
{
    public class HoldableObject : InteractiveBase
    {
        public int IconID;
        public bool isHolded;
        public LocalizedString ObjectName;
        public string TargetAnimationSetID;
        public override LocalizedString Hint { get => new LocalizedString(OperateHint,OperateHintFallBack, ObjectName);}
    }
}
