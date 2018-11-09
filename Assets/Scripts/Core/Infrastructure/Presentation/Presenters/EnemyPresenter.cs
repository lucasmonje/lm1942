using System.Linq;
using Core.Configurations;
using Core.Domain.Entities;
using Core.Domain.Events;
using Core.Domain.Models;
using Core.Domain.Presentation;
using Core.Infrastructure.Actions;
using Core.Infrastructure.Dtos;
using Core.Infrastructure.Presentation.Models;
using Core.Utils.Unity;
using Functional.Maybe;
using UniRx;
using UnityEngine;

namespace Core.Infrastructure.Presentation.Presenters
{
    public class EnemyPresenter
    {
        private const int SpeedMultiplier = 300;

        private readonly EnemyView view;
        private EnemyViewModel enemyViewModel;
        private Vector3 target;

        private WayPointDto currentWayPoint;
        private Maybe<int> currentWayPointIndex;
        private readonly RemoveCollision removeCollision;
        private readonly EnemyShoots enemyShoots;

        public EnemyPresenter(EnemyView view, RemoveCollision removeCollision, EnemyShoots enemyShoots)
        {
            this.view = view;
            this.removeCollision = removeCollision;
            this.enemyShoots = enemyShoots;
        }

        public void Init(EnemyViewModel enemyViewModel)
        {
            this.enemyViewModel = enemyViewModel;
        }

        private void ExecuteAction(AirplaneAction action)
        {
            switch (action)
            {
                case AirplaneAction.None:
                    break;
                case AirplaneAction.Fire:
                    enemyShoots.Execute(view.GetScreenPosition(), enemyViewModel.GunDefinition);
                    break;
            }
        }

        private void NextWayPoint()
        {
            var next = currentWayPointIndex.SelectOrElse(index => index + 1, () => 0);

            if (enemyViewModel.PathDefinition.WayPoints.Length > next)
            {
                currentWayPoint = enemyViewModel.PathDefinition.WayPoints.ElementAt(next);
                currentWayPointIndex = next.ToMaybe();
                target = CanvasHelper.GetCanvasPosition(currentWayPoint.Position);
                view.RotateTowards(target);
            }
            else
            {
                removeCollision.Execute(view);
                view.DestroyView();
            }
        }

        public void OnFixedUpdate()
        {
            currentWayPointIndex.DoWhenAbsent(() =>
            {
                view.SetAnchoredPosition(CanvasHelper.GetCanvasPosition(enemyViewModel.PathDefinition.SpawnPosition));
                NextWayPoint();
            });
        }

        public void OnUpdate(float deltaTime)
        {
            if (currentWayPoint != null)
            {
                var speed = enemyViewModel.Speed * SpeedMultiplier * deltaTime;
                var currentPosition = view.GetScreenPosition();
                var transformPosition = Vector2.MoveTowards(currentPosition, target, speed);
                view.SetAnchoredPosition(transformPosition);

                var distance = Vector2.Distance(currentPosition, target);
                if (distance <= 0.01f)
                {
                    ExecuteAction(currentWayPoint.Action);
                    NextWayPoint();
                }
            }
        }

        public EnemyViewModel GetModel()
        {
            return enemyViewModel;
        }
    }
}