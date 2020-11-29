namespace SignalRWebPackTests.Patterns.Strategy
{
    using SignalRWebPack.Patterns.Strategy;
    using System;
    using Xunit;
    using System.Collections.Generic;
    using SignalRWebPack.Models;
    using SignalRWebPack.Patterns.Command;
    using SignalRWebPack.Logic;

    public class PlayerCollisionTests
    {
        private PlayerCollision _testClass;
        private List<ExplosionCell> explosions { get; set; }
        private static Session session = SessionManager.Instance.GetSession(SessionManager.Instance.ActiveSessionCode);
        private Map gameMap = session.Map;
        private List<Powerup> powerups = session.powerups;
        private List<Player> players = session.Players;

        public PlayerCollisionTests()
        {
            _testClass = new PlayerCollision();
            explosions = new List<ExplosionCell>();
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new PlayerCollision();
            Assert.NotNull(instance);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(10, 10)]
        [InlineData(100, 156)]
        public void ExplosionCollisionTest(int playerX, int playerY)
        {
            session.RegisterPlayer(new Player("Player1", "test1", playerX, playerY));
            var collisionTarget = players[players.Count - 1];
            var explodedAt = new DateTime(1441082850);
            var powerupList = new List<Powerup>();
            var oldPlayerLives = players[players.Count - 1].lives;

            _testClass.ExplosionCollisionStrategy(collisionTarget, explosions, explodedAt, powerupList);

            var newPlayerLives = players[players.Count - 1].lives;
            Assert.True((oldPlayerLives - newPlayerLives) == 1);
            Assert.True(collisionTarget.invulnerable);
            


        }

        [Theory]
        [InlineData(1, 1, 1, 2)]
        [InlineData(10, 10, 1, 2)]
        [InlineData(1, 1, 12, 21)]
        public void PlayerCollisionTest(int playerX1, int playerY1, int playerX2, int playerY2)
        {
            session.RegisterPlayer(new Player("Player1", "test1", playerX1, playerY1));
            session.RegisterPlayer(new Player("Player2", "test2", playerX2, playerY2));
            var collider = players[players.Count - 1];
            var collisionTarget = players[players.Count - 2];

            _testClass.PlayerCollisionStrategy(collider, collisionTarget, new List<Powerup>(), null);

            var colliderCoords = 15 * playerY1 + playerX1;
            var targetCoords = 15 * playerY2 + playerX2;

            Assert.NotEqual(colliderCoords, targetCoords);
        }


        [Fact]
        public void CannotCallExplosionCollisionStrategyWithNullCollisionTarget()
        {
            Assert.Throws<InvalidOperationException>(() => _testClass.ExplosionCollisionStrategy(default(object), new List<ExplosionCell>(), new DateTime(2090873606), new List<Powerup>()));
        }

        [Fact]
        public void CannotCallExplosionCollisionStrategyWithNullExplosions()
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.ExplosionCollisionStrategy(new Player("TestValue435820232", "TestValue1867908353", 2131444976, 1004254920), default(List<ExplosionCell>), new DateTime(1443186062), new List<Powerup>()));
        }

        [Fact]
        public void CannotCallPlayerCollisionStrategyWithNullPlayer()
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.PlayerCollisionStrategy(default(Player), new Player("TestValue435820232", "TestValue1867908353", 2131444976, 1004254920), new List<Powerup>(), new PowerupInvoker()));
        }

        [Fact]
        public void CannotCallPlayerCollisionStrategyWithNullCollisionTarget()
        {
            Assert.Throws<InvalidOperationException>(() => _testClass.PlayerCollisionStrategy(new Player("TestValue435820232", "TestValue1867908353", 2131444976, 1004254920), default(object), new List<Powerup>(), new PowerupInvoker()));
        }

    }
}