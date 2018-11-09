using System;
using Core.Domain.Events;
using Core.Domain.Proccesors;
using UniRx;
using UnityEngine;

namespace Core.Infrastructure.Actions
{
    public class StartLevel
    {
        private readonly Func<CompositeDisposable> gamePlayDisposablesProvider;
        private readonly IObservable<GamePlayEvent> observeGameOver;
        private readonly Startable[] startables;
        private CompositeDisposable focusDisposable;

        public StartLevel(Func<CompositeDisposable> gamePlayDisposablesProvider, 
            IObservable<GamePlayEvent> observeGameOver,
            params Startable[] startables)
        {
            this.startables = startables;
            this.gamePlayDisposablesProvider = gamePlayDisposablesProvider;
            this.observeGameOver = observeGameOver;
        }

        public void Execute()
        {
            var gamePlayDisposables = gamePlayDisposablesProvider();
            
            foreach (var startable in startables)
                gamePlayDisposables.Add(startable.Start());
            
            observeGameOver.Subscribe(_ => gamePlayDisposables.Dispose());
            ListenApplicationFocus(gamePlayDisposables);
        }

        private void ListenApplicationFocus(CompositeDisposable gamePlayDisposables)
        {
            if (focusDisposable != null) focusDisposable.Dispose();
            
            gamePlayDisposables.Add(focusDisposable = new CompositeDisposable
            {
                Observable.EveryApplicationFocus().Subscribe(value => Time.timeScale = value ? 1 : 0)
            });

        }
    }
}