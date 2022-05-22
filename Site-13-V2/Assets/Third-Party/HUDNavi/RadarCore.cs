using CLUNL.Unity3D.Sync;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace HUDNavi
{
    public class RadarCore : MonoBehaviour
    {
        public static RadarCore CurrentRadar;
        public bool useMultiThread;
        public GameObject Detector;
        public List<RadarDot> dots = new List<RadarDot>();
        public float RadarRadius = 50;
        public bool isOn = true;
        public float North_Base_Angle;
        public bool ReverseHorizontalRotation = false;
        public GameObject RadarObject;
        [HideInInspector]
        public List<MotionTarget> Targets;
        [HideInInspector]
        public Vector3 SafeDetectorPos;
        /// <summary>
        /// The detection distance.
        /// </summary>
        public float Distance = 25;
        public int RefreshTime=500;
        bool UseSeparatedThread = false;
        IEnumerator Start()
        {
            CurrentRadar = this;
            yield return null;
            if(useMultiThread)
            if (Dispatcher.Inited())
            {
                UseSeparatedThread = true;
                Running = true;
                Task.Run(async () =>
                {
                    while (Running)
                    {
                        await Task.Delay(RefreshTime);
                        if (isOn)
                        {

                            foreach (var item in Targets)
                            {
                                if (item.Safe_isActiveAndEnabled == false)
                                {
                                    if (item.RadarPointActive == true)
                                    {
                                        Dispatcher.Invoke(() => { item.RadarPoint.SetActive(false); });
                                    }
                                    continue;
                                }
                                float _x = (item.PositionS.x - SafeDetectorPos.x) / Distance;
                                float _y = (item.PositionS.z - SafeDetectorPos.z) / Distance;
                                bool isMoving = item.isMoving;
                                if (item.isAlwaysDetected == true) isMoving = true;
                                if (isMoving == true)
                                {
                                    if (_x * _x + _y * _y > 1)
                                    {
                                        if (item.ShowBeyoundBoundary == false)
                                        {
                                            if (item.RadarPointActive == true)
                                                Dispatcher.Invoke(() => { item.RadarPoint.SetActive(false); });
                                        }
                                        else
                                        {
                                            if (item.RadarPointActive == false)
                                                Dispatcher.Invoke(() => { item.RadarPoint.SetActive(true); });

                                            var _len = Mathf.Sqrt(_x * _x + _y * _y);
                                            var ratio = 1 / _len;
                                            var ___ = new Vector2(_x * RadarRadius * ratio, _y * RadarRadius * ratio);
                                            Dispatcher.Invoke(() =>
                                            {
                                                var rt = item.RadarPoint.transform as RectTransform;
                                                rt.anchoredPosition = ___;
                                            });
                                        }
                                    }
                                    else
                                    {
                                        if (item.RadarPointActive == false)
                                            Dispatcher.Invoke(() => { item.RadarPoint.SetActive(true); });
                                        var ___ = new Vector2(_x * RadarRadius, _y * RadarRadius);
                                        Dispatcher.Invoke(() =>
                                        {
                                            var rt = item.RadarPoint.transform as RectTransform;
                                            rt.anchoredPosition = ___;
                                        });
                                    }
                                }
                                else
                                {
                                    if (item.RadarPointActive == true)
                                        Dispatcher.Invoke(() => { item.RadarPoint.SetActive(false); });
                                }

                            }
                        }
                        else
                        {

                            foreach (var item in Targets)
                            {
                                if (item.RadarPointActive == true)
                                    Dispatcher.Invoke(() => { item.RadarPoint.SetActive(false); });
                            }
                        }
                    }
                });
            }
        }

        public void OnDestroy()
        {
            Running = false;
        }
        bool Running = false;
        public void Add(MotionTarget target)
        {
            Targets.Add(target);
            var d = dots[target.RadarIconType].Dot;
            target.RadarPoint = GameObject.Instantiate(d, RadarObject.transform);
            target.RadarPoint.GetComponent<RadarPoint>().color = dots[target.RadarIconType].Color;
        }
        int Cycle = 100;
        int _Cycle = 0;
        void Update()
        {
            if (_Cycle > Cycle)
            {
                Targets.Remove(null);
                _Cycle = 0;
            }
            {
                _Cycle++;
            }
            if (isOn == true)
            {
                SafeDetectorPos = Detector.transform.position;
                var rotatoin = (ReverseHorizontalRotation ? -1 : 1);
                if (!UseSeparatedThread)
                {

                    foreach (var item in Targets)
                    {
                        if (item == null) continue;
                        if (item.isActiveAndEnabled == false)
                        {
                            if (item.RadarPoint.activeSelf == true) item.RadarPoint.SetActive(false);
                            continue;
                        }
                        float _x = (item.transform.position.x - Detector.transform.position.x) / Distance;
                        float _y = (item.transform.position.z - Detector.transform.position.z) / Distance;
                        bool isMoving = item.isMoving;
                        if (item.isAlwaysDetected == true) isMoving = true;
                        if (isMoving == true)
                        {
                            if (_x * _x + _y * _y > 1)
                            {
                                if (item.ShowBeyoundBoundary == false)
                                {
                                    if (item.RadarPoint.activeSelf == true) item.RadarPoint.SetActive(false);
                                }
                                else
                                {
                                    if (item.RadarPoint.activeSelf == false) item.RadarPoint.SetActive(true);

                                    var _len = Mathf.Sqrt(_x * _x + _y * _y);
                                    var ratio = 1 / _len;
                                    var rt = item.RadarPoint.transform as RectTransform;
                                    rt.anchoredPosition = new Vector2(_x * RadarRadius * ratio, _y * RadarRadius * ratio);
                                }
                            }
                            else
                            {
                                if (item.RadarPoint.activeSelf == false) item.RadarPoint.SetActive(true);
                                var rt = item.RadarPoint.transform as RectTransform;
                                rt.anchoredPosition = new Vector2(_x * RadarRadius, _y * RadarRadius);
                            }
                        }
                        else
                        {
                            if (item.RadarPoint.activeSelf == true) item.RadarPoint.SetActive(false);
                        }

                    }
                }
                {
                    RadarObject.transform.localEulerAngles = new Vector3(0, 0, North_Base_Angle + (rotatoin * Detector.transform.localEulerAngles.y));
                }
            }
            else
            {
                if(UseSeparatedThread)
                foreach (var item in Targets)
                {
                    if (item.RadarPoint.activeSelf == true) item.RadarPoint.SetActive(false);
                }
            }
        }
    }
    [Serializable]
    public class RadarDot
    {
        public GameObject Dot;
        public Color Color;
        public int TypeID;
    }
}