using Core.Domain.Presentation;
using Core.Infrastructure.Factories;
using Core.Infrastructure.Presentation.Presenters;

namespace Core.Infrastructure.Presentation.Providers
{
    public static class EnemyPresenterProvider
    {
        public static EnemyPresenter Provide(EnemyView enemyView)
        {
            return new EnemyPresenter(enemyView, GameProvider.ProvideRemoveCollision(), GameProvider.ProvideEnemyShoots());
        }
    }
}