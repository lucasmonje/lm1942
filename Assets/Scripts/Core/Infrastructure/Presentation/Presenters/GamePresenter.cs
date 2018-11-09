using Core.Configurations;
using Core.Domain.Presentation;
using Core.Infrastructure.Actions;

namespace Core.Infrastructure.Presentation.Presenters
{
    public class GamePresenter
    {
        private readonly GameView view;
        private readonly Configuration configuration;
        private readonly StartLevel startLevel;

        public GamePresenter(GameView view, Configuration configuration, StartLevel startLevel)
        {
            this.view = view;
            this.configuration = configuration;
            this.startLevel = startLevel;
        }

        public void Init()
        {
            view.InstantiatePlayer(configuration.Player);
            startLevel.Execute();
        }
    }
}