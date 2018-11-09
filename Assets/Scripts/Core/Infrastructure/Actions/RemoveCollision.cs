using Core.Domain.Events;
using Core.Domain.Models;
using Core.Domain.Repositories;
using UniRx;

namespace Core.Infrastructure.Actions
{
    public class RemoveCollision
    {
        private readonly CollisionRepository collisionRepository;
        private readonly Subject<CollisionEvent> collisionEventObserver;

        public RemoveCollision(CollisionRepository collisionRepository,
            Subject<CollisionEvent> collisionEventObserver)
        {
            this.collisionRepository = collisionRepository;
            this.collisionEventObserver = collisionEventObserver;
        }

        public void Execute(GameCollision collision)
        {
            collisionRepository.Delete(collision.Id);
            var enemiesCount = collisionRepository.GetAll().GetEnemies().Length;
            collisionEventObserver.OnNext(CollisionEvent.RemoveCollision(collision.Id, enemiesCount));
        }
    }
}