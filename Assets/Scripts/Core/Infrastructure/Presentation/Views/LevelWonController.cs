using Core.Domain.Presentation;
using Core.Infrastructure.Presentation.Presenters;
using Core.Infrastructure.Presentation.Providers;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Infrastructure.Presentation.Views
{
    public class LevelWonController : MonoBehaviour, LevelWonView
    {
        public Text Level;
        public Text Score;
        public Text MaxScore;
        private LevelWonPresenter presenter;
        
        private void Awake()
        {
            presenter = LevelWonPresenterProvider.Provide(this);
            presenter.Init();
        }

        public void SetLevel(int level)
        {
            Level.text = string.Format("Level {0}", level);
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
            presenter.OnDestroy();
        }
    }
}