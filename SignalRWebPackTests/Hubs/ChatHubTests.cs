namespace SignalRWebPackTests.Hubs
{
    using SignalRWebPack.Hubs;
    using System;
    using Xunit;
    using Moq;
    using SignalRWebPack.Models;
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;
    using SignalRWebPack.Logic;

    public class ChatHubTests
    {
        private ChatHub _testClass;
        //mock objects for Hub
        private Mock<IHubCallerClients> mockClients;
        private Mock<IClientProxy> mockClientProxy;
        private Mock<HubCallerContext> mockContext;
        private const string connectionId = "TestValue13930115";
        public ChatHubTests()
        {
            mockClients = new Mock<IHubCallerClients>();
            mockClientProxy = new Mock<IClientProxy>();
            mockContext = new Mock<HubCallerContext>();

            //get: Context.ConnectionId will return the const
            mockContext.Setup(context => context.ConnectionId).Returns(connectionId);
            //get: Clients.All will return the mock client proxy object with methods that do nothing.
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);
            //initialize and supply mock objects
            _testClass = new ChatHub() { Clients = mockClients.Object, Context = mockContext.Object };
        }

        [Fact]
        public async Task CanCallNewMessage()
        {
            //testing if a random message will be sent off to all clients
            var messageContainer = new Message { Content = "TestValue1917839078", Class = "TestValue691187458" };
            //send message
            await _testClass.NewMessage(messageContainer);
            //verify that all clients would be called
            mockClients.Verify(clients => clients.All, Times.Once);
            //verify that calling works
            mockClientProxy.Verify(
                clientProxy => clientProxy.SendCoreAsync(It.IsAny<string>(), It.Is<object[]>(o => o != null), default),
                Times.Exactly(1)
            );

            //testing if dump message returns content
            messageContainer = new Message { Content = "/dump", Class = "" };
            await _testClass.NewMessage(messageContainer);
            mockClients.Verify(clients => clients.All, Times.Exactly(2));

            //testing joining
            messageContainer = new Message { Content = "/join", Class = "" };
            //cant mock extension method and cant actually send anything so it will always throw in testing case. this is ok.
            await Assert.ThrowsAsync<NullReferenceException>(async () => await _testClass.NewMessage(messageContainer));
            //make sure specific clients were called (exception occurs on return from this method)
            mockClients.Verify(clients => clients.Client(It.IsAny<string>()), Times.Once);
            //assert player exists. if player doesnt exist, MatchId returns -1.
            var playerId = SessionManager.Instance.GetSession(null).MatchId(connectionId);
            Assert.NotEqual(playerId, -1);

            //testing name change. above player was created with a random name.
            var name = "stinky";
            messageContainer = new Message { Content = $"/setname {name}", Class = "" };
            await Assert.ThrowsAsync<NullReferenceException>(async () => await _testClass.NewMessage(messageContainer));
            mockClients.Verify(clients => clients.Client(It.IsAny<string>()), Times.Exactly(2));
            //test if username change worked
            Assert.Equal(name, SessionManager.Instance.GetSession(null).Username(connectionId));

            //test creating session
            messageContainer = new Message { Content = $"/create", Class = "" };
            await Assert.ThrowsAsync<NullReferenceException>(async () => await _testClass.NewMessage(messageContainer));
            mockClients.Verify(clients => clients.Client(It.IsAny<string>()), Times.Exactly(3));
        }
    }
}