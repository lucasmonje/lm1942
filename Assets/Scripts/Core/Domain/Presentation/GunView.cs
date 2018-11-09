using Core.Domain.Entities;
using Core.Domain.Models;
using UnityEngine;

namespace Core.Domain.Presentation
{
    public interface GunView : GameCollision
    {
        void Init(GunDefinition gunDefinition, bool enhancedGun);
        void SetAnchoredPosition(Vector3 position);
        Vector2 GetScreenPosition();
        void RotateTowards(Vector3 target);
        void DestroyView();
        Vector3 GetAnchoredPosition();
        int GetDamage();
    }
}