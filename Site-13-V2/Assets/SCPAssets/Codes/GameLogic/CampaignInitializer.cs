using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Site13Kernel.Core;
namespace Site13Kernel.GameLogic
{
    public class CampaignInitializer : ControlledBehavior
    {
        public bool isDone;
        public List<SyncInitializer> syncInitizers= new List<SyncInitializer>();
        public override void Init()
        {
            Parent._OnRefresh.Add(this);
            Parent._OnFixedRefresh.Add(this);
            foreach (var item in syncInitizers)
            {
                item.Init();
            }
        }
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            foreach (var item in syncInitizers)
            {
                item.Execute();
            }
            CollectIsDone();
        }
        public override void FixedRefresh(float DeltaTime, float UnscaledDeltaTime)
        {
            foreach (var item in syncInitizers)
            {
                item.FixedExecute();
            }
            CollectIsDone();
        }
        void CollectIsDone()
        {
            bool __Done=true;
            foreach (var item in syncInitizers)
            {
                __Done &= item.isDone;
            }
            isDone = __Done;
            if (isDone)
            {
                Parent.UnregisterFixedRefresh(this);
                Parent.UnregisterRefresh(this);
            }
        }
    }
}
