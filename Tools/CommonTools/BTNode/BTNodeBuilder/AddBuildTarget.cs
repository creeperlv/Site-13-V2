using BTNode.Core;
using CLUNL.ConsoleAppHelper;
using Site13Kernel.Utilities;
using Site13Project.Core;

namespace BTNodeBuilder
{
    [DependentFeature("BTNodeBuilder", "ADDBUILDTARGET", Description = "", Options = new string[] { "T,TARGET", "C,CONF,CONFIGURATION" },
        OptionDescriptions = new string[] { "Specify a target to build.", "The index of the configuration to operation" })]
    public class AddBuildTarget : IFeature
    {
        public void Execute(ParameterList Parameters, string MainParameter)
        {
            if (MainParameter == null)
            {
                Output.OutLine(new ErrorMsg { ID = "BT.B.ERR.0", Fallback = "A project file must be specified." });
                return;
            }
            if (MainParameter == string.Empty)
            {
                Output.OutLine(new ErrorMsg { ID = "BT.B.ERR.0", Fallback = "A project file must be specified." });
                return;
            }
            if (!File.Exists(MainParameter))
            {
                Output.OutLine(new ErrorMsg { ID = "BT.B.ERR.1", Fallback = "Specified project file does not exist." });
                return;
            }
            var Proj = new LoadedProject(new FileInfo(MainParameter));

            int? C = 0;
            if ((C = Parameters.Query<int>("C")) == null)
            {
                Output.OutLine(new WarnMsg { ID = "BT.B.WARN.0", Fallback = "Configuration not specified, using first configuration by default." });
            }
            string? T = "";
            if ((T = Parameters.Query<string>("T")) == null)
            {
                Output.OutLine(new ErrorMsg { ID = "BT.B.ERR.2", Fallback = "No targegt specified." });
                return;
            }
            var __conf = Proj.Project.Configurations[C ?? 0];
            var item = BuildItem.ObtainItem(new FileInfo(T), Proj);
            if (item.SourceFile is not null)
                __conf.Properties[item.SourceFile] = item.TargetFile!;
            File.Delete(MainParameter);
            File.WriteAllText(MainParameter, JsonUtilities.Serialize(Proj.Project));
            Output.OutLine("BT.INFO.3", "Done.");
        }
    }
}