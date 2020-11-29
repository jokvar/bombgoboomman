namespace SignalRWebPackTests.Patterns.Strategy
{
    using SignalRWebPack.Patterns.Strategy;
    using System;
    using Xunit;
    using System.Collections.Generic;
    using SignalRWebPack.Models;
    using SignalRWebPack.Patterns.Command;

    public class CollisionStrategyTests
    {
        private class TestCollisionStrategy : CollisionStrategy
        {
            public override void ExplosionCollisionStrategy(object collisionTarget, List<ExplosionCell> explosions, DateTime explodedAt, List<Powerup> powerupList)
            {
            }

            public override void PlayerCollisionStrategy(Player player, object collisionTarget, List<Powerup> collisionList, PowerupInvoker powerupInvoker)
            {
            }
        }

        private TestCollisionStrategy _testClass;

        public CollisionStrategyTests()
        {
            _testClass = new TestCollisionStrategy();
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new TestCollisionStrategy();
            Assert.NotNull(instance);
        }
    }
}