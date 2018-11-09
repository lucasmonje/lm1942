using System;
using Core.Domain.Entities;
using Core.Domain.Models;
using Core.Domain.Presentation;
using Core.Domain.Utils;
using Core.Infrastructure.Presentation.Presenters;
using Core.Infrastructure.Presentation.Providers;
using Core.Utils.Extensions;
using Core.Utils.Unity;
using UniRx;
using UnityEngine;

namespace Core.Infrastructure.Presentation.Views
{
    public class PlayerController : MonoBehaviour, PlayerView
    {
        public AnimationController AirplaneAnimator;
        public AnimationController ExplotionAnimator;
        public RectTransform CollisionTarget;
        private RectTransform limits;
        private float speed;
        private RectTransform rectTransform;
        private PlayerPresenter presenter;
        private PlayerPoints playerPoints;
        private CompositeDisposable disposables;
        private bool isExploting;

        public string Id
        {
            get { return "Player"; }
        }

        public RectTransform MainRectTransform
        {
            get { return rectTransform; }
        }

        public RectTransform CollisionRectTransform
        {
            get { return CollisionTarget; }
        }

        public CollisionType CollisionType
        {
            get { return CollisionType.Player; }
        }

        public PlayerPoints GetPlayerPoints()
        {
            return playerPoints;
        }

        public IObservable<Unit> StartAnimation()
        {
            AirplaneAnimator.Animator.SetTrigger("hit");
            return AirplaneAnimator.OnEndAnimationClipObservable();
        }
        
        public IObservable<Unit> Hit()
        {
            presenter.Hit();
            AirplaneAnimator.Animator.SetTrigger("hit");
            return AirplaneAnimator.OnEndAnimationClipObservable();
        }

        public void EnhanceGun()
        {
            presenter.EnhanceGun();
        }

        public void Explote()
        {
            isExploting = true;
            CollisionTarget.gameObject.SetActive(false);
            disposables.Add(ExplotionAnimator.OnEndAnimationClipObservable().
                Subscribe(_ => DestroyView()));
            ExplotionAnimator.gameObject.SetActive(true);
        }

        private void Awake()
        {
            disposables = new CompositeDisposable();
            rectTransform = GetComponent<RectTransform>();
            presenter = PlayerPresenterProvider.Provide(this);
        }

        public void Init(PlayerDefinition playerDefinition)
        {
            speed = playerDefinition.Speed;
            presenter.Init(playerDefinition);
        }

        public void CreateLimits(int left, int right, int top, int bottom)
        {
            var go = new GameObject(gameObject.name + "Limits");
            limits = go.AddComponent<RectTransform>();
            limits.SetParent(rectTransform.parent);
            limits.SetAsFirstSibling();
            limits.Stretch();
            limits.offsetMax = new Vector2(-right, -top);
            limits.offsetMin = new Vector2(left, bottom);
            limits.localScale = Vector3.one;
        }

        public Vector2 GetScreenPosition()
        {
            return CanvasHelper.InverseTransformPoint(rectTransform.position);
        }

        private void Update()
        {
            if (isExploting) return;
            
            playerPoints = CanvasHelper.GetPlayerPoints(GetScreenPosition(), 5);
            
            if (Input.GetButton("Fire1"))
                presenter.OnFirePressed();
        }

        private void OnDestroy()
        {
            if (disposables != null && !disposables.IsDisposed) disposables.Dispose();
            presenter.OnDestroy();
        }

        public void DestroyView()
        {
            if (gameObject != null)
                Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            if (limits == null || isExploting) return;

            var movement = new Vector2(Input.GetAxis("Horizontal"), 0) * speed;
            rectTransform.anchoredPosition += movement;
            if (!limits.Overlaps(rectTransform))
                rectTransform.anchoredPosition -= movement;

            movement = new Vector2(0, Input.GetAxis("Vertical")) * speed;
            rectTransform.anchoredPosition += movement;
            if (!limits.Overlaps(rectTransform))
                rectTransform.anchoredPosition -= movement;
        }
    }
}