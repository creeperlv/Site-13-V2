using System.Collections.Generic;
using System.Threading.Tasks;

namespace Site13Kernel.IO.FileSystem
{
    public class VirtualFolder : IStorageItem, IStorageItem2
    {
        string _Name;
        IStorageItem _Parent;
        internal List<VirtualFolder> ChildrenFolders=new List<VirtualFolder>();
        internal List<VirtualFile> ChildrenFiles=new List<VirtualFile>();
        internal VirtualFolder(string Name, IStorageItem Parent)
        {
            _Name = Name;
            _Parent = Parent;
        }
        public IStorageItem Parent => _Parent;

        public string RealPath => throw new System.NotImplementedException();

        public string Name
        {
            get => _Name;
            set => throw new System.NotSupportedException();
        }

        public bool VirtualResource => true;

        public bool NetworkResource => false;

        public void Delete()
        {
            throw new System.NotSupportedException();
        }

        public Task DeleteAsync()
        {
            throw new System.NotSupportedException();
        }

        public IEnumerator<IStorageItem> EnumerateItems()
        {
            foreach (var item in ChildrenFolders)
            {
                yield return item;
            }
            foreach (var item in ChildrenFiles)
            {
                yield return item;
            }

        }


        public bool TryGetItem(string Name, out IStorageItem item)
        {
            foreach (var si in ChildrenFolders)
            {
                if (si.Name == Name)
                {
                    item = si;
                    return true;
                }
            }
            foreach (var si in ChildrenFiles)
            {
                if (si.Name == Name)
                {
                    item = si;
                    return true;
                }
            }
            item = null;
            return false;
        }
    }
}
