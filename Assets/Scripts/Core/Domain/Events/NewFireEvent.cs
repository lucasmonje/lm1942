using Core.Domain.Entities;
using UnityEngine;

namespace Core.Domain.Events
{
    public class NewFireEvent
    {
        private const string NewFireFromPlayerEvent = "NewFireFromPlayerEvent";
        private const string NewFireFromEnemyEvent = "NewFireFromEnemyEvent";

        private string eventName;
        public Vector2 StartPosition { get; private set; }
        public GunDefinition GunDefinition { get; private set; }

        public static NewFireEvent FromPlayer(Vector2 startPosition)
        {
            return new NewFireEvent {eventName = NewFireFromPlayerEvent, StartPosition = startPosition};
        }

        public static NewFireEvent FromEnemy(Vector2 startPosition, GunDefinition gunDefinition)
        {
            return new NewFireEvent
            {
                eventName = NewFireFromEnemyEvent,
                StartPosition = startPosition,
                GunDefinition = gunDefinition
            };
        }

        public bool IsFromPlayer()
        {
            return NewFireFromPlayerEvent.Equals(eventName);
        }

        public bool IsFromEnemy()
        {
            return NewFireFromEnemyEvent.Equals(eventName);
        }
    }
}