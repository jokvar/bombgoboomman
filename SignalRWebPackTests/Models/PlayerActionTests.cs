namespace SignalRWebPackTests.Models
{
    using SignalRWebPack.Models;
    using System;
    using Xunit;

    public class PlayerActionTests
    {
        private PlayerAction _testClass;
        private ActionEnums _action;

        public PlayerActionTests()
        {
            _action = ActionEnums.Left;
            _testClass = new PlayerAction(_action);
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new PlayerAction(_action);
            Assert.NotNull(instance);
        }

        [Fact]
        public void actionIsInitializedCorrectly()
        {
            Assert.Equal(_action, _testClass.action);
        }

        [Fact]
        public void CanSetAndGetaction()
        {
            var testValue = ActionEnums.Down;
            _testClass.action = testValue;
            Assert.Equal(testValue, _testClass.action);
        }
    }
}