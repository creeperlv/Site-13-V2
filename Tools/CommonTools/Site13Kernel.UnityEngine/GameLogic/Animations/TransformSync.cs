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
		public void Start()
		{
			foreach (var transform in transforms)
			{
				transform.PositionDelta = transform.Target.localPosition - transform.Source.localPosition;
				//transform.RotationDelta = Quaternion.Inverse(transform.Source.localRotation)*transform.Target.localRotation;
				transform.RotationDelta = (Quaternion.Euler(transform.Target.eulerAngles - transform.Source.eulerAngles));
			}
		}

		public void Update()
		{
			foreach (var item in transforms)
			{
				item.Target.localPosition = item.Source.localPosition + item.PositionDelta;
				item.Target.rotation = item.Source.rotation * item.RotationDelta;
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
