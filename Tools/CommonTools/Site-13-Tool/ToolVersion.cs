using CLUNL.ConsoleAppHelper;
using Site13Kernel.Data.Localization;

namespace Site_13_Tool
{
    [DependentVersion("Site-13-Tool")]
    public class ToolVersion : IFeatureCollectionVersion
    {
        public string GetVersionString()
        {
            return typeof(LocalizationDefinition).Assembly.GetName().Version.ToString();
        }
    }
}
