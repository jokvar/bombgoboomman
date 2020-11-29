namespace SignalRWebPackTests.Patterns.Strategy
{
    using SignalRWebPack.Patterns.Strategy;
    using System;
    using Xunit;
    using System.Collections.Generic;
    using SignalRWebPack.Models;
    using SignalRWebPack.Logic;
    using System.Linq;

    public class BoxCollisionTests
    {
        private BoxCollision _testClass;
        private List<ExplosionCell> explosions { get; set; }
        private static Session session = SessionManager.Instance.GetSession(SessionManager.Instance.ActiveSessionCode);
        private Map gameMap = session.Map;
        private List<Powerup> powerups = session.powerups;
        private List<Player> players = session.Players;

        public BoxCollisionTests()
        {
            _testClass = new BoxCollision();
            explosions = new List<ExplosionCell>();
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new BoxCollision();
            Assert.NotNull(instance);
        }

        [Theory]
        [InlineData(2, 1)]
        [InlineData(20, 10)]
        public void ExplosionCollisionTest(int boxX, int boxY)
        {
            gameMap.tiles[15 * boxY + boxX] = new Box() { x = boxX, y = boxY };
            var collisionTarget = gameMap.tiles[15 * boxY + boxX];

            var explodedAt = new DateTime(1441082850);
            var powerupList = new List<Powerup>();
            
            _testClass.ExplosionCollisionStrategy(collisionTarget, explosions, explodedAt, powerupList);

            var boxTile = explosions.Where(e => e.x == boxX && e.y == boxY).FirstOrDefault();
            Assert.NotNull(boxTile);
        }

        [Theory]
        [InlineData(2, 1)]
        [InlineData(20, 10)]
        public void PlayerCollisionTest(int boxX, int boxY)
        {
            session.RegisterPlayer(new Player("Player1", "test1", 1, 1));
            gameMap.tiles[15 * boxY + boxX] = new Box() { x = boxX, y = boxY };
            var collisionTarget = gameMap.tiles[15 * boxY + boxX];

            _testClass.PlayerCollisionStrategy(players[players.Count - 1], collisionTarget, new List<Powerup>(), null);

            //converting coordinates to tile indices to check whether the player was moved 
            //into the same coordinates as the box after collision was resolved
            var playerConvertedCoords = 15 * players[players.Count - 1].y + players[players.Count - 1].x;
            var boxConvertedCoords = 15 * boxY + boxX;

            Assert.NotEqual(boxConvertedCoords, playerConvertedCoords);
        }


        [Fact]
        public void CannotCallExplosionCollisionStrategyWithNullCollisionTarget()
        {
            Assert.Throws<InvalidOperationException>(() => _testClass.ExplosionCollisionStrategy(default(object), new List<ExplosionCell>(), new DateTime(560105582), new List<Powerup>()));
        }

        [Fact]
        public void CannotCallPlayerCollisionStrategyWithNullPlayer()
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.PlayerCollisionStrategy(default(Player), new Box(), new List<Powerup>(), null));
        }

        [Fact]
        public void CannotCallPlayerCollisionStrategyWithNullCollisionTarget()
        {
            Assert.Throws<InvalidOperationException>(() => _testClass.PlayerCollisionStrategy(new Player("TestValue1941448378", "TestValue1795784244", 1821748893, 1684513367), default(object), new List<Powerup>(), null));
        }

        [Fact]
        public void CannotCallGeneratePowerupWithNullPowerups()
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.GeneratePowerup(451707530, 1031966805, default(List<Powerup>)));
        }
    }
}