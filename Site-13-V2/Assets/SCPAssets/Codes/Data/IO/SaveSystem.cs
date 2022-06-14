using Site13Kernel.GameLogic.RuntimeScenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Site13Kernel.Data.IO
{
    public class SaveSystem : MonoBehaviour
    {
        public static SaveSystem Instance;
        public void Start()
        {
            Instance = this;
        }
        List<IPureData> Datas = new List<IPureData>();
        public void Collect()
        {
            Datas.Clear();
            Datas.Add(LevelRuntimeRegistry.Instance.ObtainData());
            Datas.Add(TrackingPredefinedObjects.Instance.ObtainData());
        }

    }
}
