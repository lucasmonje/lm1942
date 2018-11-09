using Core.Domain.Presentation;
using Core.Infrastructure.Presentation.Presenters;
using Core.Infrastructure.Presentation.Providers;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Infrastructure.Presentation.Views
{
    public class HudController : MonoBehaviour, HudView
    {
        public Text Lives;
        public Text Health;
        public Text Level;
        public Text Score;
        public Text MaxScore;
        
        private HudPresenter presenter;

        private void Awake()
        {
            presenter = HudPresenterProvider.Provide(this);
            presenter.Init();
        }

        public void SetLevel(int level)
        {
            Level.text = string.Format("Level {0}", level);
        }

        public void SetHealth(int health)
        {
            Health.text = string.Format("Health {0}", health);
        }

        public void SetLives(int lives)
        {
            Lives.text = string.Format("Lives {0}", lives);
        }

        public void SetScore(int score)
        {
            Score.text = string.Format("Score {0}", score);
        }

        public void SetMaxScore(int maxScore)
        {
            MaxScore.text = string.Format("MaxScore {0}", maxScore);
        }

        private void OnDestroy()
        {
            presenter.OnDestroy();
        }
    }
}