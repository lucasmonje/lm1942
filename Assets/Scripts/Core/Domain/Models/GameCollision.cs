using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Functional.Maybe;

namespace Core.Domain.Models
{
    public interface GameCollision
    {
        string Id { get; }
        CollisionType CollisionType { get; }
        RectTransform MainRectTransform { get; }
        RectTransform CollisionRectTransform { get; }
    }

    public static class GameCollisionExtensions
    {
        public static GameCollision[] GetEnemyBullets(this IEnumerable<GameCollision> list)
        {
            return list.Where(i => i.CollisionType.IsEnemyBullet()).ToArray();
        }

        public static GameCollision[] GetPlayerBullets(this IEnumerable<GameCollision> list)
        {
            return list.Where(i => i.CollisionType.IsPlayerBullet()).ToArray();
        }

        public static GameCollision[] GetPowerUps(this IEnumerable<GameCollision> list)
        {
            return list.Where(i => i.CollisionType.IsPowerUp()).ToArray();
        }

        public static GameCollision[] GetEnemies(this IEnumerable<GameCollision> list)
        {
            return list.Where(i => i.CollisionType.IsEnemy()).ToArray();
        }

        public static Maybe<GameCollision> GetPlayer(this IEnumerable<GameCollision> list)
        {
            return list.FirstOrDefault(i => i.CollisionType.IsPlayer()).ToMaybe();
        }

        public static string ToDebugString(this IEnumerable<GameCollision> list)
        {
            return "[GameCollisions]" + string.Join("," + Environment.NewLine, list.Select(Format).ToArray());
        }

        public static string ToDebugString(this GameCollision collision)
        {
            return Format(collision);
        }

        private static string Format(GameCollision collision)
        {
            return string.Format("id='{0}'; CollisionType='{1}'; MainRectTransform='{2}'", collision.Id,
                collision.CollisionType, collision.MainRectTransform);
        }
    }
}