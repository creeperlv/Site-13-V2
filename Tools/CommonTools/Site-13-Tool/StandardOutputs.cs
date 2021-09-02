using CLUNL.ConsoleAppHelper;

namespace Site_13_Tool
{
    public static class StandardOutputs
    {
        public static void OutputNotImplMsg()
        {
            Output.OutLine(new WarnMsg { ID = "Warning.000", Fallback = "This feature has not been implemented yet." });
        }
    }
}
