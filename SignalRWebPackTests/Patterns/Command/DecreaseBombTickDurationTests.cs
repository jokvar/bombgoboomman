namespace SignalRWebPackTests.Patterns.Command
{
    using SignalRWebPack.Patterns.Command;
    using System;
    using Xunit;
    using SignalRWebPack.Models;
    using SignalRWebPack.Logic;

    public class DecreaseBombTickDurationTests
    {
        private DecreaseBombTickDuration _testClass;
        private Player __player;

        public DecreaseBombTickDurationTests()
        {
            __player = new Player("TestValue1865647587", "TestValue2131367710", 199764800, 1372910220) { bombTickDuration = 2 };
            _testClass = new DecreaseBombTickDuration(__player);
            SessionManager.Instance.GetSession(null).RegisterPlayer(__player, true);
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new DecreaseBombTickDuration(__player);
            Assert.NotNull(instance);
        }

        [Fact]
        public void CannotConstructWithNull_player()
        {
            Assert.Throws<ArgumentNullException>(() => new DecreaseBombTickDuration(default(Player)));
        }

        [Fact]
        public void CanCallExecute()
        {
            var expected = __player.bombTickDuration - 1;
            _testClass.Execute();
            Assert.Equal(expected, __player.bombTickDuration);
            var min = __player.Defaults.MinBombTickDuration;
            __player.bombTickDuration = min;
            _testClass.Execute();
            Assert.Equal(min, __player.bombTickDuration);
        }

        [Fact]
        public void CanCallUndo()
        {
            var expected = __player.bombTickDuration + 1;
            _testClass.Undo();
            Assert.Equal(expected, __player.bombTickDuration);
            var max = __player.Defaults.MaxBombTickDuration;
            __player.bombTickDuration = max;
            _testClass.Undo();
            Assert.Equal(max, __player.bombTickDuration);
        }
    }
}