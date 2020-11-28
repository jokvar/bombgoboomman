namespace SignalRWebPackTests.Models
{
    using SignalRWebPack.Models;
    using System;
    using Xunit;
    using System.Collections.Generic;

    public class BombTests
    {
        private Bomb _testClass;
        private int _x;
        private int _y;
        private int _tickDuration;
        private int _sizeMultiplier;

        public BombTests()
        {
            _x = 1;
            _y = 2;
            _tickDuration = 10;
            _sizeMultiplier = 1;
            _testClass = new Bomb(_x, _y, _tickDuration, _sizeMultiplier);
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new Bomb(_x, _y, _tickDuration, _sizeMultiplier);
            Assert.NotNull(instance);
            instance = new Bomb();
            Assert.NotNull(instance);
        }
        [Fact]
        public void CanCallNullExplosion()
        {
            _testClass.NullExplosion();
            Explosion e = _testClass.explosion;
            Assert.Null(e);
        }

        [Fact]
        public void CanCallSetValues()
        {
            var x = 2093510706;
            var y = 1457767777;
            _testClass.SetValues(x, y);
            var tempX = _testClass.x;
            var tempY = _testClass.y;
            Assert.Equal(x, tempX);
            Assert.Equal(y, tempY);
        }

        [Fact]
        public void CanCallExplode()
        {
            _testClass.hasExploded = false;
            _testClass.Explode();
            Explosion e = _testClass.GetExplosion();
            List<ExplosionCell> list = e.GetExplosionCells();
            Assert.IsType<ExplosionCell>(list[0]);
        }
        [Fact]
        public void CanNotCallGetExplosionCellOutOfBounds()
        {
            _testClass.hasExploded = false;
            _testClass.Explode();
            Explosion e = _testClass.GetExplosion();
            List<ExplosionCell> list = e.GetExplosionCells();
            Assert.Throws<ArgumentOutOfRangeException>(() => list[90]);
        }
        [Fact]
        public void ExplodeBombWithHasExploded()
        {
            _testClass.hasExploded = true;
            _testClass.Explode();
            Explosion e = _testClass.GetExplosion();
            Assert.Throws<NullReferenceException>(() => e.GetExplosionCells());
        }

        [Fact]
        public void tickDurationIsInitializedCorrectly()
        {
            Assert.Equal(_tickDuration, _testClass.tickDuration);
        }

        [Fact]
        public void CanSetAndGettickDuration()
        {
            var testValue = 1450859732;
            _testClass.tickDuration = testValue;
            Assert.Equal(testValue, _testClass.tickDuration);
        }

        [Fact]
        public void CanSetAndGetplantedAt()
        {
            var testValue = new DateTime(1226937759);
            _testClass.plantedAt = testValue;
            Assert.Equal(testValue, _testClass.plantedAt);
        }

        [Fact]
        public void CanSetAndGetexplodesAt()
        {
            var testValue = new DateTime(1752106313);
            _testClass.explodesAt = testValue;
            Assert.Equal(testValue, _testClass.explodesAt);
        }

        [Fact]
        public void CanSetAndGetpreExplodeTexture()
        {
            var testValue = "TestValue131606221";
            _testClass.preExplodeTexture = testValue;
            Assert.Equal(testValue, _testClass.preExplodeTexture);
        }

        [Fact]
        public void CanSetAndGetexplosionSizeMultiplier()
        {
            var testValue = 453571326;
            _testClass.explosionSizeMultiplier = testValue;
            Assert.Equal(testValue, _testClass.explosionSizeMultiplier);
        }

        [Fact]
        public void CanSetAndGetexplosion()
        {
            var testValue = new Explosion(116077263, 1735809681, false, 2059788771);
            _testClass.explosion = testValue;
            Assert.Equal(testValue, _testClass.explosion);
        }

        [Fact]
        public void CanSetAndGethasExploded()
        {
            var testValue = true;
            _testClass.hasExploded = testValue;
            Assert.Equal(testValue, _testClass.hasExploded);
        }
    }
}