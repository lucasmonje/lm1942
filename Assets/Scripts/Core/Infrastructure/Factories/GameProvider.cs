using Core.Configurations;
using Core.Domain.Events;
using Core.Domain.Models;
using Core.Domain.Presentation;
using Core.Domain.Proccesors;
using Core.Domain.Repositories;
using Core.Infrastructure.Actions;
using Core.Infrastructure.Presentation.Views;
using Core.Infrastructure.Proccesors;
using Core.Infrastructure.Repositories;
using UniRx;

namespace Core.Infrastructure.Factories
{
    public static class GameProvider
    {
        private const string GamePlayDisposablesKey = "GamePlayCompositeDisposable";

        public static Configuration ProvideConfiguration()
        {
            return ConfigurationLoader.GetInstance().Configuration;
        }

        public static IObserver<NewFireEvent> ProvideNewFireObserver()
        {
            return ProvideNewFireSubject();
        }

        public static IObservable<NewFireEvent> ProvideNewFireObservable()
        {
            return ProvideNewFireSubject();
        }

        private static Subject<NewFireEvent> ProvideNewFireSubject()
        {
            return Provider.GetOrInstanciate<Subject<NewFireEvent>>(() => new Subject<NewFireEvent>());
        }

        private static CollisionRepository ProvideCollisionRepository()
        {
            return Provider.GetOrInstanciate<CollisionRepository>(() => new InMemoryCollisionRepository());
        }

        public static void Flush()
        {
            Provider.Flush();
        }

        public static CollisionInstantiator ProvideCollisionInstantiator()
        {
            return Provider.GetOrInstanciate<CollisionInstantiator>(() =>
                new CollisionInstantiator(ProvideCollisionRepository(), ProvideCollisionEventObserver()));
        }

        public static RemoveCollision ProvideRemoveCollision()
        {
            return Provider.GetOrInstanciate<RemoveCollision>(() =>
                new RemoveCollision(ProvideCollisionRepository(), ProvideCollisionEventObserver()));
        }

        public static Subject<CollisionEvent> ProvideCollisionEventObserver()
        {
            return ProvideCollisionEventSubject();
        }

        public static Subject<CollisionEvent> ProvideCollisionEventObservable()
        {
            return ProvideCollisionEventSubject();
        }

        private static Subject<CollisionEvent> ProvideCollisionEventSubject()
        {
            return Provider.GetOrInstanciate<Subject<CollisionEvent>>(() => new Subject<CollisionEvent>());
        }

        public static PlayerShoots ProvidePlayerShoots()
        {
            return Provider.GetOrInstanciate<PlayerShoots>(() => new PlayerShoots(
                ProvideCollisionInstantiator(), ProvideConfiguration()));
        }

        public static EnemyShoots ProvideEnemyShoots()
        {
            return Provider.GetOrInstanciate<EnemyShoots>(() => new EnemyShoots(
                ProvideCollisionInstantiator()));
        }

        public static SpawnPowerUp ProvideSpawnPowerUp()
        {
            return Provider.GetOrInstanciate<SpawnPowerUp>(() => new SpawnPowerUp(
                ProvideCollisionInstantiator()));
        }

        public static StartLevel ProvideStartLevel()
        {
            return Provider.GetOrInstanciate<StartLevel>(
                () => new StartLevel(FlushGamePlayCompositeDisposable, ObserveGameOver(),
                    ProvideInstantiateEnemiesWhenLevelStart(), ProvideObserveCollisionsWhenLevelStart(),
                    ProvideSetInitialPlayerStats(), ProvideSpawnPlayerWhenHeLoseALife(),
                    ProvideSpawnPowerUpInTheHalfGame(), ProvideObserveLevelWon(),
                    ProvideSaveMaxScoreOnGameOver()));
        }

        private static CompositeDisposable ProvidGamePlayCompositeDisposable()
        {
            return Provider.GetOrInstanciate<CompositeDisposable>(() => new CompositeDisposable(),
                GamePlayDisposablesKey);
        }

        private static CompositeDisposable FlushGamePlayCompositeDisposable()
        {
            ProvidGamePlayCompositeDisposable().Dispose();
            var compositeDisposable = new CompositeDisposable();
            Provider.Set<CompositeDisposable>(compositeDisposable, GamePlayDisposablesKey);
            return compositeDisposable;
        }

        public static InitNextLevel ProvideInitNextLevel()
        {
            return Provider.GetOrInstanciate<InitNextLevel>(() =>
                new InitNextLevel(ProvideStartLevel(), ProvidePlayerRepository(), ProvideConfiguration(),
                    ProvideGamePlayEventSubject()));
        }

        public static PlayerRepository ProvidePlayerRepository()
        {
            return Provider.GetOrInstanciate<PlayerRepository>(() =>
                new InMemoryPlayerRepository(ProvidePlayerStatsChanges()));
        }

        public static Subject<Stats> ProvidePlayerStatsChanges()
        {
            return Provider.GetOrInstanciate<Subject<Stats>>(() => new Subject<Stats>(), "PlayerStatsChanges");
        }

        private static InstantiateEnemiesWhenLevelStart ProvideInstantiateEnemiesWhenLevelStart()
        {
            return Provider.GetOrInstanciate<InstantiateEnemiesWhenLevelStart>(() =>
                new InstantiateEnemiesWhenLevelStart(ProvidePlayerRepository(), ProvideConfiguration(),
                    ProvideGamePlayEventSubject()));
        }

