namespace SignalRWebPackTests.Models.TransportModels
{
    using SignalRWebPack.Models.TransportModels;
    using System;
    using Xunit;

    public class TMapTests
    {
        private TMap _testClass;

        public TMapTests()
        {
            _testClass = new TMap();
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new TMap();
            Assert.NotNull(instance);
        }

        [Fact]
        public void CanSetAndGettiles()
        {
            var testValue = new[] { new TTile(), new TTile(), new TTile() };
            _testClass.tiles = testValue;
            Assert.Equal(testValue, _testClass.tiles);
        }
    }
}