using System.Collections.Generic;
using Core.Domain.Models;
using UniRx;

namespace Core.Domain.Repositories
{
    public interface CollisionRepository : Repository
    {
        void Put(GameCollision gameCollision);
        GameCollision[] GetAll();
        IObservable<GameCollision> GetById(string id);
        void Put(IEnumerable<GameCollision> collisions);
        void Delete(IEnumerable<string> collisionIds);
        void Delete(string collisionId);
    }
}