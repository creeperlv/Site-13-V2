using System.Collections.Generic;

namespace Site13Kernel.Core.TagSystem
{
    public class TransformSyncSystem : SystemBase
    {
        public override void Init()
        {
            this.Collection.Descriptions.Add(new EntityCollectionDescription() { Types = new List<System.Type> { typeof(SyncGlobalTransform) } });
        }
        public override void Execute(float DT, float UDT)
        {
            foreach (var item in Collection.Descriptions[0].Resultes)
            {
                var SGT=item.Components[0] as SyncGlobalTransform;
                var t = item.transform;
                var _p = t.position;
                var _r = t.rotation;
                var _s = t.lossyScale;
                lock (SGT.Position)
                {
                    SGT.Position = Utilities.DataConversion.SerializeVector3(_p);
                }
                lock (SGT.Rotation)
                {
                    SGT.Rotation = Utilities.DataConversion.SerializeQuaternion(_r);
                }
                lock (SGT.Scale)
                {
                    SGT.Scale = Utilities.DataConversion.SerializeVector3(_s);
                }
            }
        }
    }
}
