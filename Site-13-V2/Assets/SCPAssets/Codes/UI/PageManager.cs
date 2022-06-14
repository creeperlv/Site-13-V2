using Site13Kernel.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.UI
{
    public class PageManager : ControlledBehavior
    {
        public List<LogicalPage> ControlledPages;
        public CanvasGroup Cover;
        public float AnimationSpeed = 1;
        public int CurrentIndex = 0;
        public int _CurrentIndex = 0;
        float DeltaT = 1;
        bool B00 = false;
        bool B01 = false;
        public override void Init()
        {
            this.Parent.RegisterRefresh(this);
            foreach (var item in ControlledPages)
            {
                item.ControlledPage.Init();
                item.ControlledPage.ParentManager = this;
            }
        }
        public override void Refresh(float DeltaTime, float UnscaledDeltaTime)
        {
            ControlledPages[CurrentIndex].ControlledPage.Refresh(DeltaTime, UnscaledDeltaTime);

            {
                var T = DeltaTime * AnimationSpeed;
                if (!B00)
                    if (DeltaT > 0)
                        DeltaT -= T;
                if (DeltaT <= 0)
                {

                    if (Cover.gameObject.activeSelf)
                        Cover.gameObject.SetActive(false);
                    Cover.blocksRaycasts = false;
                }
                else
                {
                    if (!Cover.gameObject.activeSelf)
                        Cover.gameObject.SetActive(true);
                    if (!Cover.blocksRaycasts)
                        Cover.blocksRaycasts = true;
                }
                Cover.alpha = DeltaT;
            }

        }
        public void ShowPage(int i)
        {
            CurrentIndex = i;
            StartCoroutine(ToBlack());
        }
        IEnumerator ToBlack()
        {
            if (B01) yield break;
            B01 = true;
            B00 = true;
            while (DeltaT < 1)
            {
                DeltaT += Time.deltaTime * AnimationSpeed;
                yield return null;
            }
            var LP = ControlledPages[_CurrentIndex];
            var NP = ControlledPages[CurrentIndex];
            _CurrentIndex = CurrentIndex;
            if (LP.ControlledPage.gameObject.activeSelf)
            {
                LP.ControlledPage.gameObject.SetActive(false);
            }
            if (LP.Cam.activeSelf)
            {
                LP.Cam.SetActive(false);
            }
            if (!NP.ControlledPage.gameObject.activeSelf)
            {
                NP.ControlledPage.gameObject.SetActive(true);
            }
            if (!NP.Cam.activeSelf)
            {
                NP.Cam.SetActive(true);
            }
            B00 = false;
            B01 = false;
        }
    }
    [Serializable]
    public class LogicalPage
    {
        public GameObject Cam;
        public Page ControlledPage;
        [HideInInspector]
        public float DeltaT;
    }
}
