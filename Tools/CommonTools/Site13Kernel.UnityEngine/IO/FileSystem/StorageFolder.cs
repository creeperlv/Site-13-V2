using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Site13Kernel.IO.FileSystem
{
    public class StorageFolder : IStorageItem, IStorageItem2
    {
        string _Path;
        bool _NetworkResource;
        bool _VirtualResource;
        bool DirectoryInfoWorkflow = false;
        DirectoryInfo DI;
        IStorageItem _Parent;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal StorageFolder(DirectoryInfo directory, IStorageItem Parent)
        {
            DirectoryInfoWorkflow = true;
            DI = directory;
            _NetworkResource = false;
            _VirtualResource = false;
            _Parent = Parent;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal StorageFolder(string AbsolutePath, bool isNetworkResource, bool isVirtualResource, IStorageItem Parent)
        {
            _Path = AbsolutePath;
            _NetworkResource = isNetworkResource;
            _VirtualResource = isVirtualResource;
            _Parent = Parent;
            if (!isNetworkResource)
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
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (DirectoryInfoWorkflow) return DI.Name;
                if (!_NetworkResource && !_VirtualResource)
                {
                    return Path.GetFileName(_Path);
                }
                else
                    return _Path.Split('/').Last();
            }
            set => throw new System.NotImplementedException();
        }

        public bool VirtualResource => _VirtualResource;


        public virtual void Delete()
        {
            if (DirectoryInfoWorkflow)
            {
                DI.Delete(true);
                return;
            }
            if (!_NetworkResource)
            {
                Directory.Delete(_Path, true);
            }
            else
            {

            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual async Task DeleteAsync()
        {
            await Task.Run(() => { Delete(); });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator<IStorageItem> EnumerateItems()
        {
            if (DirectoryInfoWorkflow)
            {
                foreach (var item in DI.EnumerateFiles())
                {
                    yield return new StorageFile(item, this);
                }
                foreach (var item in DI.EnumerateDirectories())
                {
                    yield return new StorageFolder(item, this);
                }
                yield break;
            }
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
            if (!_NetworkResource && !_VirtualResource)
            {
                var __PATH = Path.Combine(DI.FullName, Name);
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
        public StorageFolder CreateOrOpenFolder(string Name)
        {
            if (TryOpenFolder(Name, out var F))
            {
                return F;
            }
            else if (TryCreateFolder(Name, out var F2))
            {
                return F2;
            }
            return null;
        }
        public bool TryCreateFolder(string Name, out StorageFolder folder)
        {
            if (!_NetworkResource && !_VirtualResource)
            {
                var __PATH = Path.Combine(DI.FullName, Name);
                try
                {
                    Directory.CreateDirectory(__PATH);
                    folder = new StorageFolder(__PATH, _NetworkResource, _VirtualResource, this);
                    return true;
                }
                catch (System.Exception)
                {
                }
            }
            folder = null;
            return false;
        }
        public bool TryOpenFolder(string Name, out StorageFolder folder)
        {
            if (!_NetworkResource && !_VirtualResource)
            {
                var __PATH = Path.Combine(DI.FullName, Name);
                if (Directory.Exists(__PATH))
                {
                    folder = new StorageFolder(__PATH, _NetworkResource, _VirtualResource, this);
                    return true;
                }
            }
            folder = null;
            return false;
        }
        public StorageFile CreateOrOpenFile(string Name)
        {
            if (TryOpenFile(Name, out var F))
            {
                return F;
            }
            else if (TryCreateFile(Name, out var F2))
            {
                return F2;
            }
            return null;
        }
        public bool TryCreateFile(string Name, out StorageFile file)
        {
            if (!_NetworkResource && !_VirtualResource)
            {
                var __PATH = Path.Combine(DI.FullName, Name);
                try
                {
                    File.Create(__PATH).Close();
                    file = new StorageFile(__PATH, _NetworkResource, _VirtualResource, this);
                    return true;
                }
                catch (System.Exception)
                {
                }
            }
            file = null;
            return false;
        }
        public bool TryOpenFile(string Name, out StorageFile file)
        {
            if (!_NetworkResource && !_VirtualResource)
            {
                var __PATH = Path.Combine(DI.FullName, Name);
                if (File.Exists(__PATH))
                {
                    file = new StorageFile(__PATH, _NetworkResource, _VirtualResource, this);
                    return true;
                }
            }
            file = null;
            return false;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator StorageFolder(DirectoryInfo di)
        {
            return new StorageFolder(di, null);
        }
    }
}
