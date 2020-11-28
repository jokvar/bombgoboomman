namespace SignalRWebPackTests.Patterns.AbstractFactory
{
    using SignalRWebPack.Patterns.AbstractFactory;
    using SignalRWebPack.Models;
    using System;
    using Xunit;

    public class TileFactoryTests
    {
        private TileFactory _testClass;

        public TileFactoryTests()
        {
            _testClass = new TileFactory();
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new TileFactory();
            Assert.NotNull(instance);
        }

        [Fact]
        public void CanCallGetObject()
        {
            var type = "wall";
            var result = _testClass.GetObject(type);
            Assert.IsType<Wall>(result);
            var _type = "box";
            var _result = _testClass.GetObject(_type);
            Assert.IsType<Box>(_result);
            var __type = "empty";
            var __result = _testClass.GetObject(__type);
            Assert.IsType<EmptyTile>(__result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("WRONG TYPE")]
        [InlineData("")]
        public void CannotCallGetObjectWithInvalidType(string value)
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.GetObject(value));
        }
    }
}