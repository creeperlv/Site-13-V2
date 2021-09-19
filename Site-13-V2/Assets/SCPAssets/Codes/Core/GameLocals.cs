using Site13Kernel.Core.Controllers;
using Site13Kernel.Data;
using Site13Kernel.GameLogic.CampaignActions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Core
{
    [Serializable]
    public class GameLocals 
    {
        public EntityController CurrentDefaultController;
        public SerialCampaignScript CurrentScipt;
        public void Destory()
        {
            CurrentDefaultController.DestoryAll();
        }
    }
}
