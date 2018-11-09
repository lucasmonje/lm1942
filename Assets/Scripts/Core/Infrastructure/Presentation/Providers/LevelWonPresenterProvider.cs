using Core.Domain.Presentation;
using Core.Infrastructure.Factories;
using Core.Infrastructure.Presentation.Presenters;

namespace Core.Infrastructure.Presentation.Providers
{
    public static class LevelWonPresenterProvider
    {
        public static LevelWonPresenter Provide(LevelWonView view)
        {
            return new LevelWonPresenter(view, GameProvider.ProvideGetPlayerStats(), GameProvider.ProvideInitNextLevel());
        }
    }
}