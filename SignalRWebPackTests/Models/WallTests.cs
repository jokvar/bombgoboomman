namespace SignalRWebPackTests.Models
{
    using SignalRWebPack.Models;
    using System;
    using Xunit;

    public class WallTests
    {
        private Wall _testClass;

        public WallTests()
        {
            _testClass = new Wall();
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new Wall();
            Assert.NotNull(instance);
        }

        [Fact]
        public void CanCallSetValues()
        {
            var x = 2081405018;
            var y = 1022212982;
            var texture = "TestValue1376669768";
            _testClass.SetValues(x, y, texture);
            Assert.Equal(x, _testClass.x);
            Assert.Equal(y, _testClass.y);
            Assert.Equal(texture, _testClass.texture);
        }

        [Fact]
        public void CannotCallSetValuesWithInvalidTexture()
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.SetValues(849000595, 1687312618, null));
        }
    }
}