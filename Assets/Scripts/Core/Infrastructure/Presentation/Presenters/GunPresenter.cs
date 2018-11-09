using Core.Domain.Entities;
using Core.Domain.Presentation;
using Core.Domain.Utils;
using Core.Infrastructure.Actions;
using Core.Infrastructure.Factories;
using Core.Utils.Unity;
using Functional.Maybe;
using UnityEngine;

namespace Core.Infrastructure.Presentation.Presenters
{
    public class GunPresenter
    {
        private const int SpeedMultiplier = 300;
        private readonly System.Random random = new System.Random();
        private readonly GunView view;
        private readonly RemoveCollision removeCollision;
        private GunDefinition gunDefinition;
        private Maybe<Vector2> toPosition;
        private bool enhancedGun;

        public GunPresenter(GunView view, RemoveCollision removeCollision)
        {
            this.view = view;
            this.removeCollision = removeCollision;
        }

        public void Init(GunDefinition gunDefinition)
        {
            this.gunDefinition = gunDefinition;
        }

        public void OnUpdate(float deltaTime)
        {
            toPosition.Do(target =>
            {
                var speed = gunDefinition.Speed * SpeedMultiplier * deltaTime;
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
                var target = Vector2.zero;
                if (Position.PlayerLocation.Equals(gunDefinition.ToPosition))
                    GameProvider.GetGameView().ToMaybe()
                        .Do(game =>
                        {
                            var playerPoint = game.GetPlayerPoints().Choose();
                            var tileSize = CanvasHelper.GetTileSize();
                            var multiplier = random.Next(2) == 1 ? 1 : -1;
                            if (CanvasHelper.IsLeft(playerPoint.Position) ||
                                CanvasHelper.IsRight(playerPoint.Position))
                            {
                                target = playerPoint.ScreenPosition +
                                         new Vector2(0f, random.Next((int) (tileSize.y)) * multiplier);
                            }
                            else
                            {
                                target = playerPoint.ScreenPosition +
                                         new Vector2(random.Next((int) (tileSize.x * 2)) * multiplier, 0f);
                            }
                        });
                else
                    target = CanvasHelper.GetCanvasPosition(gunDefinition.ToPosition, view.MainRectTransform);

                toPosition = target.ToMaybe();
                view.RotateTowards(target);
            });
        }

        public int GetDamage()
        {
            return gunDefinition.Damage;
        }
    }
}