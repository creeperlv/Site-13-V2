using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

namespace Site13Kernel.Data.IO
{

    public class TranslationData : MonoBehaviour, IData
    {
        public float POS_X;
        public float POS_Y;
        public float POS_Z;
        public float ROT_X;
        public float ROT_Y;
        public float ROT_Z;
        public float ROT_W;
        public float SCL_X;
        public float SCL_Y;
        public float SCL_Z;

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Tran.Pos.X", POS_X, typeof(float));
            info.AddValue("Tran.Pos.Y", POS_Y, typeof(float));
            info.AddValue("Tran.Pos.Z", POS_Z, typeof(float));
            info.AddValue("Tran.Rot.X", ROT_X, typeof(float));
            info.AddValue("Tran.Rot.Y", ROT_Y, typeof(float));
            info.AddValue("Tran.Rot.Z", ROT_Z, typeof(float));
            info.AddValue("Tran.Rot.W", ROT_W, typeof(float));
            info.AddValue("Tran.Scl.W", SCL_X, typeof(float));
            info.AddValue("Tran.Scl.Y", SCL_Y, typeof(float));
            info.AddValue("Tran.Scl.Z", SCL_Z, typeof(float));

        }

        public void Load(IData data)
        {
            if (data is TranslationData t)
            {
                POS_X = t.POS_X;
                POS_Y = t.POS_Y;
                POS_Z = t.POS_Z;

                ROT_X = t.ROT_X;
                ROT_Y = t.ROT_Y;
                ROT_Z = t.ROT_Z;
                ROT_W = t.ROT_W;

                SCL_X = t.SCL_X;
                SCL_Y = t.SCL_Y;
                SCL_Z = t.SCL_Z;

                var POS = transform.position;
                POS.x = POS_X;
                POS.y = POS_Y;
                POS.z = POS_Z;
                transform.position = POS;

                var ROT = transform.rotation;
                ROT.x = ROT_X;
                ROT.y = ROT_Y;
                ROT.z = ROT_Z;
                ROT.w = ROT_W;
                transform.rotation = ROT;

                var SCL = transform.localScale;
                SCL.x = SCL_X;
                SCL.y = SCL_Y;
                SCL.z = SCL_Z;
                transform.localScale = SCL;
            }
            else
            {
                Diagnostics.Debug.LogError($"Type of saved data ({data.GetType()}) does not match {typeof(TranslationData)}.");
            }
        }

        public void Save()
        {
            var POS = transform.position;
            POS_X = POS.x;
            POS_Y = POS.y;
            POS_Z = POS.z;
            var ROT = transform.rotation;
            ROT_X = ROT.x;
            ROT_Y = ROT.y;
            ROT_Z = ROT.z;
            ROT_W = ROT.w;

            var SCL = transform.localScale;
            SCL_X = SCL.x;
            SCL_Y = SCL.y;
            SCL_Z = SCL.z;
        }
    }
}
