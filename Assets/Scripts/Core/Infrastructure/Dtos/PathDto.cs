using System;
using Core.Domain.Utils;

namespace Core.Infrastructure.Dtos
{
    [Serializable]
    public class PathDto
    {
        public string Name;
        public Position SpawnPosition;
        public WayPointDto[] WayPoints;
    }
}