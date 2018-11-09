using System;
using System.Linq;
using Core.Domain.Entities;
using Core.Domain.Models;
using Core.Infrastructure.Dtos;

namespace Core.Configurations
{
    public static class ConfigurationParser
    {
        public static Type GetDtoType()
        {
            return typeof(ConfigurationDto);
        }

        public static Configuration Parse(object obj)
        {
            var dto = obj as ConfigurationDto;
            var allGuns = dto.AllGuns.Select(GunDefinition.FromDto).ToList();
            var allEnemies = dto.AllEnemies.Select(EnemyDefinition.FromDto).ToList();
            var allPowerUps = dto.AllPowerUps.Select(PowerUpDefinition.FromDto).ToList();
            var allLevels = dto.AllLevels.Select(LevelDefinition.FromDto).ToList();
            var levelsConfig = new Configuration
            {
                Player = PlayerDefinition.FromDto(dto.Player),
                AllGuns = allGuns,
                AllEnemies = allEnemies,
                AllPowerUps = allPowerUps,
                AllPaths = dto.AllPaths.Select(PathDefinition.FromDto).ToList(),
                AllLevels = allLevels
            };

            return levelsConfig;
        }
    }
}