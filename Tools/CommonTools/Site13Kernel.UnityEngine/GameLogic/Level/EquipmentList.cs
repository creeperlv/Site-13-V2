using Site13Kernel.Core;
using Site13Kernel.Data;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.GameLogic.Level
{
    public class EquipmentManifest : ControlledBehavior
    {
        public static EquipmentManifest Instance;
        public Dictionary<int, EquipmentDefinition> EqupimentMap = new Dictionary<int, EquipmentDefinition>();
        public KVList<int, EquipmentDefinition> Equipments;
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
