using CLUNL.ConsoleAppHelper;
using Site13Kernel.Utilities;

namespace BTNodeBuilder
{
    [DependentVersion("BTNodeBuilder")]
    public class VersionProvider : IFeatureCollectionVersion
    {
        public string GetVersionString()
        {
            return (typeof(JsonUtilities).Assembly.GetName().Version??new Version(1,0,0,0)).ToString();
        }
    }
}