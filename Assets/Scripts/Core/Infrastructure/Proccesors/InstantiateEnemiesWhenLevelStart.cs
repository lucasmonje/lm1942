using System;
using System.Linq;
using Core.Configurations;
using Core.Domain.Entities;
using Core.Domain.Events;
using Core.Domain.Models;
using Core.Domain.Presentation;
using Core.Domain.Proccesors;
using Core.Domain.Repositories;
using Core.Infrastructure.Actions;
using Core.Infrastructure.Factories;
using Core.Utils.Extensions;
using Functional.Maybe;
using UniRx;
using UnityEngine;

namespace Core.Infrastructure.Proccesors
{
    public class InstantiateEnemiesWhenLevelStart : Startable
    {
        private readonly PlayerRepository playerRepository;
        private readonly Configuration configuration;
        private readonly IObserver<GamePlayEvent> gamePlayEventObserver;

        public InstantiateEnemiesWhenLevelStart(PlayerRepository playerRepository, Configuration configuration,
            IObserver<GamePlayEvent> gamePlayEventObserver)
        {
            this.playerRepository = playerRepository;
            this.configuration = configuration;
            this.gamePlayEventObserver = gamePlayEventObserver;
        }

        public IDisposable Start()
        {
            var level = playerRepository.GetLevel();
            return configuration.AllLevels.ElementAtOrDefault(level).ToMaybe()
                .SelectMaybe(levelDef => levelDef.EnemiesList.ElementAtOrDefault(0).ToMaybe(), Tuple.Create)
                .SelectOrElse(t => Cycle(t.Item1, t.Item2).ObserveOn(Scheduler.MainThread).Subscribe(), 
                    () =>
                    {
                        Debug.LogError("NO MORE LEVELS CONFIGURED");
                        return Disposable.Empty;
                    });
        }

        private IObservable<long> Cycle(LevelDefinition levelDefinition, EnemiesDefinition currentEnemies)
        {
            return Observable.Start(() => currentEnemies)
                .SelectMany(def =>
                    def.Delay.Equals(0)
                        ? InstantiateEnemies(def)
                        : Observable.Timer(TimeSpan.FromSeconds(def.Delay))
                            .SelectMany(_ => InstantiateEnemies(def)))
                .SelectMany(_ =>
                    levelDefinition.EnemiesList.Next(currentEnemies).SelectOrElse(next => Cycle(levelDefinition, next),
                        () =>
                        {
                            gamePlayEventObserver.OnNext(GamePlayEvent.LastRound());
                            return Observable.Empty<long>();
                        }));
        }

        private IObservable<Unit> InstantiateEnemies(EnemiesDefinition enemiesDef)
        {
            return Observable.Interval(TimeSpan.FromSeconds(enemiesDef.InstantiationInterval))
                .Take(enemiesDef.Count)
                .Do(_ =>
                {
                    var def = configuration.GetEnemyByName(enemiesDef.EnemyName);
                    GameProvider.GetGameView().InstantiateEnemy(def);
                })
                .AsSingleUnitObservable();
        }
    }
}