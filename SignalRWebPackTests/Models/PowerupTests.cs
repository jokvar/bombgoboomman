namespace SignalRWebPackTests.Models
{
    using SignalRWebPack.Models;
    using System;
    using Xunit;

    public class PowerupTests
    {
        private Powerup _testClass;
        private Powerup_type _type;
        private int _x;
        private int _y;

        public PowerupTests()
        {
            _type = Powerup_type.AdditionalBomb;
            _x = 195791256;
            _y = 385822384;
            _testClass = new Powerup(_type, _x, _y);
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new Powerup(_type, _x, _y);
            Assert.NotNull(instance);
            instance = new Powerup();
            Assert.NotNull(instance);
        }

        [Fact]
        public void CanCallSetValues()
        {
            var type = Powerup_type.PowerDown;
            var x = 1968659415;
            var y = 528445231;
            _testClass.SetValues(type, x, y);
            Assert.Equal(x, _testClass.x);
            Assert.Equal(y, _testClass.y);
            Assert.Equal(type, _testClass.type);
        }

        [Fact]
        public void CanCallGetTextures()
        {
            Powerup p = new Powerup(Powerup_type.PowerDown, 1, 1);
            p.textures.Add("test");
            var result = p.GetTextures();
            Assert.Equal("test", result[0]);
        }

        [Fact]
        public void typeIsInitializedCorrectly()
        {
            Assert.Equal(_type, _testClass.type);
        }

        [Fact]
        public void CanSetAndGettype()
        {
            var testValue = Powerup_type.PowerDownX3;
            _testClass.type = testValue;
            Assert.Equal(testValue, _testClass.type);
        }

        [Fact]
        public void CanSetAndGetexistDuration()
        {
            var testValue = 20161795;
            _testClass.existDuration = testValue;
            Assert.Equal(testValue, _testClass.existDuration);
        }

        [Fact]
        public void CanSetAndGetplantedAt()
        {
            var testValue = new DateTime(1267344188);
            _testClass.plantedAt = testValue;
            Assert.Equal(testValue, _testClass.plantedAt);
        }
    }
}