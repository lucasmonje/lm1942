using Core.Domain.Presentation;
using Core.Infrastructure.Factories;
using Core.Infrastructure.Presentation.Presenters;

namespace Core.Infrastructure.Presentation.Providers
{
    public static class PowerUpPresenterProvider
    {
        public static PowerUpPresenter Provide(PowerUpView view)
        {
            return new PowerUpPresenter(view, GameProvider.ProvideRemoveCollision());
        }
    }
}