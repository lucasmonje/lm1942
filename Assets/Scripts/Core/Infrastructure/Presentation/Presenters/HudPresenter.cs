using Core.Domain.Events;
using Core.Domain.Models;
using Core.Domain.Presentation;
using UniRx;

namespace Core.Infrastructure.Presentation.Presenters
{
    public class HudPresenter
    {
        private readonly HudView hudView;
        private readonly IObservable<Stats> playerStatsChanges;
        private readonly IObservable<GamePlayEvent> gamePlayEventObservable;
        private CompositeDisposable disposables;

        public HudPresenter(HudView hudView, IObservable<Stats> playerStatsChanges)
        {
            this.hudView = hudView;
            this.playerStatsChanges = playerStatsChanges;
        }

        public void Init()
        {
            AddSubscriptions();
        }

        private void AddSubscriptions()
        {
            disposables = new CompositeDisposable();
            disposables.Add(playerStatsChanges.Subscribe(UpdateHud));
        }

        private void UpdateHud(Stats stats)
        {
            hudView.SetLevel(stats.Level + 1);
            hudView.SetHealth(stats.Health);
            hudView.SetLives(stats.Lives);
            hudView.SetScore(stats.Score);
            hudView.SetMaxScore(stats.MaxScore);
        }

        public void OnDestroy()
        {
            if (disposables != null && !disposables.IsDisposed)
                disposables.Dispose();
        }
    }
}