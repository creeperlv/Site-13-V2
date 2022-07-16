using System.Collections.Generic;

namespace Site13Kernel.UI.Elements
{
    public class RadioButtonGroupOverToggleButton
    {
        List<ToggleButton> buttons = new List<ToggleButton>();
        public void RegisterButton(ToggleButton tb)
        {
            buttons.Add(tb);
            tb.Checked.Add(() => {
                UncheckAll(tb);
            });
        }
        public bool FirstButton(out ToggleButton firstButton)
        {
            if(buttons.Count == 0)
            {
                firstButton = null;
                return false;
            }
            else
            {
                firstButton=buttons[0];
                return true;
            }
        }
        void UncheckAll(ToggleButton tb = null)
        {
            foreach (var item in buttons)
            {
                if(item != tb)
                {
                    item.isOn = false;
                }
            }
        }
    }
}
