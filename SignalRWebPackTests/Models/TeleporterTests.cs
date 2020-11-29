namespace SignalRWebPackTests.Models
{
    using SignalRWebPack.Models;
    using System;
    using Xunit;

    public class TeleporterTests
    {
        private Teleporter _testClass;

        public TeleporterTests()
        {
            _testClass = new Teleporter();
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new Teleporter();
            Assert.NotNull(instance);
        }

        [Fact]
        public void CanSetAndGetteleportLinkId()
        {
            var testValue = 1944607008;
            _testClass.teleportLinkId = testValue;
            Assert.Equal(testValue, _testClass.teleportLinkId);
        }
    }
}