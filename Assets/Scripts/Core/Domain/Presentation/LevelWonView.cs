namespace Core.Domain.Presentation
{
    public interface LevelWonView
    {
        void SetLevel(int level);
        void SetScore(int score);
        void SetMaxScore(int maxScore);
        void DestroyView();
    }
}