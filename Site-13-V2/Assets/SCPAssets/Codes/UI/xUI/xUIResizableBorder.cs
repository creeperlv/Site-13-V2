using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Site13Kernel
{
    public class xUIResizableBorder : MonoBehaviour
    {
        public RectTransform TargetTransform;
        public bool IsCentered = false;
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
                RightBorder(v);
            };
            Bottom.OnDragDelta = (v) =>
            {
                BottomBorder(v);
            };
            Top.OnDragDelta = (v) =>
            {
                TopBorder(v);
            };
            Left.OnDragDelta = (v) =>
            {
                LeftBorder(v);
            };
            if (BottomR != null)
                BottomR.OnDragDelta = (v) =>
                {
                    BottomBorder(v);
                    RightBorder(v);
                    return;
                };
            if (BottomR != null)
                BottomL.OnDragDelta = (v) =>
                {

                    BottomBorder(v);
                    LeftBorder(v);
                    return;
                };
            if (TopR != null)
                TopR.OnDragDelta = (v) =>
                {

                    TopBorder(v);
                    RightBorder(v);
                    return;

                };
            if (TopL != null)
                TopL.OnDragDelta = (v) =>
                {

                    TopBorder(v);
                    LeftBorder(v);
                    return;

                };
        }

        private void LeftBorder(Vector2 v)
        {
            var p = TargetTransform.anchoredPosition;

            if (IsCentered)

                p.x -= v.x / 2;
            else p.x -= v.x;
            TargetTransform.anchoredPosition = p;
            var s = TargetTransform.sizeDelta;
            if (IsCentered)
                s.x += v.x;/// 2;
            else
                s.x += v.x;
            TargetTransform.sizeDelta = s;
            if (OnSizeChange != null) OnSizeChange(TargetTransform.sizeDelta);
            if (OnPositionChange != null) OnPositionChange(TargetTransform.anchoredPosition);
        }

        private void TopBorder(Vector2 v)
        {
            var p = TargetTransform.anchoredPosition;
            if (IsCentered)
            {
                p.y -= v.y / 2;
            }
            else
                p.y -= v.y;
            TargetTransform.anchoredPosition = p;
            var s = TargetTransform.sizeDelta;

            if (IsCentered)
                s.y -= v.y;// / 2;
            else
                s.y -= v.y;
            TargetTransform.sizeDelta = s;
            if (OnSizeChange != null) OnSizeChange(TargetTransform.sizeDelta);
            if (OnPositionChange != null) OnPositionChange(TargetTransform.anchoredPosition);
        }

        private void BottomBorder(Vector2 v)
        {
            if (IsCentered)
            {
                var p = TargetTransform.anchoredPosition;
                p.y -= v.y / 2;
                TargetTransform.anchoredPosition = p;
            }
            var s = TargetTransform.sizeDelta;
            s.y += v.y;
            TargetTransform.sizeDelta = s;
            if (OnSizeChange != null) OnSizeChange(TargetTransform.sizeDelta);
        }

        private void RightBorder(Vector2 v)
        {
            if (IsCentered)
            {
                var p = TargetTransform.anchoredPosition;
                p.x -= v.x / 2;
                TargetTransform.anchoredPosition = p;
            }
            var s = TargetTransform.sizeDelta;
            s.x -= v.x;
            TargetTransform.sizeDelta = s;
            if (OnSizeChange != null) OnSizeChange(TargetTransform.sizeDelta);
        }
    }
}
