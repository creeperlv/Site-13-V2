using Site13Kernel.Data;
using Site13Kernel.SceneBuild.Serializables;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace Site13Kernel.SceneBuild
{
    [Serializable]
    public class EditableData : IEditableComponent
    {
        public PrefabReference Reference;
        [SerializeField]
        public List<EditableComponent> components;

        public void CopyTo(IEditableComponent editableComponent)
        {
            if (editableComponent is EditableData data)
            {
                foreach (var component in components)
                {
                    foreach (var item in data.components)
                    {
                        component.CopyTo(item);
                    }
                }
            }
        }

        public void UpdateScene()
        {
            foreach (var item in components)
            {
                item.UpdateScene();
            }
        }
        public void UpdateValue()
        {
            foreach (var item in components)
            {
                item.UpdateValue();
            }
        }
        public SerializableData ToSerializableData()
        {

            List<SerializableBase> components = new List<SerializableBase>();
            foreach (var item in this.components)
            {
                components.Add(item.ObtainSerializable());
            }
            return new SerializableData { Reference = Reference, components = components };
        }
        public static implicit operator SerializableData(EditableData data)
        {
            return data.ToSerializableData();
        }
    }
}