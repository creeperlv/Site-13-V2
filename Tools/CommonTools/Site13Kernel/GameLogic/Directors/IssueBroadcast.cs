using Site13Kernel.Data.Attributes;
using System;

namespace Site13Kernel.GameLogic.Directors
{
    [Catalog("System Messages")]
    [Serializable]
    public class IssueBroadcast : EventBase
    {
        public BroadCastItem BroadCast;
    }
    [Catalog("System Messages")]
    [Serializable]
    public class SetAchievementEvent : EventBase
    {
        public string AchievenmentID;
        public SetOperation Operation;
        public float Amount;
    }
    public enum SetOperation
    {
        Add, Set
    }

}