using CLUNL.ConsoleAppHelper;

namespace Site_13_Tool
{
    [DependentVersion("Site-13-Tool")]
    public class ToolVersion : IFeatureCollectionVersion
    {
        public string GetVersionString()
        {
            return this.GetType().Assembly.GetName().Version.ToString();
        }
    }
}
