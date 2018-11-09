using System;

namespace Core.Infrastructure.Dtos
{
    [Serializable]
    public class EnemiesDto
    {
        public float Delay;
        public string EnemyName;
        public int Count;
        public float InstantiationInterval;
    }
}