namespace Core.Domain.Presentation
{
    public interface HudView
    {
        void SetLevel(int level);
        void SetHealth(int health);
        void SetLives(int lives);
        void SetScore(int score);
        void SetMaxScore(int maxScore);
    }
}