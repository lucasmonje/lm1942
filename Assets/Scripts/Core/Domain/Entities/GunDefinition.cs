using Core.Domain.Models;
using Core.Domain.Utils;
using Core.Infrastructure.Dtos;
using Core.Utils.Unity;
using UnityEngine;

namespace Core.Domain.Entities
{
    public class GunDefinition : ScriptableObject, Instantiable
    {
        public string Name;
        public GameObject Prefab;
        public int Damage;
        public int AmountBullets;
        [Range(1, 10)] public int Speed;
        public Position ToPosition;

        public GameObject GetPrefab()
        {
            return Prefab;
        }

        public static GunDefinition FromDto(GunDto gunDto)
        {
            var gun = CreateInstance<GunDefinition>();
            gun.Name = gunDto.Name;
            gun.Prefab = ResourceHelper.LoadPrefab(gunDto.Name, gunDto.ResourcePath);
            gun.Damage = gunDto.Damage;
            gun.AmountBullets = gunDto.AmountBullets;
            gun.Speed = gunDto.Speed;
            gun.ToPosition = gunDto.ToPosition;
            return gun;
        }
    }
}