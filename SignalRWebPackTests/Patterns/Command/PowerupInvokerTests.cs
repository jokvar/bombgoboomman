namespace SignalRWebPackTests.Patterns.Command
{
    using SignalRWebPack.Patterns.Command;
    using System;
    using Xunit;
    using SignalRWebPack.Models;
    using SignalRWebPack.Logic;

    public class PowerupInvokerTests
    {
        private PowerupInvoker _testClass;
        private Player __player;

        public PowerupInvokerTests()
        {
            _testClass = new PowerupInvoker();
            __player = new Player("TestValue1893646907", "TestValue1977100354", 233697183, 365252170) { explosionSizeMultiplier = 3 };
            SessionManager.Instance.GetSession(null).RegisterPlayer(__player.id, true);
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new PowerupInvoker();
            Assert.NotNull(instance);
        }

        [Fact]
        public void CanCallExecuteCommand()
        {
            var PowerupCommand = new IncreaseExplosionSize(__player);
            var expected = __player.explosionSizeMultiplier + 1;
            _testClass.ExecuteCommand(PowerupCommand);
            Assert.Equal(expected, __player.explosionSizeMultiplier);
            Assert.Equal(1, _testClass.StackSize);
        }

        [Fact]
        public void CannotCallExecuteCommandWithNullPowerupCommand()
        {
            Assert.Throws<NullReferenceException>(() => _testClass.ExecuteCommand(default(PowerupCommand)));
        }

        [Fact]
        public void CanCallUndo()
        {
            var PowerupCommand = new IncreaseExplosionSize(__player);
            for (int i = 0; i < 4; i++)
            {
                _testClass.ExecuteCommand(PowerupCommand);
            }
            Assert.Equal(4, _testClass.StackSize);
            var expected = __player.explosionSizeMultiplier - 1;
            _testClass.Undo(1);
            Assert.Equal(expected, __player.explosionSizeMultiplier);
            Assert.Equal(3, _testClass.StackSize);
            _testClass.Undo(3);
            Assert.Equal(0, _testClass.StackSize);
        }

        [Fact]
        public void CanGetStackSize()
        {
            Assert.IsType<int>(_testClass.StackSize);
            var PowerupCommand = new IncreaseExplosionSize(__player);
            _testClass.ExecuteCommand(PowerupCommand);
            Assert.Equal(1, _testClass.StackSize);
            _testClass.ExecuteCommand(PowerupCommand);
            Assert.Equal(2, _testClass.StackSize);
        }
    }
}