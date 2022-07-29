using Site13Kernel.Core;
using Site13Kernel.GameLogic.Directors;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Site13Kernel.Data.Achievements
{
    public class AchievementList : ControlledBehavior
    {
        public static AchievementList Instance;
        public GameObject AchievementAnimation;
        public Image AchievementIcon;
        public Text AchievementTitle;
        public Text AchievementDesc;
        public List<HLAchievementItem> Definitions;
        public List<AchievementItemBase> Completion;
        public override void Init()
        {
            Instance = this;
        }
        public void SetAchievement(AchievementItemBase item, SetOperation operation)
        {
            if (!CheckCompleted(item))
            {
                switch (operation)
                {
                    case SetOperation.Add:
                        foreach (var ach_item in Completion)
                        {
                            if (ach_item.ID == item.ID)
                            {
                                ach_item.CurrentAmount += item.CurrentAmount;
                                break;
                            }
                        }
                        break;
                    case SetOperation.Set:
                        foreach (var ach_item in Completion)
                        {
                            if (ach_item.ID == item.ID)
                            {
                                ach_item.CurrentAmount = item.CurrentAmount;
                                break;
                            }
                        }
                        break;
                    default:
                        break;
                }
                if (CheckCompleted(item))
                {
                    ShowCompletion(item);
                }
            }
        }
        public void ShowCompletion(AchievementItemBase item)
        {
            foreach (var ach_item in Definitions)
            {
                if (ach_item.ID == item.ID)
                {
                    AchievementIcon.sprite = ach_item.Icon;
                    AchievementTitle.text = ach_item.Title;
                    AchievementDesc.text = ach_item.Content;
                    AchievementAnimation.SetActive(false);
                    AchievementAnimation.SetActive(true);
                    return;
                }
            }
        }
        public bool CheckCompleted(AchievementItemBase itemBase)
        {
            return false;
        }
    }
}
