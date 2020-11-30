namespace SignalRWebPackTests.Patterns.Decorator
{
    using SignalRWebPack.Patterns.Decorator;
    using System;
    using Xunit;
    using SignalRWebPack.Models;

    public class BackgroundDecoratorTests
    {
        private GameObject Powerup_additionalType;
        private GameObject Powerup_bombTickType;

        public BackgroundDecoratorTests()
        {
            Powerup_additionalType = new Powerup(Powerup_type.AdditionalBomb,0, 0);
            Powerup_bombTickType = new Powerup(Powerup_type.BombTickDuration, 0, 0);
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new BackgroundDecorator(Powerup_additionalType);
            Assert.NotNull(instance);
            Assert.Contains("powerup_bomb_naked", instance.textures);
            instance = new BackgroundDecorator(Powerup_bombTickType);
            Assert.NotNull(instance);
            Assert.Contains("powerup_bombTick_naked", instance.textures);
        }

        [Fact]
        public void CannotConstructWithNullGameObject()
        {
            Assert.Throws<ArgumentNullException>(() => new BackgroundDecorator(default(GameObject)));
        }
    }
}