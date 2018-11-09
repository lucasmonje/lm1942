using System.Collections.Generic;
using Core.Configurations;
using Core.Domain.Models;

namespace Core.Infrastructure.Dtos
{
    public class ConfigurationDto
    {
        public PlayerDto Player;
        public List<EnemyDto> AllEnemies = new List<EnemyDto>();
        public List<GunDto> AllGuns = new List<GunDto>();
        public List<PowerUpDto> AllPowerUps = new List<PowerUpDto>();
        public List<PathDto> AllPaths = new List<PathDto>();
        public List<LevelDto> AllLevels = new List<LevelDto>();
    }
}