using Site13Kernel.Core;
using Site13Kernel.GameLogic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Site13Kernel.UI
{
    public class FirstScreen : ControlledBehavior
    {
        public Image Cover;
        public TMPro.TMP_Text HintText;
        Color c1;
        Color c2;
        public AudioSource MainUIBGM;
        public AudioSource Wooon;
        public List<int> BGSceneIDs;
        public override void Init()
        {
            c1 = Cover.color;
            c2 = HintText.color;
            if (GameRuntime.CurrentGlobals.MainUIBGM == null)
            {
                DontDestroyOnLoad(MainUIBGM);
                GameRuntime.CurrentGlobals.MainUIBGM = MainUIBGM;
                GameRuntime.CurrentGlobals.MainUIBGM.Play();
            }
            else
            {
                MainUIBGM.Stop();

            }
            Parent.RegisterRefresh(this);
            foreach (var item in BGSceneIDs)
            {
                SceneLoader.Instance.ShowScene(item);
            }
        }
        int State0 = 0;
        bool State1 = false;
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            if (State0 < 2)
                if (Input.anyKeyDown)
                {
                    Wooon.Play();
                    HintText.gameObject.SetActive(false);
                    State0 = 2;
                }
            if (State1)
            {
                c2.a -= DeltaTime;
                HintText.color = c2;
                if (c2.a <= 0)
                    State1 = !State1;
            }
            else
            {
                c2.a += DeltaTime;
                HintText.color = c2;
                if (c2.a >= 1)
                    State1 = !State1;
            }

            switch (State0)
            {
                case 0:
                    {
                        c1.a -= DeltaTime;
                        Cover.color = c1;
                        if (c1.a <= 0)
                        {
                            State0 = 1;
                        }
                    }
                    break;
                case 1:
                    {

                    }
                    break;
                case 2:
                    {

                        c1.a += DeltaTime;
                        Cover.color = c1;
                        if (c1.a >= 1)
                        {
                            State0 = 3;
                        }
                    }
                    break;
                case 3:
                    {
                        SceneLoader.Instance.LoadScene(GameRuntime.CurrentGlobals.MainMenuSceneID, true, false, false);
                        Parent.UnregisterRefresh(this);
                    }
                    break;
                default:
                    break;
            }
        }
    }

}