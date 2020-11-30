namespace SignalRWebPackTests.Hubs
{
    using SignalRWebPack.Hubs;
    using System;
    using Xunit;
    using SignalRWebPack.Models;
    using System.Threading.Tasks;

    public class ChatHubTests
    {
        private ChatHub _testClass;

        public ChatHubTests()
        {
            _testClass = new ChatHub();
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new ChatHub();
            Assert.NotNull(instance);
        }

        [Fact]
        public async Task CanCallNewMessage()
        {
            var messageContainer = new Message { Content = "TestValue1879708180", Class = "TestValue932364089" };
            await _testClass.NewMessage(messageContainer);
            Assert.True(false, "Create or modify test");
        }

        [Fact]
        public async Task CannotCallNewMessageWithNullMessageContainer()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _testClass.NewMessage(default(Message)));
        }

        [Fact]
        public async Task CanCallCreateSession()
        {
            var mapName = "TestValue1318093492";
            await _testClass.CreateSession(mapName);
            Assert.True(false, "Create or modify test");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CannotCallCreateSessionWithInvalidMapName(string value)
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _testClass.CreateSession(value));
        }

        [Fact]
        public async Task CanCallJoinSession()
        {
            var roomCode = "TestValue52358545";
            await _testClass.JoinSession(roomCode);
            Assert.True(false, "Create or modify test");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CannotCallJoinSessionWithInvalidRoomCode(string value)
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _testClass.JoinSession(value));
        }

        [Fact]
        public async Task CanCallSendInput()
        {
            var input = new PlayerAction(ActionEnums.Right);
            await _testClass.SendInput(input);
            Assert.True(false, "Create or modify test");
        }

        [Fact]
        public async Task CannotCallSendInputWithNullInput()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _testClass.SendInput(default(PlayerAction)));
        }
    }
}