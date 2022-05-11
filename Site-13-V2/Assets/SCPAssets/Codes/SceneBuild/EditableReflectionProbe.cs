using Newtonsoft.Json;
using Site13Kernel.SceneBuild.Serializables;
using System;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Rendering;

namespace Site13Kernel.SceneBuild
{
    //[Serializable]
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class EditableReflectionProbe : EditableComponent, ISerializable
    {
        [JsonIgnore]
        public ReflectionProbe ControlledProbe;
        [JsonProperty]
        public ReflectionProbeRefreshMode RefreshMode;
        [JsonProperty]
        public SerializableVector3 Size;
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("RefreshMode", RefreshMode, typeof(ReflectionProbeRefreshMode));
            info.AddValue("Size", Size, typeof(SerializableVector3));
        }
        public override SerializableBase ObtainSerializable()
        {
            return new SerializableReflectionProbe {  RefreshMode = RefreshMode, Size = Size };
        }
        public override void ApplySerializable(SerializableBase serializable)
        {
            if(serializable is SerializableReflectionProbe rp)
            {
                RefreshMode = rp.RefreshMode;
                Size = rp.Size;
            }
        }
        public override void UpdateScene()
        {
            ControlledProbe.refreshMode = RefreshMode;
            ControlledProbe.size = Size;
        }
        public override void UpdateValue()
        {
            RefreshMode=ControlledProbe.refreshMode;
            Size=ControlledProbe.size;
        }
        public override void CopyTo(IEditableComponent editableComponent)
        {
            if(editableComponent is EditableReflectionProbe p)
            {
                p.RefreshMode= RefreshMode;
                p.Size= Size;
            }
        }
    }
}