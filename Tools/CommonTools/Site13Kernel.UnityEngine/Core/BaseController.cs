using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Site13Kernel.Core
{

    public class BaseController : MonoBehaviour
    {
        //[InspectorName("Objects That contains classes that only implements <color=#2288EE>IControllable</color>")]
        public List<GameObject> OnInit_Interface = new List<GameObject>();
        public List<ControlledBehavior> OnInit=new List<ControlledBehavior>();
        public List<IControllable> _OnInit=new List<IControllable>();
        public List<ControlledBehavior> OnRefresh=new List<ControlledBehavior>();
        public List<IControllable> _OnRefresh=new List<IControllable>();
        public List<ControlledBehavior> OnFixedRefresh=new List<ControlledBehavior>();
        public List<IControllable> _OnFixedRefresh=new List<IControllable>();
        public void RegisterRefresh<T>(T obj) where T : IControllable
        {
            if (!_OnRefresh.Contains(obj))
                _OnRefresh.Add(obj);
        }
        public void RegisterFixedRefresh<T>(T obj) where T : IControllable
        {
            if (!_OnFixedRefresh.Contains(obj))
                _OnFixedRefresh.Add(obj);
        }
        public void SerializeAll()
        {
            foreach (var item in OnInit_Interface)
            {
                _OnInit.AddRange(item.GetComponents<IControllable>());
            }
            foreach (var item in OnInit)
            {
                _OnInit.Add(item);
            }
            foreach (var item in OnRefresh)
            {
                _OnRefresh.Add(item);
            }
            foreach (var item in OnFixedRefresh)
            {
                _OnFixedRefresh.Add(item);
            }
        }

        public void UnregisterRefresh<T>(T obj) where T : IControllable
        {
            _OnRefresh.Remove(obj);
        }
        public void UnregisterFixedRefresh<T>(T obj) where T : IControllable
        {
            _OnFixedRefresh.Remove(obj);
        }
        public T GetBehavior<T>() where T : ControlledBehavior
        {
            foreach (var item in _OnInit)
            {
                if (item is T t)
                {
                    return t;
                }
            }
            foreach (var item in _OnRefresh)
            {
                if (item is T t)
                {
                    return t;
                }
            }
            foreach (var item in _OnFixedRefresh)
            {
                if (item is T t)
                {
                    return t;
                }
            }
            return null;
        }
        public void InitBehavior(IControllable behavior)
        {
            behavior.Parent = this;
        }
    }
}
