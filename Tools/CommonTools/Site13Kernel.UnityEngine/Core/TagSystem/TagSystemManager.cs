using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.Core.TagSystem
{
    public class TagSystemManager : MonoBehaviour
    {
        public static TagSystemManager Instance;
        public List<EntityCollectionDescription> entityCollectionDescriptions = new List<EntityCollectionDescription>();
        public List<SystemBase> RegisteredSystems = new List<SystemBase>();
        public void Start()
        {
            Instance = this;
        }
        public void AddObject(GameObject _obj)
        {
            foreach (var item in entityCollectionDescriptions)
            {
                bool _hit = true;
                foreach (var t in item.Types)
                {
                    if (!_obj.TryGetComponent(t,out _))
                    {
                        _hit = false;
                        break;
                    }
                }
                if (_hit)
                {
                    item.Resultes.Add(_obj);
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RegisterSystem(SystemBase system)
        {
            foreach (var item in system.Collection.Descriptions)
            {
                entityCollectionDescriptions.Add(item);
            }
            RegisteredSystems.Add(system);
        }
    }
}
