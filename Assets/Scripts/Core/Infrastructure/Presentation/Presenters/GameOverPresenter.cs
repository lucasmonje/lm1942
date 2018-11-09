using Core.Domain.Events;
using Core.Domain.Presentation;
using Core.Infrastructure.Actions;
using UniRx;

namespace Core.Infrastructure.Presentation.Presenters
{
    public class GameOverPresenter
    {
        private readonly GameOverView view;
        private readonly GetPlayerStats getPlayerStats;
        private readonly IObserver<GamePlayEvent> gamePlayEventObserver;

        public GameOverPresenter(GameOverView view, GetPlayerStats getPlayerStats,
            IObserver<GamePlayEvent> gamePlayEventObserver)
        {
            this.view = view;
            this.getPlayerStats = getPlayerStats;
            this.gamePlayEventObserver = gamePlayEventObserver;
        }

        public void Init()
        {
            var stats = getPlayerStats.Execute();
            view.SetScore(stats.Score);
            view.SetMaxScore(stats.MaxScore);
        }

        public void OnTryAgainButtonPressed()
        {
            gamePlayEventObserver.OnNext(GamePlayEvent.TryAgain());
            view.DestroyView();
        }

    }
}