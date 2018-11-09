using UnityEngine;

namespace Demo
{
    public class TweenRotation : Tweener
    {
        public Vector3 From;
        public Vector3 To;
        public bool QuaternionLerp = false;

        Transform trans;

        public Transform CachedTransform
        {
            get
            {
                if (trans == null)
                    trans = transform;
                return trans;
            }
        }

        /// Tween's current value.
        public Quaternion Value
        {
            get { return CachedTransform.localRotation; }
            set { CachedTransform.localRotation = value; }
        }

        /// Tween the value.
        protected override void OnUpdate(float factor, bool isFinished)
        {
            Value = QuaternionLerp
                ? Quaternion.Slerp(Quaternion.Euler(From), Quaternion.Euler(To), factor)
                : Quaternion.Euler(new Vector3(
                    Mathf.Lerp(From.x, To.x, factor),
                    Mathf.Lerp(From.y, To.y, factor),
                    Mathf.Lerp(From.z, To.z, factor)));
        }

        /// Start the tweening operation.
        static public TweenRotation Begin(GameObject go, float duration, Quaternion rot)
        {
            TweenRotation comp = Begin<TweenRotation>(go, duration);
            comp.From = comp.Value.eulerAngles;
            comp.To = rot.eulerAngles;

            if (duration <= 0f)
            {
                comp.Sample(1f, true);
                comp.enabled = false;
            }

            return comp;
        }

        [ContextMenu("Set 'From' to current value")]
        public override void SetStartToCurrentValue()
        {
            From = Value.eulerAngles;
        }

        [ContextMenu("Set 'To' to current value")]
        public override void SetEndToCurrentValue()
        {
            To = Value.eulerAngles;
        }

        [ContextMenu("Assume value of 'From'")]
        void SetCurrentValueToStart()
        {
            Value = Quaternion.Euler(From);
        }

        [ContextMenu("Assume value of 'To'")]
        void SetCurrentValueToEnd()
        {
            Value = Quaternion.Euler(To);
        }
    }
}