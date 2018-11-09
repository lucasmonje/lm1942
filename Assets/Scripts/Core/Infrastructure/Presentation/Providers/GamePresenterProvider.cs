using Core.Domain.Presentation;
using Core.Infrastructure.Factories;
using Core.Infrastructure.Presentation.Presenters;

namespace Core.Infrastructure.Presentation.Providers
{
    public static class GamePresenterProvider
    {
        public static GamePresenter Provide(GameView gameView)
        {
            return new GamePresenter(gameView, GameProvider.ProvideConfiguration(), GameProvider.ProvideStartLevel());
        }
    }
}