using Microsoft.VisualStudio.TestTools.UnitTesting;
using Site13Kernel.UI.xUI;

namespace Site13Kernel.Tests
{
    [TestClass]
    public class xUITest
    {
        [TestMethod]
        public void composer_test()
        {
            string UIContent = 
                @"<Window Title=""Sample"">
<Grid>
    <Text>Sample Text</Text>
</Grid>
</Window>";
            var a=UIComposer.Parse(UIContent);
        }
    }
}
