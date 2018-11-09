using Core.Domain.Models;
using Core.Domain.Utils;
using Core.Infrastructure.Dtos;
using Core.Utils.Unity;
using UnityEngine;

namespace Core.Domain.Entities
{
    public class PowerUpDefinition : ScriptableObject, Instantiable
    {
        public string Name;
        public GameObject Prefab;
        public Position FromPosition;
        public Position ToPosition;

        public GameObject GetPrefab()
        {
            return Prefab;
        }

        public static PowerUpDefinition FromDto(PowerUpDto powerUpDto)
        {
            var powerUp = CreateInstance<PowerUpDefinition>();
            powerUp.Name = powerUpDto.Name;
            powerUp.FromPosition = powerUpDto.FromPosition;
            powerUp.ToPosition = powerUpDto.ToPosition;
            powerUp.Prefab = ResourceHelper.LoadPrefab(powerUpDto.Name, powerUpDto.ResourcePath);
            return powerUp;
        }
    }
}