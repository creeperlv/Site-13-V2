using CLUNL.Localization;
using Site13Kernel.SceneBuild;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.UI.Forge
{
    public class ForgeComponentEditorBase : MonoBehaviour
    {
        public ToggleButton Header;
        public List<GameObject> TogglableFields;
        public LocalizedString Name;
        [HideInInspector]
        public ForgeSystem CurrentSystem;
        public virtual void Init()
        {
            Header.Content = Name;
            Header.OnToggle = (v) => {
                foreach (var item in TogglableFields)
                {
                    item.gameObject.SetActive(v);
                }
                CurrentSystem.ForceObjectMenuUpdate();
            };
        }
        public virtual void SetComponent(EditableComponent component)
        {
        }
    }
}