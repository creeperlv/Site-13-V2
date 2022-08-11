using CLUNL.ConsoleAppHelper;
namespace BTNodeBuilder
{
    [DependentFeature("BTNodeBuilder", "BUILD", Description = "Build a project", Options = new string[] { "C" },
        OptionDescriptions = new string[] { "Conditions, use like -c:DEBUG;TRACE " })]
    public class Build : IFeature
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
            Site13Project.Core.LoadedProject? LP = null;
            try
            {
                LP = new Site13Project.Core.LoadedProject(new FileInfo(MainParameter));
            }
            catch (Exception e)
            {
                Output.OutLine(new ErrorMsg { ID = "BT.B.ERR.1", Fallback = "Specified project file cannot be loaded." });
                Output.OutLine(new ErrorMsg { ID = "ST", Fallback = e.Message });
                return;
            }
            if(LP is not null)
            {
                
            }
        }
    }
}