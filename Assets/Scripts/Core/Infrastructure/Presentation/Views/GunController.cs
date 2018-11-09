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
    public class GunController : MonoBehaviour, GunView
    {
        public RectTransform CollisionTarget;
        public RectTransform EnhancedGunCollider;
        public RectTransform EnhancedGunContainer;

        private string id;
        private RectTransform rectTransform;
        private GunPresenter presenter;
        private bool enhancedGun;
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            presenter = GunPresenterProvider.Provide(this);
        }

        public void Init(GunDefinition gunDefinition, bool enhancedGun)
        {
            this.enhancedGun = enhancedGun;
            presenter.Init(gunDefinition);

            if (enhancedGun)
            {
                CollisionTarget.gameObject.SetActive(false);
                for (var i = 0; i < EnhancedGunContainer.childCount; i++)
                    EnhancedGunContainer.GetChild(i).gameObject.SetActive(true);
                EnhancedGunContainer.gameObject.SetActive(true);
            }
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
            get { return enhancedGun ? EnhancedGunCollider : CollisionTarget; }
        }

        public CollisionType CollisionType
        {
            get { return CollisionType.PlayerBullet; }
        }

        public Vector3 GetAnchoredPosition()
        {
            return rectTransform.anchoredPosition;
        }

        public int GetDamage()
        {
            return presenter.GetDamage();
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