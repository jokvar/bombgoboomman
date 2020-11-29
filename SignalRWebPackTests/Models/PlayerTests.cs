namespace SignalRWebPackTests.Models
{
    using SignalRWebPack.Models;
    using System;
    using Xunit;
    using SignalRWebPack.Patterns.Strategy;
    using System.Collections.Generic;
    using SignalRWebPack.Patterns.Command;
    using SignalRWebPack.Patterns.AbstractFactory;

    public class PlayerTests
    {
        private Player _testClass;
        private string _name;
        private string _id;
        private int _x;
        private int _y;

        public PlayerTests()
        {
            _name = "TestValue256248079";
            _id = "TestValue460638409";
            _x = 1402468627;
            _y = 1334940519;
            _testClass = new Player(_name, _id, _x, _y);
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new Player(_name, _id, _x, _y);
            Assert.NotNull(instance);
        }

        [Fact]
        public void CanCallSetCollisionStrategy()
        {
            var collisionStrategy = new WallCollision();
            _testClass.SetCollisionStrategy(collisionStrategy);
            Assert.Equal(collisionStrategy, _testClass.GetCollisionStrategy());
        }

        [Fact]
        public void CannotCallResolvePlayerCollisions()
        {
            var collisionStrategy = new WallCollision();
            _testClass.SetCollisionStrategy(collisionStrategy);
            Assert.Equal(collisionStrategy, _testClass.GetCollisionStrategy());
            var player = new Player("TestValue1799524994", "TestValue2082513202", 100, 200);
            Wall collisionTarget = new Wall();
            Box collisionTargetBox = new Box();
            var collisionList = new List<Powerup>();
            var powerupInvoker = new PowerupInvoker();

            Assert.Throws<InvalidOperationException>(() => _testClass.ResolvePlayerCollision(player, collisionTargetBox, collisionList, powerupInvoker));

            var collisionStrategyPow = new PowerupCollision();
            _testClass.SetCollisionStrategy(collisionStrategyPow);
            Assert.Throws<InvalidOperationException>(() => _testClass.ResolvePlayerCollision(player, collisionTargetBox, collisionList, powerupInvoker));

            var collisionStrategyPlayer = new PlayerCollision();
            _testClass.SetCollisionStrategy(collisionStrategyPlayer);
            Assert.Throws<InvalidOperationException>(() => _testClass.ResolvePlayerCollision(player, collisionTargetBox, collisionList, powerupInvoker));

            var collisionStrategyExplosion = new ExplosionCollision();
            _testClass.SetCollisionStrategy(collisionStrategyExplosion);
            Assert.Throws<InvalidOperationException>(() => _testClass.ResolvePlayerCollision(player, collisionTargetBox, collisionList, powerupInvoker));

            var collisionStrategyEmpty = new EmptyTileCollision();
            _testClass.SetCollisionStrategy(collisionStrategyEmpty);
            Assert.Throws<InvalidOperationException>(() => _testClass.ResolvePlayerCollision(player, collisionTargetBox, collisionList, powerupInvoker));

            var collisionStrategyBomb = new BombCollision();
            _testClass.SetCollisionStrategy(collisionStrategyBomb);
            Assert.Throws<InvalidOperationException>(() => _testClass.ResolvePlayerCollision(player, collisionTargetBox, collisionList, powerupInvoker));

            var collisionStrategyBox = new BoxCollision();
            _testClass.SetCollisionStrategy(collisionStrategyBox);
            Assert.Throws<InvalidOperationException>(() => _testClass.ResolvePlayerCollision(player, collisionTarget, collisionList, powerupInvoker));
        }

        [Fact]
        public void CannotCallResolvePlayerCollisionWithNullPlayer()
        {
            Assert.Throws<NullReferenceException>(() => _testClass.ResolvePlayerCollision(default(Player), new object(), new List<Powerup>(), new PowerupInvoker()));
        }

        [Fact]
        public void CannotCallResolvePlayerCollisionWithNullCollisionTarget()
        {
            Assert.Throws<NullReferenceException>(() => _testClass.ResolvePlayerCollision(new Player("TestValue32590763", "TestValue1558707683", 1602296550, 887236001), default(object), new List<Powerup>(), new PowerupInvoker()));
        }

        [Fact]
        public void CannotCallResolvePlayerCollisionWithNullCollisionList()
        {
            Assert.Throws<NullReferenceException>(() => _testClass.ResolvePlayerCollision(new Player("TestValue1307103635", "TestValue149568714", 173912787, 1074483002), new object(), default(List<Powerup>), new PowerupInvoker()));
        }

        [Fact]
        public void CannotCallResolvePlayerCollisionWithNullPowerupInvoker()
        {
            Assert.Throws<NullReferenceException>(() => _testClass.ResolvePlayerCollision(new Player("TestValue687774123", "TestValue1063990271", 1352874447, 7610258), new object(), new List<Powerup>(), default(PowerupInvoker)));
        }

        [Fact]
        public void CanCallPlaceBomb()
        {
            _testClass.PlaceBomb();
            Assert.NotNull(_testClass.bombs[0]);
        }

        [Fact]
        public void CanCallGetBombCount()
        {
            _testClass.maxBombs = 3;
            _testClass.PlaceBomb();
            _testClass.PlaceBomb();
            _testClass.PlaceBomb();
            int count = _testClass.GetBombCount();
            Assert.Equal(3, count);
        }

        [Fact]
        public void CanCallCheckInvulnerabilityPeriods()
        {
            DateTime date = DateTime.Now;
            TimeSpan time = new TimeSpan(36, 0, 0, 0);
            DateTime combined = date.Add(time);
            _testClass.invulnerable = true;
            _testClass.invulnerableUntil = combined;
            _testClass.CheckInvulnerabilityPeriods();
            Assert.True(_testClass.invulnerable);
            _testClass.invulnerableUntil = date;
            _testClass.CheckInvulnerabilityPeriods();
            Assert.False(_testClass.invulnerable);
        }

        [Fact]
        public void CanCallRefreshBombList()
        {
            _testClass.PlaceBomb();
            int count = _testClass.activeBombCount;
            Assert.Equal(1, count);
            Bomb b = _testClass.bombs[0];
            _testClass.RefreshBombList(b);
            count = _testClass.activeBombCount;
            Assert.Equal(0, count);
        }

        [Fact]
        public void CannotCallRefreshBombListWithNullBomb()
        {
            Assert.Throws<NullReferenceException>(() => _testClass.RefreshBombList(default(Bomb)));
        }

        [Fact]
        public void CanCallGetBomb()
        {
            _testClass.x = 1;
            _testClass.y = 1;
            int x = 1;
            int y = 1;
            _testClass.PlaceBomb();
            var result = _testClass.GetBomb(1, 1);
            Assert.IsType<Bomb>(result);
        }

        [Fact]
        public void CannotCallGetBombWrongCoordinates()
        {
            _testClass.x = 1;
            _testClass.y = 1;
            int x = 1;
            int y = 1;
            _testClass.PlaceBomb();
            var result = _testClass.GetBomb(1, 2);
            Assert.Null(result);
        }

        [Fact]
        public void CanSetAndGetlives()
        {
            var testValue = 526293267;
            _testClass.lives = testValue;
            Assert.Equal(testValue, _testClass.lives);
        }

        [Fact]
        public void nameIsInitializedCorrectly()
        {
            Assert.Equal(_name, _testClass.name);
        }

        [Fact]
        public void CanSetAndGetname()
        {
            var testValue = "TestValue1817785510";
            _testClass.name = testValue;
            Assert.Equal(testValue, _testClass.name);
        }

        [Fact]
        public void idIsInitializedCorrectly()
        {
            Assert.Equal(_id, _testClass.id);
        }

        [Fact]
        public void CanSetAndGetid()
        {
            var testValue = "TestValue2074410222";
            _testClass.id = testValue;
            Assert.Equal(testValue, _testClass.id);
        }

        [Fact]
        public void CanSetAndGetinvulnerable()
        {
            var testValue = true;
            _testClass.invulnerable = testValue;
            Assert.Equal(testValue, _testClass.invulnerable);
        }

        [Fact]
        public void CanSetAndGetspeedMultiplier()
        {
            var testValue = 1182725235.45;
            _testClass.speedMultiplier = testValue;
            Assert.Equal(testValue, _testClass.speedMultiplier);
        }

        [Fact]
        public void CanSetAndGetready()
        {
            var testValue = true;
            _testClass.ready = testValue;
            Assert.Equal(testValue, _testClass.ready);
        }

        [Fact]
        public void CanSetAndGetinvulnerableSince()
        {
            var testValue = new DateTime(1326792908);
            _testClass.invulnerableSince = testValue;
            Assert.Equal(testValue, _testClass.invulnerableSince);
        }

        [Fact]
        public void CanSetAndGetinvulnerableUntil()
        {
            var testValue = new DateTime(1012518245);
            _testClass.invulnerableUntil = testValue;
            Assert.Equal(testValue, _testClass.invulnerableUntil);
        }

        [Fact]
        public void CanSetAndGetinvulnerabilityDuration()
        {
            var testValue = 477745070;
            _testClass.invulnerabilityDuration = testValue;
            Assert.Equal(testValue, _testClass.invulnerabilityDuration);
        }

        [Fact]
        public void CanSetAndGetmaxBombs()
        {
            var testValue = 1164635795;
            _testClass.maxBombs = testValue;
            Assert.Equal(testValue, _testClass.maxBombs);
        }

        [Fact]
        public void CanSetAndGetactiveBombCount()
        {
            var testValue = 534311359;
            _testClass.activeBombCount = testValue;
            Assert.Equal(testValue, _testClass.activeBombCount);
        }

        [Fact]
        public void CanSetAndGetexplosionSizeMultiplier()
        {
            var testValue = 1056864339;
            _testClass.explosionSizeMultiplier = testValue;
            Assert.Equal(testValue, _testClass.explosionSizeMultiplier);
        }

        [Fact]
        public void CanSetAndGetbombTickDuration()
        {
            var testValue = 1179388942;
            _testClass.bombTickDuration = testValue;
            Assert.Equal(testValue, _testClass.bombTickDuration);
        }

        [Fact]
        public void CanSetAndGetbombs()
        {
            var testValue = new List<Bomb>();
            _testClass.bombs = testValue;
            Assert.Equal(testValue, _testClass.bombs);
        }

        [Fact]
        public void CanGetIsAlive()
        {
            Assert.IsType<bool>(_testClass.IsAlive);
            Assert.True(_testClass.IsAlive);
        }
    }
}