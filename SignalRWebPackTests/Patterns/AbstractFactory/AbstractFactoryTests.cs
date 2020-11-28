namespace SignalRWebPackTests.Patterns.AbstractFactory
{
    using SignalRWebPack.Patterns.AbstractFactory;
    using System;
    using Xunit;
    using SignalRWebPack.Models;

    public class AbstractFactoryTests
    {
        private class TestAbstractFactory : AbstractFactory
        {
            public override GameObject GetObject(string objectType)
            {
                return default(GameObject);
            }
        }

        private TestAbstractFactory _testClass;

        public AbstractFactoryTests()
        {
            _testClass = new TestAbstractFactory();
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new TestAbstractFactory();
            Assert.NotNull(instance);
        }
    }
}