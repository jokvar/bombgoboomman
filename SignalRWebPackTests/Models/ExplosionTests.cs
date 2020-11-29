namespace SignalRWebPackTests.Models
{
    using SignalRWebPack.Models;
    using System;
    using Xunit;
    using SignalRWebPack.Logic;
    using System.Collections.Generic;
    using System.Linq;

    public class ExplosionTests
    {
        private Explosion _testClass;
        private int _x;
        private int _y;
        private bool _isExpired;
        private int _explosionSizeMultiplier;
        private List<ExplosionCell> explosions { get; set; }
        private static Session session = SessionManager.Instance.GetSession(SessionManager.Instance.ActiveSessionCode);
        private Map gameMap = session.Map;
        private List<Powerup> powerups = session.powerups;
        private List<Player> players = session.Players;

        public ExplosionTests()
        {
            _x = 1;
            _y = 2;
            _isExpired = false;
            _explosionSizeMultiplier = 5;
            _testClass = new Explosion(_x, _y, _isExpired, _explosionSizeMultiplier);
            explosions = new List<ExplosionCell>();
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new Explosion(_x, _y, _isExpired, _explosionSizeMultiplier);
            Assert.NotNull(instance);
        }

        [Fact]
        public void CanCallSpawnExplosions()
        {
            var firstBombX = 1;
            var firstBombY = 2;

            var secondBombX = 1;
            var secondBombY = 1;

            session.RegisterPlayer(new Player("Player1", "test1", firstBombX, firstBombY));

            var firstBoxX = 3;
            var firstBoxY = 1;
            gameMap.tiles[15 * firstBoxY + firstBoxX] = new Box() { x = firstBoxX, y = firstBoxY };

            var secondBoxX = 1;
            var secondBoxY = 3;
            gameMap.tiles[15 * secondBoxY + secondBoxX] = new Box() { x = secondBoxX, y = secondBoxY };

            var firstEmptyX = 4;
            var firstEmptyY = 1;
            gameMap.tiles[15 * firstEmptyY + firstEmptyX] = new EmptyTile() { x = firstEmptyX, y = firstEmptyY };

            var secondEmptyX = 1;
            var secondEmptyY = 4;
            gameMap.tiles[15 * secondEmptyY + secondEmptyX] = new EmptyTile() { x = secondEmptyX, y = secondEmptyY };

            var firstPowerX = 2;
            var firstPowerY = 1;
            Powerup powerup = new Powerup(Powerup_type.AdditionalBomb, firstPowerX, firstPowerY);
            powerups.Add(powerup);

            players[0].PlaceBomb();

            players[0].x = secondBombX;
            players[0].y = secondBombY;
            players[0].maxBombs++;

            players[0].PlaceBomb();

            players[0].bombs[0].hasExploded = true;
            _testClass.SpawnExplosions(firstBombX, firstBombY);
            explosions = _testClass.GetExplosionCells();
            explosions.AddRange(players[0].bombs[1].explosion.GetExplosionCells());

            //asserting whether collisions resolved correctly
            //checking whether center of explosion spawned
            var explosionTile = explosions.Where(e => e.x == firstBombX && e.y == firstBombY).FirstOrDefault();
            Assert.NotNull(explosionTile);

            //checking whether explosion detonated the second bomb
            explosionTile = explosions.Where(e => e.x == secondBombX && e.y == secondBombY).FirstOrDefault();
            Assert.NotNull(explosionTile);

            //checking whether explosion destroyed powerup
            var powerupTile = powerups.Where(e => e.x == firstPowerX && e.y == firstPowerY).FirstOrDefault();
            Assert.Null(powerupTile);

            //checking whether explosion spread to the first box and destroyed it
            explosionTile = explosions.Where(e => e.x == firstBoxX && e.y == firstBoxY).FirstOrDefault();
            Assert.NotNull(explosionTile);

            var boxTile = gameMap.tiles[15 * firstBoxY + firstBoxX];
            Assert.IsType<EmptyTile>(boxTile);

            //checking whether explosion spread to the second box and destroyed it
            explosionTile = explosions.Where(e => e.x == secondBoxX && e.y == secondBoxY).FirstOrDefault();
            Assert.NotNull(explosionTile);

            boxTile = gameMap.tiles[15 * secondBoxY + secondBoxX];
            Assert.IsType<EmptyTile>(boxTile);


            //checking whether explosion spread past the boxes
            explosionTile = explosions.Where(e => e.x == firstEmptyX && e.y == firstEmptyY).FirstOrDefault();
            Assert.Null(explosionTile);

            explosionTile = explosions.Where(e => e.x == secondEmptyX && e.y == secondEmptyY).FirstOrDefault();
            Assert.Null(explosionTile);

            //checking whether explosion spread to wall
            explosionTile = explosions.Where(e => e.x == 2 && e.y == 2).FirstOrDefault();
            Assert.Null(explosionTile);
        }

        [Fact]
        public void isExpiredIsInitializedCorrectly()
        {
            Assert.Equal(_isExpired, _testClass.isExpired);
        }

        [Fact]
        public void CanSetAndGetisExpired()
        {
            var testValue = false;
            _testClass.isExpired = testValue;
            Assert.Equal(testValue, _testClass.isExpired);
        }

        [Fact]
        public void explosionSizeMultiplierIsInitializedCorrectly()
        {
            Assert.Equal(_explosionSizeMultiplier, _testClass.explosionSizeMultiplier);
        }

        [Fact]
        public void CanSetAndGetexplosionSizeMultiplier()
        {
            var testValue = 822679544;
            _testClass.explosionSizeMultiplier = testValue;
            Assert.Equal(testValue, _testClass.explosionSizeMultiplier);
        }

        [Fact]
        public void CanSetAndGetsize()
        {
            var testValue = 1187283400;
            _testClass.size = testValue;
            Assert.Equal(testValue, _testClass.size);
        }
    }
}