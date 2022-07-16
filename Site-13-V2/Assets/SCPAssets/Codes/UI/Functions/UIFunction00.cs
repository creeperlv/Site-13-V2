using CLUNL.Localization;
using Site13Kernel.Core;
using Site13Kernel.GameLogic;
using Site13Kernel.UI;
using Site13Kernel.UI.General;
using Site13Kernel.UI.Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel
{
    public class UIFunction00 : Page
    {
        public int CustomizationPage = 1;
        public float FadeDuration = 0.5f;
        static LocalizedString DialogExitTitle = new LocalizedString("Dialog.Exit.Title", "Are you sure to exit?");
        static LocalizedString Yes = new LocalizedString("Dialog.Yes", "Yes");
        static LocalizedString No = new LocalizedString("Dialog.No", "No");
        public void ShowCustomizationPage()
        {
            ParentManager.ShowPage(CustomizationPage);
        }
        public void RequestExitDialog()
        {
            DialogManager.Show(DialogExitTitle, "", Yes, () => { Application.Quit(); }, No, null);
        }
        public void EnterForgeUI()
        {
            if (SceneLoader.Instance.LoadScene("FORGEUI", true, false, false))
            {
            }
            else
            {
                DialogManager.Show("FORGE NOT ENABLED.", "FORGE FEATURE IS NOT ENABLED IN THIS VERSION OF SITE-13.", "OK", () => { });
            }
        }
        public void ToSettings()
        {
            StartCoroutine(WaitAndToSettings());
        }
        public IEnumerator WaitAndToSettings()
        {
            GlobalBlackCover.RequestShowCover(0.5f, 0.01f, 0.5f);
            yield return new WaitForSeconds(FadeDuration);

            SettingsController.Instance.Show(() => {
                GlobalBlackCover.RequestShowCover(0.5f, 0.01f, 0.5f);
            }, () => {
                this.gameObject.SetActive(true);
            });
            this.gameObject.SetActive(false);
        }
        public void EnterFirefightUI()
        {

            if (SceneLoader.Instance.LoadScene("FIREFIGHTUI", true, false, false))
            {
                GameRuntime.CurrentGlobals.MainUIBGM.Stop();
            }
            else
            {
                DialogManager.Show("FIREFIGHT NOT ENABLED.", "FIREFIGHT FEATURE IS NOT ENABLED IN THIS VERSION OF SITE-13.", "OK", () => { });
            }
        }
    }
}
