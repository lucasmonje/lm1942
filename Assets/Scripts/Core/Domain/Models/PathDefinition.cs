using Core.Domain.Utils;
using Core.Infrastructure.Dtos;
using UnityEngine;

namespace Core.Domain.Models
{
    public class PathDefinition : ScriptableObject
    {
        public string Name;
        public Position SpawnPosition;
        public WayPointDto[] WayPoints;

        public static PathDefinition FromDto(PathDto dto)
        {
            var pathDefinition = CreateInstance<PathDefinition>();
            pathDefinition.Name = dto.Name;
            pathDefinition.SpawnPosition = dto.SpawnPosition;
            pathDefinition.WayPoints = dto.WayPoints;
            return pathDefinition;
        }
    }
}