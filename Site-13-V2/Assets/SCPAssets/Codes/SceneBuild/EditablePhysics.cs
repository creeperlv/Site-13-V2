using Newtonsoft.Json;
using Site13Kernel.SceneBuild;
using Site13Kernel.SceneBuild.Serializables;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace Site13Kernel.SceneBuild
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class EditablePhysics : EditableComponent, ISerializable
    {
        [JsonIgnore]
        public List<Collider> Colliders;
        [JsonProperty]
        public bool useCollider;
        [JsonProperty]
        public bool useRigidbody;
        [JsonProperty]
        public bool useGravity;
        [JsonIgnore]
        public bool UseRenderMode;
        [JsonProperty]
        public float Mass;
        [JsonProperty]
        public float Drag;
        [JsonProperty]
        public float AngularDrag;
        public override void UpdateScene()
        {
            var r = GetComponent<Rigidbody>();
            if (useRigidbody)
            {
                if (r != null)
                {

                    r.useGravity = useGravity;
                    r.mass = Mass;
                    r.drag = Drag;
                    r.angularDrag = AngularDrag;
                }
                else
                {
                    var _r = gameObject.AddComponent<Rigidbody>();
                    _r.useGravity = useGravity;
                    _r.mass = Mass;
                    _r.drag = Drag;
                    _r.angularDrag = AngularDrag;
                }
            }
            else
            {
                if (r != null)
                {
                    Destroy(r);
                }
            }
            foreach (var item in Colliders)
            {
                item.enabled = useCollider;
                if (useRigidbody)
                {
                    if (item is MeshCollider mc)
                    {
                        mc.convex = true;
                    }
                }
                else
                {
                    if (item is MeshCollider mc)
                    {
                        mc.convex = false;
                    }
                }
            }
        }
        public Vector3 ObtainSize()
        {
            if (UseRenderMode)
            {
                if (Colliders.Count == 0)
                    return Colliders[0].GetComponent<Renderer>().bounds.size;
                Bounds Final = Colliders[0].GetComponent<Renderer>().bounds;
                for (int i = 0; i < Colliders.Count; i++)
                {
                    Final.Encapsulate(Colliders[i].GetComponent<Renderer>().bounds);
                }
                return Final.size;
            }
            else
            {
                if (Colliders.Count == 0)
                    return Colliders[0].bounds.size;
                Bounds Final = Colliders[0].bounds;
                for (int i = 0; i < Colliders.Count; i++)
                {
                    Final.Encapsulate(Colliders[i].bounds);

                }
                return Final.size;
            }
        }
        public override void UpdateValue()
        {
        }
        public override SerializableBase ObtainSerializable()
        {
            return new SerializablePhysics
            {
                AngularDrag = AngularDrag,
                useCollider = useCollider,
                useGravity = useGravity,
                useRigidbody = useRigidbody,
                UseRenderMode = UseRenderMode,
                Mass = Mass,
                Drag = Drag
            };
        }

        public override void ApplySerializable(SerializableBase serializable)
        {
            if (serializable is SerializablePhysics p)
            {
                AngularDrag = p.AngularDrag;
                useCollider = p.useCollider;
                useGravity = p.useGravity;
                useRigidbody = p.useRigidbody;
                Mass = p.Mass;
                Drag = p.Drag;
                UseRenderMode = p.UseRenderMode;
            }
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("useCollider", useCollider, typeof(bool));
            info.AddValue("useRigidbody", useRigidbody, typeof(bool));
            info.AddValue("useGravity", useGravity, typeof(bool));
            info.AddValue("Mass", Mass, typeof(float));
            info.AddValue("Drag", Drag, typeof(float));
            info.AddValue("AngularDrag", AngularDrag, typeof(float));
        }
    }
}