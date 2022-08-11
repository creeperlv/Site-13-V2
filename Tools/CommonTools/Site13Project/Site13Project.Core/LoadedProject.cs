using Site13Kernel.Utilities;
using System.IO;

namespace Site13Project.Core
{
    public class LoadedProject
    {
        public FileInfo UsingProjectFile;
        public Project Project;
        public LoadedProject(FileInfo usingProjectFile)
        {
            UsingProjectFile = usingProjectFile;
            Project = JsonUtilities.Deserialize<Project>(File.ReadAllText(usingProjectFile.FullName));
        }
    }
}
