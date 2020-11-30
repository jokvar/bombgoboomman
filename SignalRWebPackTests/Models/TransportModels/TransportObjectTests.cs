namespace SignalRWebPackTests.Models.TransportModels
{
    using SignalRWebPack.Models.TransportModels;
    using System;
    using Xunit;

    public class TransportObjectTests
    {
        private TransportObject _testClass;

        public TransportObjectTests()
        {
            _testClass = new TransportObject();
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new TransportObject();
            Assert.NotNull(instance);
        }

        [Fact]
        public void CanSetAndGetx()
        {
            var testValue = 1493947893.24;
            _testClass.x = testValue;
            Assert.Equal(testValue, _testClass.x);
        }

        [Fact]
        public void CanSetAndGety()
        {
            var testValue = 140119778.7;
            _testClass.y = testValue;
            Assert.Equal(testValue, _testClass.y);
        }

        [Fact]
        public void CanSetAndGettexture()
        {
            var testValue = "TestValue2011668699";
            _testClass.texture = testValue;
            Assert.Equal(testValue, _testClass.texture);
        }
    }
}