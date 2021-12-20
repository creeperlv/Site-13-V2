using Site13Kernel.Core.Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.Props
{
    public class DoorDiscoverer : MonoBehaviour
    {
        public bool DiscoverOnStart=false;
        public BaseController DoorController;
        public GameObject LookUpRoot;
        void Start()
        {
            if (DiscoverOnStart)
            {
                Discover();
            }
        }
        public void Discover()
        {
            var DOORS=LookUpRoot.GetComponentsInChildren<BaseDoor>();
            foreach (var DOOR in DOORS)
            {
                if (DOOR.isDiscoverable == false) continue;
                DOOR.Parent = DoorController;
                DOOR.Init();
                DoorController.RegisterRefresh(DOOR);
            }
        }
       
    }
}
