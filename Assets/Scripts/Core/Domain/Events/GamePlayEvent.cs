namespace Core.Domain.Events
{
    public class GamePlayEvent
    {
        private const string LastRoundEvent = "LastRoundEvent";
        private const string LevelWonEvent = "LevelWonEvent";
        private const string GameOverEvent = "GameOverEvent";
        private const string LostLifeEvent = "LostLifeEvent";
        private const string TryAgainEvent = "TryAgainEvent";

        private string eventName;

        public static GamePlayEvent LastRound()
        {
            return new GamePlayEvent {eventName = LastRoundEvent};
        }
        
        public static GamePlayEvent LevelWon()
        {
            return new GamePlayEvent {eventName = LevelWonEvent};
        }
        
        public static GamePlayEvent GameOver()
        {
            return new GamePlayEvent {eventName = GameOverEvent};
        }
        
        public static GamePlayEvent LostLife()
        {
            return new GamePlayEvent {eventName = LostLifeEvent};
        }
        
        public static GamePlayEvent TryAgain()
        {
            return new GamePlayEvent {eventName = TryAgainEvent};
        }
        
        public bool IsLastRoundEvent()
        {
            return LastRoundEvent.Equals(eventName);
        }

        public bool IsLevelWonEvent()
        {
            return LevelWonEvent.Equals(eventName);
        }
        
        public bool IsGameOverEvent()
        {
            return GameOverEvent.Equals(eventName);
        }

        public bool IsLostLifeEvent()
        {
            return LostLifeEvent.Equals(eventName);
        }
        
        public bool IsTryAgainEvent()
        {
            return TryAgainEvent.Equals(eventName);
        }
    }
}