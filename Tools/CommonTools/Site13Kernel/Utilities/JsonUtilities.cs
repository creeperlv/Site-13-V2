using Newtonsoft.Json;

namespace Site13Kernel.Utilities
{
    public static class JsonUtilities
    {
        static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects,
            TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
            Formatting = Formatting.Indented,
        };
        public static string Serialize<T>(T data)
        {
            return JsonConvert.SerializeObject(data, settings);
        }
        public static T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data,settings);
        }
    }
}
