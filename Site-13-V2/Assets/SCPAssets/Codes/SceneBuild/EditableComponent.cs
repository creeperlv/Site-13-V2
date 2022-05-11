using Newtonsoft.Json;
using Site13Kernel.SceneBuild.Serializables;
using System.Runtime.Serialization;
using UnityEngine;
namespace Site13Kernel.SceneBuild
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class EditableComponent : MonoBehaviour,IEditableComponent,ISerializable
    {
        //public new GameObject gameObject { get { return base.gameObject; } }
        public virtual void UpdateValue() { }
        public virtual void UpdateScene() { }

        public virtual void CopyTo(IEditableComponent editableComponent)
        {

        }
        public virtual void ApplySerializable(SerializableBase serializable)
        {

        }
        public virtual SerializableBase ObtainSerializable()
        {
            return null;
        }
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
    public interface IEditableComponent
    {
        void UpdateValue();
        void UpdateScene();
        void CopyTo(IEditableComponent editableComponent);
    }
}