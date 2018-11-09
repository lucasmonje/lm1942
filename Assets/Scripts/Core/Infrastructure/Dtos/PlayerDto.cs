using System;
using Core.Domain.Entities;
using UnityEngine;

namespace Core.Infrastructure.Dtos
{
    [Serializable]
    public class PlayerDto
    {
        public string ResourcePath;
        public int Speed;
        public int Lives;
        public int Health;
        public int FireRate;
        public int LimitLeft;
        public int LimitRight;
        public int LimitTop;
        public int LimitBottom;
        public GunDto GunDto;
    }
}