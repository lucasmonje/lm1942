using Core.Domain.Entities;
using Core.Domain.Models;
using Core.Domain.Presentation;
using Core.Domain.Utils;
using Core.Infrastructure.Actions;
using Core.Infrastructure.Factories;
using Core.Infrastructure.Presentation.Models;
using Core.Infrastructure.Presentation.Presenters;
using Core.Infrastructure.Presentation.Providers;
using Core.Utils.Extensions;
using UniRx;
using UnityEngine;

namespace Core.Infrastructure.Presentation.Views
{
    public class GameController : MonoBehaviour, GameView
    {
        private static readonly Vector3 InitialPlayerPosition = new Vector3(0, -700);

        public RectTransform PlayerContainer;
        public RectTransform EnemiesContainer;
        public RectTransform ShootsContainer;
        public RectTransform PowerUpsContainer;

        private GamePresenter presenter;
        private PlayerController playerController;
        private CollisionInstantiator collisionInstantiator;
        private CompositeDisposable disposables = new CompositeDisposable();
        private MapEnemyViewModel mapEnemyViewModel;

        public RectTransform GetPlayerContainer()
        {
            return PlayerContainer;
        }

        public RectTransform GetEnemiesContainer()
        {
            return EnemiesContainer;
        }

        public RectTransform GetShootsContainer()
        {
            return ShootsContainer;
        }

        public PlayerPoints GetPlayerPoints()
        {
            return playerController.GetPlayerPoints();
        }

        public RectTransform GetPowerUpsContainer()
        {
            return PowerUpsContainer;
        }

        public Vector3 GetPlayerScreenPosition()
        {
            return playerController != null ? ((Vector3) playerController.GetScreenPosition()) : Vector3.zero;
        }

        private void Awake()
        {
            collisionInstantiator = GameProvider.ProvideCollisionInstantiator();
            presenter = GamePresenterProvider.Provide(this);
            mapEnemyViewModel = GameProvider.ProvideMapEnemyViewModel();

        }

        public void Init()
        {
            presenter.Init();
        }

        public void InstantiatePlayer(PlayerDefinition playerDefinition)
        {
            playerController = Instantiate(playerDefinition.GetPrefab()).GetComponent<PlayerController>();
            var rectTransform = playerController.GetComponent<RectTransform>();
            rectTransform.SetParent(PlayerContainer);
            rectTransform.ScaleOne();
            rectTransform.localPosition = InitialPlayerPosition;
            playerController.Init(playerDefinition);
            disposables.Add(playerController.StartAnimation()
                .Subscribe(_ => collisionInstantiator.AddCollision(playerController)));
        }

        public void InstantiateEnemy(EnemyDefinition enemyDefinition)
        {
            var enemyController = collisionInstantiator.Execute<EnemyController>(enemyDefinition);
            var rectTransform = enemyController.GetComponent<RectTransform>();
            rectTransform.SetParent(EnemiesContainer);
            rectTransform.ScaleOne();
            enemyController.Init(mapEnemyViewModel.Execute(enemyController.Id, enemyDefinition));
        }
        
        private void OnDestroy()
        {
            if (!disposables.IsDisposed) disposables.Dispose();
        }
    }
}