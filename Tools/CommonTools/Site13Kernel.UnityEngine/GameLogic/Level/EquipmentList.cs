using Site13Kernel.Core;
using Site13Kernel.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.GameLogic.Level
{
    public class EquipmentManifest : ControlledBehavior
    {
        public static EquipmentManifest Instance;
        public Dictionary<int, BaseEquipment> EqupimentMap = new Dictionary<int, BaseEquipment>();
        public KVList<int, BaseEquipment> Equipments;
        public bool UseControlledInitializer;
        void __init()
        {
            Instance = this;
            EqupimentMap = Equipments.ObtainMap();
        }
        public void Start()
        {
            if (!UseControlledInitializer)
            {
                __init();
            }
        }
        public override void Init()
        {
            if (UseControlledInitializer)
            {
                __init();
            }
        }
    }
}
