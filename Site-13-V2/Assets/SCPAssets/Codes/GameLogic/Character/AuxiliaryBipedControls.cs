using Site13Kernel.GameLogic.FPS;
using UnityEngine;

namespace Site13Kernel.GameLogic.Character
{
    public class AuxiliaryBipedControls : MonoBehaviour
    {
        public MeleeArea meleeArea;
        public void MeleeStart()
        {
            meleeArea.StartDetection();
        }
        public void MeleeStop()
        {
            meleeArea.StopDetection();
        }
    }
}
