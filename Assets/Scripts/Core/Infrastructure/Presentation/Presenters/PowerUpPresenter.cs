using Core.Domain.Entities;
using Core.Domain.Presentation;
using Core.Infrastructure.Actions;
using Core.Utils.Unity;
using Functional.Maybe;
using UnityEngine;

namespace Core.Infrastructure.Presentation.Presenters
{
    public class PowerUpPresenter
    {
        private const int SpeedMultiplier = 100;

        private readonly PowerUpView view;
        private readonly RemoveCollision removeCollision;
        private PowerUpDefinition powerUpDefinition;
        private Maybe<Vector2> toPosition;

        public PowerUpPresenter(PowerUpView view, RemoveCollision removeCollision)
        {
            this.view = view;
            this.removeCollision = removeCollision;
        }

        public void Init(PowerUpDefinition powerUpDefinition)
        {
            this.powerUpDefinition = powerUpDefinition;
        }

        public void OnUpdate(float deltaTime)
        {
            toPosition.Do(target =>
            {
                var speed = SpeedMultiplier * deltaTime;
                var currentPosition = view.GetScreenPosition();
                var transformPosition = Vector2.MoveTowards(currentPosition, target, speed);
                view.SetAnchoredPosition(transformPosition);
                var distance = Vector2.Distance(currentPosition, target);
                if (distance <= 0.01f)
                {
                    removeCollision.Execute(view);
                    view.DestroyView();
                }
            });
        }

        public void OnFixedUpdate()
        {
            toPosition.DoWhenAbsent(() =>
            {
                var start = CanvasHelper.GetCanvasPosition(powerUpDefinition.FromPosition, view.MainRectTransform);
                view.SetAnchoredPosition(start);
                var target = CanvasHelper.GetCanvasPosition(powerUpDefinition.ToPosition, view.MainRectTransform);
                toPosition = target.ToMaybe();
            });
        }
    }
}