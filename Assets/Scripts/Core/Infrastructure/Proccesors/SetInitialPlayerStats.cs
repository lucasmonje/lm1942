using System;
using Core.Configurations;
using Core.Domain.Presentation;
using Core.Domain.Proccesors;
using Core.Domain.Repositories;
using UniRx;

namespace Core.Infrastructure.Proccesors
{
    public class SetInitialPlayerStats : Startable
    {
        private readonly PlayerRepository playerRepository;
        private readonly Configuration configuration;

        public SetInitialPlayerStats(PlayerRepository playerRepository, Configuration configuration)
        {
            this.playerRepository = playerRepository;
            this.configuration = configuration;
        }

        public IDisposable Start()
        {
            playerRepository.SetScore(0);
            playerRepository.SetHealth(configuration.Player.Health);
            playerRepository.SetLives(configuration.Player.Lives);
            return Disposable.Empty;
        }
    }
}