using Core.Domain.Utils;
using UniRx;
using UnityEngine;

namespace Core.Domain.Models
{
    public class Point : Weighted
    {
        public Position Position { get; private set; }
        public Vector2 ScreenPosition { get; private set; }
        public float DistanceBetweenPlayer { get; private set; }
        public int Weight { get; set; }

        public Point(Position position, Vector2 screenPosition, float distanceBetweenPlayer)
        {
            Position = position;
            ScreenPosition = screenPosition;
            DistanceBetweenPlayer = distanceBetweenPlayer;
        }
    }
}