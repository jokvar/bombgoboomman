namespace SignalRWebPackTests.Models
{
    using SignalRWebPack.Models;
    using System;
    using Xunit;

    public class TileTests
    {
        private Tile _testClass;

        public TileTests()
        {
            _testClass = new Tile();
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new Tile();
            Assert.NotNull(instance);
        }
    }
}