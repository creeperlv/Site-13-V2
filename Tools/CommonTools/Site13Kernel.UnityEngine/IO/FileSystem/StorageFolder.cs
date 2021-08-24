using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Site13Kernel.IO.FileSystem
{
    public class StorageFolder : IStorageItem, IStorageItem2
    {
        string _Path;
        bool _NetworkResource;
        bool _VirtualResource;
        DirectoryInfo DI;
        IStorageItem _Parent;
        internal StorageFolder(string AbsolutePath, bool isNetworkResource,bool isVirtualResource, IStorageItem Parent)
        {
            _Path = AbsolutePath;
            _NetworkResource = isNetworkResource;
            _VirtualResource = isVirtualResource;
            _Parent = Parent;
            if (isNetworkResource)
            {
                DI = new DirectoryInfo(AbsolutePath);
            }
        }

        public string RealPath => _Path;

        public bool NetworkResource => _NetworkResource;
        /// <summary>
        /// Root if it is null.
        /// </summary>
        public IStorageItem Parent => _Parent;

        public string Name
        {
            get  {
                if (!_NetworkResource && !_VirtualResource)
                {
                    return Path.GetFileName(_Path);
                }
                else
                    return _Path.Split('/').Last();
            }
            set => throw new System.NotImplementedException();
        }

        public bool VirtualResource => throw new System.NotImplementedException();

        public virtual void Delete()
        {
            if (!_NetworkResource)
            {
                Directory.Delete(_Path, true);
            }
            else
            {

            }
        }

        public virtual async Task DeleteAsync()
        {
            await Task.Run(() => { Delete(); });
        }

        public IEnumerator<IStorageItem> EnumerateItems()
        {
            if (!_NetworkResource && !_VirtualResource)
            {
                foreach (var item in Directory.EnumerateDirectories(_Path))
                {
                    yield return new StorageFolder(Path.Combine(_Path, item), false, false, this);
                }
                foreach (var item in Directory.EnumerateFiles(_Path))
                {
                    yield return new StorageFile(Path.Combine(_Path, item), false, false, this);
                }
            }
            else
                yield break;
        }

        public virtual bool TryGetItem(string Name, out IStorageItem item)
        {
            if (!_NetworkResource&&!_VirtualResource)
            {
                var __PATH=Path.Combine(DI.FullName,Name);
                if (File.Exists(__PATH))
                {
                    item = new StorageFile(__PATH, _NetworkResource, _VirtualResource, this);
                    return true;
                }
                else if (Directory.Exists(__PATH))
                {
                    item = new StorageFolder(__PATH, _NetworkResource, _VirtualResource, this);
                    return true;
                }
                else
                {
                    item = null;
                    return false;
                }
            }
            else
            {
                item = null;
                return false;
            }
        }
    }
}
