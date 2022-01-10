using Site13Kernel.Core;
using Site13Kernel.Core.CustomizedInput;
using Site13Kernel.GameLogic;
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
        public static PauseMenu CurrentMenu;
        public override void Init()
        {
            CurrentMenu = this;
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
                SceneLoader.Instance.EndLevel();
            };
            Parent.RegisterRefresh(this);
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
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;
                Time.timeScale = 1;
            }
        }
    }
}
