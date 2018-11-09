using Core.Configurations;
using Core.Domain.Events;
using Core.Domain.Presentation;
using Core.Domain.Repositories;
using UniRx;
using UnityEngine;

namespace Core.Infrastructure.Actions
{
    public class PlayerHealthDecrease
    {
        private readonly CollisionRepository collisionRepository;
        private readonly PlayerRepository playerRepository;
        private readonly IObserver<GamePlayEvent> gamePlayEventObserver;
        private readonly Configuration configuration;

        public PlayerHealthDecrease(CollisionRepository collisionRepository, PlayerRepository playerRepository,
            IObserver<GamePlayEvent> gamePlayEventObserver, Configuration configuration)
        {
            this.collisionRepository = collisionRepository;
            this.playerRepository = playerRepository;
            this.gamePlayEventObserver = gamePlayEventObserver;
            this.configuration = configuration;
        }

        public void Execute(PlayerView playerView, int damage)
        {
            var health = playerRepository.GetHealth();
            var newhealth = Mathf.Max(health - damage, 0);
            playerRepository.SetHealth(newhealth);

            if (newhealth.Equals(0))
            {
                var lives = playerRepository.GetLives() - 1;
                playerRepository.SetLives(lives);
                playerRepository.SetHealth(configuration.Player.Health);
                playerView.Explote();
                gamePlayEventObserver.OnNext(lives.Equals(0) ? GamePlayEvent.GameOver() : GamePlayEvent.LostLife());
            }
            else
            {
                playerView.Hit().Subscribe(_ => collisionRepository.Put(playerView));
            }
        }
    }
}