using System;

namespace Site13Kernel.Core
{
    [Serializable]
    public class IDLock {
        public int CurrentID = -1;
        public bool Request(int ID) {
            return CurrentID == -1|| CurrentID == ID;
        }
        public bool Unlock(int ID)
        {
            if(CurrentID == -1) return true;
            if(CurrentID==ID)
            {
                CurrentID = -1;
                return true;
            }  
            return false;
        }
    }
}
