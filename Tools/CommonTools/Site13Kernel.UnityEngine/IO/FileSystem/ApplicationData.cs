using System.IO;
using UnityEngine;

namespace Site13Kernel.IO.FileSystem
{
    public class ApplicationData
    {
        public static StorageFolder LocalFolder;
        public static VirtualFolder BaseFolder;
        public static bool GetItem(string Path, out IStorageItem Item)
        {
            Path = Path.Replace("\\", "/");
            if (Path.StartsWith('/'))
            {
                Path = "ROOT" + Path;
            }
            else if (!Path.StartsWith("ROOT/"))
            {
                Path = "ROOT/" + Path;
            }
            var paths=Path.Split('/');
            IStorageItem2 Parent=LocalFolder;
            for (int i = 0; i < paths.Length; i++)
            {
                using (var SI2 = Parent.EnumerateItems())
                {
                    bool Hitted=false;
                    do
                    {
                        if (SI2.Current.Name == paths[i])
                        {
                            if (SI2.Current is IStorageItem2 item2)
                            {
                                Hitted = true;
                                Parent = item2;
                                break;
                            }
                            else if (SI2.Current is IStorageItem item)
                            {
                                if (i == paths.Length - 1)
                                {
                                    Item = item;
                                    return true;
                                }
                            }
                        }
                    }
                    while (SI2.MoveNext());
                    if (!Hitted)
                    {
                        if (Parent is VirtualFolder)
                        {
                            Item = null;
                            return false;
                        }
                        else
                        {
                            i = 0;
                            Parent = BaseFolder;
                        }
                    }
                }

            }
            Item = null;
            return false;
        }
        public static void Init(BaseMap BaseFolderMapping = null)
        {
            var Data=Path.Combine(Application.persistentDataPath,"ROOT");
            if (!Directory.Exists(Data))
            {
                Directory.CreateDirectory(Data);
            }
            LocalFolder = new StorageFolder(Data, false, false, null);
            if (BaseFolderMapping != null)
            {
                BaseFolder = new VirtualFolder("ROOT", null);
                foreach (var item in BaseFolderMapping.VirtualFileNodes)
                {
                    RecursiveInit(BaseFolder, item);
                }
            }
        }
        static void RecursiveInit(VirtualFolder Parent, VirtualFileNode node)
        {
            if (node.isEndPoint)
            {
                Parent.ChildrenFiles.Add(new VirtualFile(node.Name, Parent, node.obj));
            }
            else
            {
                var VF=new VirtualFolder(node.Name, Parent);
                Parent.ChildrenFolders.Add(VF);
                foreach (var item in node.Children)
                {
                    RecursiveInit(VF, item);
                }
            }
        }
    }
}
