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

    public class PowerupCollisionTests
    {
        private PowerupCollision _testClass;
        private List<ExplosionCell> explosions { get; set; }
        private static Session session = SessionManager.Instance.GetSession(SessionManager.Instance.ActiveSessionCode);
        private Map gameMap = session.Map;
        private List<Powerup> powerups = session.powerups;
        private List<Player> players = session.Players;
        private PowerupInvoker powerupInvoker = session.powerupInvoker;

        public PowerupCollisionTests()
        {
            _testClass = new PowerupCollision();
            explosions = new List<ExplosionCell>();
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new PowerupCollision();
            Assert.NotNull(instance);
        }

        [Theory]
        [InlineData(Powerup_type.AdditionalBomb, 1, 1)]
        [InlineData(Powerup_type.BombTickDuration, 10, 10)]
        [InlineData(Powerup_type.ExplosionSize, 1, 8)]
        public void ExplosionCollisionTest(Powerup_type type, int powerupX, int powerupY)
        {
            var collisionTarget = new Powerup(type, powerupX, powerupY);
            powerups.Add(collisionTarget);
            var explodedAt = new DateTime(1441082850);

            _testClass.ExplosionCollisionStrategy(collisionTarget, explosions, explodedAt, powerups);

            var explosionTile = explosions.Where(e => e.x == powerupX && e.y == powerupY).FirstOrDefault();
            Assert.NotNull(explosionTile);

            var powerup = powerups.Where(e => e.x == powerupX && e.y == powerupY).FirstOrDefault();
            Assert.Null(powerup);
        }

        [Theory]
        [InlineData(Powerup_type.AdditionalBomb, 1, 1)]
        [InlineData(Powerup_type.BombTickDuration, 10, 10)]
        [InlineData(Powerup_type.ExplosionSize, 1, 8)]
        public void PlayerCollisionTest(Powerup_type type, int powerupX, int powerupY)
        {
            session.RegisterPlayer(new Player("Player1", "test1", 1, 1));
            var collisionTarget = new Powerup(type, powerupX, powerupY);
            powerups.Add(collisionTarget);
            var explodedAt = new DateTime(1441082850);

            var oldMaxBombs = players[players.Count - 1].maxBombs;
            var oldTickDuration = players[players.Count - 1].bombTickDuration;
            var oldExplosionSize = players[players.Count - 1].explosionSizeMultiplier;

            _testClass.PlayerCollisionStrategy(players[players.Count - 1], collisionTarget, powerups, powerupInvoker);

            var powerup = powerups.Where(e => e.x == powerupX && e.y == powerupY).FirstOrDefault();
            Assert.Null(powerup);
            switch (type)
            {
                case Powerup_type.AdditionalBomb:
                    var newMaxBombs = players[players.Count - 1].maxBombs;
                    Assert.True((newMaxBombs - oldMaxBombs) == 1);
                    break;

                case Powerup_type.BombTickDuration:
                    var newTickDuration = players[players.Count - 1].bombTickDuration;
                    Assert.True((oldTickDuration - newTickDuration) == 1);
                    break;

                case Powerup_type.ExplosionSize:
                    var newExplosionSize = players[players.Count - 1].explosionSizeMultiplier;
                    Assert.True((newExplosionSize - oldExplosionSize) == 1);
                    break;
            }
        }


        [Fact]
        public void CannotCallExplosionCollisionStrategyWithNullCollisionTarget()
        {
            Assert.Throws<InvalidOperationException>(() => _testClass.ExplosionCollisionStrategy(default(object), new List<ExplosionCell>(), new DateTime(505702403), new List<Powerup>()));
        }

        [Fact]
        public void CannotCallExplosionCollisionStrategyWithNullExplosions()
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.ExplosionCollisionStrategy(new Powerup(), default(List<ExplosionCell>), new DateTime(2137724634), new List<Powerup>()));
        }

        [Fact]
        public void CannotCallExplosionCollisionStrategyWithNullCollisionList()
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.ExplosionCollisionStrategy(new Powerup(), new List<ExplosionCell>(), new DateTime(1261291783), default(List<Powerup>)));
        }


        [Fact]
        public void CannotCallPlayerCollisionStrategyWithNullPlayer()
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.PlayerCollisionStrategy(default(Player), new Powerup(), new List<Powerup>(), new PowerupInvoker()));
        }

        [Fact]
        public void CannotCallPlayerCollisionStrategyWithNullCollisionTarget()
        {
            Assert.Throws<InvalidOperationException>(() => _testClass.PlayerCollisionStrategy(new Player("TestValue144931174", "TestValue958859504", 209549847, 1310095642), default(object), new List<Powerup>(), new PowerupInvoker()));
        }

        [Fact]
        public void CannotCallPlayerCollisionStrategyWithNullCollisionList()
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.PlayerCollisionStrategy(new Player("TestValue546911343", "TestValue893078310", 747675191, 1503461512), new Powerup(), default(List<Powerup>), new PowerupInvoker()));
        }

        [Fact]
        public void CannotCallResolvePowerupWithNullPlayerReference()
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.ResolvePowerup(default(Player), new Powerup(), new PowerupInvoker()));
        }

        [Fact]
        public void CannotCallResolvePowerupWithNullPowerup()
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.ResolvePowerup(new Player("TestValue1143557840", "TestValue1000815213", 187023402, 1847340191), default(Powerup), new PowerupInvoker()));
        }

    }
}