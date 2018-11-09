using System;
using Core.Domain.Events;
using Core.Domain.Proccesors;
using UniRx;

namespace Core.Infrastructure.Proccesors
{
    public class ObserveLevelWon : Startable
    {
        private readonly ISubject<GamePlayEvent> gamePlayEventSubject;
        private readonly IObservable<CollisionEvent> collisionObservable;

        public ObserveLevelWon(ISubject<GamePlayEvent> gamePlayEventSubject,
            IObservable<CollisionEvent> collisionObservable)
        {
            this.gamePlayEventSubject = gamePlayEventSubject;
            this.collisionObservable = collisionObservable;
        }

        public IDisposable Start()
        {
            return gamePlayEventSubject
                .Where(@event => @event.IsLastRoundEvent())
                .SelectMany(_ => collisionObservable)
                .Where(@event => @event.IsARemoveCollision())
                .Where(@event => @event.EnemiesCount.Equals(0))
                .Do(_ => gamePlayEventSubject.OnNext(GamePlayEvent.LevelWon()))
                .First()
                .Subscribe();
        }
    }
}