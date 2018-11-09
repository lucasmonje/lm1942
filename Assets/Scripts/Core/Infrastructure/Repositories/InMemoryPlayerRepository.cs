using Core.Domain.Models;
using Core.Domain.Repositories;
using UniRx;
using UnityEngine;

namespace Core.Infrastructure.Repositories
{
    public class InMemoryPlayerRepository : PlayerRepository
    {
        private readonly IObserver<Stats> playerStatObserver;
        private const string MaxScoreKey = "UserRepositoryMaxScore";
        private int level;
        private int lives;
        private int health;
        private int score;
        private int maxScore;

        public InMemoryPlayerRepository(IObserver<Stats> playerStatObserver)
        {
            this.playerStatObserver = playerStatObserver;
            Observable.Start(() =>
                {
                    maxScore = PlayerPrefs.GetInt(MaxScoreKey);
                    playerStatObserver.OnNext(GetStats());
                    return Unit.Default;
                }, Scheduler.MainThread)
                .Subscribe();
        }

        public Stats GetStats()
        {
            return new Stats(level, health, lives, score, maxScore);
        }
        
        public void SetLevel(int value)
        {
            level = value;
            playerStatObserver.OnNext(GetStats());
        }

        public int GetLevel()
        {
            return level;
        }

        public void SetLives(int value)
        {
            lives = value;
            playerStatObserver.OnNext(GetStats());
        }

        public int GetLives()
        {
            return lives;
        }

        public void SetHealth(int value)
        {
            health = value;
            playerStatObserver.OnNext(GetStats());
        }

        public int GetHealth()
        {
            return health;
        }

        public int GetScore()
        {
            return score;
        }

        public void AddScore(int scorePoints)
        {
            score += scorePoints;
            playerStatObserver.OnNext(GetStats());
        }

        public void SetScore(int value)
        {
            score = value;
            playerStatObserver.OnNext(GetStats());
        }

        public int GetMaxScore()
        {
            return maxScore;
        }

        public void SetMaxScore(int value)
        {
            maxScore = value;
            Put(MaxScoreKey, maxScore);
            playerStatObserver.OnNext(GetStats());
        }

        public void Clear()
        {
            level = 0;
            lives = 0;
            health = 0;
            score = 0;
            playerStatObserver.OnNext(GetStats());
        }

        private static void Put(string key, int value)
        {
            Observable.Start(() =>
                {
                    PlayerPrefs.SetInt(key, value);
                    PlayerPrefs.Save();
                }, Scheduler.MainThread)
                .Subscribe();
        }
    }
}