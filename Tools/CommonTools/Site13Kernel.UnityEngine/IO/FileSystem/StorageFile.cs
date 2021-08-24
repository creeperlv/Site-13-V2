using System.IO;
using System.Threading.Tasks;

namespace Site13Kernel.IO.FileSystem
{
    public class StorageFile : IStorageItem
    {
        string _Path;
        bool _NetworkResource;
        bool _VirtualResource;
        IStorageItem _Parent;
        internal StorageFile(string AbsolutePath,bool isNetworkResource,bool isVirtualResource, IStorageItem Parent)
        {
            _Path = AbsolutePath;
            _NetworkResource = isNetworkResource;
            _VirtualResource = isVirtualResource;
            _Parent = Parent;
        }

        public string RealPath => _Path;

        public bool NetworkResource => _NetworkResource;

        public IStorageItem Parent => _Parent;

        public string Name
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }

        public bool VirtualResource => _VirtualResource;

        public void Delete()
        {
            if (!_NetworkResource)
            {
                File.Delete(_Path);
            }
            else
            {
            
            }
        }
        public async Task DeleteAsync()
        {
            await Task.Run(() => { Delete(); });
        }
    }
}
