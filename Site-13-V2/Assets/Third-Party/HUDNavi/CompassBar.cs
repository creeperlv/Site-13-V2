using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HUDNavi
{
    public class CompassBar : MonoBehaviour
    {
        public GameObject Target;
        Vector3 startPosition;
        float Rate;
        public RectTransform Holder;
        void Start()
        {
            startPosition = transform.position;
            Rate = Holder.rect.width / 360f;
        }

        void Update()
        {
            float Angle = Target.transform.rotation.eulerAngles.y;
            if (Angle > 180) { Angle -= 360; }
            transform.position = startPosition + new Vector3(Angle * Rate, 0, 0);
        }
    }
}