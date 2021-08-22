using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.UI
{
    [Serializable]
    public class ButtonGroup
    {
        public List<ToggleButton> Buttons=new List<ToggleButton>();
        public Action<int> OnSelected=null;
        public void Init()
        {
            var index=0;
            var Selected=0;
            foreach (var item in Buttons)
            {
                var i=index;
                item.OnToggle = (b) =>
                {
                    //item.IsOn = true;
                    if (b == true)
                    {
                        Selected = i;
                        UnselectOthers(item);
                        if (OnSelected != null)
                        {
                            OnSelected(i);
                        }
                    }
                    else
                    {
                        if (Selected == i)
                        {
                            item.IsOn = true;
                        }
                    }
                };
                index++;
            }
        }
        public void UnselectOthers(ToggleButton Exception)
        {
            foreach (var item in Buttons)
            {
                if (item != Exception)
                    item.IsOn = false;
            }
        }
    }
}
