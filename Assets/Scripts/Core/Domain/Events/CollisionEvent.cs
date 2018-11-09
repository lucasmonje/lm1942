namespace Core.Domain.Events
{
    public class CollisionEvent
    {
        private const string InstantiatedCollisionEvent = "InstantiatedCollisionEvent";
        private const string RemoveCollisionEvent = "RemoveCollisionEvent";
        
        private string eventName;
        
        public string CollisionId { get; private set; }
        public int EnemiesCount { get; private set; }

        public static CollisionEvent InstantiatedCollision(string collisionId)
        {
            return new CollisionEvent {eventName = InstantiatedCollisionEvent, CollisionId = collisionId};
        }
        
        public static CollisionEvent RemoveCollision(string collisionId, int enemiesCount)
        {
            return new CollisionEvent {eventName = RemoveCollisionEvent, CollisionId = collisionId, EnemiesCount = enemiesCount};
        }

        public bool IsAInstantiatedCollision()
        {
            return InstantiatedCollisionEvent.Equals(eventName);
        }
        
        public bool IsARemoveCollision()
        {
            return RemoveCollisionEvent.Equals(eventName);
        }
    }
}