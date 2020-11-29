namespace SignalRWebPackTests.Models
{
    using SignalRWebPack.Models;
    using System;
    using Xunit;

    public class EmptyTileTests
    {
        private EmptyTile _testClass;

        public EmptyTileTests()
        {
            _testClass = new EmptyTile();
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new EmptyTile();
            Assert.NotNull(instance);
        }

        [Fact]
        public void CanCallSetValues()
        {
            var x = 1678972401;
            var y = 390401566;
            var texture = "TestValue";
            _testClass.SetValues(x, y, texture);
            Assert.Equal(x, _testClass.x);
            Assert.Equal(y, _testClass.y);
            Assert.Equal(texture, _testClass.texture);
        }

        [Fact]
        public void CannotCallSetValuesWithNullTexture()
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.SetValues(162136192, 503076549, null));
        }
    }
}