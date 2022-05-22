using CLUNL.Unity3D.Sync;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
namespace HUDNavi
{
    public class NavigationCore : MonoBehaviour
    {
        public static NavigationCore CurrentCore;
        public bool useMutliThread;
        public List<NavigationTarget> Targets = new List<NavigationTarget>();
        public Camera TargetCamera;
        public List<PresetNaviPoint> OnScreenPresets;
        public List<PresetNaviPoint> OffScreenPresets;
        public GameObject HUDPointsHolder;
        public int DistancePrecision = 0;
        public float MaxPointDistance = -1;
        public float DistanceIntensity = 1;
        public string DistanceSI = "M";
        public Thickness ArrowAreaPadding;

        public int RefreshTime = 500;
        List<string> CullingIDs = new List<string>();
        [Serializable]
        public class PresetNaviPoint
        {
            public string ID;
            public NavigationPoint Point;
        }
        [Serializable]
        public class Thickness
        {
            public float Up = 100;
            public float Down = 100;
            public float Left = 100;
            public float Right = 100;
            public Thickness()
            {

            }
            public Thickness(float Up, float Down, float Left, float Right)
            {
                this.Up = Up;
                this.Down = Down;
                this.Left = Left;
                this.Right = Right;
            }
            public Thickness(Vector4 vector4)
            {
                Up = vector4.w;
                Down = vector4.x;
                Left = vector4.y;
                Right = vector4.z;
            }
        }
        public void CullNaviPoint(string name)
        {
            if (CullingIDs.Contains(name)) return;
            CullingIDs.Add(name);
        }
        public void UncullNaviPoint(string name)
        {
            if (CullingIDs.Contains(name)) CullingIDs.Remove(name);
        }
        [HideInInspector]
        public Vector3 SafeTargetCamPos;
        [HideInInspector]
        public float HolderW;
        [HideInInspector]
        public float HolderH;
        bool UseSeparatedThread = false;
        bool Running = false;
        bool l = false;
        public void Init(NavigationTarget item)
        {
            l = true;
            lock (Targets)
            {

                if (item.MappedHUDPoint == null)
                {
                    NavigationPoint Preferred = OnScreenPresets.First().Point;
                    foreach (var Point in OnScreenPresets)
                    {
                        if (Point.ID == item.TargetNavigationPointType)
                        {
                            Preferred = Point.Point;
                        }
                    }
                    {
                        item.MappedHUDPoint = GameObject.Instantiate(Preferred, HUDPointsHolder.transform).GetComponent<NavigationPoint>();
                        if (item.MappedHUDPoint.Label != null)
                            item.MappedHUDPoint.Label.text = item.label;
                    }
                }
                if (item.MappedHUDArrow == null)
                {
                    NavigationPoint Preferred = OffScreenPresets.First().Point;
                    foreach (var Point in OffScreenPresets)
                    {
                        if (Point.ID == item.TargetNavigationPointType)
                        {
                            Preferred = Point.Point;
                        }
                    }
                    {
                        item.MappedHUDArrow = GameObject.Instantiate(Preferred, HUDPointsHolder.transform).GetComponent<NavigationPoint>();
                        if (item.MappedHUDPoint.Label != null)
                            item.MappedHUDArrow.Label.text = item.label;
                    }
                }
                Targets.Add(item);
            }
            l = false;
            item.isAdded = true;
        }
        IEnumerator Start()
        {
            CurrentCore = this;
            yield return null;
            if (useMutliThread)
                if (Dispatcher.Inited())
                {
                    UseSeparatedThread = true;
                    Running = true;

                    Task.Run(async () =>
                    {
                        while (Running)
                        {
                            await Task.Delay(RefreshTime);
                            if (l) continue;
                            //try
                            {
                                lock (Targets)
                                {

                                    {

                                        if (ShowPoints == true)
                                        {
                                            foreach (var item in Targets)
                                            {
                                                bool willShow = true;
                                                if (item.isAdded == false) continue;
                                                if (MaxPointDistance != -1)
                                                {
                                                    if (item.OverrideMaxShowDistance != -2)
                                                    {
                                                        if (item.OverrideMaxShowDistance == -1)
                                                        {

                                                            if ((item.SafePos - SafeTargetCamPos).magnitude > MaxPointDistance)
                                                            {
                                                                willShow = false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if ((item.SafePos - SafeTargetCamPos).magnitude > item.OverrideMaxShowDistance)
                                                            {
                                                                willShow = false;
                                                            }
                                                        }
                                                    }
                                                    else willShow = true;
                                                }
                                                else
                                                {
                                                    if (item.OverrideMaxShowDistance != -2)
                                                    {
                                                        if (item.OverrideMaxShowDistance != -1)
                                                        {
                                                            if ((item.SafePos - SafeTargetCamPos).magnitude > item.OverrideMaxShowDistance)
                                                            {
                                                                willShow = false;
                                                            }
                                                        }
                                                    }
                                                    else willShow = true;
                                                }
                                                if (willShow == true)
                                                {
                                                    if(item.HierarchyActive == false)
                                                    {
                                                        willShow = false;
                                                    }else
                                                    foreach (var CulledID in CullingIDs)
                                                    {
                                                        if (item.TargetNavigationPointType == CulledID)
                                                        {
                                                            willShow = false;
                                                            break;
                                                        }
                                                    }
                                                }
                                                if (item._Show == false || willShow == false)
                                                {

                                                    {
                                                        if (item.SafeMappedHUDPointActive == true)
                                                            Dispatcher.Invoke(() => item.MappedHUDPoint.gameObject.SetActive(false));
                                                        if (item.SafeMappedHUDArrowActive == true)
                                                            Dispatcher.Invoke(() => item.MappedHUDArrow.gameObject.SetActive(false));
                                                    }
                                                    continue;
                                                }
                                                else
                                                {
                                                    var v = MapToCam_TS(item.SafePos);
                                                    if (IsMappedObjVisible(v))
                                                    {
                                                        if (item.SafeMappedHUDPointActive == false)
                                                            Dispatcher.Invoke(() =>
                                                                item.MappedHUDPoint.gameObject.SetActive(true));
                                                        if (item.SafeMappedHUDArrowActive == true)
                                                            Dispatcher.Invoke(() =>
                                                                item.MappedHUDArrow.gameObject.SetActive(false));
                                                        v.Scale(new Vector3(HolderW, HolderH, 0));
                                                        Dispatcher.Invoke(() =>
                                                        {
                                                            item.MappedHUDPoint.transform.position = v;
                                                        });
                                                        if (item.ShowDistance)
                                                        {
                                                            //Deal with label and distance.
                                                            Dispatcher.Invoke(() =>
                                                                {
                                                                    UpdateDistance(item, item.MappedHUDPoint);
                                                                });
                                                        }
                                                        else
                                                        {
                                                            if (item.SafeMappedHUDPointActive)
                                                                Dispatcher.Invoke(() =>
                                                                {
                                                                    item.MappedHUDPoint.Distance.gameObject.SetActive(false);
                                                                });
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (ShowArrows == true)
                                                        {
                                                            Dispatcher.Invoke(() =>
                                                            {
                                                                if (item.MappedHUDPoint.gameObject.activeSelf == true)
                                                                    item.MappedHUDPoint.gameObject.SetActive(false);
                                                                if (item.MappedHUDArrow.gameObject.activeSelf == false && item.WillShowOffScreenPoint == true)
                                                                    item.MappedHUDArrow.gameObject.SetActive(true);
                                                            });
                                                            if (item.WillShowOffScreenPoint == true)
                                                            {
                                                                {
                                                                    var location = v;
                                                                    if (location.z < 0) location *= -1;

                                                                    var RelateSystem2 = new Vector3(.5f, .5f, 0);//Center of the screen.
                                                                    location -= RelateSystem2;//Convert to relate to center of the screen.
                                                                    float Angle = Mathf.Atan2(location.y, location.x);//Angle of center.
                                                                    Dispatcher.Invoke(() =>
                                                                        {
                                                                            {
                                                                                var r = item.MappedHUDArrow.Icon.transform.rotation;
                                                                                r.eulerAngles = new Vector3(0, 0, Angle * Mathf.Rad2Deg);
                                                                                item.MappedHUDArrow.Icon.transform.rotation = r;
                                                                            }
                                                                        });
                                                                    Vector3 ActualPosition;
                                                                    if (location.x > 0)
                                                                    {
                                                                        ActualPosition = new Vector3(RelateSystem2.x, location.y, 0);
                                                                    }
                                                                    else
                                                                    {
                                                                        ActualPosition = new Vector3(-RelateSystem2.x, location.y, 0);
                                                                    }
                                                                    if (location.y > RelateSystem2.y)
                                                                    {
                                                                        ActualPosition = new Vector3(location.x, RelateSystem2.y, 0);
                                                                    }
                                                                    else if (location.y < -RelateSystem2.y)
                                                                    {
                                                                        ActualPosition = new Vector3(location.x, -RelateSystem2.y, 0);
                                                                    }
                                                                    ActualPosition += RelateSystem2;
                                                                    ActualPosition.Scale(new Vector3(HolderW, HolderH, 0));
                                                                    ActualPosition.x = Mathf.Clamp(ActualPosition.x, ArrowAreaPadding.Left, HolderW - ArrowAreaPadding.Right);
                                                                    ActualPosition.y = Mathf.Clamp(ActualPosition.y, ArrowAreaPadding.Down, HolderH - ArrowAreaPadding.Up);
                                                                    Dispatcher.Invoke(() =>
                                                                    {
                                                                        if (ActualPosition.y == float.NaN) ActualPosition.y = 0;
                                                                        if (ActualPosition.z == float.NaN) ActualPosition.z = 0;
                                                                        item.MappedHUDArrow.transform.position = ActualPosition;
                                                                    });

                                                                }
                                                                if (item.ShowDistance)
                                                                {
                                                                    //Deal with label and distance.
                                                                    Dispatcher.Invoke(() =>
                                                                        {
                                                                            if (item.MappedHUDArrow == null)
                                                                                UpdateDistance(item, item.MappedHUDArrow);
                                                                        });
                                                                }
                                                                else
                                                                {
                                                                    Dispatcher.Invoke(() =>
                                                                    {
                                                                        if (item.MappedHUDArrow == null)
                                                                            if (item.MappedHUDArrow.Distance.gameObject.activeSelf) item.MappedHUDArrow.Distance.gameObject.SetActive(false);
                                                                    });
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (item.SafeMappedHUDPointActive == true)
                                                                Dispatcher.Invoke(() =>
                                                                {
                                                                    if (item.MappedHUDPoint == null)
                                                                        item.MappedHUDPoint.gameObject.SetActive(false);
                                                                });
                                                            if (item.SafeMappedHUDArrowActive == true)
                                                                Dispatcher.Invoke(() =>
                                                                {
                                                                    if (item.MappedHUDArrow == null)
                                                                        item.MappedHUDArrow.gameObject.SetActive(false);
                                                                });
                                                        }
                                                    }
                                                }
                                            }
                                            if (Disabled == true) Disabled = false;
                                        }
                                        else
                                        {
                                            if (Disabled == false)
                                            {
                                                Disabled = true;
                                                foreach (var item in Targets)
                                                {

                                                    if (item.SafeMappedHUDPointActive == true)
                                                        Dispatcher.Invoke(() =>
                                                        {
                                                            if (item.MappedHUDPoint == null)
                                                                item.MappedHUDPoint.gameObject.SetActive(false);
                                                        });
                                                    if (item.SafeMappedHUDArrowActive == true)
                                                        Dispatcher.Invoke(() =>
                                                        {
                                                            if (item.MappedHUDArrow == null)
                                                                item.MappedHUDArrow.gameObject.SetActive(false);
                                                        });
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //catch (Exception e)
                            //{
                            //    Dispatcher.Invoke(() => { Debug.LogError(e); });
                            //}
                        }
                    });
                }
        }
        Matrix4x4 C_VP;
        public static Vector3 MapToCam(Camera camera, Vector3 Target)
        {

            return camera.WorldToViewportPoint(Target);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector3 MapToCam_TS(Vector3 Target)
        {
            var pos = new Vector4(Target.x, Target.y, Target.z, 1);
            Vector4 clipPos = C_VP * pos;
            float x = .5f + .5f * clipPos.x / clipPos.w;
            float y = .5f + .5f * clipPos.y / clipPos.w;

            return new Vector3(x, y, clipPos.w);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsMappedObjVisible(Vector3 Target)
        {
            return Target.z > 0 && Target.x > 0 && Target.x < 1 && Target.y > 0 && Target.y < 1;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateDistance(NavigationTarget target, NavigationPoint point)
        {
            if (point.Distance.text == null) return;
            float l = (target.transform.position - TargetCamera.transform.position).magnitude * DistanceIntensity;
            point.Distance.text = $"{l.ToString($"f{DistancePrecision}")} {DistanceSI}";
        }
        public bool ShowPoints = true;
        public bool ShowArrows = true;
        bool Disabled = false;
        void Update()
        {
            if (UseSeparatedThread)
            {
                C_VP = TargetCamera.projectionMatrix * TargetCamera.worldToCameraMatrix;
                //C_VP = TargetCamera.previousViewProjectionMatrix;
                var t = HUDPointsHolder.transform as RectTransform;
                HolderW = t.rect.width;
                HolderH = t.rect.height;
                SafeTargetCamPos = TargetCamera.transform.position;
                return;
            }
            if (ShowPoints == true)
            {
                foreach (var item in Targets)
                {
                    if (item.MappedHUDPoint == null)
                    {
                        NavigationPoint Preferred = OnScreenPresets.First().Point;
                        foreach (var Point in OnScreenPresets)
                        {
                            if (Point.ID == item.TargetNavigationPointType)
                            {
                                Preferred = Point.Point;
                            }
                        }
                        item.MappedHUDPoint = GameObject.Instantiate(Preferred, HUDPointsHolder.transform).GetComponent<NavigationPoint>();
                        if (item.MappedHUDPoint.Label != null)
                            item.MappedHUDPoint.Label.text = item.label;
                    }
                    if (item.MappedHUDArrow == null)
                    {
                        NavigationPoint Preferred = OffScreenPresets.First().Point;
                        foreach (var Point in OffScreenPresets)
                        {
                            if (Point.ID == item.TargetNavigationPointType)
                            {
                                Preferred = Point.Point;
                            }
                        }
                        item.MappedHUDArrow = GameObject.Instantiate(Preferred, HUDPointsHolder.transform).GetComponent<NavigationPoint>();
                        if (item.MappedHUDPoint.Label != null)
                            item.MappedHUDArrow.Label.text = item.label;
                    }
                    bool willShow = true;
                    if (MaxPointDistance != -1)
                    {
                        if (item.OverrideMaxShowDistance != -2)
                        {
                            if (item.OverrideMaxShowDistance == -1)
                            {
                                if ((item.transform.position - TargetCamera.transform.position).magnitude > MaxPointDistance)
                                {
                                    willShow = false;
                                }
                            }
                            else
                            {
                                if ((item.transform.position - TargetCamera.transform.position).magnitude > item.OverrideMaxShowDistance)
                                {
                                    willShow = false;
                                }
                            }
                        }
                        else willShow = true;
                    }
                    else
                    {
                        if (item.OverrideMaxShowDistance != -2)
                        {
                            if (item.OverrideMaxShowDistance != -1)
                            {
                                if ((item.transform.position - TargetCamera.transform.position).magnitude > item.OverrideMaxShowDistance)
                                {
                                    willShow = false;
                                }
                            }
                        }
                        else willShow = true;
                    }
                    if (willShow == true)
                    {
                        foreach (var CulledID in CullingIDs)
                        {
                            if (item.TargetNavigationPointType == CulledID)
                            {
                                willShow = false;
                                break;
                            }
                        }
                    }
                    if (item._Show == false || willShow == false)
                    {
                        if (item.MappedHUDPoint.gameObject.activeSelf == true)
                            item.MappedHUDPoint.gameObject.SetActive(false);
                        if (item.MappedHUDArrow.gameObject.activeSelf == true)
                            item.MappedHUDArrow.gameObject.SetActive(false);
                        continue;
                    }
                    else
                    {
                        var v = MapToCam(TargetCamera, item.transform.position);
                        if (IsMappedObjVisible(v))
                        {
                            if (item.MappedHUDPoint.gameObject.activeSelf == false)
                                item.MappedHUDPoint.gameObject.SetActive(true);
                            if (item.MappedHUDArrow.gameObject.activeSelf == true)
                                item.MappedHUDArrow.gameObject.SetActive(false);
                            var t = HUDPointsHolder.transform as RectTransform;
                            v.Scale(new Vector3(t.rect.width, t.rect.height, 0));
                            item.MappedHUDPoint.transform.position = v;
                            if (item.ShowDistance)
                            {
                                //Deal with label and distance.
                                UpdateDistance(item, item.MappedHUDPoint);
                            }
                            else
                            {
                                if (item.MappedHUDPoint.Distance.gameObject.activeSelf) item.MappedHUDPoint.Distance.gameObject.SetActive(false);
                            }
                        }
                        else
                        {
                            if (ShowArrows == true)
                            {

                                if (item.MappedHUDPoint.gameObject.activeSelf == true)
                                    item.MappedHUDPoint.gameObject.SetActive(false);
                                if (item.MappedHUDArrow.gameObject.activeSelf == false && item.WillShowOffScreenPoint == true)
                                    item.MappedHUDArrow.gameObject.SetActive(true);
                                if (item.WillShowOffScreenPoint == true)
                                {
                                    {
                                        var t = HUDPointsHolder.transform as RectTransform;
                                        var HolderW = t.rect.width;
                                        var HolderH = t.rect.height;
                                        var location = v;
                                        if (location.z < 0) location *= -1;

                                        var RelateSystem2 = new Vector3(.5f, .5f, 0);//Center of the screen.
                                        location -= RelateSystem2;//Convert to relate to center of the screen.
                                        float Angle = Mathf.Atan2(location.y, location.x);//Angle of center.
                                        {
                                            var r = item.MappedHUDArrow.Icon.transform.rotation;
                                            r.eulerAngles = new Vector3(0, 0, Angle * Mathf.Rad2Deg);
                                            item.MappedHUDArrow.Icon.transform.rotation = r;
                                        }
                                        Vector3 ActualPosition;
                                        if (location.x > 0)
                                        {
                                            ActualPosition = new Vector3(RelateSystem2.x, location.y, 0);
                                        }
                                        else
                                        {
                                            ActualPosition = new Vector3(-RelateSystem2.x, location.y, 0);
                                        }
                                        if (location.y > RelateSystem2.y)
                                        {
                                            ActualPosition = new Vector3(location.x, RelateSystem2.y, 0);
                                        }
                                        else if (location.y < -RelateSystem2.y)
                                        {
                                            ActualPosition = new Vector3(location.x, -RelateSystem2.y, 0);
                                        }
                                        ActualPosition += RelateSystem2;
                                        ActualPosition.Scale(new Vector3(HolderW, HolderH, 0));
                                        ActualPosition.x = Mathf.Clamp(ActualPosition.x, ArrowAreaPadding.Left, HolderW - ArrowAreaPadding.Right);
                                        ActualPosition.y = Mathf.Clamp(ActualPosition.y, ArrowAreaPadding.Down, HolderH - ArrowAreaPadding.Up);
                                        item.MappedHUDArrow.transform.position = ActualPosition;

                                    }
                                    if (item.ShowDistance)
                                    {
                                        //Deal with label and distance.
                                        UpdateDistance(item, item.MappedHUDArrow);
                                    }
                                    else
                                    {
                                        if (item.MappedHUDArrow.Distance.gameObject.activeSelf) item.MappedHUDArrow.Distance.gameObject.SetActive(false);
                                    }
                                }
                            }
                            else
                            {

                                if (item.MappedHUDPoint.gameObject.activeSelf == true)
                                    item.MappedHUDPoint.gameObject.SetActive(false);
                                if (item.MappedHUDArrow.gameObject.activeSelf == true)
                                    item.MappedHUDArrow.gameObject.SetActive(false);
                            }
                        }
                    }
                }
                if (Disabled == true) Disabled = false;
            }
            else
            {
                if (Disabled == false)
                {
                    Disabled = true;
                    foreach (var item in Targets)
                    {
                        if (item.MappedHUDPoint == null)
                        {
                            NavigationPoint Preferred = OnScreenPresets.First().Point;
                            foreach (var Point in OnScreenPresets)
                            {
                                if (Point.ID == item.TargetNavigationPointType)
                                {
                                    Preferred = Point.Point;
                                }
                            }
                            item.MappedHUDPoint = GameObject.Instantiate(Preferred, HUDPointsHolder.transform).GetComponent<NavigationPoint>();
                            if (item.MappedHUDPoint.Label != null)
                                item.MappedHUDPoint.Label.text = item.label;
                        }
                        if (item.MappedHUDArrow == null)
                        {
                            NavigationPoint Preferred = OffScreenPresets.First().Point;
                            foreach (var Point in OffScreenPresets)
                            {
                                if (Point.ID == item.TargetNavigationPointType)
                                {
                                    Preferred = Point.Point;
                                }
                            }
                            item.MappedHUDArrow = GameObject.Instantiate(Preferred, HUDPointsHolder.transform).GetComponent<NavigationPoint>();
                            if (item.MappedHUDPoint.Label != null)
                                item.MappedHUDArrow.Label.text = item.label;
                        }
                        if (item.MappedHUDPoint.gameObject.activeSelf == true)
                            item.MappedHUDPoint.gameObject.SetActive(false);
                        if (item.MappedHUDArrow.gameObject.activeSelf == true)
                            item.MappedHUDArrow.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

}