using System;
using System.Collections.Generic;
using Core.Domain.Entities;
using Core.Domain.Models;
using Core.Domain.Presentation;
using Core.Infrastructure.Presentation.Presenters;
using Core.Infrastructure.Presentation.Providers;
using Core.Utils.Unity;
using UnityEngine;

namespace Core.Infrastructure.Presentation.Views
{
    public class EnemyGunController : MonoBehaviour, GunView
    {
        public RectTransform CollisionTarget;

        public List<GameObject> Bullets;

        private string id;
        private RectTransform rectTransform;
        private GunPresenter presenter;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            presenter = GunPresenterProvider.Provide(this);
        }

        public void Init(GunDefinition gunDefinition, bool enhancedGun)
        {
            presenter.Init(gunDefinition);
        }

        public int GetDamage()
        {
            return presenter.GetDamage();
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
        
        public RectTransform CollisionRectTransform
        {
            get { return CollisionTarget; }
        }

        public CollisionType CollisionType
        {
            get { return CollisionType.EnemyBullet; }
        }

        public Vector3 GetAnchoredPosition()
        {
            return rectTransform.anchoredPosition;
        }

        public void SetAnchoredPosition(Vector3 position)
        {
            rectTransform.anchoredPosition = position;
        }

        public Vector2 GetScreenPosition()
        {
            return CanvasHelper.InverseTransformPoint(rectTransform.position);
        }

        public void RotateTowards(Vector3 target)
        {
            var vectorToTarget = target - CanvasHelper.InverseTransformPoint(rectTransform.position);
            var angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            var q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = q;
        }

        public void DestroyView()
        {
            Destroy(gameObject);
        }
    }
}