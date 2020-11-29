namespace SignalRWebPackTests.Models
{
    using SignalRWebPack.Models;
    using System;
    using Xunit;
    using SignalRWebPack.Patterns.Strategy;
    using System.Collections.Generic;
    using Moq;

    public class ExplosionCellTests
    {
        private ExplosionCell _testClass;
        private DateTime _explodedAt;
        private int _x;
        private int _y;

        public ExplosionCellTests()
        {
            _explodedAt = new DateTime(990564130);
            _x = 260083261;
            _y = 143174149;
            _testClass = new ExplosionCell(_explodedAt, _x, _y);
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new ExplosionCell(_explodedAt, _x, _y);
            Assert.NotNull(instance);
            instance = new ExplosionCell();
            Assert.NotNull(instance);
        }

        [Fact]
        public void CanCallSetCollisionStrategy()
        {
            var collisionStrategy = new BombCollision();
            _testClass.SetCollisionStrategy(collisionStrategy);
            Assert.IsType<BombCollision>(_testClass.GetCollisionStrategy());
        }

        [Fact]
        public void CannotCallSetCollisionStrategyWithNullCollisionStrategy()
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.SetCollisionStrategy(default));
        }

        [Fact]
        public void CannotCallResolveExplosionCollisionWithNullCollisionTarget()
        {
            _testClass.SetCollisionStrategy(new BombCollision());
            Assert.Throws<InvalidOperationException>(() => _testClass.ResolveExplosionCollision(default, new List<ExplosionCell>(), new DateTime(273539065), new List<Powerup>()));

            _testClass.SetCollisionStrategy(new BoxCollision());
            Assert.Throws<InvalidOperationException>(() => _testClass.ResolveExplosionCollision(default, new List<ExplosionCell>(), new DateTime(273539065), new List<Powerup>()));

            _testClass.SetCollisionStrategy(new EmptyTileCollision());
            Assert.Throws<InvalidOperationException>(() => _testClass.ResolveExplosionCollision(default, new List<ExplosionCell>(), new DateTime(273539065), new List<Powerup>()));

            _testClass.SetCollisionStrategy(new ExplosionCollision());
            Assert.Throws<InvalidOperationException>(() => _testClass.ResolveExplosionCollision(default, new List<ExplosionCell>(), new DateTime(273539065), new List<Powerup>()));

            _testClass.SetCollisionStrategy(new PlayerCollision());
            Assert.Throws<InvalidOperationException>(() => _testClass.ResolveExplosionCollision(default, new List<ExplosionCell>(), new DateTime(273539065), new List<Powerup>()));

            _testClass.SetCollisionStrategy(new PowerupCollision());
            Assert.Throws<InvalidOperationException>(() => _testClass.ResolveExplosionCollision(default, new List<ExplosionCell>(), new DateTime(273539065), new List<Powerup>()));

            _testClass.SetCollisionStrategy(new WallCollision());
            Assert.Throws<InvalidOperationException>(() => _testClass.ResolveExplosionCollision(default, new List<ExplosionCell>(), new DateTime(273539065), new List<Powerup>()));
        }

        [Fact]
        public void CannotCallResolveExplosionCollisionWithNullExplosions()
        {
            _testClass.SetCollisionStrategy(new ExplosionCollision());
            Assert.Throws<ArgumentNullException>(() => _testClass.ResolveExplosionCollision(new ExplosionCell(), default(List<ExplosionCell>), new DateTime(681512742), new List<Powerup>()));
        }

        [Fact]
        public void CannotCallResolveExplosionCollisionWithNullCollisionList()
        {
            _testClass.SetCollisionStrategy(new PowerupCollision());
            Assert.Throws<ArgumentNullException>(() => _testClass.ResolveExplosionCollision(new Powerup(), new List<ExplosionCell>(), new DateTime(95664051), default(List<Powerup>)));
        }

        [Fact]
        public void CanSetAndGetdamage()
        {
            var testValue = 743784528;
            _testClass.damage = testValue;
            Assert.Equal(testValue, _testClass.damage);
        }

        [Fact]
        public void CanSetAndGetexplosionDuration()
        {
            var testValue = 1537676201;
            _testClass.explosionDuration = testValue;
            Assert.Equal(testValue, _testClass.explosionDuration);
        }

        [Fact]
        public void explodedAtIsInitializedCorrectly()
        {
            Assert.Equal(_explodedAt, _testClass.explodedAt);
        }

        [Fact]
        public void CanSetAndGetexplodedAt()
        {
            var testValue = new DateTime(338061435);
            _testClass.explodedAt = testValue;
            Assert.Equal(testValue, _testClass.explodedAt);
        }

        [Fact]
        public void CanSetAndGetexpiresAt()
        {
            var testValue = new DateTime(1873882390);
            _testClass.expiresAt = testValue;
            Assert.Equal(testValue, _testClass.expiresAt);
        }

        [Fact]
        public void CanSetAndGetisExpired()
        {
            var testValue = true;
            _testClass.isExpired = testValue;
            Assert.Equal(testValue, _testClass.isExpired);
        }
    }
}