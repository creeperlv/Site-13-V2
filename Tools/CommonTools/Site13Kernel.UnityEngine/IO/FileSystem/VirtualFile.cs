using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Site13Kernel.IO.FileSystem
{
    public class VirtualFile : IStorageItem,IUnityStorageItem
    {
        string _Name;
        IStorageItem _Parent;
        UnityEngine.Object _obj;
        internal VirtualFile(string Name, IStorageItem Parent,UnityEngine.Object obj)
        {
            _Name = Name;
            _Parent = Parent;
            _obj = obj;
        }


        public IStorageItem Parent => _Parent;

        public string Name
        {
            get => _Name;
            set => throw new NotSupportedException();
        }

        public bool VirtualResource => true;

        public string RealPath => throw new NotSupportedException();

        public bool NetworkResource => false;

        public void Delete()
        {
            UnityEngine.Object.Destroy(_obj);
        }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task DeleteAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            throw new NotSupportedException();
            //await Task.Run(() => { Delete(); });
        }

        public UnityEngine.Object GetObject()
        {
            return _obj;
        }
    }
}
