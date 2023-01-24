using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel
{
    public class xUIWindowImpl : MonoBehaviour
    {
        public RectTransform ControlledTransform;
        public xUIResizableBorder Border;
        public Vector2 MinSize = new Vector2(300, 50);
        void Start()
        {
            Border.OnSizeChange = (v) => {
                if (v.y < MinSize.y)
                {
                    var s=ControlledTransform.sizeDelta;
                    s.y = MinSize.y;
                    ControlledTransform.sizeDelta = s;
                }
                if (v.x < MinSize.x)
                {
                    var s = ControlledTransform.sizeDelta;
                    s.x = MinSize.x;
                    ControlledTransform.sizeDelta = s;
                }
            };
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
