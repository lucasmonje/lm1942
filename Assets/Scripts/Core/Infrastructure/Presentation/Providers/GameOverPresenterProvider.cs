using Core.Domain.Presentation;
using Core.Infrastructure.Factories;
using Core.Infrastructure.Presentation.Presenters;

namespace Core.Infrastructure.Presentation.Providers
{
    public static class GameOverPresenterProvider
    {
        public static GameOverPresenter Provide(GameOverView view)
        {
            return new GameOverPresenter(view, GameProvider.ProvideGetPlayerStats(), GameProvider.ProvideGamePlayEventSubject());
        }
    }
}