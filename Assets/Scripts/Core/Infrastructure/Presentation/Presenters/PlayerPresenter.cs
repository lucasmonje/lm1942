using System;
using Core.Domain.Entities;
using Core.Domain.Events;
using Core.Domain.Presentation;
using Core.Infrastructure.Actions;
using UniRx;

namespace Core.Infrastructure.Presentation.Presenters
{
    public class PlayerPresenter
    {
        private readonly PlayerView view;
        private readonly PlayerShoots playerShoots;
        private readonly IObserver<NewFireEvent> newFireObserver;
        private readonly Subject<Unit> newFireSubject = new Subject<Unit>();
        private PlayerDefinition playerDefinition;
        private CompositeDisposable disposables;
        private bool enhancedGun;
        
        public PlayerPresenter(PlayerView view, PlayerShoots playerShoots)
        {
            this.view = view;
            this.playerShoots = playerShoots;
        }

        public void Init(PlayerDefinition def)
        {
            playerDefinition = def;
            view.CreateLimits(def.LimitLeft, def.LimitRight, def.LimitTop, def.LimitBottom);

            disposables = new CompositeDisposable(newFireSubject
                .Throttle(TimeSpan.FromMilliseconds(def.FireRate))
                .Select(_ => NewFireEvent.FromPlayer(view.GetScreenPosition()))
                .Subscribe(newFireEvent => playerShoots.Execute(view.GetScreenPosition(), enhancedGun)));
        }

        public void OnFirePressed()
        {
            newFireSubject.OnNext(Unit.Default);
        }

        public void OnDestroy()
        {
            DisposeSubscriptions();
        }

        private void DisposeSubscriptions()
        {
            if (disposables != null && !disposables.IsDisposed)
                disposables.Dispose();
        }

        public void EnhanceGun()
        {
            enhancedGun = true;
        }

        public void Hit()
        {
            enhancedGun = false;
        }
    }
}