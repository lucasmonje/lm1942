using UniRx;
using UnityEngine;

namespace Core.Infrastructure.Presentation.Views
{
    [RequireComponent(typeof(Animator))]
    public class AnimationController : MonoBehaviour
    {
        public Animator Animator { get; private set; }

        private void Awake()
        {
            Animator = GetComponent<Animator>();
        }

        private readonly Subject<Unit> endAnimationSubject = new Subject<Unit>();
        /// <summary>
        ///  OnEndAnimationClip() is called from the AirplaneAnimator when the animation dissapear.
        /// </summary>
        public void OnEndAnimationClip()
        {
            endAnimationSubject.OnNext(Unit.Default);
        }

        public IObservable<Unit> OnEndAnimationClipObservable()
        {
            return endAnimationSubject.First();
        }
    }
}