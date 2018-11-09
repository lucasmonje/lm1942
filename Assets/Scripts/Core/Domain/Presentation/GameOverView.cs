namespace Core.Domain.Presentation
{
    public interface GameOverView
    {
        void SetScore(int score);
        void SetMaxScore(int maxScore);
        void DestroyView();
    }
}