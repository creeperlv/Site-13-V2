using Site13Kernel.Core;

namespace Site13Kernel.UEFI
{
    public class ParameterProcessor : ControlledBehavior
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns>True to interrupt boot process (will not enter splash screen.)</returns>
        public virtual bool Process(string[] args)
        {
            return false;
        }
    }
}
