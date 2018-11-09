using Core.Domain.Presentation;

namespace Core.Infrastructure.Actions
{
    public class PlayerCollidesWithPowerUp
    {
        public void Execute(PowerUpView powerUpView, PlayerView playerView)
        {
            powerUpView.DestroyView();
            playerView.EnhanceGun();
        }
    }
}