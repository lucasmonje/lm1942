using Core.Domain.Models;

namespace Core.Domain.Repositories
{
    public interface PlayerRepository : Repository
    {
        void SetLevel(int value);
        int GetLevel();
        void SetLives(int value);
        int GetLives();
        void SetHealth(int value);
        int GetHealth();        
        int GetScore();
        void AddScore(int scorePoints);
        void SetScore(int value);
        int GetMaxScore();
        void SetMaxScore(int value);
        Stats GetStats();
    }
}