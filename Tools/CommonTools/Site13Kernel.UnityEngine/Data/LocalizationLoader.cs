using CLUNL.Localization;
using Site13Kernel.Core;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Site13Kernel.Data
{
    public class LocalizationLoader:ControlledBehavior
    {
        public bool LoadOnStartInsteadOfInit = false;
        public bool ClearBeforeLoad = false;
        public KVList<string, TextAsset> LanguageFiles = new KVList<string, TextAsset>();
        public void Start()
        {
            if (LoadOnStartInsteadOfInit) Load();
        }
        public override void Init()
        {
            if (!LoadOnStartInsteadOfInit) Load();
        }
        void Load()
        {

            var Lan = Settings.CurrentSettings.LanguageCode.ToUpper();
            TextAsset ToLoad = LanguageFiles.PrefabDefinitions[0].Value;
            foreach (var item in LanguageFiles.PrefabDefinitions)
            {
                if (item.Key.ToUpper() == Lan)
                {
                    ToLoad = item.Value;
                    break;
                }
            }
            if (ClearBeforeLoad) Language.ClearLoadedStrings();
            Language.LoadFromString(ToLoad.text);
        }
    }
}
