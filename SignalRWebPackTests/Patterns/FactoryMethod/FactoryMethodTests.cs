using Xunit;
using SignalRWebPack.Patterns.FactoryMethod;
using System;
using System.Collections.Generic;
using System.Text;
using SignalRWebPack.Models;

namespace SignalRWebPackTests.Patterns.FactoryMethod
{
    public class ExplosionTransportCreatorTests
    {
        [Fact()]
        public void FactoryMethodTest()
        {
            var creator = new ExplosionTransportCreator();
            var transportObj = creator.FactoryMethod();
            Assert.IsType<ExplosionTransport>(transportObj);
        }
    }
    public class BombTransportCreatorTests
    {
        [Fact()]
        public void FactoryMethodTest()
        {
            var creator = new BombTransportCreator();
            var transportObj = creator.FactoryMethod();
            Assert.IsType<BombTransport>(transportObj);
        }
    }
    public class PowerupTransportCreatorTests
    {
        [Fact()]
        public void FactoryMethodTest()
        {
            var creator = new PowerupTransportCreator();
            var transportObj = creator.FactoryMethod();
            Assert.IsType<PowerupTransport>(transportObj);
        }
    }

    public class BombTransportTests
    {
        [Fact()]
        public void PackTest()
        {
            Bomb gameObject = new Bomb(0, 0, 1, 1);
            BombTransport transportObject = new BombTransport();
            transportObject.Pack(gameObject);
            Assert.Equal(gameObject.texture, transportObject.texture);
            Assert.Equal(gameObject.x, transportObject.x);
            Assert.Equal(gameObject.y, transportObject.y);
            Player invalidGameObject = new Player("a", "a", 0, 0);
            Assert.Throws<ArgumentException>(() => transportObject.Pack(invalidGameObject));
        }
    }

    public class ExplosionTransportTests
    {
        [Fact()]
        public void PackTest()
        {
            ExplosionCell gameObject = new ExplosionCell(DateTime.Now, 0, 0);
            ExplosionTransport transportObject = new ExplosionTransport();
            transportObject.Pack(gameObject);
            Assert.Equal(gameObject.texture, transportObject.texture);
            Assert.Equal(gameObject.x, transportObject.x);
            Assert.Equal(gameObject.y, transportObject.y);
            Player invalidGameObject = new Player("a", "a", 0, 0);
            Assert.Throws<ArgumentException>(() => transportObject.Pack(invalidGameObject));
        }
    }
    public class PowerupTransportTests
    {
        [Fact()]
        public void PackTest()
        {
            Powerup gameObject = new Powerup(Powerup_type.AdditionalBomb, 1, 1);
            gameObject.textures = new List<string> { "a", "b" };
            PowerupTransport transportObject = new PowerupTransport();
            transportObject.Pack(gameObject);
            Assert.Equal(gameObject.texture, transportObject.texture);
            Assert.Equal(gameObject.x, transportObject.x);
            Assert.Equal(gameObject.y, transportObject.y);
            Assert.Equal(gameObject.textures, transportObject.textures);
            Player invalidGameObject = new Player("a", "a", 0, 0);
            Assert.Throws<ArgumentException>(() => transportObject.Pack(invalidGameObject));
        }
    }
    public class TransportObjectCreatorTests
    {
        private class TestTransportObjectCreator : TransportObjectCreator
        {
            public override ITransportObject FactoryMethod()
            {
                return new PowerupTransport();
            }
        }

        private TestTransportObjectCreator _testClass;

        public TransportObjectCreatorTests()
        {
            _testClass = new TestTransportObjectCreator();
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new TestTransportObjectCreator();
            Assert.NotNull(instance);
        }

        [Fact]
        public void CannotCallPackWithNullGameObject()
        {
            Assert.Throws<ArgumentNullException>(() => _testClass.Pack(default(GameObject)));
        }

        [Fact]
        public void PackPerformsMapping()
        {
            var gameObject = new Powerup(Powerup_type.AdditionalBomb, 1, 1);
            var result = _testClass.Pack(gameObject);
            Assert.Equal(gameObject.texture, result.texture);
            Assert.Equal(gameObject.x, result.x);
            Assert.Equal(gameObject.y, result.y);
        }
    }
}
