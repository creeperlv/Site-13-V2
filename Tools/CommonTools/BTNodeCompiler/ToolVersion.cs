using CLUNL.ConsoleAppHelper;
using Site13Kernel.GameLogic.BT.Serialization;

namespace BTNodeCompiler
{
    [CLUNL.ConsoleAppHelper.DependentVersion("BTNode")]
    public class ToolVersion : IFeatureCollectionVersion
    {
        public string GetVersionString()
        {
            return (typeof(SerializableGraph).Assembly.GetName().Version?? new Version(1,0,0,0)).ToString();
        }
    }

}