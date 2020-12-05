namespace SignalRWebPackTests.Patterns.Command
{
    using SignalRWebPack.Patterns.Command;
    using System;
    using Xunit;
    using SignalRWebPack.Models;
    using SignalRWebPack.Logic;

    public class IncreaseExplosionSizeTests
    {
        private IncreaseExplosionSize _testClass;
        private Player __player;

        public IncreaseExplosionSizeTests()
        {
            __player = new Player("TestValue1649814934", "TestValue1393875115", 1432929387, 2084644688) { explosionSizeMultiplier = 3};            _testClass = new IncreaseExplosionSize(__player);
            SessionManager.Instance.GetSession(null).RegisterPlayer(__player, true);
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new IncreaseExplosionSize(__player);
            Assert.NotNull(instance);
        }

        [Fact]
        public void CannotConstructWithNull_player()
        {
            Assert.Throws<ArgumentNullException>(() => new IncreaseExplosionSize(default(Player)));
        }

        [Fact]
        public void CanCallExecute()
        {
            var expected = __player.explosionSizeMultiplier + 1;
            _testClass.Execute();
            Assert.Equal(expected, __player.explosionSizeMultiplier);
            var max = DefaultPowerupValues.MaxExplosionSize;
            __player.explosionSizeMultiplier = max;
            _testClass.Execute();
            Assert.Equal(max, __player.explosionSizeMultiplier);
        }

        [Fact]
        public void CanCallUndo()
        {
            var expected = __player.explosionSizeMultiplier - 1;
            _testClass.Undo();
            Assert.Equal(expected, __player.explosionSizeMultiplier);
            var min = DefaultPowerupValues.MinExplosionSize;
            __player.explosionSizeMultiplier = min;
            _testClass.Undo();
            Assert.Equal(min, __player.explosionSizeMultiplier);
        }
    }
}