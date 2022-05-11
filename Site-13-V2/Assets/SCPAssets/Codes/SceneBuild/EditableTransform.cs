using Newtonsoft.Json;
using Site13Kernel.SceneBuild.Serializables;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using UnityEngine;
namespace Site13Kernel.SceneBuild
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class EditableTransform : EditableComponent, ISerializable
    {
        [JsonProperty]
        public SerializableVector3 Position;
        [JsonProperty]
        public SerializableQuaternion Rotation;
        [JsonProperty]
        public SerializableVector3 Scale;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("WorldPosition", Position, typeof(SerializableVector3));
            info.AddValue("WorldRotation", Rotation, typeof(SerializableQuaternion));
            info.AddValue("LocalScale", Scale, typeof(SerializableVector3));
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void CopyTo(IEditableComponent editableComponent)
        {
            if (editableComponent is EditableTransform t)
            {
                t.Position = Position;
                t.Rotation = Rotation;
                t.Scale = Scale;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void ApplySerializable(SerializableBase serializable)
        {
            if (serializable is SerializableTransform t)
            {
                Position = t.Position;
                Rotation = t.Rotation;
                Scale = t.Scale;
                Debug.Log("Applied");
                UpdateScene();
            }
            else
            {
                Debug.Log("Drop:"+serializable.GetType()    );
            }

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override SerializableBase ObtainSerializable()
        {
            return new SerializableTransform { Position = Position, Rotation = Rotation, Scale = Scale };
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void UpdateScene()
        {
            transform.position = Position;
            transform.rotation = Rotation;
            transform.localScale = Scale;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override void UpdateValue()
        {
            Position = transform.position;
            Rotation = transform.rotation;
            Scale = transform.localScale;
        }
    }
}