using System;
using System.Linq;
using Core.Domain.Events;
using Core.Domain.Models;
using Core.Domain.Presentation;
using Core.Domain.Proccesors;
using Core.Domain.Repositories;
using Core.Infrastructure.Actions;
using Core.Utils.Extensions;
using Functional.Maybe;
using UniRx;

namespace Core.Infrastructure.Proccesors
{
    public class ObserveCollisionsWhenLevelStart : Startable
    {
        private readonly CollisionRepository collisionRepository;
        private readonly IObserver<CollisionEvent> collisionEventObserver;
        private readonly BulletCollidesWithEnemy bulletCollidesWithEnemy;
        private readonly BulletCollidesWithPlayer bulletCollidesWithPlayer;
        private readonly PlayerCollidesWithPowerUp playerCollidesWithPowerUp;
        private readonly PlayerCollidesWithEnemy playerCollidesWithEnemy;

        public ObserveCollisionsWhenLevelStart(CollisionRepository collisionRepository,
            IObserver<CollisionEvent> collisionEventObserver, BulletCollidesWithEnemy bulletCollidesWithEnemy,
            BulletCollidesWithPlayer bulletCollidesWithPlayer, PlayerCollidesWithPowerUp playerCollidesWithPowerUp,
            PlayerCollidesWithEnemy playerCollidesWithEnemy)
        {
            this.collisionRepository = collisionRepository;
            this.collisionEventObserver = collisionEventObserver;
            this.bulletCollidesWithEnemy = bulletCollidesWithEnemy;
            this.bulletCollidesWithPlayer = bulletCollidesWithPlayer;
            this.playerCollidesWithPowerUp = playerCollidesWithPowerUp;
            this.playerCollidesWithEnemy = playerCollidesWithEnemy;
        }

        public IDisposable Start()
        {
            return Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    var playerCollides = false;
                    var gameCollisions = collisionRepository.GetAll();
                    var enemies = gameCollisions.GetEnemies();

                    gameCollisions.GetPlayer()
                        .Do(player =>
                        {
                            var playerRect = player.CollisionRectTransform;
                            var enemyBullets = gameCollisions.GetEnemyBullets();
                            foreach (var enemyBullet in enemyBullets)
                            {
                                if (enemyBullet.CollisionRectTransform.Overlaps(playerRect))
                                {
                                    collisionRepository.Delete(enemyBullet.Id);
                                    collisionRepository.Delete(player.Id);
                                    bulletCollidesWithPlayer.Execute(enemyBullet as GunView, player as PlayerView);
                                    playerCollides = true;
                                    break;
                                }
                            }

                            if (!playerCollides)
                            {
                                var powerUps = gameCollisions.GetPowerUps();
                                foreach (var powerUp in powerUps)
                                {
                                    if (powerUp.CollisionRectTransform.Overlaps(playerRect))
                                    {
                                        collisionRepository.Delete(powerUp.Id);
                                        playerCollidesWithPowerUp.Execute(powerUp as PowerUpView, player as PlayerView);
                                        playerCollides = true;
                                        break;
                                    }
                                }
                            }

                            if (!playerCollides)
                            {
                                var enemy = enemies.FirstOrDefault(e =>
                                    e.CollisionRectTransform != null &&
                                    (player.CollisionRectTransform != null &&
                                     player.CollisionRectTransform.Overlaps(e.CollisionRectTransform)));
                                if (enemy != null)
                                {
                                    collisionRepository.Delete(player.Id);
                                    collisionRepository.Delete(enemy.Id);
                                    playerCollidesWithEnemy.Execute(player as PlayerView, enemy as EnemyView);
                                }
                            }
                            
                        });

                    var playerBullets = gameCollisions.GetPlayerBullets();

                    for (var index = 0; index < playerBullets.Length; index++)
                    {
                        var playerBullet = playerBullets[index];

                        var enemy = enemies.FirstOrDefault(e =>
                            e.CollisionRectTransform != null &&
                            (playerBullet.CollisionRectTransform != null &&
                             playerBullet.CollisionRectTransform.Overlaps(e.CollisionRectTransform)));
                        if (enemy != null)
                        {
                            var worldRect = playerBullet.CollisionRectTransform.WorldRect();
                            collisionRepository.Delete(playerBullet.Id);
                            collisionRepository.Delete(enemy.Id);
                            bulletCollidesWithEnemy.Execute(playerBullet as GunView, enemy as EnemyView);
                        }
                    }
                });
        }
    }
}