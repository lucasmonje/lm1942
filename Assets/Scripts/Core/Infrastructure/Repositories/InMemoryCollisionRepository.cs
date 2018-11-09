using System.Collections.Generic;
using System.Linq;
using Core.Domain.Models;
using Core.Domain.Repositories;
using Functional.Maybe;
using UniRx;

namespace Core.Infrastructure.Repositories
{
    public class InMemoryCollisionRepository : CollisionRepository
    {
        private readonly Dictionary<string, GameCollision> dictionary = new Dictionary<string, GameCollision>();

        public void Put(IEnumerable<GameCollision> collisions)
        {
            foreach (var collision in collisions)
            {
                dictionary[collision.Id] = collision;
            }
        }

        public void Put(GameCollision gameCollision)
        {
            dictionary[gameCollision.Id] = gameCollision;
        }

        public IObservable<GameCollision> GetById(string collisionId)
        {
            return Get(collisionId).SelectOrElse(Observable.Return, Observable.Empty<GameCollision>);
        }

        private Maybe<GameCollision> Get(string collisionId)
        {
            if (collisionId == null) return Maybe<GameCollision>.Nothing;
            GameCollision result;
            return dictionary.TryGetValue(collisionId, out result)
                ? result.ToMaybe()
                : Maybe<GameCollision>.Nothing;
        }

        public GameCollision[] GetAll()
        {
            return dictionary.Values.ToArray();
        }

        public void Delete(IEnumerable<string> collisionIds)
        {
            foreach (var collisionId in collisionIds)
                Delete(collisionId);
        }

        public void Delete(string collisionId)
        {
            dictionary.Remove(collisionId);
        }

        public void Clear()
        {
            dictionary.Clear();
        }
    }
}