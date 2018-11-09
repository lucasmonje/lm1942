using Core.Configurations;
using Core.Domain.Presentation;
using UnityEngine;

namespace Core.Infrastructure.Actions
{
    public class PlayerCollidesWithEnemy
    {
        private readonly PlayerHealthDecrease playerHealthDecrease;
        private readonly Configuration configuration;


        public PlayerCollidesWithEnemy(PlayerHealthDecrease playerHealthDecrease, Configuration configuration)
        {
            this.playerHealthDecrease = playerHealthDecrease;
            this.configuration = configuration;
        }

        public void Execute(PlayerView playerView, EnemyView enemyView)
        {
            var enemyViewModel = enemyView.GetModel();
            enemyViewModel.Health = Mathf.Max(enemyViewModel.Health - 1, 0);
            if (enemyViewModel.Health.Equals(0))
                enemyView.Explote();

            playerHealthDecrease.Execute(playerView, configuration.Player.Health);
        }
    }
}