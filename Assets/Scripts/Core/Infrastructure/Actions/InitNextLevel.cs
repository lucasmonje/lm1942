using System.Linq;
using Core.Configurations;
using Core.Domain.Events;
using Core.Domain.Repositories;
using Functional.Maybe;
using UniRx;

namespace Core.Infrastructure.Actions
{
    public class InitNextLevel
    {
        private readonly StartLevel startLevel;
        private readonly PlayerRepository playerRepository;
        private readonly Configuration configuration;
        private readonly IObserver<GamePlayEvent> gamePlayEventObserver;

        public InitNextLevel(StartLevel startLevel, PlayerRepository playerRepository, Configuration configuration,
            IObserver<GamePlayEvent> gamePlayEventObserver)
        {
            this.startLevel = startLevel;
            this.playerRepository = playerRepository;
            this.configuration = configuration;
            this.gamePlayEventObserver = gamePlayEventObserver;
        }

        public void Execute()
        {
            var nextLevel = playerRepository.GetLevel() + 1;
            configuration.AllLevels.ElementAtOrDefault(nextLevel).ToMaybe()
                .Match(_ =>
                {
                    playerRepository.SetLevel(nextLevel);
                    startLevel.Execute();
                }, () => gamePlayEventObserver.OnNext(GamePlayEvent.GameOver()));
        }
    }
}