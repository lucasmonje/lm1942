using System;
using Core.Domain.Entities;
using Core.Domain.Models;
using Core.Domain.Presentation;
using Core.Infrastructure.Presentation.Presenters;
using Core.Infrastructure.Presentation.Providers;
using Core.Utils.Unity;
using UnityEngine;

namespace Core.Infrastructure.Presentation.Views
{
    public class PowerUpController : MonoBehaviour, PowerUpView
    {
        public RectTransform CollisionTarget;

        private string id;
        private RectTransform rectTransform;
        private PowerUpPresenter presenter;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            presenter = PowerUpPresenterProvider.Provide(this);
        }

        public void Init(PowerUpDefinition powerUpDefinition)
        {
            presenter.Init(powerUpDefinition);
        }

        private void Update()
        {
            presenter.OnUpdate(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            presenter.OnFixedUpdate();
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
            get { return CollisionType.PowerUp; }
        }

        public RectTransform CollisionRectTransform
        {
            get { return CollisionTarget; }
        }

        public Vector3 GetAnchoredPosition()
        {
            return rectTransform.anchoredPosition;
        }

        public void SetAnchoredPosition(Vector3 position)
        {
            rectTransform.anchoredPosition = position;
        }

        Vector2 PowerUpView.GetScreenPosition()
        {
            return GetScreenPosition();
        }

        public Vector3 GetScreenPosition()
        {
            return CanvasHelper.InverseTransformPoint(rectTransform.position);
        }

        public void DestroyView()
        {
            Destroy(gameObject);
        }
    }
}