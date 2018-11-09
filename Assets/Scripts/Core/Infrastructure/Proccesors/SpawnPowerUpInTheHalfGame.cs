using System;
using System.Linq;
using Core.Configurations;
using Core.Domain.Proccesors;
using Core.Domain.Repositories;
using Core.Infrastructure.Actions;
using Functional.Maybe;
using UniRx;

namespace Core.Infrastructure.Proccesors
{
    public class SpawnPowerUpInTheHalfGame : Startable
    {
        private readonly Configuration configuration;
        private readonly PlayerRepository playerRepository;
        private readonly SpawnPowerUp spawnPowerUp;

        public SpawnPowerUpInTheHalfGame(Configuration configuration, PlayerRepository playerRepository,
            SpawnPowerUp spawnPowerUp)
        {
            this.configuration = configuration;
            this.playerRepository = playerRepository;
            this.spawnPowerUp = spawnPowerUp;
        }

        public IDisposable Start()
        {
            var level = playerRepository.GetLevel();
            return configuration.AllLevels.ElementAtOrDefault(level).ToMaybe()
                .SelectOrElse(levelDef => Observable.Interval(TimeSpan.FromSeconds(20))
                        .ObserveOn(Scheduler.MainThread)
                        .Take(levelDef.PowerUps)
                        .Do(_ => spawnPowerUp.Execute(configuration.AllPowerUps.ElementAt(0))).Subscribe(),
                    () => Disposable.Empty);
        }
    }
}