using CLUNL.ConsoleAppHelper;
using Site13Kernel.Utilities;
using Site13Project.Core;

namespace BTNodeBuilder
{
    [DependentFeature("BTNodeBuilder", "NEWPROJECT", Description = "", Options = new string[] { }, OptionDescriptions = new string[] { })]
    public class CreateProject : IFeature
    {
        public void Execute(ParameterList Parameters, string MainParameter)
        {
            Project p = new Project();
            var predefinedProperties = new Dictionary<string, string>();
            predefinedProperties.Add("Output", "bin");
            predefinedProperties.Add("TargetType", "json");
            p.Configurations.Add(new Configuration() { Properties = predefinedProperties });
            FileInfo fi = new FileInfo(MainParameter);
            var di = fi.Directory;
            if (!di.Exists) di.Create();
            if (File.Exists(MainParameter)) File.Delete(MainParameter);
            File.WriteAllText(MainParameter, JsonUtilities.Serialize(p));
            Output.OutLine("BT.INFO.3", "Done.");
        }
    }
}