using Core.Domain.Entities;
using Core.Domain.Models;
using Core.Domain.Utils;
using Core.Infrastructure.Presentation.Models;
using UniRx;
using UnityEngine;

namespace Core.Domain.Presentation
{
    public interface GameView
    {
        void Init();
        void InstantiatePlayer(PlayerDefinition playerDefinition);
        void InstantiateEnemy(EnemyDefinition enemyDefinition);
        RectTransform GetPlayerContainer();
        RectTransform GetEnemiesContainer();
        RectTransform GetShootsContainer();
        Vector3 GetPlayerScreenPosition();
        PlayerPoints GetPlayerPoints();
        RectTransform GetPowerUpsContainer();
    }
}