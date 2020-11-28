namespace SignalRWebPackTests.Patterns.AbstractFactory
{
    using SignalRWebPack.Patterns.AbstractFactory;
    using SignalRWebPack.Models;
    using System;
    using Xunit;

    public class ObjectFactoryTests
    {
        private ObjectFactory _testClass;

        public ObjectFactoryTests()
        {
            _testClass = new ObjectFactory();
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new ObjectFactory();
            Assert.NotNull(instance);
        }

        [Fact]
        public void CanCallGetObject()
        {
            var objectType = "explosion";
            var result = _testClass.GetObject(objectType);
            Assert.IsType<ExplosionCell>(result);
            var _objectType = "bomb";
            var _result = _testClass.GetObject(_objectType);
            Assert.IsType<Bomb>(_result);
            var __objectType = "powerup";
            var __result = _testClass.GetObject(__objectType);
            Assert.IsType<Powerup>(__result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("WRONG TYPE")]
        [InlineData("   ")]
        [InlineData("")]
        public void CannotCallGetObjectWithInvalidObjectType(string value)
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.GetObject(value));
        }
    }
}