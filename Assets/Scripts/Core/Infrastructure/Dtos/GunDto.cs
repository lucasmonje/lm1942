using System;
using Core.Domain.Utils;

namespace Core.Infrastructure.Dtos
{
    [Serializable]
    public class GunDto
    {
        public string Name;
        public string ResourcePath;
        public int Damage;
        public int AmountBullets;
        public Position ToPosition;
        public int Speed;
    }
}