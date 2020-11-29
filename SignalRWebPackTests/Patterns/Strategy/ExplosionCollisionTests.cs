namespace SignalRWebPackTests.Patterns.Strategy
{
    using SignalRWebPack.Patterns.Strategy;
    using System;
    using Xunit;
    using System.Collections.Generic;
    using SignalRWebPack.Models;
    using SignalRWebPack.Logic;
    using System.Linq;

    public class ExplosionCollisionTests
    {
        private ExplosionCollision _testClass;
        private List<ExplosionCell> explosions { get; set; }
        private static Session session = SessionManager.Instance.GetSession(SessionManager.Instance.ActiveSessionCode);
        private Map gameMap = session.Map;
        private List<Powerup> powerups = session.powerups;
        private List<Player> players = session.Players;

        public ExplosionCollisionTests()
        {
            _testClass = new ExplosionCollision();
            explosions = new List<ExplosionCell>();
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new ExplosionCollision();
            Assert.NotNull(instance);
        }


        [Theory]
        [InlineData(2, 1)]
        [InlineData(20, 10)]
        public void ExplosionCollisionTest(int explosionX, int explosionY)
        {
            var collisionTarget = new ExplosionCell(DateTime.Now, explosionX, explosionY);

            var explodedAt = new DateTime(1441082850);
            var powerupList = new List<Powerup>();

            _testClass.ExplosionCollisionStrategy(collisionTarget, explosions, explodedAt, powerupList);

            var explosionTile = explosions.Where(e => e.x == explosionX && e.y == explosionY).FirstOrDefault();
            Assert.NotNull(explosionTile);
        }

        [Theory]
        [InlineData(2, 1)]
        [InlineData(20, 10)]
        public void PlayerCollisionTest(int explosionX, int explosionY)
        {
            session.RegisterPlayer(new Player("Player1", "test1", 1, 1));
            var collisionTarget = new ExplosionCell(DateTime.Now, explosionX, explosionY);
            var oldPlayerLives = players[players.Count - 1].lives;

            _testClass.PlayerCollisionStrategy(players[players.Count - 1], collisionTarget, new List<Powerup>(), null);

            var newPlayerLives = players[players.Count - 1].lives;

            Assert.True((oldPlayerLives - newPlayerLives) == 1);
            Assert.True(players[players.Count - 1].invulnerable);

            //converting coordinates to tile indices to check whether the player was moved 
            //into the same coordinates as the box after collision was resolved
            var playerConvertedCoords = 15 * players[players.Count - 1].y + players[players.Count - 1].x;
            var explosionConvertedCoords = 15 * explosionY + explosionX;
            Assert.Equal(explosionConvertedCoords, playerConvertedCoords);
        }


        [Fact]
        public void CannotCallExplosionCollisionStrategyWithNullCollisionTarget()
        {
            Assert.Throws<InvalidOperationException>(() => _testClass.ExplosionCollisionStrategy(default(object), new List<ExplosionCell>(), new DateTime(69685599), new List<Powerup>()));
        }

        [Fact]
        public void CannotCallExplosionCollisionStrategyWithNullExplosions()
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.ExplosionCollisionStrategy(new ExplosionCell(), default(List<ExplosionCell>), new DateTime(1886734471), new List<Powerup>()));
        }

        [Fact]
        public void CannotCallPlayerCollisionStrategyWithNullPlayer()
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.PlayerCollisionStrategy(default(Player), new ExplosionCell(), new List<Powerup>(), null));
        }

        [Fact]
        public void CannotCallPlayerCollisionStrategyWithNullCollisionTarget()
        {
            Assert.Throws<InvalidOperationException>(() => _testClass.PlayerCollisionStrategy(new Player("TestValue1888117947", "TestValue1127519169", 2005070133, 2092239742), default(object), new List<Powerup>(), null));
        }

    }
}