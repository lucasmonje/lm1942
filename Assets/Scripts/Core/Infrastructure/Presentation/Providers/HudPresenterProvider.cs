using Core.Domain.Presentation;
using Core.Infrastructure.Factories;
using Core.Infrastructure.Presentation.Presenters;

namespace Core.Infrastructure.Presentation.Providers
{
    public static class HudPresenterProvider
    {
        public static HudPresenter Provide(HudView hudView)
        {
            return new HudPresenter(hudView, GameProvider.ProvidePlayerStatsChanges());
        }
    }
}