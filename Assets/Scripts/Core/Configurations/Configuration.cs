using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domain.Entities;
using Core.Domain.Models;
using Core.Infrastructure.Dtos;
using NSubstitute.Routing.Handlers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Configurations
{
    public class Configuration
    {
        public PlayerDefinition Player;
        public List<EnemyDefinition> AllEnemies = new List<EnemyDefinition>();
        public List<GunDefinition> AllGuns = new List<GunDefinition>();
        public List<PowerUpDefinition> AllPowerUps = new List<PowerUpDefinition>();
        public List<PathDefinition> AllPaths = new List<PathDefinition>();
        public List<LevelDefinition> AllLevels = new List<LevelDefinition>();

        public static Configuration Create()
        {
            return new Configuration();
        }

        public EnemyDefinition GetEnemyByName(string enemyName)
        {
            return AllEnemies.FirstOrDefault(e => e.Name.Equals(enemyName));
        }

        public ConfigurationDto ToDto(Func<Object, string> getAssetPath)
        {
            return new ConfigurationDto
            {
                Player = new PlayerDto
                {
                    Lives = Player.Lives,
                    Health = Player.Health,
                    Speed = Player.Speed,
                    ResourcePath = getAssetPath(Player.Prefab),
                    FireRate = Player.FireRate,
                    LimitLeft = Player.LimitLeft,
                    LimitRight = Player.LimitRight,
                    LimitTop = Player.LimitTop,
                    LimitBottom = Player.LimitBottom,
                    GunDto = ToGunDto(getAssetPath, Player.GunDefinition)
                },
                AllGuns = AllGuns.Select(gun => ToGunDto(getAssetPath, gun)).ToList(),
                AllEnemies = AllEnemies.Select(enemy => new EnemyDto
                {
                    Name = enemy.Name,
                    Health = enemy.Health,
                    Speed = enemy.Speed,
                    ResourcePath = getAssetPath(enemy.Prefab),
                    GunName = enemy.GunName,
                    PathName = enemy.PathName,
                    ScorePoints = enemy.ScorePoints
                }).ToList(),
                AllPowerUps = AllPowerUps.Select(p => new PowerUpDto
                {
                    Name = p.Name,
                    ResourcePath = getAssetPath(p.Prefab),
                    FromPosition = p.FromPosition,
                    ToPosition = p.ToPosition
                }).ToList(),
                AllPaths = AllPaths.Select(p => new PathDto
                {
                    Name = p.Name,
                    SpawnPosition = p.SpawnPosition,
                    WayPoints = p.WayPoints
                }).ToList(),
                AllLevels = AllLevels.Select(ToLevelDto).ToList()
            };
        }

        private static LevelDto ToLevelDto(LevelDefinition levelDefinition)
        {
            return new LevelDto
            {
                PowerUps = levelDefinition.PowerUps,
                Rounds = levelDefinition.Rounds,
                EnemiesList = levelDefinition.EnemiesList.Select(e => new EnemiesDto
                {
                    Count = e.Count,
                    Delay = e.Delay,
                    EnemyName = e.EnemyName,
                    InstantiationInterval = e.InstantiationInterval
                }).ToList()
            };
        }

        private static GunDto ToGunDto(Func<Object, string> getAssetPath, GunDefinition gun)
        {
            return new GunDto
            {
                Name = gun.Name,
                Damage = gun.Damage,
                ResourcePath = getAssetPath(gun.Prefab),
                ToPosition = gun.ToPosition,
                Speed = gun.Speed,
                AmountBullets = gun.AmountBullets
            };
        }
    }
}