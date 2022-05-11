using Site13Kernel.Core.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.AI
{
    public class AlwaysPointToPlayer : MonoBehaviour
    {
        void Start()
        {
        
        }

        
        void Update()
        {
            if (FPSController.Instance != null)
            {
                this.transform.LookAt(FPSController.Instance.MainCam.transform);
            }
        }
    }
}
