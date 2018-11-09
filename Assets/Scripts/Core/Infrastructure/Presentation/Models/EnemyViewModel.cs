using Core.Domain.Entities;
using Core.Domain.Models;

namespace Core.Infrastructure.Presentation.Models
{
    public class EnemyViewModel
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public int Speed { get; private set; }
        public int ScorePoints { get; private set; }
        public int Health { get; set; }
        public PathDefinition PathDefinition { get; private set; }
        public GunDefinition GunDefinition { get; private set; }

        public EnemyViewModel(string id, string name, int speed, int scorePoints, int health,
            PathDefinition pathDefinition, GunDefinition gunDefinition)
        {
            Id = id;
            Name = name;
            Speed = speed;
            ScorePoints = scorePoints;
            Health = health;
            PathDefinition = pathDefinition;
            GunDefinition = gunDefinition;
        }
    }
}