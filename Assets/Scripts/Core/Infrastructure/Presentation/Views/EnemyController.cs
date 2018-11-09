using System;
using Core.Domain.Models;
using Core.Domain.Presentation;
using Core.Infrastructure.Presentation.Models;
using Core.Infrastructure.Presentation.Presenters;
using Core.Infrastructure.Presentation.Providers;
using Core.Utils.Unity;
using UniRx;
using UnityEngine;

namespace Core.Infrastructure.Presentation.Views
{
    public class EnemyController : MonoBehaviour, EnemyView
    {
        public RectTransform CollisionTarget;
        public AnimationController ExplotionAnimator;

        private RectTransform rectTransform;
        private EnemyPresenter presenter;
        private string id;
        private bool isExploting;
        private IDisposable disposable;

        private void Awake()
        {
            CollisionTarget.gameObject.SetActive(false);
            rectTransform = GetComponent<RectTransform>();
            presenter = EnemyPresenterProvider.Provide(this);
        }

        public void Init(EnemyViewModel enemyViewModel)
        {
            presenter.Init(enemyViewModel);
        }

        public void Explote()
        {
            isExploting = true;
            CollisionTarget.gameObject.SetActive(false);
            disposable = ExplotionAnimator.OnEndAnimationClipObservable().Subscribe(_ => DestroyView());
            ExplotionAnimator.gameObject.SetActive(true);
        }

        public EnemyViewModel GetModel()
        {
            return presenter.GetModel();
        }

        public void SetAnchoredPosition(Vector3 position)
        {
            rectTransform.anchoredPosition = position;
        }

        private void Update()
        {
            if (!isExploting) presenter.OnUpdate(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (!isExploting) presenter.OnFixedUpdate();
        }

        public Vector3 GetScreenPosition()
        {
            return CanvasHelper.InverseTransformPoint(rectTransform.position);
        }

        public void RotateTowards(Vector3 target)
        {
            var vectorToTarget = target - CanvasHelper.InverseTransformPoint(rectTransform.position);
            var angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            var q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = q;
            CollisionTarget.gameObject.SetActive(true);
        }

        public void DestroyView()
        {
            if (gameObject != null)
                Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (disposable != null) disposable.Dispose();
        }

        public string Id
        {
            get { return id ?? (id = Guid.NewGuid().ToString()); }
        }

        public RectTransform MainRectTransform
        {
            get { return rectTransform; }
        }

        public CollisionType CollisionType
        {
            get { return CollisionType.Enemy; }
        }

        public RectTransform CollisionRectTransform
        {
            get { return CollisionTarget; }
        }
    }
}