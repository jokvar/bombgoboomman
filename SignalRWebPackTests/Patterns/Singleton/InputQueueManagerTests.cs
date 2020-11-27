namespace SignalRWebPackTests.Patterns.Singleton
{
    using SignalRWebPack.Patterns.Singleton;
    using System;
    using Xunit;
    using SignalRWebPack.Models;
    using SignalRWebPack.Logic;

    public class InputQueueManagerTests
    {
        private Player __player;

        public InputQueueManagerTests()
        {
            __player = new Player("TestValue1893646907", "TestValue1977100354", 233697183, 365252170) { lives = 1 };
            SessionManager.Instance.GetSession(null).RegisterPlayer(__player, true);
        }

        [Fact]
        public void CanCallAddToInputQueue()
        {
            InputQueueManager.Instance.FlushInputQueue();
            var _action = new PlayerAction(ActionEnums.Down);
            Assert.Equal(0, InputQueueManager.Instance.StackSize);
            InputQueueManager.Instance.AddToInputQueue(__player.id, _action);
            Assert.Equal(1, InputQueueManager.Instance.StackSize);
            //__player.lives = 0;
            //_testClass.AddToInputQueue(__player.id, _action);
            //Assert.Equal(1, _testClass.StackSize);
        }

        [Fact]
        public void CannotCallAddToInputQueueWithNull_action()
        {
            Assert.Throws<ArgumentNullException>(() => InputQueueManager.Instance.AddToInputQueue("TestValue555651329", default(PlayerAction)));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void CannotCallAddToInputQueueWithInvalid_connectionId(string value)
        {
            Assert.Throws<ArgumentNullException>(() => InputQueueManager.Instance.AddToInputQueue(value, new PlayerAction(ActionEnums.Up)));
        }

        [Fact]
        public void CanCallReadOne()
        {
            var _action = new PlayerAction(ActionEnums.Down);
            Assert.Equal(0, InputQueueManager.Instance.StackSize);
            InputQueueManager.Instance.AddToInputQueue(__player.id, _action);
            Assert.Equal(1, InputQueueManager.Instance.StackSize);
            var result = InputQueueManager.Instance.ReadOne();
            Assert.Equal(_action, result.Item2);
            Assert.Equal(0, InputQueueManager.Instance.StackSize);
        }

        [Fact]
        public void CanCallFlushInputQueue()
        {
            var _action = new PlayerAction(ActionEnums.Down);
            Assert.Equal(0, InputQueueManager.Instance.StackSize);
            InputQueueManager.Instance.AddToInputQueue(__player.id, _action);
            Assert.Equal(1, InputQueueManager.Instance.StackSize);
            InputQueueManager.Instance.AddToInputQueue(__player.id, _action);
            Assert.Equal(2, InputQueueManager.Instance.StackSize);
            InputQueueManager.Instance.FlushInputQueue();
            Assert.Equal(0, InputQueueManager.Instance.StackSize);
        }

        [Fact]
        public void CanGetInstance()
        {
            var instance = InputQueueManager.Instance;
            Assert.IsType<InputQueueManager>(instance);
            Assert.Same(instance, InputQueueManager.Instance);
        }
    }
}