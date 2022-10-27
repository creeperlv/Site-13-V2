using Site13Kernel.GameLogic.Effects;
using Site13Kernel.GameLogic.FPS;
using UnityEngine;

namespace Site13Kernel.GameLogic.Character
{
    public class AuxiliaryBipedControls : MonoBehaviour
    {
        public MeleeArea meleeArea;
        public CameraShakeEffect CSE;
        public float CSE_Run_Intensity = 1;
        public float CSE_Run_H = 1;
        public float CSE_Run_V = 1;
        public float CSE_Run_R = 25;
        public float CSE_Run_DimIntensity = 3;


        public float CSE_Melee_Intensity = 1;
        public float CSE_Melee_H = 1;
        public float CSE_Melee_V = 1;
        public float CSE_Melee_R = 25;
        public float CSE_Melee_DimIntensity = 3;
        public bool CSE_Melee_isR = false;

        public float CSE_Walk_Intensity = 1;
        public float CSE_Walk_H= 1;
        public float CSE_Walk_V= 1;
        public float CSE_Walk_R = 25;
        public float CSE_Walk_DimIntensity = 2;
        public void MeleeStart()
        {
            meleeArea.StartDetection();
        }
        public void ShakeCamRunStep()
        {
            
            CSE.SetShake(Intensity: CSE_Run_Intensity, willDiminish: true, DiminishIntensity: CSE_Run_DimIntensity, true, 
                RotationShakeSpeed: CSE_Run_R,HorizontalBaseIntensity:CSE_Run_H,VerticalBaseIntensity:CSE_Run_V);
        }
        public void ShakeCamWalkStep()
        {
            CSE.SetShake(Intensity: CSE_Walk_Intensity, willDiminish: true, DiminishIntensity: CSE_Walk_DimIntensity, true,
                RotationShakeSpeed: CSE_Walk_R, HorizontalBaseIntensity: CSE_Walk_H, VerticalBaseIntensity: CSE_Walk_V);
        }
        public void ShakeCamMelee()
        {
            CSE.SetShake(Intensity: CSE_Melee_Intensity, willDiminish: true, DiminishIntensity: CSE_Melee_DimIntensity, CSE_Melee_isR,
                RotationShakeSpeed: CSE_Melee_R, HorizontalBaseIntensity: CSE_Melee_H, VerticalBaseIntensity: CSE_Melee_V);
        }
        public void MeleeStop()
        {
            meleeArea.StopDetection();
        }
    }
}
