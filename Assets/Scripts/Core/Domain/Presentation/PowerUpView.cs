using Core.Domain.Entities;
using Core.Domain.Models;
using UnityEngine;

namespace Core.Domain.Presentation
{
    public interface PowerUpView : GameCollision
    {
        void Init(PowerUpDefinition powerUpDefinition);
        void SetAnchoredPosition(Vector3 position);
        void DestroyView();
        Vector2 GetScreenPosition();
    }
}