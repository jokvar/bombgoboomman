namespace SignalRWebPackTests.Patterns.Strategy
{
    using SignalRWebPack.Patterns.Strategy;
    using System;
    using Xunit;
    using System.Collections.Generic;
    using SignalRWebPack.Models;
    using SignalRWebPack.Patterns.Command;
    using SignalRWebPack.Logic;
    using System.Linq;

    public class WallCollisionTests
    {
        private WallCollision _testClass;
        private List<ExplosionCell> explosions { get; set; }
        private static Session session = SessionManager.Instance.GetSession(SessionManager.Instance.ActiveSessionCode);
        private Map gameMap = session.Map;
        private List<Powerup> powerups = session.powerups;
        private List<Player> players = session.Players;

        public WallCollisionTests()
        {
            _testClass = new WallCollision();
            explosions = new List<ExplosionCell>();
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new WallCollision();
            Assert.NotNull(instance);
        }

        [Fact]
        public void CanCallExplosionCollisionStrategy()
        {
            gameMap.tiles[15 * 1 + 1] = new Wall() { x = 1, y = 1 };
            var collisionTarget = gameMap.tiles[15 * 1 + 1];
            var explodedAt = new DateTime(1536314247);
            var collisionList = new List<Powerup>();
            _testClass.ExplosionCollisionStrategy(collisionTarget, explosions, explodedAt, collisionList);

            var explosionTile = explosions.Where(e => e.x == 1 && e.y == 1).FirstOrDefault();
            Assert.Null(explosionTile);
        }

        [Fact]
        public void CannotCallExplosionCollisionStrategyWithNullCollisionTarget()
        {
            Assert.Throws<InvalidOperationException>(() => _testClass.ExplosionCollisionStrategy(default(object), new List<ExplosionCell>(), new DateTime(1321179354), new List<Powerup>()));
        }

        [Fact]
        public void CannotCallExplosionCollisionStrategyWithNullExplosions()
        {
            Assert.Throws<InvalidOperationException>(() => _testClass.ExplosionCollisionStrategy(new object(), default(List<ExplosionCell>), new DateTime(431085725), new List<Powerup>()));
        }

        [Fact]
        public void CanCallPlayerCollisionStrategy()
        {
            session.RegisterPlayer(new Player("Player1", "test1", 2, 2));
            gameMap.tiles[15 * 1 + 1] = new Wall() { x = 1, y = 1 };
            var collisionTarget = gameMap.tiles[15 * 1 + 1];
            var collisionList = new List<Powerup>();
            _testClass.PlayerCollisionStrategy(players[players.Count - 1], collisionTarget, collisionList, null);

            //converting coordinates to tile indices to check whether the player was moved 
            //into the same coordinates as the box after collision was resolved
            var playerConvertedCoords = 15 * players[players.Count - 1].y + players[players.Count - 1].x;
            var wallConvertedCoords = 15 * 1 + 1;
            Assert.NotEqual(wallConvertedCoords, playerConvertedCoords);
        }

        [Fact]
        public void CannotCallPlayerCollisionStrategyWithNullPlayer()
        {
            Assert.Throws<InvalidOperationException>(() => _testClass.PlayerCollisionStrategy(default(Player), new object(), new List<Powerup>(), new PowerupInvoker()));
        }

        [Fact]
        public void CannotCallPlayerCollisionStrategyWithNullCollisionTarget()
        {
            Assert.Throws<InvalidOperationException>(() => _testClass.PlayerCollisionStrategy(new Player("TestValue1412522758", "TestValue1538653815", 419415674, 1624444375), default(object), new List<Powerup>(), new PowerupInvoker()));
        }

    }
}