using Core.Configurations;
using Core.Domain.Presentation;
using Core.Infrastructure.Factories;
using Core.Utils.Extensions;
using UnityEngine;

namespace Core.Infrastructure.Actions
{
    public class PlayerShoots
    {
        private readonly CollisionInstantiator collisionInstantiator;
        private readonly Configuration configuration;

        public PlayerShoots(CollisionInstantiator collisionInstantiator, Configuration configuration)
        {
            this.collisionInstantiator = collisionInstantiator;
            this.configuration = configuration;
        }

        public void Execute(Vector2 startPosition, bool enhancedGun)
        {
            var gunDefinition = configuration.Player.GunDefinition;
            var gunView = collisionInstantiator.Execute<GunView>(gunDefinition);
            gunView.MainRectTransform.SetParent(GameProvider.GetGameView().GetShootsContainer());
            gunView.MainRectTransform.anchoredPosition = gunView.MainRectTransform.InverseTransformPoint(startPosition);
            gunView.MainRectTransform.ScaleOne();
            gunView.Init(gunDefinition, enhancedGun);
        }
    }
}