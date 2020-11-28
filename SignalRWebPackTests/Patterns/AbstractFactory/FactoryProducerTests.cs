namespace SignalRWebPackTests.Patterns.AbstractFactory
{
    using SignalRWebPack.Patterns.AbstractFactory;
    using System;
    using Xunit;

    public class FactoryProducerTests
    {
        private FactoryProducer _testClass;

        public FactoryProducerTests()
        {
            _testClass = new FactoryProducer();
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new FactoryProducer();
            Assert.NotNull(instance);
        }

        [Fact]
        public void CanCallgetFactory()
        {
            var FactoryType = "ObjectFactory";
            var result = FactoryProducer.getFactory(FactoryType);
            Assert.IsType<ObjectFactory>(result);
            var _FactoryType = "TileFactory";
            var _result = FactoryProducer.getFactory(_FactoryType);
            Assert.IsType<TileFactory>(_result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("THIS IS A WRONG TYPE")]
        [InlineData("   ")]
        public void CannotCallgetFactoryWithInvalidFactoryType(string value)
        {
            Assert.Throws<ArgumentNullException>(() => FactoryProducer.getFactory(value));
        }
    }
}