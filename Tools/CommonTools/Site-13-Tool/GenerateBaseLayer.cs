using CLUNL.ConsoleAppHelper;
using Site13Kernel.Tools.Data.Localization;
using System;
using System.IO;

namespace Site_13_Tool
{
    [DependentFeature("Site-13-Tool",
        "generate-language-base",
        Description = "Generate the Base Language Definitions",
        Options = new string[] { "output,o" },
        OptionDescriptions = new string[] { "Where to store generated files" })]
    public class GenerateBaseLayer : IFeature
    {
        public void Execute(ParameterList Parameters, string MainParameter)
        {
            /**
             *
             * ROOT\Locales\
             *              Locales.json
             *              en-us\
             *                    Locale.json
             *                    Language.lang
             *                    ... Other Resources
             *              ... Other Language Codes
             * 
             **/
            var obj = Parameters.Query("output");
            if (obj is bool b)
            {
                if (!b)
                {
                    Output.OutLine(new ErrorMsg { ID = "Error.000", Fallback = "Parameter is not correct." });
                    return;
                }
            }
            string OutputLocation = (string)obj;
            if (OutputLocation == "")
            {

                Output.OutLine(new ErrorMsg { ID = "Error.000", Fallback = "Parameter is not correct." });
                return;
            }
            DirectoryInfo directory = new DirectoryInfo(OutputLocation);
            if (File.Exists(OutputLocation))
            {
                Output.OutLine(new ErrorMsg { ID = "Error.001", Fallback = "Specified location is invalidate." });
                return;
            }
            try
            {
                if (!directory.Exists)
                {
                    directory.Create();
                    if (!directory.Exists)
                    {
                        Output.OutLine(new ErrorMsg { ID = "Error.001", Fallback = "Specified location is invalidate." });
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                Output.OutLine($"Fetal Error:{e}");
                return;
            }
            var LOCALE= LanguageTools.GenerateLocalizationBase();

            LOCALE.Item1.InstalledLocalizations.Add(LOCALE.Item2.LanguageCode, LOCALE.Item2.LanguageStringFile);
            StandardOutputs.OutputNotImplMsg();
        }
    }
}
