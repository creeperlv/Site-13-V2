using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.UI.HUD
{
    public class MedalItem : MonoBehaviour
    {
        public Image ControlledImage;
        public Transform ControlledTransform;
        float DT;
        public float UpperScale = 1.5f;
        public float AnimeTime = 0.2f;
        public bool isDone = false;
        public void upd(float deltaT)
        {
            if (isDone) return;
            if (DT < AnimeTime)
            {
                float inten = DT / AnimeTime;
                var c=ControlledImage.color;
                c.a = inten;
                ControlledImage.color = c;
                var s = (UpperScale+1) - inten* UpperScale;
                ControlledTransform.localScale = new Vector3(s, s, s);
            }else if (DT > AnimeTime && DT < 3f)
            {
                var c = ControlledImage.color;
                c.a = 1;
                ControlledImage.color = c;
                ControlledTransform.localScale = Vector3.one;
            }
            else if (DT > 3f&&DT<3+ AnimeTime)
            {
                float inten = (DT - 3) / AnimeTime;
                var c = ControlledImage.color;
                c.a = 1-inten;
                ControlledImage.color = c;
                var s = 1+inten * UpperScale;
                ControlledTransform.localScale = new Vector3(s, s, s);
            }
            else
            {
                var c = ControlledImage.color;
                c.a = 0;
                ControlledImage.color = c;


                isDone = true;
            }
            DT += deltaT;
        }
    }
}
