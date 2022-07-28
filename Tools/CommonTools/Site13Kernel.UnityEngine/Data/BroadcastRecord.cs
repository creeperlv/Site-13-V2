using Site13Kernel.Core;
using Site13Kernel.GameLogic.Directors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.Data
{
    public class BroadcastRecord : ControlledBehavior
    {
        public static BroadcastRecord Instance;
        public List<BroadCastItem> Broadcasts;
        public Site13Event<BroadCastItem> Subscribers = new Site13Event<BroadCastItem>();
        public override void Init()
        {
            Instance = this;
        }
        public void IssueBroadCast(BroadCastItem Content)
        {
            Broadcasts.Add(Content);
            Subscribers.Invoke(Content);
        }
        public void Subscribe(Action<BroadCastItem> A)
        {
            Subscribers.Add(A);
        }
        public void Unsub(Action<BroadCastItem> A)
        {
            Subscribers.Remove(A);
        }
    }
}
