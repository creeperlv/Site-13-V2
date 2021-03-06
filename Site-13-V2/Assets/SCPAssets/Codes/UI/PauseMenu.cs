using Site13Kernel.Core;
using Site13Kernel.Core.CustomizedInput;
using Site13Kernel.GameLogic;
using Site13Kernel.UI.General;
using Site13Kernel.UI.Settings;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Site13Kernel.UI
{
    public class PauseMenu : ControlledBehavior
    {
        public UIButton MainMenu;
        public UIButton Continue;
        public UIButton SettingsButton;
        public static PauseMenu CurrentMenu;
        public override void Init()
        {
            CurrentMenu = this;
            SettingsButton.OnClick = () => {
                StartCoroutine(WaitAndToSettings());
            };
            Continue.OnClick = () =>
            {
                this.gameObject.SetActive(!this.gameObject.activeSelf);
                Toggle(this.gameObject.activeSelf);
            };
            MainMenu.OnClick = () =>
            {
                {
                    this.gameObject.SetActive(!this.gameObject.activeSelf);
                    Toggle(this.gameObject.activeSelf);
                }
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                SceneLoader.Instance.EndLevel();
            };
            Parent.RegisterRefresh(this);
        }
        public IEnumerator WaitAndToSettings()
        {
            GlobalBlackCover.RequestShowCover(0.5f, 0.01f, 0.5f);
            yield return new WaitForSecondsRealtime(0.5f);

            SettingsController.Instance.Show(() => {
                GlobalBlackCover.RequestShowCover(0.5f, 0.01f, 0.5f);
            }, () => {
                this.gameObject.SetActive(true);
            });
            this.gameObject.SetActive(false);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            if (GameRuntime.CurrentGlobals.isInLevel)
            {
                if (InputProcessor.GetInputDown("Esc"))
                {
                    this.gameObject.SetActive(!this.gameObject.activeSelf);
                    Toggle(this.gameObject.activeSelf);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Toggle(bool b)
        {
            GameRuntime.CurrentGlobals.isPaused = b;
            if (b)
            {
                Time.timeScale = 0;
                AudioListener.pause = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                AudioListener.pause = false;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;
                Time.timeScale = 1;
            }
        }
    }
}
