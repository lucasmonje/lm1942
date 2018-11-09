using System;
using Core.Domain.Utils;

namespace Core.Infrastructure.Dtos
{
    [Serializable]
    public class PowerUpDto
    {
        public string Name;
        public string ResourcePath;
        public Position FromPosition;
        public Position ToPosition;

    }
}