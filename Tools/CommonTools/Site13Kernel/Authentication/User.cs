using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.Authentication
{
    [Serializable]
    public class User:IEqualityComparer<User>
    {
        public Guid UserID;
        public string UserName;

        public bool Equals(User x, User y)
        {
            return x.UserID == y.UserID;
        }

        public override int GetHashCode()
        {
            return UserID.GetHashCode();
        }

        public int GetHashCode(User obj)
        {
            return obj.UserID.GetHashCode();
        }
    }
}
