using Core.Domain.Presentation;

namespace Core.Infrastructure.Actions
{
    public class BulletCollidesWithPlayer
    {
        private readonly PlayerHealthDecrease playerHealthDecrease;

        public BulletCollidesWithPlayer(PlayerHealthDecrease playerHealthDecrease)
        {
            this.playerHealthDecrease = playerHealthDecrease;
        }

        public void Execute(GunView gunView, PlayerView playerView)
        {
            var damage = gunView.GetDamage();
            playerHealthDecrease.Execute(playerView, damage);
            gunView.DestroyView();
        }
    }
}