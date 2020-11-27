namespace SignalRWebPackTests.Patterns.Command
{
    using SignalRWebPack.Patterns.Command;
    using System;
    using Xunit;
    using SignalRWebPack.Models;
    using SignalRWebPack.Logic;

    public class IncreaseBombCountTests
    {
        private IncreaseBombCount _testClass;
        private Player __player;

        public IncreaseBombCountTests()
        {
            __player = new Player("TestValue1143607526", "TestValue157478733", 458037480, 2128707886) { maxBombs = 2};
            _testClass = new IncreaseBombCount(__player);
            SessionManager.Instance.GetSession(null).RegisterPlayer(__player, true);
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new IncreaseBombCount(__player);
            Assert.NotNull(instance);
        }

        [Fact]
        public void CannotConstructWithNull_player()
        {
            Assert.Throws<ArgumentNullException>(() => new IncreaseBombCount(default(Player)));
        }

        [Fact]
        public void CanCallExecute()
        {
            var expected = __player.maxBombs + 1;
            _testClass.Execute();
            Assert.Equal(expected, __player.maxBombs);
            var max = __player.Defaults.MaxBombs;
            __player.maxBombs = max;
            _testClass.Execute();
            Assert.Equal(max, __player.maxBombs);
        }

        [Fact]
        public void CanCallUndo()
        {
            var expected = __player.maxBombs - 1;
            _testClass.Undo();
            Assert.Equal(expected, __player.maxBombs);
            var min = __player.Defaults.MinBombs;
            __player.maxBombs = min;
            _testClass.Undo();
            Assert.Equal(min, __player.maxBombs);
        }
    }
}