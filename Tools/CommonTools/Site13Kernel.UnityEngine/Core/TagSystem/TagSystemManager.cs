using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Site13Kernel.Core.TagSystem
{
    public class TagSystemManager : MonoBehaviour
    {
        public static TagSystemManager Instance;
        public List<EntityCollectionDescription> entityCollectionDescriptions = new List<EntityCollectionDescription>();
        public List<SystemBase> RegisteredSystems = new List<SystemBase>();
        public int ManualLimitation = 0;
        public Dictionary<int , List<SystemBase>> DistrubutedSystems = new Dictionary<int , List<SystemBase>>();
        [NonSerialized]
        public int RealThreads;
        CancellationTokenSource TokenSource = new CancellationTokenSource();
        public void Start()
        {
            Instance = this;
            if (ManualLimitation > 0)
            {
                RealThreads = ManualLimitation;
            }
            else
            {
                RealThreads = Environment.ProcessorCount;
            }
            TokenSource = new CancellationTokenSource();
            CancellationToken ct = TokenSource.Token;
            for (int i = 1 ; i < RealThreads ; i++)
            {
                var t=Task.Run(()=>ExecuteSystemInThread(i,ct),TokenSource.Token);
               
            }
        }
        float TimeScale = 1;
        
        void ExecuteSystemInThread(int ID,CancellationToken ctoken)
        {
            Stopwatch stopwatch = new Stopwatch();
            List<SystemBase> systemBases = new List<SystemBase>();
            float t = stopwatch.ElapsedMilliseconds / 1000f;
            while (true)
            {
                float ct = stopwatch.ElapsedMilliseconds / 1000f;
                float udt = ct - t;
                float dt = udt*TimeScale;
                foreach (var sys in systemBases)
                {
                    sys.Execute(dt , udt);
                }
                if (ctoken.IsCancellationRequested)
                {
                    return;
                }
            }
        }
        public void OnDestroy()
        {
            TokenSource.Cancel();
        }
        void ExecutePerFrame()
        {

        }
        public void AddObject(GameObject _obj)
        {
            var holder = _obj.GetComponent<ComponentHolder>();
            foreach (var item in entityCollectionDescriptions)
            {
                bool _hit = true;
                foreach (var t in item.Types)
                {
                    if (_obj.TryGetComponent(t , out var comp))
                    {
                        var __c = comp as IComponent;
                        if (__c != null)
                            holder.Components.Add(__c);
                    }
                    else
                    {
                        _hit = false;
                        break;
                    }
                }
                if (_hit)
                {
                    item.Resultes.Add(holder);
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
    public class TaggedSystemObjectGenreator
    {

    }
}
