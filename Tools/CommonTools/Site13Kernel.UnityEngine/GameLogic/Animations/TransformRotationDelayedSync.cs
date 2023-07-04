using Site13Kernel.Core;
using Site13Kernel.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.Animations
{
	public class TransformRotationDelayedSync : ControlledBehavior
	{
		public Transform Source;
		public Transform Target;
		public float Tolerance = 15;
		public float Speed = 10;
		public bool WillTriggerAnimation;
		public Animator TargetAnimator;
		public List<string> AnimationTrigger;
		public bool IsLocalSpace = true;
		public bool IsMoving;
		public float Termination = 2;
		public void Sync()
		{

			Quaternion Src;
			Quaternion Tgt;
			if (IsLocalSpace)
			{
				Src = Source.localRotation;
				Tgt = Target.localRotation;
				if (Src != Tgt) Target.localRotation = Src;
			}
			else
			{

				Src = Source.rotation;
				Tgt = Target.rotation;
				if (Src != Tgt) Target.rotation = Src;
			}
		}
		public void Update()
		{
			float angle;
			Quaternion Src;
			Quaternion Tgt;
			if (IsLocalSpace)
			{
				Src = Source.localRotation;
				Tgt = Target.localRotation;
			}
			else
			{

				Src = Source.rotation;
				Tgt = Target.rotation;
			}
			angle = Quaternion.Angle(Src , Tgt);
			if (IsMoving)
			{
				if (angle <= Termination)
				{
					IsMoving = false;
					return;
				}
				var f_q = Quaternion.RotateTowards(Tgt , Src , angle * Speed * Time.deltaTime);
				if (IsLocalSpace)
				{
					Target.localRotation = f_q;
				}
				else Target.rotation = f_q;
			}
			else
			{
				if (angle >= Tolerance)
				{
					IsMoving = true;
				}
				if (WillTriggerAnimation)
				{
					if (TargetAnimator != null)
					{
						TargetAnimator.SetTrigger(AnimationTrigger.PickOne());
					}
				}
			}
		}
	}
}
