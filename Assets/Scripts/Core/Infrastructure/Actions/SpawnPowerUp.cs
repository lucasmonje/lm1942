using Core.Domain.Entities;
using Core.Domain.Presentation;
using Core.Infrastructure.Factories;
using Core.Utils.Extensions;

namespace Core.Infrastructure.Actions
{
    public class SpawnPowerUp
    {
        private readonly CollisionInstantiator collisionInstantiator;

        public SpawnPowerUp(CollisionInstantiator collisionInstantiator)
        {
            this.collisionInstantiator = collisionInstantiator;
        }

        public void Execute(PowerUpDefinition powerUpDefinition)
        {
            var powerUpView = collisionInstantiator.Execute<PowerUpView>(powerUpDefinition);
            powerUpView.MainRectTransform.SetParent(GameProvider.GetGameView().GetPowerUpsContainer());
            powerUpView.MainRectTransform.ScaleOne();
            powerUpView.Init(powerUpDefinition);
        }
    }
}