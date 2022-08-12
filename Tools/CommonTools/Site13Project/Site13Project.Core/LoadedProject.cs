using Site13Kernel.Utilities;
using System.Collections.Generic;
using System.IO;

namespace Site13Project.Core
{
    public class LoadedProject
    {
        public FileInfo UsingProjectFile;
        public Project Project;
        private DirectoryInfo? home;
        public List<string> Conditions = new List<string>();
        Configuration? __config = null;
        public void AddCondition(string value)
        {
            Conditions.Add(value);
            __config = null;
        }
        public void RemoveCondition(string value)
        {
            Conditions.Remove(value);
            __config = null;
        }
        public void ClearCondition()
        {
            Conditions.Clear();
            __config = null;
        }
        public Configuration ObtainCurrentConfiguration()
        {
            if (__config is null)
            {
                __config = Project.ObtainCurrent(Conditions.ToArray());
            }
            return __config;
        }
        public LoadedProject(FileInfo usingProjectFile)
        {
            UsingProjectFile = usingProjectFile;
            Home = usingProjectFile.Directory;
            Project = JsonUtilities.Deserialize<Project>(File.ReadAllText(usingProjectFile.FullName));
        }
        public string MakeRelative(FileInfo fileInfo)
        {
            if (home == null) return "";
            return Path.GetRelativePath(home.FullName, fileInfo.FullName);
        }
        public DirectoryInfo? Home { get => home; private set => home = value; }
    }
}
