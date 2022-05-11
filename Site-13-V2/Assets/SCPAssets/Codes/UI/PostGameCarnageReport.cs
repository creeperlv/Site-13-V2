using CLUNL.Localization;
using Site13Kernel.Core;
using Site13Kernel.Data;
using Site13Kernel.GameLogic;
using Site13Kernel.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.UI
{
    public class PostGameCarnageReport : MonoBehaviour
    {
        public PostGameCarnageReportItem Template;
        public List<KVPair<string, Sprite>> ItemIcons= new List<KVPair<string, Sprite>>();
        public List<KVPair<string, LocalizedString>> ItemDescriptions= new List<KVPair<string, LocalizedString>>();
        Dictionary<string, Sprite> _ICON;
        Dictionary<string, LocalizedString> _DESC;
        public Transform ReportHolders;
        public UIButton MainMenu;
        void Start()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GameRuntime.CurrentGlobals.isInLevel = false;
            _ICON = CollectionUtilities.ToDictionary(ItemIcons);
            _DESC = CollectionUtilities.ToDictionary(ItemDescriptions);
            foreach (var item in ScoreBoard.Kills)
            {
                var _i=Instantiate(Template, ReportHolders);
                var __i = _i.GetComponent<PostGameCarnageReportItem>();
                var _b=_ICON.TryGetValue(item.Key, out var _icon);
                __i.SetData(_DESC[item.Key], item.Value, _b ? _icon : null);
            }
            MainMenu.OnClick = () => {
                SceneLoader.Instance.EndLevel();
            };
        }

    }
    public class ScoreBoard
    {
        public static Dictionary<string, int> Kills = new Dictionary<string, int>();
        public static void Count(string ID)
        {
            if (Kills.ContainsKey(ID))
            {
                Kills[ID]++;
            }
            else
            {
                Kills.Add(ID, 1);
            }
        }
        public static void ClearScore()
        {
            Kills.Clear();
        }
    }
}
