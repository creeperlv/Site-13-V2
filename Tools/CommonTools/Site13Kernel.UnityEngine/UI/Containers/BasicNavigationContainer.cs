using Site13Kernel.Core;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Site13Kernel.UI.Containers
{
    public class BasicNavigationContainer : ControlledBehavior
    {
        public List<NavigatableItem> Children;
        public bool initOnStart = false;
        public float AnimationSpeed = 1;
        int SelectedIndex = 0;
        public int LastSelectedIndex = 0;
        bool __inited = false;
        public void Start()
        {
            if (initOnStart) __init();
        }
        public void Update()
        {
            float DeltaTime = Time.deltaTime;
            var _DeltaTime = DeltaTime * AnimationSpeed;
            for (int i = 0; i < Children.Count; i++)
            {
                if (SelectedIndex == i)
                {
                    Children[i].Show(_DeltaTime);
                }
                else
                {
                    Children[i].Hide(_DeltaTime);

                }
            }

        }
        void __init()
        {
            if (__inited) return;
            for (int i = 0; i < Children.Count; i++)
            {
                var item = Children[i];
                int _i = i;
                Site13Event clicked = new Site13Event();
                clicked.Add(() => {
                    SelectedIndex = _i;
                });
                foreach (var access_p in item.AccessPoints)
                {
                    access_p.OnClick = clicked;
                }
            }
            __inited = true;
        }

        internal void UpdateLastIndex()
        {
            LastSelectedIndex = SelectedIndex;
        }
    }
}
