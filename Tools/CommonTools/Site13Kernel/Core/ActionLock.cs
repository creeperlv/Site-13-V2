using System;
using System.Collections.Generic;

namespace Site13Kernel.Core
{
    [Serializable]
    public class ActionLock : IDLock
    {
        public Dictionary<int, ActionDef> Actions = new Dictionary<int, ActionDef>();
        public bool TryExecute(int ID, Action action)
        {
            if (Request(ID))
            {
                action();
                return true;
            }
            return false;
        }
        //public void Do(int ID)
        //{
        //    TryExecute(ID, () => { });
        //}
    }
    [Serializable]
    public class Trigger
    {
        public bool IsOn
        {
            get => _isOn; set
            {
                if (_isOn != value)
                {
                    if (TriggerAction != null) TriggerAction(_isOn);
                }
                _isOn = value;
            }
        }
        public void SetWithoutNotification(bool value)
        {
            _isOn = value;
        }
        public Action<bool> TriggerAction = null;
        public bool _isOn = false;
    }
    public enum TernaryStates
    {
        Off = 0, Operating = 1, On = 2
    }
    [Serializable]
    public class ActionDef
    {
        public int ID;
        public List<int> ConflictedAction;
        public bool CannotBeCanceled = false;
        public Action Execute;
        public Action CancelAction;
        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }

}
