using Site13Kernel.GameLogic.BT.Serialization;
using Site13Kernel.Utilities;
using Site13Project.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BTNode.Core
{
    [Serializable]
    public class BuildItem
    {
        public string? SourceFile;
        public string? TargetFile;
        public BuildItem()
        {

        }
        public void Build(LoadedProject project)
        {
            var output = CommonProperties.Query(Property.Output, project, "bin");
            var type = CommonProperties.Query(Property.TargetType, project, "json");
            var SG = JsonUtilities.Deserialize<SerializableGraph>(File.ReadAllText(SourceFile));
            var __node = SG.Build();
            string __d = Path.Combine(project.Home!.FullName, output);
            if (!Directory.Exists(output))
            {
                Directory.CreateDirectory(__d);
            }
            string O = Path.Combine(__d, TargetFile);
            switch (type.ToUpper())
            {
                case "JSON":
                    {
                        if (File.Exists(O)) File.Delete(O);
                        File.WriteAllText(O, JsonUtilities.Serialize(__node));
                    }
                    break;
                case "BINARY":
                    {
                        if (!O.ToUpper().EndsWith(".BYTES")) O += ".bytes";
                        if (File.Exists(O)) File.Delete(O);
                        File.WriteAllBytes(O, BinaryUtilities.Serialize(__node));
                    }
                    break;
                default:
                    break;
            }
        }
        public static BuildItem ObtainItem(FileInfo SourceFile, LoadedProject RtProj)
        {
            var BuildItem = new BuildItem();
            var __path = RtProj.MakeRelative(SourceFile);
            BuildItem.SourceFile = __path;
            BuildItem.TargetFile = RtProj.ObtainCurrentConfiguration().Query(__path, RtProj.MakeRelative(SourceFile));
            return BuildItem;
        }
        public static List<BuildItem> Discover(LoadedProject Proj)
        {
            List<BuildItem> BuildItems = new List<BuildItem>();
            Discover(ref BuildItems, Proj.Home!, Proj);
            return BuildItems;
        }
        public static void Discover(ref List<BuildItem> buildItems, DirectoryInfo D, LoadedProject Proj)
        {
            switch (D.Name.ToUpper())
            {
                case "BIN":
                case "OBJ":
                case ".GIT":
                    return;
                default:
                    break;
            }

            foreach (var item in D.EnumerateFiles())
            {
                if (item.Name.ToUpper().EndsWith(".JSON"))
                    buildItems.Add(ObtainItem(item, Proj));
            }
            foreach (var item in D.EnumerateDirectories())
            {
                Discover(ref buildItems, item, Proj);
            }
        }
    }
}
