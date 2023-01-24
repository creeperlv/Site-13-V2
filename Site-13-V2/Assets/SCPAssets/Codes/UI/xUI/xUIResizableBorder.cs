using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel
{
    public class xUIResizableBorder : MonoBehaviour
    {
        public RectTransform TargetTransform;
        public xUIDraggablePrimitive Top;
        public xUIDraggablePrimitive Bottom;
        public xUIDraggablePrimitive Left;
        public xUIDraggablePrimitive Right;
        public xUIDraggablePrimitive TopL;
        public xUIDraggablePrimitive TopR;
        public xUIDraggablePrimitive BottomL;
        public xUIDraggablePrimitive BottomR;
        public xUIDraggable WindowDrag;
        public Action<Vector2> OnSizeChange = null;
        public Action<Vector2> OnPositionChange = null;
        void Start()
        {
            Right.OnDragDelta = (v) =>
            {
                var s = TargetTransform.sizeDelta;
                s.x -= v.x;
                TargetTransform.sizeDelta = s;
                if (OnSizeChange != null) OnSizeChange(TargetTransform.sizeDelta);
            };
            Bottom.OnDragDelta = (v) =>
            {
                var s = TargetTransform.sizeDelta;
                s.y += v.y;
                TargetTransform.sizeDelta = s;
                if (OnSizeChange != null) OnSizeChange(TargetTransform.sizeDelta);
            };
            Top.OnDragDelta = (v) =>
            {
                var p = TargetTransform.anchoredPosition;
                p.y -= v.y;
                TargetTransform.anchoredPosition = p;
                var s = TargetTransform.sizeDelta;
                s.y -= v.y;
                TargetTransform.sizeDelta = s;
                if (OnSizeChange != null) OnSizeChange(TargetTransform.sizeDelta);
                if (OnPositionChange != null) OnPositionChange(TargetTransform.anchoredPosition);
            };
            Left.OnDragDelta = (v) =>
            {
                var p = TargetTransform.anchoredPosition;
                p.x -= v.x;
                TargetTransform.anchoredPosition = p;
                var s = TargetTransform.sizeDelta;
                s.x += v.x;
                TargetTransform.sizeDelta = s;
                if (OnSizeChange != null) OnSizeChange(TargetTransform.sizeDelta);
                if (OnPositionChange != null) OnPositionChange(TargetTransform.anchoredPosition);
            };
            BottomR.OnDragDelta = (v) =>
            {

                var s = TargetTransform.sizeDelta;
                s.y += v.y;
                s.x -= v.x;
                TargetTransform.sizeDelta = s;
                if (OnSizeChange != null) OnSizeChange(TargetTransform.sizeDelta);
            };
            BottomL.OnDragDelta = (v) =>
            {

                var p = TargetTransform.anchoredPosition;
                p.x -= v.x;
                TargetTransform.anchoredPosition = p;
                var s = TargetTransform.sizeDelta;
                s.y += v.y;
                s.x += v.x;
                TargetTransform.sizeDelta = s;
                if (OnSizeChange != null) OnSizeChange(TargetTransform.sizeDelta);
                if (OnPositionChange!= null) OnPositionChange(TargetTransform.anchoredPosition);
            };
            TopR.OnDragDelta = (v) =>
            {
                var p = TargetTransform.anchoredPosition;
                p.y -= v.y;
                TargetTransform.anchoredPosition = p;
                var s = TargetTransform.sizeDelta;
                s.y -= v.y;
                s.x -= v.x;
                TargetTransform.sizeDelta = s;
                if (OnSizeChange != null) OnSizeChange(TargetTransform.sizeDelta);
                if (OnPositionChange!= null) OnPositionChange(TargetTransform.anchoredPosition);

            };
            TopL.OnDragDelta = (v) =>
            {
                var p = TargetTransform.anchoredPosition;
                p.y -= v.y;
                p.x -= v.x;
                TargetTransform.anchoredPosition = p;
                var s = TargetTransform.sizeDelta;
                s.y -= v.y;
                s.x += v.x;
                TargetTransform.sizeDelta = s;
                if (OnSizeChange != null) OnSizeChange(TargetTransform.sizeDelta);
                if (OnPositionChange!= null) OnPositionChange(TargetTransform.anchoredPosition);

            };
        }

    }
}
