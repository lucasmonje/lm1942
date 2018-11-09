namespace Core.Domain.Models
{
    public class Stats
    {
        public int Level { get; private set; }
        public int Health { get; private set; }
        public int Lives { get; private set; }
        public int Score { get; private set; }
        public int MaxScore { get; private set; }

        public Stats(int level, int health, int lives, int score, int maxScore)
        {
            Level = level;
            Health = health;
            Lives = lives;
            Score = score;
            MaxScore = maxScore;
        }
    }
}