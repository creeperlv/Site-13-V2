using Site13Kernel.Authentication;
using System;
using System.IO;
using System.Text;

namespace Site13Kernel.Registry
{
    public class RegistryCore
    {
        public static RegistryCore Instance;
        DirectoryInfo Root;
        public RegistryCore(DirectoryInfo Root)
        {
            this.Root = Root;
        }
        public static RegistryItem Query(User user, string path)
        {
            if (Instance == null) return null;
            return Instance.QueryItem(user, path);
        }
        public RegistryItem QueryItem(User user, string path)
        {
            if (path.Contains("..")) throw new PathEscapeException(path);
            string _path = PathCorrection(user, path);
            string _file = Path.Combine(Root.FullName, _path);
            if (File.Exists(_file))
            {
                RegistryItem item = RegistryItem.ReadFromFile(_file);
                return item;
            }
            else return null;
        }
        public void Write(User user, string path, RegistryItem item)
        {

            if (path.Contains("..")) throw new PathEscapeException(path);
            string _path = PathCorrection(user, path);
            string _file = Path.Combine(Root.FullName, _path);
            FileInfo fi = new FileInfo(_file);
            var d = fi.Directory;
            if (!d.Exists) d.Create();
            item.WriteToFile(_file);
        }
        string PathCorrection(User user, string path)
        {
            if (path.StartsWith("LOCAL_MACHINE"))
            {
                return path;
            }
            else if (path.StartsWith("CURRENT_USER"))
            {
                path.Replace("CURRENT_USER", $"Users/{user.UserID}");
            }
            else if (path.StartsWith("USERS"))
            {
                path.Replace("USERS", $"Users");
            }
            return path;
        }
    }

    [Serializable]
    public class PathEscapeException : Exception
    {
        public PathEscapeException(string path) : base($"Path escape detected in:{path}") { }
        public PathEscapeException(string path, Exception inner) : base($"Path escape detected in:{path}", inner) { }
    }
    public class RegistryItem
    {
        public DataType DataType;
        public byte[] Data;
        public static RegistryItem FromDWORD(int d)
        {
            RegistryItem item = new RegistryItem();
            item.DataType = DataType.DWORD;
            item.Data = BitConverter.GetBytes(d);
            return item;
        }
        public static RegistryItem FromQWORD(long d)
        {
            RegistryItem item = new RegistryItem();
            item.DataType = DataType.QWORD;
            item.Data = BitConverter.GetBytes(d);
            return item;
        }
        public static RegistryItem FromString(string str)
        {
            RegistryItem item = new RegistryItem();
            item.DataType = DataType.STR;
            item.Data = Encoding.Default.GetBytes(str);
            return item;
        }
        public void WriteToFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (Stream s = File.OpenWrite(path))
            {
                s.WriteByte((byte)(int)DataType);
                if (DataType == DataType.STR)
                {
                    s.Write(BitConverter.GetBytes(Data.Length));
                }
                s.Write(Data);
            }
        }
        public object GetValue()
        {
            switch (DataType)
            {
                case DataType.DWORD:
                    return BitConverter.ToInt32(Data);
                case DataType.QWORD:
                    return BitConverter.ToInt64(Data);
                case DataType.STR:
                    return Encoding.Default.GetString(Data);
                default:
                    break;
            }
            return null;
        }
        public static RegistryItem ReadFromFile(string path)
        {
            using (Stream s = File.OpenRead(path))
            {
                RegistryItem registryItem = new RegistryItem();
                registryItem.DataType = (DataType)s.ReadByte();
                switch (registryItem.DataType)
                {
                    case DataType.DWORD:
                        registryItem.Data = new byte[4];
                        s.Read(registryItem.Data, 0, 4);
                        break;
                    case DataType.QWORD:
                        registryItem.Data = new byte[8];
                        s.Read(registryItem.Data, 0, 8);
                        break;
                    case DataType.STR:
                        {
                            byte[] b = new byte[4];
                            s.Read(b, 0, 4);
                            int length = BitConverter.ToInt32(b);
                            registryItem.Data = new byte[length];
                            s.Read(registryItem.Data, 0, length);
                        }
                        break;
                    default:
                        break;
                }
                return registryItem;
            }
        }
    }
    public enum DataType
    {
        DWORD, QWORD, STR
    }
}
