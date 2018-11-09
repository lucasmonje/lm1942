using Core.Domain.Entities;
using Core.Domain.Presentation;
using Core.Infrastructure.Factories;
using Core.Utils.Extensions;
using UnityEngine;

namespace Core.Infrastructure.Actions
{
    public class EnemyShoots
    {
        private readonly CollisionInstantiator collisionInstantiator;

        public EnemyShoots(CollisionInstantiator collisionInstantiator)
        {
            this.collisionInstantiator = collisionInstantiator;
        }

        public void Execute(Vector2 startPosition, GunDefinition gunDefinition)
        {
            for (var i = 0; i < gunDefinition.AmountBullets; i++)
            {
                var gunView = collisionInstantiator.Execute<GunView>(gunDefinition);
                gunView.MainRectTransform.SetParent(GameProvider.GetGameView().GetShootsContainer());
                gunView.MainRectTransform.anchoredPosition = gunView.MainRectTransform.InverseTransformPoint(startPosition);
                gunView.MainRectTransform.ScaleOne();
                gunView.Init(gunDefinition, false);
            }
        }
    }
}