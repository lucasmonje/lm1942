using Core.Domain.Presentation;
using Core.Domain.Repositories;
using UnityEngine;

namespace Core.Infrastructure.Actions
{
    public class BulletCollidesWithEnemy
    {
        private readonly PlayerRepository playerRepository;

        public BulletCollidesWithEnemy(PlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }

        public void Execute(GunView gunView, EnemyView enemyView)
        {
            var enemyViewModel = enemyView.GetModel();
            enemyViewModel.Health = Mathf.Max(enemyViewModel.Health - 1, 0);

            if (enemyViewModel.Health.Equals(0))
            {
                playerRepository.AddScore(enemyViewModel.ScorePoints);
                gunView.DestroyView();
                enemyView.Explote();
            }
        }
    }
}