namespace SignalRWebPackTests.Patterns.Command
{
    using SignalRWebPack.Patterns.Command;
    using System;
    using Xunit;
    using SignalRWebPack.Models;

    public class PowerupCommandTests
    {
        private class TestPowerupCommand : PowerupCommand
        {
            public TestPowerupCommand(Player player) : base(player)
            {
            }

            public override void Execute()
            {
            }

            public override void Undo()
            {
            }

            public void Kill()
            {
                _player.lives = -1;
            }
        }

        private TestPowerupCommand _testClass;
        private Player _player;

        public PowerupCommandTests()
        {
            _player = new Player("TestValue764592158", "TestValue404138553", 1545824815, 1659117088);
            _player.lives = 1;
            _testClass = new TestPowerupCommand(_player);
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new TestPowerupCommand(_player);
            Assert.NotNull(instance);
        }

        [Fact]
        public void CannotConstructWithNullPlayer()
        {
            Assert.Throws<ArgumentNullException>(() => new TestPowerupCommand(default(Player)));
        }

        [Fact]
        public void TestIsPlayerAlive()
        {
            Assert.IsType<bool>(_testClass.IsPlayerAlive);
            Assert.True(_testClass.IsPlayerAlive);
            _testClass.Kill();
            Assert.False(_testClass.IsPlayerAlive);
        }
    }
}