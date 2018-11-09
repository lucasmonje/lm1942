using Core.Domain.Presentation;
using Core.Infrastructure.Factories;
using Core.Infrastructure.Presentation.Presenters;

namespace Core.Infrastructure.Presentation.Providers
{
    public static class PlayerPresenterProvider
    {
        public static PlayerPresenter Provide(PlayerView playerView)
        {
            return new PlayerPresenter(playerView, GameProvider.ProvidePlayerShoots());
        }
    }
}