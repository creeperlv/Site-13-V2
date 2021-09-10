using Site13Kernel.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.UI.Settings
{
    public class SettingsItem : SettingsBehavior
    {
        public string SettingsItemID;
        public Text Description;
        public GameObject Editable;
        IEditable __edit;
        void Start()
        {
            __edit = Editable.GetComponent<IEditable>();
            switch (SettingsItemID)
            {
                case Constants.RenderScale:
                    {
                        __edit.InitValue(1);
                        __edit.SetCallback((v) =>
                        {
                            if (v is float f)
                            {
                                GameRuntime.CurrentGlobals.UsingAsset.renderScale = f;
                                Data.Settings.CurrentSettings.RenderScale=f;
                                Data.Settings.Save();
                            }
                        });
                    }
                    break;
                case Constants.FullScreen:
                    {
                        __edit.InitValue(Screen.fullScreen);
                        __edit.SetCallback((v) =>
                        {
                            if (v is bool b)
                            {
                                Screen.fullScreen = b;
                                Data.Settings.CurrentSettings.FullScreen=b;
                                Data.Settings.Save();
                            }
                        });
                    }
                    break;
                default:
                    break;
            }
        }
        public override void Init()
        {

        }

    }
    public class SettingsBehavior : MonoBehaviour
    {
        public virtual void Init()
        {

        }
    }
}
