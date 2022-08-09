using Ceras;
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
            ReferenceLoopHandling= ReferenceLoopHandling.Serialize,
            NullValueHandling= NullValueHandling.Include
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
    public static class BinaryUtilities
    {
        static SerializerConfig config = new SerializerConfig() {
             PreserveReferences=true,
        };
        public static byte[] Serialize<T>(T Data)
        {
            CerasSerializer serializer = new CerasSerializer(config);
            return serializer.Serialize(Data);
        }
    }
}
