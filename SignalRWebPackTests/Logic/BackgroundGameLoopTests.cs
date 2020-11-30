namespace SignalRWebPackTests.Logic
{
    using SignalRWebPack.Logic;
    using System;
    using Xunit;
    using Moq;

    public class BackgroundGameLoopTests
    {
        private BackgroundGameLoop _testClass;
        private IGameLogic _gameLogic;

        public BackgroundGameLoopTests()
        {
            _gameLogic = new Mock<IGameLogic>().Object;
            _testClass = new BackgroundGameLoop(_gameLogic);
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new BackgroundGameLoop(_gameLogic);
            Assert.NotNull(instance);
        }

        [Fact]
        public void CannotConstructWithNullGameLogic()
        {
            Assert.Throws<ArgumentNullException>(() => new BackgroundGameLoop(default(IGameLogic)));
        }
    }
}