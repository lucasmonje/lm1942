using System;
using UnityEngine;

namespace Demo
{
    public abstract class Tweener : MonoBehaviour
    {
        public enum Method
        {
            Linear,
            EaseIn,
            EaseOut,
            EaseInOut,
            BounceIn,
            BounceOut,
        }

        public enum Style
        {
            Once,
            Loop,
            PingPong,
        }

        public enum PlayDirection
        {
            Reverse = -1,
            Toggle = 0,
            Forward = 1,
        }

        [HideInInspector] public Method PlayMethod = Method.Linear;

        [HideInInspector] public Style PlayStyle = Style.Once;

        [HideInInspector] public AnimationCurve PlayAnimationCurve =
            new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

        [HideInInspector] public float Delay = 0f;

        [HideInInspector] public float Duration = 1f;

        /// Whether the tweener will use steeper curves for ease in / out style interpolation.
        [HideInInspector] public bool SteeperCurves = false;

        //[HideInInspector]
        //public List<EventDelegate> onFinished = new List<EventDelegate>();

        bool started = false;
        float startTime = 0f;
        float duration = 0f;
        float amountPerDelta = 1000f;
        float factor = 0f;
        public Action<bool> OnEnabledChanged;

        bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;

                if (OnEnabledChanged != null)
                    OnEnabledChanged(value);
            }
        }

        /// Amount advanced per delta time.
        public float AmountPerDelta
        {
            get
            {
                if (duration != Duration)
                {
                    duration = Duration;
                    amountPerDelta = Mathf.Abs((Duration > 0f) ? 1f / Duration : 1000f) * Mathf.Sign(amountPerDelta);
                }

                return amountPerDelta;
            }
        }

        /// Tween factor, 0-1 range.
        public float TweenFactor
        {
            get { return factor; }
            set { factor = Mathf.Clamp01(value); }
        }

        /// Direction that the tween is currently playing in.
        public PlayDirection Direction
        {
            get { return AmountPerDelta < 0f ? PlayDirection.Reverse : PlayDirection.Forward; }
        }

        /// This function is called by Unity when you add a component. Automatically set the starting values for convenience.
        void Reset()
        {
            if (!started)
            {
                SetStartToCurrentValue();
                SetEndToCurrentValue();
            }
        }

        /// Update as soon as it's started so that there is no delay.
        protected virtual void Start()
        {
            Update();
        }

        /// Update the tweening factor and call the virtual update function.
        void Update()
        {
            float delta = Time.deltaTime;
            float time = Time.time;

            if (!started)
            {
                started = true;
                startTime = time + Delay;
            }

            if (time < startTime)
                return;

            // Advance the sampling factor
            factor += AmountPerDelta * delta;

            // Loop style simply resets the play factor after it exceeds 1.
            if (PlayStyle == Style.Loop)
            {
                if (factor > 1f)
                {
                    factor -= Mathf.Floor(factor);
                }
            }
            else if (PlayStyle == Style.PingPong)
            {
                // Ping-pong style reverses the direction
                if (factor > 1f)
                {
                    factor = 1f - (factor - Mathf.Floor(factor));
                    amountPerDelta = -amountPerDelta;
                }
                else if (factor < 0f)
                {
                    factor = -factor;
                    factor -= Mathf.Floor(factor);
                    amountPerDelta = -amountPerDelta;
                }
            }

            // If the factor goes out of range and this is a one-time tweening operation, disable the script
            if ((PlayStyle == Style.Once) && (Duration == 0f || factor > 1f || factor < 0f))
            {
                factor = Mathf.Clamp01(factor);
                Sample(factor, true);

                // Disable this script unless the function calls above changed something
                if (Duration == 0f || (factor == 1f && amountPerDelta > 0f || factor == 0f && amountPerDelta < 0f))
                    Enabled = false;
            }
            else
                Sample(factor, false);
        }

        /// Mark as not started when finished to enable delay on next play.
        void OnDisable()
        {
            started = false;
        }

        /// Sample the tween at the specified factor.
        public void Sample(float factor, bool isFinished)
        {
            // Calculate the sampling value
            float val = Mathf.Clamp01(factor);

            if (PlayMethod == Method.EaseIn)
            {
                val = 1f - Mathf.Sin(0.5f * Mathf.PI * (1f - val));
                if (SteeperCurves)
                    val *= val;
            }
            else if (PlayMethod == Method.EaseOut)
            {
                val = Mathf.Sin(0.5f * Mathf.PI * val);

                if (SteeperCurves)
                {
                    val = 1f - val;
                    val = 1f - val * val;
                }
            }
            else if (PlayMethod == Method.EaseInOut)
            {
                const float pi2 = Mathf.PI * 2f;
                val = val - Mathf.Sin(val * pi2) / pi2;

                if (SteeperCurves)
                {
                    val = val * 2f - 1f;
                    float sign = Mathf.Sign(val);
                    val = 1f - Mathf.Abs(val);
                    val = 1f - val * val;
                    val = sign * val * 0.5f + 0.5f;
                }
            }
            else if (PlayMethod == Method.BounceIn)
            {
                val = BounceLogic(val);
            }
            else if (PlayMethod == Method.BounceOut)
            {
                val = 1f - BounceLogic(1f - val);
            }

            // Call the virtual update
            OnUpdate((PlayAnimationCurve != null) ? PlayAnimationCurve.Evaluate(val) : val, isFinished);
        }

        /// Main Bounce logic to simplify the Sample function
        float BounceLogic(float val)
        {
            if (val < 0.363636f) // 0.363636 = (1/ 2.75)
            {
                val = 7.5685f * val * val;
            }
            else if (val < 0.727272f) // 0.727272 = (2 / 2.75)
            {
                val = 7.5625f * (val -= 0.545454f) * val + 0.75f; // 0.545454f = (1.5 / 2.75)
            }
            else if (val < 0.909090f) // 0.909090 = (2.5 / 2.75)
            {
                val = 7.5625f * (val -= 0.818181f) * val + 0.9375f; // 0.818181 = (2.25 / 2.75)
            }
            else
            {
                val = 7.5625f * (val -= 0.9545454f) * val + 0.984375f; // 0.9545454 = (2.625 / 2.75)
            }

            return val;
        }

        /// Play the tween forward.
        public void PlayForward()
        {
            Play(true);
        }

        /// Play the tween in reverse.
        public void PlayReverse()
        {
            Play(false);
        }

        /// Manually activate the tweening process, reversing it if necessary.
        public void Play(bool forward)
        {
            amountPerDelta = Mathf.Abs(AmountPerDelta);
            if (!forward)
                amountPerDelta = -amountPerDelta;
            Enabled = true;
            Update();
        }

        /// Manually reset the tweener's state to the beginning.
        /// If the tween is playing forward, this means the tween's start.
        /// If the tween is playing in reverse, this means the tween's end.
        public void ResetToBeginning()
        {
            started = false;
            factor = (AmountPerDelta < 0f) ? 1f : 0f;
            Sample(factor, false);
        }

        /// Manually start the tweening process, reversing its direction.
        public void Toggle()
        {
            if (factor > 0f)
            {
                amountPerDelta = -AmountPerDelta;
            }
            else
            {
                amountPerDelta = Mathf.Abs(AmountPerDelta);
            }

            Enabled = true;
        }

        /// Actual tweening logic should go here.
        abstract protected void OnUpdate(float factor, bool isFinished);

        /// Starts the tweening operation.
        static public T Begin<T>(GameObject go, float duration) where T : Tweener
        {
            T comp = go.GetComponent<T>();
            if (comp == null)
            {
                comp = go.AddComponent<T>();
                if (comp == null)
                {
                    return default(T);
                }
            }

            comp.started = false;
            comp.factor = 0f;
            comp.amountPerDelta = Mathf.Abs(comp.AmountPerDelta);
            comp.Enabled = true;
            comp.Duration = duration;
            comp.PlayStyle = Style.Once;
            comp.PlayAnimationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

            return comp;
        }

        /// Set the 'from' value to the current one.
        public virtual void SetStartToCurrentValue()
        {
        }

        /// Set the 'to' value to the current one.
        public virtual void SetEndToCurrentValue()
        {
        }
    }
}