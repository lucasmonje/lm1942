using Core.Domain.Models;
using Core.Domain.Utils;
using Core.Infrastructure.Dtos;
using Core.Utils;
using Core.Utils.Unity;
using UnityEngine;

namespace Core.Domain.Entities
{
    public class EnemyDefinition : ScriptableObject, Instantiable
    {
        public string Name;
        public int Health;
        public GameObject Prefab;
        [Range(1, 10)] public int Speed;
        [SerializeField] [StringInList(typeof(PropertyDrawersHelper), "GetAllGuns")] public string GunName;
        [SerializeField] [StringInList(typeof(PropertyDrawersHelper), "GetAllPaths")] public string PathName;
        public int ScorePoints;

        public GameObject GetPrefab()
        {
            return Prefab;
        }

        public static EnemyDefinition FromDto(EnemyDto enemyDto)
        {
            var enemy = CreateInstance<EnemyDefinition>();
            enemy.Name = enemyDto.Name;
            enemy.Prefab = ResourceHelper.LoadPrefab(enemyDto.Name, enemyDto.ResourcePath);
            enemy.Speed = enemyDto.Speed;
            enemy.GunName = enemyDto.GunName;
            enemy.PathName = enemyDto.PathName;
            enemy.ScorePoints = enemyDto.ScorePoints;
            enemy.Health = enemyDto.Health;
            return enemy;
        }
    }
}