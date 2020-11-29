namespace SignalRWebPackTests.Patterns.Strategy
{
    using SignalRWebPack.Patterns.Strategy;
    using System;
    using Xunit;
    using System.Collections.Generic;
    using SignalRWebPack.Models;
    using SignalRWebPack.Logic;
    using SignalRWebPack.Patterns.Command;

    public class BombCollisionTests
    {
        private BombCollision _testClass;
        private List<ExplosionCell> explosions { get; set; }
        private static Session session = SessionManager.Instance.GetSession(SessionManager.Instance.ActiveSessionCode);
        private Map gameMap = session.Map;
        private List<Powerup> powerups = session.powerups;
        private List<Player> players = session.Players;

        public BombCollisionTests()
        {
            _testClass = new BombCollision();
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new BombCollision();
            Assert.NotNull(instance);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 6)]
        [InlineData(6, 10)]
        [InlineData(100, 100)]
        public void BombCollisionTest(int playerX, int playerY)
        {
            session.RegisterPlayer(new Player("Player1", "test1", playerX, playerY));
            players[players.Count - 1].PlaceBomb();
            var collisionTarget = players[players.Count - 1].bombs[players[players.Count - 1].bombs.Count - 1];
            
            var explodedAt = new DateTime(1441082850);
            var powerupList = new List<Powerup>();
            _testClass.ExplosionCollisionStrategy(collisionTarget, new List<ExplosionCell>(), explodedAt, powerupList);
            var explosion = players[players.Count - 1].bombs[players[players.Count - 1].bombs.Count - 1].explosion;
            Assert.NotEmpty(explosion.GetExplosionCells());
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 6)]
        [InlineData(6, 10)]
        public void PlayerCollisionTest(int playerX, int playerY)
        {
            session.RegisterPlayer(new Player("Player1", "test1", playerX, playerY));
            players[players.Count - 1].PlaceBomb();
            players[players.Count - 1].bombs[players[players.Count - 1].bombs.Count - 1].hasExploded = true;
            var collisionTarget = players[players.Count - 1].bombs[players[players.Count - 1].bombs.Count - 1];

            var explodedAt = new DateTime(1441082850);
            var powerupList = new List<Powerup>();
            _testClass.PlayerCollisionStrategy(players[players.Count - 1], collisionTarget, powerups, null);
            var isInvuln = players[players.Count - 1].invulnerable;
            Assert.True(isInvuln);
        }

        [Fact]
        public void CannotCallExplosionCollisionStrategyWithNullCollisionTarget()
        {
            Assert.Throws<InvalidOperationException>(() => _testClass.ExplosionCollisionStrategy(default(object), new List<ExplosionCell>(), new DateTime(1572264501), new List<Powerup>()));
        }

        [Fact]
        public void CannotCallPlayerCollisionStrategyWithNullPlayer()
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.PlayerCollisionStrategy(default(Player), new Bomb(), new List<Powerup>(), new PowerupInvoker()));
        }

        [Fact]
        public void CannotCallPlayerCollisionStrategyWithNullCollisionTarget()
        {
            Assert.Throws<InvalidOperationException>(() => _testClass.PlayerCollisionStrategy(new Player("TestValue1838950266", "TestValue1121116717", 1754198966, 1232860484), default(object), new List<Powerup>(), new PowerupInvoker()));
        }
    }
}