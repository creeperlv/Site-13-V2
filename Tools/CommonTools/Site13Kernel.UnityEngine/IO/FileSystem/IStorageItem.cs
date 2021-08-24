using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Site13Kernel.IO.FileSystem
{
    public interface IStorageItem
    {
        public IStorageItem Parent
        {
            get;
        }
        public string RealPath
        {
            get;
        }
        public string Name
        {
            get;
            set;
        }
        public bool VirtualResource
        {
            get;
        }
        public bool NetworkResource
        {
            get;
        }
        void Delete();
        Task DeleteAsync();
    }
    public interface IStorageItem2
    {
        bool TryGetItem(string Name, out IStorageItem item);
        IEnumerator<IStorageItem> EnumerateItems();
    }
    public interface IUnityStorageItem
    {
        UnityEngine.Object GetObject();
    }
}
