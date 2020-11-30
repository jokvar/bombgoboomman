namespace SignalRWebPackTests.Logic
{
    using SignalRWebPack.Logic;
    using System;
    using Xunit;
    using Moq;
    using Microsoft.AspNetCore.SignalR;
    using SignalRWebPack.Hubs;
    using Microsoft.Extensions.Logging;
    using System.Threading;
    using SignalRWebPack.Models;
    using System.Threading.Tasks;
    using System.Dynamic;
    using System.Collections.Generic;

    public class GameLogicTests
    {
        private GameLogic _testClass;
        private IHubContext<ChatHub> _hub;
        private ILogger<GameLogic> _logger;

        public GameLogicTests()
        {
            _hub = new Mock<IHubContext<ChatHub>>().Object;
            _logger = new Mock<ILogger<GameLogic>>().Object;
            _testClass = new GameLogic(_hub, _logger);
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new GameLogic(_hub, _logger);
            Assert.NotNull(instance);
        }

        [Fact]
        public void CannotConstructWithNullHub()
        {
            Assert.Throws<ArgumentNullException>(() => new GameLogic(default(IHubContext<ChatHub>), new Mock<ILogger<GameLogic>>().Object));
        }

        [Fact]
        public void CannotConstructWithNullLogger()
        {
            Assert.Throws<ArgumentNullException>(() => new GameLogic(new Mock<IHubContext<ChatHub>>().Object, default(ILogger<GameLogic>)));
        }


        [Fact]
        public void CannotCallProcessActionWithNullPlayerAction()
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.ProcessAction(default(PlayerAction), "TestValue330675390"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void CannotCallProcessActionWithInvalidId(string value)
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.ProcessAction(new PlayerAction(ActionEnums.Down), value));
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(1, 2, 31)]
        [InlineData(2, 1, 17)]
        [InlineData(0, 3, 45)]
        [InlineData(3, 0, 3)]
        public void CanCallConvertCoordsToIndex(int x, int y, int expected)
        {
            int result = _testClass.ConvertCoordsToIndex(x, y);
            Assert.Equal(expected, result);
        }            

    [Fact]
        public async Task CannotCallStartPlayingWithNullPlayerIDs()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _testClass.StartPlaying(default(string[])));
        }

        [Fact]
        public void CannotCallBroadcastWithNullMessageContainer()
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.Broadcast(default(Tuple<string, Message>)));
        }
    }
}