namespace Core.Domain.Models
{
    public enum CollisionType
    {
        Enemy, Player, EnemyBullet, PlayerBullet, PowerUp
    }
    
    public static class CollisionTypeExtensions
    {
        public static bool IsEnemyBullet(this CollisionType a)
        {
            return CollisionType.EnemyBullet.Equals(a);
        }
        
        public static bool IsPlayerBullet(this CollisionType a)
        {
            return CollisionType.PlayerBullet.Equals(a);
        }
        
        public static bool IsPowerUp(this CollisionType a)
        {
            return CollisionType.PowerUp.Equals(a);
        }
                
        public static bool IsPlayer(this CollisionType a)
        {
            return CollisionType.Player.Equals(a);
        }
                        
        public static bool IsEnemy(this CollisionType a)
        {
            return CollisionType.Enemy.Equals(a);
        }
        
        public static bool IsTarget(this CollisionType a, CollisionType b)
        {
            var aPlayerBullet = CollisionType.PlayerBullet.Equals(a);
            var bPlayer = CollisionType.Player.Equals(b);
            if (aPlayerBullet) return bPlayer;

            var aEnemyBullet = CollisionType.EnemyBullet.Equals(a);
            var bEnemy = CollisionType.Enemy.Equals(b);
            if (aEnemyBullet) return bEnemy;

            var aPowerUp = CollisionType.PowerUp.Equals(a);
            if (aPowerUp) return bPlayer;
            
            var aPlayer = CollisionType.Player.Equals(a);
            var bPlayerBullet = CollisionType.PlayerBullet.Equals(b);
            if (aPlayer) return !bPlayerBullet;

            var aEnemy = CollisionType.Enemy.Equals(a);
            if (aEnemy) return bPlayerBullet || bPlayer;   
            return false;
        }
    }
}