using Newtonsoft.Json;
using Site13Kernel.SceneBuild.Serializables;
using System.Collections;
using System.Runtime.Serialization;
using UnityEngine;
namespace Site13Kernel.SceneBuild
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class EditableObject : MonoBehaviour,ISerializable
    {
        [JsonProperty]
        public EditableData EditableData;
        public void UpdateScene()
        {
            EditableData.UpdateScene();
        }
        public void UpdateValue()
        {
            EditableData.UpdateValue();
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Data", EditableData, typeof(EditableData));
        }
        public static implicit operator SerializableObject(EditableObject EO)
        {
            return new SerializableObject() { Data = EO.EditableData };
        }
    }
}