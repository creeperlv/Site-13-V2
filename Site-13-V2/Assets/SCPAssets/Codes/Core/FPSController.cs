using Site13Kernel.Core.CustomizedInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.Core
{
    public class FPSController : ControlledBehavior
    {
        public float MoveSpeed=1f;
        public float MouseHoriztonalIntensity=1f;
        public CharacterController cc;
        public Transform Head;
        public float MaxV;
        public float MinV;
        public override void Init()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Parent.OnRefresh.Add(this);
        }
        public override void Refresh(float DeltaTime)
        {
            var MV=InputProcessor.CurrentInput.GetAxis("MoveVertical");
            var MH=InputProcessor.CurrentInput.GetAxis("MoveHorizontal");
            var V=new Vector3(MH,0,MV);
            var _V=V.normalized*MoveSpeed;
            cc.SimpleMove(_V);
            cc.transform.Rotate(0, InputProcessor.CurrentInput.GetAxis("MouseH") * MouseHoriztonalIntensity * DeltaTime, 0);
            var Head_V=InputProcessor.CurrentInput.GetAxis("MouseV") * MouseHoriztonalIntensity * DeltaTime;
            var ea=Head.localEulerAngles;
            ea.x += Head_V;
                Debug.Log($"ea.x:{ea.x},Min:{360 + MinV}");
            if (ea.x < 180)
            {
                ea.x = Mathf.Clamp(ea.x , MinV, MaxV);
            }
            else
            {
                ea.x = Mathf.Clamp(ea.x , 360 + MinV, 360);

            }
            Head.localEulerAngles = ea;
        }
        public override void FixedRefresh(float DeltaTime)
        {
        }
    }
}
