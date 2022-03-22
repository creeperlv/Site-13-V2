using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Site13Kernel.IO.FileSystem
{
    public class StorageFile : IStorageItem
    {
        string _Path;
        FileInfo _file;
        bool _NetworkResource;
        bool _VirtualResource;
        bool FileInfoWorkflow = false;
        IStorageItem _Parent;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal StorageFile(string AbsolutePath, bool isNetworkResource, bool isVirtualResource, IStorageItem Parent)
        {
            _Path = AbsolutePath;
            _NetworkResource = isNetworkResource;
            _VirtualResource = isVirtualResource;
            _Parent = Parent;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal StorageFile(FileInfo file, IStorageItem Parent)
        {
            _file = file;
            _NetworkResource = false;
            _VirtualResource = false;
            FileInfoWorkflow = true;
            _Parent = Parent;
        }

        public string RealPath
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (FileInfoWorkflow) return _file.FullName;
                return _Path;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (FileInfoWorkflow)
                {
                    _file = new FileInfo(value);
                }
                else
                {
                    _Path = value;
                }
            }
        }

        public bool NetworkResource => _NetworkResource;

        public IStorageItem Parent => _Parent;

        public string Name
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (FileInfoWorkflow)
                    return _file.Name;
                return _Path;
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                if (FileInfoWorkflow)
                {
                    _file.MoveTo(Path.Combine(_file.Directory.FullName, value));
                }
            }
        }
        public bool Exist
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { if (FileInfoWorkflow) return _file.Exists; return File.Exists(_Path); }
        }
        public bool VirtualResource => _VirtualResource;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Delete()
        {
            if (FileInfoWorkflow)
            {
                _file.Delete();
                return;
            }
            if (!_NetworkResource)
            {
                File.Delete(_Path);
            }
            else
            {

            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Stream OpenRead()
        {
            if (FileInfoWorkflow)
                return _file.OpenRead();
            else return new FileStream(_Path, FileMode.Open);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Stream Open()
        {
            if (FileInfoWorkflow) return _file.Open(FileMode.OpenOrCreate);
            else return new FileStream(_Path, FileMode.OpenOrCreate);
        }
        public async Task DeleteAsync()
        {
            await Task.Run(() => { Delete(); });
        }
        public static implicit operator StorageFile(FileInfo fi)
        {
            return new StorageFile(fi, null);
        }
    }
}
