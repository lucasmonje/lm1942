using System;
using Core.Domain.Entities;
using Core.Domain.Utils;

namespace Core.Infrastructure.Dtos
{
    [Serializable]
    public class EnemyDto
    {
        public string Name;
        public string ResourcePath;
        public int Speed;
        public string GunName;
        public string PathName;
        public int ScorePoints;
        public int Health;
    }
}