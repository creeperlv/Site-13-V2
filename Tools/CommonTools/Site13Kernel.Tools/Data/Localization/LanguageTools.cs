using CLUNL.Localization;
using Newtonsoft.Json;
using Site13Kernel.Data.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Site13Kernel.Tools.Data.Localization
{
    public class LanguageTools
    {
        static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            FloatFormatHandling = FloatFormatHandling.String,
            NullValueHandling = NullValueHandling.Ignore
        };
        public static string SerializeDefinitionCollection(LocalizationDefinitionCollection definitionCollection)
        {
            var Content = JsonConvert.SerializeObject(definitionCollection, settings);
            return Content;
        }
        public static string SerializeDefinition(LocalizationDefinition definition)
        {
            var Content = JsonConvert.SerializeObject(definition, settings);
            return Content;
        }
        public static (LocalizationDefinitionCollection, LocalizationDefinition, LanguageDefinition) GenerateLocalizationBase()
        {
            LocalizationDefinitionCollection localizationDefinitionCollection = new LocalizationDefinitionCollection();
            LocalizationDefinition definition = new LocalizationDefinition();
            LanguageDefinition ld = new LanguageDefinition();

            definition.LanguageCode = "en-US";
            return (localizationDefinitionCollection, definition, ld);
            //return (SerializeDefinitionCollection(localizationDefinitionCollection), SerializeDefinition(definition));
        }

    }
}
