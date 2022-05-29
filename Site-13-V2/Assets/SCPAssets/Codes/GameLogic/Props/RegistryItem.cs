using Site13Kernel.Core.Interactives;
using Site13Kernel.GameLogic.RuntimeScenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.Props
{
    public class RegistryItem : MonoBehaviour
    {
        public string RegistryName;
        public RegistryItemType RegistryItemType;
        public string TargetString;
        public bool TargetBool;
        public float TargetFloat;
        public SimpleTrigger Trigger;
        public bool WillSelfDesctruct = false;
        void Start()
        {
            Trigger.AddCallback(() =>
            {
                switch (RegistryItemType)
                {
                    case RegistryItemType.BOOL:
                        LevelRuntimeRegistry.Set(RegistryName, TargetBool);
                        break;
                    case RegistryItemType.STRING:
                        LevelRuntimeRegistry.Set(RegistryName, TargetString);
                        break;
                    case RegistryItemType.FLOAT:
                        LevelRuntimeRegistry.Set(RegistryName, TargetFloat);
                        break;
                    default:
                        break;
                }
                if (WillSelfDesctruct)
                {
                    Destroy(this.gameObject);
                }
            });
        }

    }
    public enum RegistryItemType
    {
        BOOL, STRING, FLOAT
    }
}
