using Microsoft.VisualStudio.TestTools.UnitTesting;
using Site13Kernel.Core.Controllers;
using Site13Kernel.Data;
using Site13Kernel.Diagnostics;
using Site13Kernel.Utilities;

namespace Site13Kernel.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void RandomToolTest()
        {
            RandomTool.Init(100);
            RandomTool.NextInt();
            RandomTool.NextInt(10);
            RandomTool.NextInt(0,1);

            RandomTool.NextInt(1,0,out var result);
        }
        [TestMethod]
        public void DebuggerTest()
        {
            Debugger.CurrentDebugger.Log("Message");
            Debugger.CurrentDebugger.LogWarning("Message");
            Debugger.CurrentDebugger.LogError("Message");
        }
        [DataTestMethod]
        public void DataTest()
        {

            BaseGrenade G = new BaseGrenade();
            G.GrenadeHashCode = 1;
            G.Explosion.CentralDamage = 1;
            G.Explosion.Power = 1;
            G.DetonationDuration = 1;
        }
    }
}