        private static ObserveCollisionsWhenLevelStart ProvideObserveCollisionsWhenLevelStart()
        {
            return Provider.GetOrInstanciate<ObserveCollisionsWhenLevelStart>(() =>
                new ObserveCollisionsWhenLevelStart(ProvideCollisionRepository(), ProvideCollisionEventObservable(),
                    ProvideBulletCollidesWithEnemy(), ProvideBulletCollidesWithPlayer(),
                    ProvidePlayerCollidesWithPowerUp(), ProvidePlayerCollidesWithEnemy()));
        }

        private static SetInitialPlayerStats ProvideSetInitialPlayerStats()
        {
            return Provider.GetOrInstanciate<SetInitialPlayerStats>(() =>
                new SetInitialPlayerStats(ProvidePlayerRepository(), ProvideConfiguration()));
        }

        private static SaveMaxScoreOnGameOver ProvideSaveMaxScoreOnGameOver()
        {
            return Provider.GetOrInstanciate<SaveMaxScoreOnGameOver>(() =>
                new SaveMaxScoreOnGameOver(ProvidePlayerRepository(), ProvideGamePlayEventObservable()));
        }

        private static ObserveLevelWon ProvideObserveLevelWon()
        {
            return Provider.GetOrInstanciate<ObserveLevelWon>(() =>
                new ObserveLevelWon(ProvideGamePlayEventSubject(), ProvideCollisionEventObservable()));
        }

        private static SpawnPowerUpInTheHalfGame ProvideSpawnPowerUpInTheHalfGame()
        {
            return Provider.GetOrInstanciate<SpawnPowerUpInTheHalfGame>(() =>
                new SpawnPowerUpInTheHalfGame(ProvideConfiguration(), ProvidePlayerRepository(),
                    ProvideSpawnPowerUp()));
        }

        private static SpawnPlayerWhenHeLoseALife ProvideSpawnPlayerWhenHeLoseALife()
        {
            return Provider.GetOrInstanciate<SpawnPlayerWhenHeLoseALife>(() =>
                new SpawnPlayerWhenHeLoseALife(ProvidePlayerRepository(), ProvideConfiguration(),
                    ProvideGamePlayEventObservable()));
        }

        private static BulletCollidesWithEnemy ProvideBulletCollidesWithEnemy()
        {
            return Provider.GetOrInstanciate<BulletCollidesWithEnemy>(() =>
                new BulletCollidesWithEnemy(ProvidePlayerRepository()));
        }

        private static BulletCollidesWithPlayer ProvideBulletCollidesWithPlayer()
        {
            return Provider.GetOrInstanciate<BulletCollidesWithPlayer>(() =>
                new BulletCollidesWithPlayer(ProvidePlayerHealthDecrease()));
        }

        private static PlayerCollidesWithPowerUp ProvidePlayerCollidesWithPowerUp()
        {
            return Provider.GetOrInstanciate<PlayerCollidesWithPowerUp>(() => new PlayerCollidesWithPowerUp());
        }

        private static PlayerCollidesWithEnemy ProvidePlayerCollidesWithEnemy()
        {
            return Provider.GetOrInstanciate<PlayerCollidesWithEnemy>(() =>
                new PlayerCollidesWithEnemy(ProvidePlayerHealthDecrease(), ProvideConfiguration()));
        }

        public static void SetGameView(GameView gameView)
        {
            Provider.Set<GameView>(gameView);
        }

        public static GameView GetGameView()
        {
            return Provider.Get<GameView>();
        }

        public static IObservable<GamePlayEvent> ProvideGamePlayEventObservable()
        {
            return ProvideGamePlayEventSubject();
        }

        public static Subject<GamePlayEvent> ProvideGamePlayEventSubject()
        {
            return Provider.GetOrInstanciate<Subject<GamePlayEvent>>(() => new Subject<GamePlayEvent>());
        }

        public static IObservable<GamePlayEvent> ObserveGameOver()
        {
            return ProvideGamePlayEventObservable().Where(@event => @event.IsGameOverEvent());
        }

        public static IObservable<GamePlayEvent> ObserveLevelWon()
        {
            return ProvideGamePlayEventObservable().Where(@event => @event.IsLevelWonEvent());
        }

        public static IObservable<GamePlayEvent> ObserveTryAgain()
        {
            return ProvideGamePlayEventObservable().Where(@event => @event.IsTryAgainEvent());
        }

        public static ClearDBs ProvideClearDBs()
        {
            return Provider.GetOrInstanciate<ClearDBs>(() =>
                new ClearDBs(ProvideCollisionRepository(), ProvidePlayerRepository()));
        }

        private static PlayerHealthDecrease ProvidePlayerHealthDecrease()
        {
            return Provider.GetOrInstanciate<PlayerHealthDecrease>(() => new PlayerHealthDecrease(
                ProvideCollisionRepository(),
                ProvidePlayerRepository(), ProvideGamePlayEventSubject(), ProvideConfiguration()));
        }

        public static GetPlayerStats ProvideGetPlayerStats()
        {
            return Provider.GetOrInstanciate<GetPlayerStats>(() => new GetPlayerStats(ProvidePlayerRepository()));
        }

        public static MapEnemyViewModel ProvideMapEnemyViewModel()
        {
            return Provider.GetOrInstanciate<MapEnemyViewModel>(() => new MapEnemyViewModel(ProvideConfiguration()));
        }
    }
}