using Site13Kernel.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.UI.Settings
{
    public class SettingsItem : MonoBehaviour
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
                            }
                        });
                    }
                    break;
                default:
                    break;
            }
        }

    }
}
