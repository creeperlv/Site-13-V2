using Site13Kernel.Assets.KoFMUST.Codes.Utilities;
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
        public override void Init()
        {

            __edit = Editable.GetComponent<IEditable>();
            switch (SettingsItemID)
            {
                case Constants.RenderScale:
                    {
                        __edit.InitValue(Parent.URP_Asset.renderScale * 100);
                        __edit.SetCallback((v) =>
                        {
                            if (v is float f)
                            {
                                Data.Settings.CurrentSettings.RenderScale = f;
                                f = f / 100f;
                                GameRuntime.CurrentGlobals.UsingAsset.renderScale = f;
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
                                Data.Settings.CurrentSettings.FullScreen = b;
                                Data.Settings.Save();
                            }
                        });
                    }
                    break;
                case Constants.Sound_UI:
                    {

                        __edit.InitValue(Data.Settings.CurrentSettings.UI_SFX);
                        __edit.SetCallback((v) =>
                        {
                            if (v is float f)
                            {
                                AudioUtility.SetVolume(Parent.UI.audioMixer, f);
                                Data.Settings.CurrentSettings.UI_SFX = f;
                                Data.Settings.Save();
                            }
                        });
                    }
                    break;
                case Constants.Sound_UI_BGM:
                    {


                        __edit.InitValue(Data.Settings.CurrentSettings.UI_BGM);
                        __edit.SetCallback((v) =>
                        {
                            if (v is float f)
                            {
                                AudioUtility.SetVolume(Parent.UI_BGM.audioMixer, f);
                                Data.Settings.CurrentSettings.UI_BGM = f;
                                Data.Settings.Save();
                            }
                        });
                    }
                    break;
                case Constants.Sound_SFX:
                    {

                        __edit.InitValue(Data.Settings.CurrentSettings.SFX);
                        __edit.SetCallback((v) =>
                        {
                            if (v is float f)
                            {
                                AudioUtility.SetVolume(Parent.SFX.audioMixer, f);
                                Data.Settings.CurrentSettings.SFX = f;
                                Data.Settings.Save();
                            }
                        });
                    }
                    break;
                case Constants.Sound_BGM:
                    {


                        __edit.InitValue(Data.Settings.CurrentSettings.BGM);
                        __edit.SetCallback((v) =>
                        {
                            if (v is float f)
                            {
                                AudioUtility.SetVolume(Parent.BGM.audioMixer, f);
                                Data.Settings.CurrentSettings.BGM = f;
                                Data.Settings.Save();
                            }
                        });
                    }
                    break;
                case Constants.Resolution:
                    {
                        if (__edit is ComboBox cb)
                        {
                            List<ComboBoxData> items = new List<ComboBoxData>();
                            foreach (var item in Screen.resolutions)
                            {
                                items.Add(new ComboBoxData { text = $"{item.width} x {item.height}" });
                            }
                            cb.AddOptions(items);
                        }
                        for (int i = 0; i < Screen.resolutions.Length; i++)
                        {
                            var r = Screen.resolutions[i];
                            if (r.width == Data.Settings.CurrentSettings.WINDOW_W && r.height == Data.Settings.CurrentSettings.WINDOW_H)
                            {
                                __edit.InitValue(i);
                                break;
                            }
                        }
                        __edit.SetCallback((v) =>
                        {
                            if (v is int i)
                            {
                                //AudioUtility.SetVolume(Parent.BGM.audioMixer, f);
                                int W = Screen.resolutions[i].width;
                                int H = Screen.resolutions[i].height;
                                Screen.SetResolution(W, H, Screen.fullScreen);
                                Data.Settings.CurrentSettings.WINDOW_W = W;
                                Data.Settings.CurrentSettings.WINDOW_H = H;
                                Data.Settings.Save();
                            }
                        });
                    }
                    break;
                case Constants.TextureQuality:
                    {
                        Debug.Log("Texture: " + QualitySettings.masterTextureLimit);
                        __edit.InitValue(3 - QualitySettings.masterTextureLimit);
                        __edit.SetCallback((v) =>
                        {
                            if (v is float f)
                            {
                                var val = (int)Mathf.Abs((int)f - 3);
                                QualitySettings.masterTextureLimit = val;
                                Data.Settings.CurrentSettings.TextureQuality = val;
                                Data.Settings.Save();
                            }
                        });
                    }
                    break;
                case Constants.Control_MouseSensitivity:
                    {
                        {
                            __edit.InitValue(Data.Settings.CurrentSettings.MouseSensibly);
                            __edit.SetCallback((v) =>
                            {
                                if (v is float f)
                                {
                                    Data.Settings.CurrentSettings.MouseSensibly = f;
                                    Data.Settings.Save();
                                }
                            });
                        }
                    }
                    break;
                case Constants.RenderShadow:
                    {
                        {
                            __edit.InitValue(Data.Settings.CurrentSettings.RenderShadow);
                            __edit.SetCallback((v) =>
                            {
                                if (v is bool b)
                                {
                                    Data.Settings.CurrentSettings.RenderShadow = b;
                                    Data.Settings.Save();
                                }
                            });
                        }
                    }
                    break;
                default:
                    break;
            }
        }

    }
    public class SettingsBehavior : MonoBehaviour
    {
        public SettingsController Parent;
        public virtual void Init()
        {

        }
    }
}
