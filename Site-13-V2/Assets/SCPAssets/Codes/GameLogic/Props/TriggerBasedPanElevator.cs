using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel.GameLogic.Props
{
    public class TriggerBasedPanElevator : MonoBehaviour
    {
        public List<EventTrigger> Pos1Triggers;
        public List<EventTrigger> Pos2Triggers;
        public Transform Pos1TargetPos;
        public Transform Pos2TargetPos;
        public float TimeLength;
        public List<GameObject> RunningConsole;
        public List<GameObject> IdleConsole;
        public List<AudioSource> StartSound;
        public List<AudioSource> EndSound;
        public List<AudioSource> RunningSound;
        public Transform ControlledTransform;
        [Header("True = 1, False = 2")]
        public bool CurrentPos;
        public Vector3 Velocity;
        void Start()
        {
            foreach (var item in Pos1Triggers)
            {
                item.AddCallback(() => {
                    StartCoroutine(Run());
                });
            }
            foreach (var item in Pos2Triggers)
            {
                item.AddCallback(() => {
                    StartCoroutine(Run());
                });
            }
            Velocity = -(Pos1TargetPos.position - Pos2TargetPos.position)/TimeLength;
        }
        bool Running = false;
        IEnumerator Run()
        {
            if (Running) yield break;
            Running = true;
            if (CurrentPos)
            {
                foreach (var item in Pos1Triggers)
                {
                    item.Executed = false;
                    item.gameObject.SetActive(false);
                }
                foreach (var item in Pos2Triggers)
                {
                    item.Executed = false;
                    item.gameObject.SetActive(true);
                }
            }
            else
            {
                foreach (var item in Pos1Triggers)
                {
                    item.Executed = false;
                    item.gameObject.SetActive(true);
                }
                foreach (var item in Pos2Triggers)
                {
                    item.Executed = false;
                    item.gameObject.SetActive(false);
                }
            }
            foreach (var item in IdleConsole)
            {
                item.SetActive(false);
            }
            foreach (var item in RunningConsole)
            {
                item.SetActive(true);
            }
            foreach (var item in StartSound)
            {
                item.Play();
            }
            foreach (var item in RunningSound)
            {
                item.Play();
            }
            yield return null;
            for (float i = 0; i < TimeLength; i+=Time.deltaTime)
            {
                if (CurrentPos)
                {
                    ControlledTransform.position += Time.deltaTime * Velocity;
                }
                else
                {
                    ControlledTransform.position -= Time.deltaTime * Velocity;
                }
                yield return null;
            }
            foreach (var item in RunningSound)
            {
                item.Stop();
            }
            foreach (var item in EndSound)
            {
                item.Play();
            }
            foreach (var item in IdleConsole)
            {
                item.SetActive(true);
            }
            foreach (var item in RunningConsole)
            {
                item.SetActive(false);
            }
            Running = false;
            CurrentPos =! CurrentPos;
        }
    }
}
