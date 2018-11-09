using UnityEngine;

namespace Core.Utils.Extensions
{
    public static class UnityObjectsExtensions
    {
        internal static void Stretch(this Component component)
        {
            var rectTransform = component.GetComponent<RectTransform>();
            if (rectTransform == null) return;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }

        internal static void StretchTop(this Component component, float height = 0)
        {
            var rectTransform = component.GetComponent<RectTransform>();
            if (rectTransform == null) return;
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = new Vector2(0, height);
            rectTransform.pivot = new Vector2(0.5f, 1f);
        }

        internal static void ScaleOne(this Component component)
        {
            component.GetComponent<Transform>().localScale = Vector3.one;
        }

        public static Rect RectRelativeTo(this RectTransform transform, Transform to)
        {
            var matrix = to.worldToLocalMatrix * transform.localToWorldMatrix;

            var rect = transform.rect;

            var p1 = new Vector2(rect.xMin, rect.yMin);
            var p2 = new Vector2(rect.xMax, rect.yMax);

            p1 = matrix.MultiplyPoint(p1);
            p2 = matrix.MultiplyPoint(p2);

            rect.xMin = p1.x;
            rect.yMin = p1.y;
            rect.xMax = p2.x;
            rect.yMax = p2.y;

            return rect;
        }

        public static void Clone(this RectTransform origin, RectTransform target)
        {
            if (target == null) return;
            target.anchorMin = origin.anchorMin;
            target.anchorMax = origin.anchorMax;
            target.offsetMax = origin.offsetMax;
            target.offsetMin = origin.offsetMin;
            target.pivot = origin.pivot;
            target.anchoredPosition = origin.anchoredPosition;
        }

        public static bool Overlaps(this RectTransform a, RectTransform b)
        {
            var aRect = a.WorldRect();
            var bRect = b.WorldRect();
            return aRect.Overlaps(bRect);
        }

        public static bool Overlaps(this RectTransform a, RectTransform b, bool allowInverse)
        {
            return a.WorldRect().Overlaps(b.WorldRect(), allowInverse);
        }

        public static Rect WorldRect(this RectTransform rectTransform)
        {
            var sizeDelta = rectTransform.rect;
            var rectTransformWidth = sizeDelta.width * rectTransform.lossyScale.x;
            var rectTransformHeight = sizeDelta.height * rectTransform.lossyScale.y;

            var position = rectTransform.position;
            return new Rect(position.x - rectTransformWidth / 2f, position.y - rectTransformHeight / 2f,
                rectTransformWidth, rectTransformHeight);
        }
    }
}