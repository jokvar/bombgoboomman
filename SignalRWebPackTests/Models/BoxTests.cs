namespace SignalRWebPackTests.Models
{
    using SignalRWebPack.Models;
    using System;
    using Xunit;

    public class BoxTests
    {
        private Box _testClass;

        public BoxTests()
        {
            _testClass = new Box();
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new Box();
            Assert.NotNull(instance);
        }

        [Fact]
        public void CanCallSetValues()
        {
            var x = 444444;
            var y = 333333;
            String texture = "Test Value";
            _testClass.SetValues(x, y, texture);
            Assert.Equal(x, _testClass.x);
            Assert.Equal(y, _testClass.y);
            Assert.Equal(texture, _testClass.texture);
        }

        [Fact]
        public void CannotCallSetValuesWithNullTexture()
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.SetValues(123453, 164299, null));
        }
    }
}