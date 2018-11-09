using Core.Infrastructure.Dtos;
using Core.Utils;
using Core.Utils.Unity;
using UnityEngine;

namespace Core.Domain.Entities
{
    public class EnemiesDefinition : ScriptableObject
    {
        [Range(0.2f, 5)] public float Delay;

        [SerializeField] [StringInList(typeof(PropertyDrawersHelper), "GetAllEnemies")]
        public string EnemyName;

        [Range(1, 30)] public int Count;
        [Range(0.2f, 1)] public float InstantiationInterval;

        public static EnemiesDefinition FromDto(EnemiesDto enemyDto)
        {
            var enemies = CreateInstance<EnemiesDefinition>();
            enemies.Count = enemyDto.Count;
            enemies.Delay = enemyDto.Delay;
            enemies.InstantiationInterval = enemyDto.InstantiationInterval;
            enemies.EnemyName = enemyDto.EnemyName;
            return enemies;
        }

        public override string ToString()
        {
            return string.Format("{0}, Delay: {1}, EnemyName: {2}, Count: {3}, InstantiationInterval: {4}",
                base.ToString(), Delay, EnemyName, Count, InstantiationInterval);
        }
    }
}