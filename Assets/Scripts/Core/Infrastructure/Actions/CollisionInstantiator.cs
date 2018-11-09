using System;
using Core.Domain.Events;
using Core.Domain.Models;
using Core.Domain.Repositories;
using UniRx;
using Object = UnityEngine.Object;

namespace Core.Infrastructure.Actions
{
    public class CollisionInstantiator
    {
        private readonly CollisionRepository collisionRepository;
        private readonly Subject<CollisionEvent> collisionEventObserver;

        public CollisionInstantiator(CollisionRepository collisionRepository,
            Subject<CollisionEvent> collisionEventObserver)
        {
            this.collisionRepository = collisionRepository;
            this.collisionEventObserver = collisionEventObserver;
        }

        public T Execute<T>(Instantiable def) where T : GameCollision
        {
            var collision = Object.Instantiate(def.GetPrefab()).GetComponent<T>();
            AddCollision(collision);
            return collision;
        }
        
        public void AddCollision(GameCollision collision)
        {
            collisionRepository.Put(collision);
            collisionEventObserver.OnNext(CollisionEvent.InstantiatedCollision(collision.Id));
        }
    }
}