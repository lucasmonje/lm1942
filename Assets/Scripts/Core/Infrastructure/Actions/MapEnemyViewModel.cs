using System.Linq;
using Core.Configurations;
using Core.Domain.Entities;
using Core.Infrastructure.Presentation.Models;
using UnityEngine;

namespace Core.Infrastructure.Actions
{
    public class MapEnemyViewModel
    {
        private readonly Configuration configuration;

        public MapEnemyViewModel(Configuration configuration)
        {
            this.configuration = configuration;
        }

        public EnemyViewModel Execute(string id, EnemyDefinition enemyDefinition)
        {
            var pathDefinition = configuration.AllPaths.FirstOrDefault(p => p.Name.Equals(enemyDefinition.PathName));
            var gunDefinition = configuration.AllGuns.FirstOrDefault(g => g.Name.Equals(enemyDefinition.GunName));
            return new EnemyViewModel(id, enemyDefinition.name,
                Mathf.Max(1, enemyDefinition.Speed),
                Mathf.Max(1, enemyDefinition.ScorePoints),
                Mathf.Max(1, enemyDefinition.Health), 
                pathDefinition, 
                gunDefinition);
        }
    }
}