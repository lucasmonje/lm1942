using System;
using Core.Domain.Events;
using Core.Domain.Proccesors;
using Core.Domain.Repositories;
using UniRx;

namespace Core.Infrastructure.Proccesors
{
    public class SaveMaxScoreOnGameOver : Startable
    {
        private readonly PlayerRepository playerRepository;
        private readonly IObservable<GamePlayEvent> gamePlayEventObservable;

        public SaveMaxScoreOnGameOver(PlayerRepository playerRepository,
            IObservable<GamePlayEvent> gamePlayEventObservable)
        {
            this.playerRepository = playerRepository;
            this.gamePlayEventObservable = gamePlayEventObservable;
        }

        public IDisposable Start()
        {
            return gamePlayEventObservable
                .Where(@event => @event.IsGameOverEvent())
                .Subscribe(_ =>
                {
                    var maxScore = playerRepository.GetMaxScore();
                    var playerScore = playerRepository.GetScore();
                    if (playerScore > maxScore) 
                        playerRepository.SetMaxScore(playerScore);
                });
        }
    }
}