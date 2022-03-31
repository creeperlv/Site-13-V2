using Site13Kernel.Core.Controllers;
using System.Runtime.CompilerServices;

namespace Site13Kernel.GameLogic.Effects
{
    public class SideEffectEffect : BaseEffect
    {
        public int ID;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Init()
        {
            EffectController.CurrentEffectController.Spawn(ID, this.transform.position, this.transform.rotation, this.transform.localScale, this.transform.parent,true);
        }
    }
}
