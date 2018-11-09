using Core.Domain.Presentation;
using Core.Infrastructure.Presentation.Presenters;
using Core.Infrastructure.Presentation.Providers;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Infrastructure.Presentation.Views
{
    public class GameOverController : MonoBehaviour, GameOverView
    {
        public Text Score;
        public Text MaxScore;
        public Button PlayAgainButton;
        private GameOverPresenter presenter;
        private CompositeDisposable disposables;

        private void Awake()
        {
            presenter = GameOverPresenterProvider.Provide(this);
            presenter.Init();
            disposables = new CompositeDisposable(PlayAgainButton.OnClickAsObservable().First()
                .Subscribe(_ => presenter.OnTryAgainButtonPressed()));
        }

        public void SetScore(int score)
        {
            Score.text = string.Format("Score {0}", score);
        }

        public void SetMaxScore(int maxScore)
        {
            MaxScore.text = string.Format("MaxScore {0}", maxScore);
        }

        public void DestroyView()
        {
            if (gameObject != null)
                Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (disposables != null && !disposables.IsDisposed)
                disposables.Dispose();
        }
    }
}