using Newtonsoft.Json;
using Site13Kernel.SceneBuild.Serializables;
using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace Site13Kernel.SceneBuild
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class EditablePointLight : EditableComponent, ISerializable
    {
        [JsonIgnore]
        public Light L;
        [JsonProperty]
        public SerializableColor Color;
        [JsonProperty]
        public float LightRange;
        [JsonProperty]
        public float LightIntensity;
        [JsonProperty]
        public LightShadows Shadows;
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Color", Color, typeof(SerializableColor));
            info.AddValue("LightRange", LightRange, typeof(float));
            info.AddValue("LightIntensity", LightIntensity, typeof(float));
            info.AddValue("Shadows", Shadows, typeof(LightShadows));
        }
        public override void UpdateScene()
        {
            L.type = LightType.Point;
            L.intensity = LightIntensity;
            L.shadows = Shadows;
            L.range = LightRange;
            L.color = Color;
        }
        public override void UpdateValue()
        {
            LightIntensity = L.intensity;
            LightRange = L.range;
            Color = L.color;
            Shadows = L.shadows;
        }
        public override void ApplySerializable(SerializableBase serializable)
        {
            if(serializable is SerializablePointLight l)
            {
                Color = l.Color;
                LightRange = l.LightRange;
                LightIntensity = l.LightIntensity;
                Shadows = l.Shadows;
            }
        }
        public override SerializableBase ObtainSerializable()
        {
            return new SerializablePointLight
            {
                Color = Color,
                LightIntensity = LightIntensity,
                LightRange = LightRange,
                Shadows = Shadows
            };
        }
        public override void CopyTo(IEditableComponent editableComponent)
        {
            if (editableComponent is EditablePointLight _L)
            {
                _L.Shadows = Shadows;
                _L.LightRange = LightRange;
                _L.LightIntensity = LightIntensity;
                _L.Color = Color;
            }
        }
    }
}