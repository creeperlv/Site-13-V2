using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.Utilities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Site13Kernel.GameLogic.Level
{
    public class SoundResources : ControlledBehavior
    {
        public static Dictionary<int, List<AudioClip>> ResourceDictionary = new Dictionary<int, List<AudioClip>>();
        public static Dictionary<int, float> SoundMultiplier = new Dictionary<int, float>();
        public KVList<int, List<AudioClip>> SoundDefinition = new KVList<int, List<AudioClip>>();
        public KVList<int, float> _SoundMultiplier = new KVList<int, float>();
        public bool ClearCurrentDefinition = false;
        public bool ControlledBehaviorWorkFlow = false;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AudioClip ObtainOneClip(int id)
        {
            if (ResourceDictionary.ContainsKey(id))
            {
                return ListOperations.ObtainOne(ResourceDictionary[id]);
            }
            return null;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float FindMultiplier(int id)
        {
            if (SoundMultiplier.ContainsKey(id))
                return SoundMultiplier[id];
            return 1;
        }
        public void __init()
        {
            if (ClearCurrentDefinition)
            {
                ResourceDictionary.Clear();
                SoundMultiplier.Clear();
            }
            DictionaryOperations.Merge(ref ResourceDictionary, SoundDefinition.ObtainMap());
            DictionaryOperations.Merge(ref SoundMultiplier, _SoundMultiplier.ObtainMap());
        }
        public void Start()
        {
            if (ControlledBehaviorWorkFlow) return;
            __init();
        }
        public override void Init()
        {
            if (ControlledBehaviorWorkFlow)
            {
                __init();
            }
        }
    }
}
