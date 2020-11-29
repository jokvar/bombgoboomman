namespace SignalRWebPackTests.Patterns.Strategy
{
    using SignalRWebPack.Patterns.Strategy;
    using System;
    using Xunit;
    using System.Collections.Generic;
    using SignalRWebPack.Models;
    using SignalRWebPack.Patterns.Command;
    using System.Linq;
    using SignalRWebPack.Logic;

    public class EmptyTileCollisionTests
    {
        private EmptyTileCollision _testClass;
        private List<ExplosionCell> explosions { get; set; }
        private static Session session = SessionManager.Instance.GetSession(SessionManager.Instance.ActiveSessionCode);
        private Map gameMap = session.Map;
        private List<Powerup> powerups = session.powerups;
        private List<Player> players = session.Players;

        public EmptyTileCollisionTests()
        {
            _testClass = new EmptyTileCollision();
            explosions = new List<ExplosionCell>();
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new EmptyTileCollision();
            Assert.NotNull(instance);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(10, 10)]
        [InlineData(1, 18)]
        public void ExplosionCollisionTest(int emptyX, int emptyY)
        {
            gameMap.tiles[15 * emptyX + emptyY] = new EmptyTile() { x = emptyX, y = emptyY };
            var collisionTarget = gameMap.tiles[15 * emptyX + emptyY];

            var explodedAt = new DateTime(1441082850);
            var powerupList = new List<Powerup>();

            _testClass.ExplosionCollisionStrategy(collisionTarget, explosions, explodedAt, powerupList);

            var emptyTile = explosions.Where(e => e.x == emptyX && e.y == emptyY).FirstOrDefault();
            Assert.NotNull(emptyTile);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(10, 10)]
        [InlineData(1, 18)]
        public void PlayerCollisionTest(int emptyX, int emptyY)
        {
            session.RegisterPlayer(new Player("Player1", "test1", 1, 1));
            gameMap.tiles[15 * emptyX + emptyY] = new EmptyTile() { x = emptyX, y = emptyY };
            var collisionTarget = gameMap.tiles[15 * emptyX + emptyY];

            _testClass.PlayerCollisionStrategy(players[players.Count - 1], collisionTarget, new List<Powerup>(), null);

            //converting coordinates to tile indices to check whether the player was moved 
            //into the same coordinates as the box after collision was resolved
            var playerConvertedCoords = 15 * players[players.Count - 1].y + players[players.Count - 1].x;
            var emptyConvertedCoords = 15 * emptyY + emptyX;

            Assert.Equal(emptyConvertedCoords, playerConvertedCoords);
        }


        [Fact]
        public void CannotCallExplosionCollisionStrategyWithNullCollisionTarget()
        {
            Assert.Throws<InvalidOperationException>(() => _testClass.ExplosionCollisionStrategy(default(object), new List<ExplosionCell>(), new DateTime(685546727), new List<Powerup>()));
        }

        [Fact]
        public void CannotCallExplosionCollisionStrategyWithNullExplosions()
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.ExplosionCollisionStrategy(new EmptyTile(), default(List<ExplosionCell>), new DateTime(1245562914), new List<Powerup>()));
        }

        [Fact]
        public void CannotCallPlayerCollisionStrategyWithNullPlayer()
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.PlayerCollisionStrategy(default(Player), new EmptyTile(), new List<Powerup>(), new PowerupInvoker()));
        }

        [Fact]
        public void CannotCallPlayerCollisionStrategyWithNullCollisionTarget()
        {
            Assert.Throws<InvalidOperationException>(() => _testClass.PlayerCollisionStrategy(new Player("TestValue241327305", "TestValue734443955", 326493363, 880977268), default(object), new List<Powerup>(), new PowerupInvoker()));
        }

    }
}