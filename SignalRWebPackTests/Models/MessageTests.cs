namespace SignalRWebPackTests.Models
{
    using SignalRWebPack.Models;
    using System;
    using Xunit;

    public class MessageTests
    {
        private Message _testClass;

        public MessageTests()
        {
            _testClass = new Message();
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new Message();
            Assert.NotNull(instance);
        }

        [Fact]
        public void CanSetAndGetContent()
        {
            var testValue = "TestValue28865493";
            _testClass.Content = testValue;
            Assert.Equal(testValue, _testClass.Content);
        }

        [Fact]
        public void CanSetAndGetClass()
        {
            var testValue = "TestValue325114304";
            _testClass.Class = testValue;
            Assert.Equal(testValue, _testClass.Class);
        }
    }
}