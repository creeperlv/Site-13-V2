using Site13Kernel.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Site13Kernel.GameLogic.Animations
{
	public class TransformSync : ControlledBehavior
	{
		public List<TrackedTransforms> transforms = new List<TrackedTransforms>();
		public bool UsePreBakedData;
		public bool GlobalPos;
		public void CalcDelta()
		{
			if (GlobalPos)
			{
				foreach (var transform in transforms)
				{
					transform.PositionDelta = transform.Target.position - transform.Source.position;
					transform.RotationDelta = (Quaternion.Euler(transform.Target.eulerAngles - transform.Source.eulerAngles));
				}
			}
			else
			{
				foreach (var transform in transforms)
				{
					transform.PositionDelta = transform.Target.localPosition - transform.Source.localPosition;
					transform.RotationDelta = (Quaternion.Euler(transform.Target.eulerAngles - transform.Source.eulerAngles));
				}
			}
		}
		public void Start()
		{
			if (!UsePreBakedData) CalcDelta();
		}

		public void Update()
		{
			if (GlobalPos)
			{
				foreach (var item in transforms)
				{
					item.Target.position = item.Source.position + item.PositionDelta;
					item.Target.rotation = item.Source.rotation * item.RotationDelta;
				}
			}
			else
			{
				foreach (var item in transforms)
				{
					item.Target.localPosition = item.Source.localPosition + item.PositionDelta;
					item.Target.rotation = item.Source.rotation * item.RotationDelta;
				}
			}
		}
	}
	[Serializable]
	public class TrackedTransforms
	{
		public Transform Source;
		public Transform Target;
		public Vector3 PositionDelta;
		public Quaternion RotationDelta;
	}
}
