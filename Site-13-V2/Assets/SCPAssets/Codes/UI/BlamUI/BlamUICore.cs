using System.Collections.Generic;

namespace Site13Kernel.UI.BlamUI
{
    public class BlamUICore
    {
        public static BlamUICore Core = new BlamUICore();
        List<BlamControl> controls = new List<BlamControl>();
        public BlamControl SelectedControl;
        public void RegisterOrEnable(BlamControl control)
        {
            controls.Add(control);
        }
        public void Disable(BlamControl control)
        {

        }
        public void Activate(BlamControl control = null)
        {
            if (control == null)
            {
                SelectedControl.ActivateAction();
            }
            else
            {
                control.ActivateAction();
            }
        }
        public void Select(BlamControl control)
        {
            //foreach (BlamControl item in controls)
            //{
            //    if(item == control)
            //    {
            //        item.Select();
            //    }
            //    else
            //    {
            //        item.UnSelect();
            //    }
            //}
            if (SelectedControl == control) { return; }
            if (SelectedControl != null) { SelectedControl.isSelected = false; SelectedControl.UnSelect(); }
            SelectedControl = control;
            control.isSelected = true;
            control.Select();
        }
    }
}
