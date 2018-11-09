using Core.Domain.Entities;
using Core.Domain.Models;
using Core.Domain.Utils;
using UniRx;
using UnityEngine;

namespace Core.Domain.Presentation
{
    public interface PlayerView : GameCollision
    {
        void Init(PlayerDefinition playerDefinition);
        Vector2 GetScreenPosition();
        void CreateLimits(int left, int right, int top, int bottom);
        PlayerPoints GetPlayerPoints();
        void Explote();
        void DestroyView();
        IObservable<Unit> Hit();
        void EnhanceGun();
    }
}