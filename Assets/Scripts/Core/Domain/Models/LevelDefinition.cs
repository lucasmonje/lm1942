using System.Collections.Generic;
using System.Linq;
using Core.Domain.Entities;
using Core.Infrastructure.Dtos;
using UnityEngine;

namespace Core.Domain.Models
{
    public class LevelDefinition : ScriptableObject
    {
        public int Rounds;
        public int PowerUps;
        public List<EnemiesDefinition> EnemiesList = new List<EnemiesDefinition>();
        
        public static LevelDefinition FromDto(LevelDto playerDto)
        {
            var level = CreateInstance<LevelDefinition>();
            level.PowerUps = playerDto.PowerUps;
            level.Rounds = playerDto.Rounds;
            level.EnemiesList = playerDto.EnemiesList.Select(EnemiesDefinition.FromDto).ToList();
            return level;
        }
    }
}