using System;
using System.Collections.Generic;

namespace Core.Infrastructure.Dtos
{
    [Serializable]
    public class LevelDto
    {
        public int Rounds;
        public int PowerUps;
        public List<EnemiesDto> EnemiesList;

    }
}