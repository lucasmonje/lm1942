using Core.Domain.Models;
using Core.Infrastructure.Dtos;
using Core.Utils.Unity;
using UnityEngine;

namespace Core.Domain.Entities
{
    public class PlayerDefinition : ScriptableObject, Instantiable
    {
        public GameObject Prefab;
        public int Speed;
        public int Health;
        public int Lives;
        [Range(10, 1000)] public int FireRate;
        public int LimitLeft;
        public int LimitRight;
        public int LimitTop;
        public int LimitBottom;
        public GunDefinition GunDefinition;

        public GameObject GetPrefab()
        {
            return Prefab;
        }
        
        public static PlayerDefinition FromDto(PlayerDto playerDto)
        {
            var player = CreateInstance<PlayerDefinition>();
            player.Prefab = ResourceHelper.LoadPrefab("player", playerDto.ResourcePath);
            player.Speed = playerDto.Speed;
            player.Health = playerDto.Health;
            player.Lives = playerDto.Lives;
            player.FireRate = playerDto.FireRate;
            player.LimitLeft = playerDto.LimitLeft;
            player.LimitRight = playerDto.LimitRight;
            player.LimitTop = playerDto.LimitTop;
            player.LimitBottom = playerDto.LimitBottom;
            player.GunDefinition = GunDefinition.FromDto(playerDto.GunDto);
            return player;
        }
    }
}