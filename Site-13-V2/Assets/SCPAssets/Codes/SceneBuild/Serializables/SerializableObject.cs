using System;

namespace Site13Kernel.SceneBuild.Serializables
{
    [Serializable]
    public class SerializableObject
    {
        public SerializableData Data;
        public void CopyTo(EditableObject __obj)
        {
            foreach (var item in __obj.EditableData.components)
            {
                foreach (var _com in Data.components)
                {
                    item.ApplySerializable(_com);
                }
            }
        }
    }
}