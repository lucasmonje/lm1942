using Core.Domain.Models;
using Core.Infrastructure.Presentation.Models;
using UnityEngine;

namespace Core.Domain.Presentation
{
    public interface EnemyView : GameCollision
    {
        void Init(EnemyViewModel enemyViewModel);
        void SetAnchoredPosition(Vector3 position);
        Vector3 GetScreenPosition();
        void RotateTowards(Vector3 target);
        void DestroyView();
        void Explote();
        EnemyViewModel GetModel();
    }
}