using Site13Kernel.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.CutScenesOnly
{
    public class NaturalWalkSimulation : MonoBehaviour
    {
        public float Cycling = 1;
        public List<AudioSource> AvailableSources;
        public List<KVPair<int, List<AudioClip>>> AvailableClips;
        public int CurrentlyUsingClipGroup;
        public Transform MonitoringTransform;
        public Transform Head;
        public float HeadZDeltaIntensity;
        int UsingSource = 0;
        Dictionary<int, List<AudioClip>> ClipGroups;
        public void Start()
        {
            ClipGroups = Utilities.CollectionUtilities.ToDictionary<int, List<AudioClip>>(AvailableClips);
        }
        float Distance;
        Vector3 LP = Vector3.zero;
        void Update()
        {
            if (LP == Vector3.zero)
            {
                LP = MonitoringTransform.position;
                return;
            }
            var DELTA = (LP - MonitoringTransform.position).magnitude;
            Distance += DELTA;
            LP = MonitoringTransform.position;
            if (Distance > Cycling)
            {
                Distance = 0;
                if (AvailableSources.Count > 0)
                {
                    AvailableSources[UsingSource].clip = Utilities.Maths.ObtainOne(ClipGroups[CurrentlyUsingClipGroup]);
                    AvailableSources[UsingSource].Play();
                    UsingSource++;
                    if (UsingSource >= AvailableClips.Count)
                    {
                        UsingSource = 0;
                    }
                }
            }
            if (Mathf.Abs(DELTA) > 0.01f)
                if (Distance < Cycling / 2)
                {
                    Head.transform.localPosition += HeadZDeltaIntensity * Time.deltaTime * Vector3.up;
                }
                else
                {
                    Head.transform.localPosition -= HeadZDeltaIntensity * Time.deltaTime * Vector3.up;
                }
        }
    }
}
