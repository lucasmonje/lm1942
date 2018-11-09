using System;
using Core.Domain.Presentation;
using Core.Infrastructure.Actions;
using UniRx;

namespace Core.Infrastructure.Presentation.Presenters
{
    public class LevelWonPresenter
    {
        private readonly LevelWonView view;
        private readonly GetPlayerStats getPlayerStats;
        private readonly InitNextLevel initNextLevel;
        private CompositeDisposable disposables;
        public LevelWonPresenter(LevelWonView view, GetPlayerStats getPlayerStats, InitNextLevel initNextLevel)
        {
            this.view = view;
            this.getPlayerStats = getPlayerStats;
            this.initNextLevel = initNextLevel;
        }

        public void Init()
        {
            var stats = getPlayerStats.Execute();
            view.SetLevel(stats.Level + 1);
            view.SetScore(stats.Score);
            view.SetMaxScore(stats.MaxScore);
            
            disposables = new CompositeDisposable(Observable.Timer(TimeSpan.FromSeconds(5)).First().Subscribe(_ =>
                {
                    initNextLevel.Execute();
                    view.DestroyView();
                }));
        }

        public void OnDestroy()
        {
            if (disposables != null && !disposables.IsDisposed)
                disposables.Dispose();
        }
    }
}