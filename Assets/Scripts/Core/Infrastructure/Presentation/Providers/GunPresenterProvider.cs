using Core.Domain.Presentation;
using Core.Infrastructure.Factories;
using Core.Infrastructure.Presentation.Presenters;

namespace Core.Infrastructure.Presentation.Providers
{
    public static class GunPresenterProvider
    {
        public static GunPresenter Provide(GunView view)
        {
            return new GunPresenter(view, GameProvider.ProvideRemoveCollision());
        }
    }
}