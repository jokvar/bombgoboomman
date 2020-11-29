namespace SignalRWebPackTests.Models
{
    using SignalRWebPack.Models;
    using System;
    using Xunit;

    public class GunpowderTests
    {
        private Gunpowder _testClass;

        public GunpowderTests()
        {
            _testClass = new Gunpowder();
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new Gunpowder();
            Assert.NotNull(instance);
        }

        [Fact]
        public void CanSetAndGetdamage()
        {
            var testValue = 974409535;
            _testClass.damage = testValue;
            Assert.Equal(testValue, _testClass.damage);
        }
    }
}