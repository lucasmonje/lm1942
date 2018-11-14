using System;
using Core.Domain.Presentation;
using Core.Infrastructure.Factories;
using Core.Utils.Extensions;
using Core.Utils.Unity;
using UniRx;
using UnityEngine;

namespace Core.Infrastructure.Presentation.Views
{
    public class InitGame : MonoBehaviour
    {
        public ScrollingBackground ScrollingBackground;
        public Canvas Canvas;
        public RectTransform GameContainer;
        public RectTransform PopupsContainer;
        public GameObject GameScreenPrefab;
        public GameObject LevelWonPrefab;
        public GameObject GameOverScreenPrefab;
        private GameView gameView;
        private CompositeDisposable disposables;

        private void Awake()
        {
            CanvasHelper.SetCanvas(Canvas);
            InstantiateGame();
        }

        private void InstantiateGame()
        {
            DisposeSubscriptions();
            var go = Instantiate(GameScreenPrefab);
            var rectTransform = go.GetComponent<RectTransform>();
            rectTransform.SetParent(GameContainer);
            rectTransform.ScaleOne();
            rectTransform.Stretch();
            gameView = go.GetComponent<GameView>();
            GameProvider.SetGameView(gameView);
            ScrollingBackground.StartMovement();
            gameView.Init();
            disposables = new CompositeDisposable(SubscribeToGameOver(), SubscribeToLevelWon(), SubscribeToTryAgain());
        }

        private IDisposable SubscribeToTryAgain()
        {
            return GameProvider.ObserveTryAgain()
                .Subscribe(_ =>
                {
                    GameProvider.ProvideClearDBs().Execute();
                    ScrollingBackground.Reset();
                    ScrollingBackground.StartMovement();
                    InstantiateGame();
                });
        }

        private IDisposable SubscribeToGameOver()
        {
            return GameProvider.ObserveGameOver()
                .Delay(TimeSpan.FromSeconds(2))
                .Subscribe(_ =>
                {
                    ClearGame();
                    ScrollingBackground.StopMovement();
                    InstantiatePopup(GameOverScreenPrefab);
                });
        }

        private IDisposable SubscribeToLevelWon()
        {
            return GameProvider.ObserveLevelWon()
                .Subscribe(_ => InstantiatePopup(LevelWonPrefab));
        }

        private void InstantiatePopup(GameObject prefab)
        {
            var go = Instantiate(prefab);
            var rectTransform = go.GetComponent<RectTransform>();
            rectTransform.SetParent(PopupsContainer);
            rectTransform.ScaleOne();
            rectTransform.Stretch();
        }

        private void ClearGame()
        {
            for (var i = 0; i < GameContainer.childCount; i++)
            {
                var child = GameContainer.GetChild(i).GetComponent<RectTransform>();
                Destroy(child.gameObject);
            }
        }

        private void OnDestroy()
        {
            DisposeSubscriptions();
        }

        private void DisposeSubscriptions()
        {
            if (disposables != null) disposables.Dispose();
        }
    }
}