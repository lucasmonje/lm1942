using System;
using Core.Configurations;
using Core.Domain.Events;
using Core.Domain.Proccesors;
using Core.Domain.Repositories;
using Core.Infrastructure.Factories;
using UniRx;

namespace Core.Infrastructure.Proccesors
{
    public class SpawnPlayerWhenHeLoseALife : Startable
    {
        private readonly PlayerRepository playerRepository;
        private readonly Configuration configuration;
        private readonly IObservable<GamePlayEvent> gamePlayEventObservable;

        public SpawnPlayerWhenHeLoseALife(PlayerRepository playerRepository, Configuration configuration,
            IObservable<GamePlayEvent> gamePlayEventObservable)
        {
            this.playerRepository = playerRepository;
            this.configuration = configuration;
            this.gamePlayEventObservable = gamePlayEventObservable;
        }

        public IDisposable Start()
        {
            return gamePlayEventObservable
                .Where(e => e.IsLostLifeEvent())
                .Where(e => playerRepository.GetLives() > 0)
                .Delay(TimeSpan.FromSeconds(2))
                .Subscribe(_ => GameProvider.GetGameView().InstantiatePlayer(configuration.Player));
        }
    }
}