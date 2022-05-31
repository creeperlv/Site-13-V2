using CLUNL.Localization;
using Site13Kernel.Data;
using Site13Kernel.Rendering;
using Site13Kernel.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.UEFI
{
    public class LocalizationProcessor : UEFIBase
    {
        public static Dictionary<string, string> AvailableLanguages = new Dictionary<string, string>();
        public string DefaultLanguageCode = "en-US";
        public KVList<string, string> LanguageNames = new KVList<string, string>();
        public KVList<string, TextAsset> LanguageFiles = new KVList<string, TextAsset>();
        public KVList<string, TextureList> LanguagedTextureLists = new KVList<string, TextureList>();
        public KVList<string, TextureList> LanguagedTextureOverrideLists = new KVList<string, TextureList>();
        public List<RemappableMaterial> Materials = new List<RemappableMaterial>();
        public override void Init()
        {
            AvailableLanguages = LanguageNames.ObtainMap();
            var Lan = Settings.CurrentSettings.LanguageCode;
            if (!AvailableLanguages.ContainsKey(Lan))
            {
                Lan = DefaultLanguageCode;
            }
            var LF = LanguageFiles.ObtainMap();
            Language.LoadFromString(LF[DefaultLanguageCode].text);
            if (Lan != DefaultLanguageCode)
            {
                Language.LoadFromString(LF[Lan].text);
            }
            var LTL = LanguagedTextureLists.ObtainMap();
            TextureList __tl = new TextureList();
            LTL[DefaultLanguageCode].__init();
            __tl.TextureMap = LTL[DefaultLanguageCode].TextureMap;
            if (Lan != DefaultLanguageCode)
            {
                LTL[Lan].__init();
                DictionaryOperations.Merge(ref __tl.TextureMap, LTL[Lan].TextureMap);
            }
            if (Settings.CurrentSettings.useAlternativeFoundationLogo)
            {
                var LTOL = LanguagedTextureOverrideLists.ObtainMap();
                LTOL[DefaultLanguageCode].__init();
                DictionaryOperations.Merge(ref __tl.TextureMap, LTOL[DefaultLanguageCode].TextureMap);
                if (Lan != DefaultLanguageCode)
                {
                    LTOL[Lan].__init();
                    DictionaryOperations.Merge(ref __tl.TextureMap, LTOL[Lan].TextureMap);
                }
            }
            TextureList.CurrentList = __tl;
            foreach (var item in Materials)
            {
                item.Map();
            }
        }
    }
}
