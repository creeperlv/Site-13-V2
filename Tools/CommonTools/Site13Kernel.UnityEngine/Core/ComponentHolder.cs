using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Site13Kernel.Core
{
    public class ComponentHolder : ControlledBehavior
    {
        public List<IComponent> Components;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ComponentHolder TryAddHolder(GameObject TargetObject)
        {
            var ComponentHolder = TargetObject.GetComponent<ComponentHolder>();
            if (ComponentHolder != null)
            {
                return ComponentHolder;
            }
            else
            {
                return TargetObject.AddComponent<ComponentHolder>();
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddComponent(GameObject TargetObject, IComponent component)
        {
            AddComponent(TargetObject.GetComponent<ComponentHolder>(), component);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddComponent(ComponentHolder TargetHolder, IComponent component)
        {
            TargetHolder.Components.Add(component);
        }
    }
}
