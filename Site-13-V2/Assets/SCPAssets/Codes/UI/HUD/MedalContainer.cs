using CLUNL.Localization;
using Site13Kernel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.UI.HUD
{
    public class MedalContainer : MonoBehaviour
    {
        public static MedalContainer Instance;
        public KVList<int, MedalItem> MedalDefinition;
        public KVList<int, LocalizedString> MedalNames;
        public Dictionary<int, MedalItem> _MedalDefinition;
        public Dictionary<int, LocalizedString> _MedalNames;
        public Transform MedalContainerTranform;
        public List<MedalItem> ControlledItems;
        public Text MedalScore;
        public Text MedalName;
        void Start()
        {
            Instance = this;
            _MedalDefinition = MedalDefinition.ObtainMap();
            _MedalNames = MedalNames.ObtainMap();
        }
        public void NewScoreAccount(int Score)
        {
            if (Score > 0)
                MedalScore.text = $"+{Score}";
            else if (Score < 0)
                MedalScore.text = $"{Score}";
            MedalScore_DT = 0;
        }
        public void NewMedal(int id)
        {
            if (_MedalNames.TryGetValue(id, out var n))
            {
                MedalName.text = n;
            }
            else
                MedalName.text = "";
            if (id != -1)
            {
                var __medal = GameObject.Instantiate(_MedalDefinition[id], MedalContainerTranform);
                ControlledItems.Add(__medal.GetComponent<MedalItem>());
            }
            MedalName_DT = 0;
        }
        float MedalScore_DT;
        float MedalName_DT;
        // Update is called once per frame
        void Update()
        {
            float t = Time.unscaledDeltaTime;
            if (MedalScore_DT < 0.2f)
            {
                float lerpd = MedalScore_DT * 5;
                var c = MedalScore.color;
                c.a = lerpd;
                MedalScore.color = c;
            }
            else if (MedalScore_DT > 0.2f && MedalScore_DT < 3f)
            {
                if (MedalScore.color.a != 1)
                {
                    var c = MedalScore.color;
                    c.a = 1;
                    MedalScore.color = c;
                }
            }
            else if (MedalScore_DT > 3f && MedalScore_DT < 3.2f)
            {
                float lerpd = (MedalScore_DT - 3) * 5;
                var c = MedalScore.color;
                c.a = 1 - lerpd;
                MedalScore.color = c;
            }
            else
            {
                if (MedalScore.color.a != 0)
                {
                    var c = MedalScore.color;
                    c.a = 0;
                    MedalScore.color = c;
                }
            }
            if (MedalName_DT < 0.2f)
            {
                float lerpd = MedalName_DT * 5;
                var c = MedalName.color;
                c.a = lerpd;
                MedalName.color = c;
            }
            else if (MedalName_DT > 0.2f && MedalName_DT < 3f)
            {
                if (MedalName.color.a != 1)
                {
                    var c = MedalName.color;
                    c.a = 1;
                    MedalName.color = c;
                }
            }
            else if (MedalName_DT > 3f && MedalName_DT < 3.2f)
            {
                float lerpd = (MedalName_DT - 3) * 5;
                var c = MedalName.color;
                c.a = 1 - lerpd;
                MedalName.color = c;
            }
            else
            {
                if (MedalName.color.a != 0)
                {
                    var c = MedalName.color;
                    c.a = 0;
                    MedalName.color = c;
                }
            }
            MedalScore_DT += t;
            MedalName_DT += t;
            foreach (var item in ControlledItems)
            {
                item.upd(t);
            }
            bool isAllDone = true;
            foreach (var item in ControlledItems)
            {
                isAllDone &= item.isDone;
            }
            if (isAllDone)
            {
                for (int i = ControlledItems.Count - 1; i >= 0; i--)
                {
                    var item = ControlledItems[i];
                    ControlledItems.RemoveAt(i);
                    Destroy(item.gameObject);
                }
            }
        }
    }
}
