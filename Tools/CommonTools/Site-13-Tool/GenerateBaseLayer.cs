using CLUNL.ConsoleAppHelper;

namespace Site_13_Tool
{
    [DependentFeature("Site-13-Tool",
        "generate-language-base", 
        Description = "Generate the Base Language Definitions", 
        Options = new string[] { "output,o" }, 
        OptionDescriptions = new string[] {"Where to store generated files" })]
    public class GenerateBaseLayer : IFeature
    {
        public void Execute(ParameterList Parameters, string MainParameter)
        {
            string OutputLocation=(string)Parameters.Query("output");
            StandardOutputs.OutputNotImplMsg();
        }
    }
}
