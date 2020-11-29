namespace SignalRWebPackTests.Models
{
    using SignalRWebPack.Models;
    using System;
    using Xunit;

    public class MapTests
    {
        private Map _testClass;

        public MapTests()
        {
            _testClass = new Map();
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new Map();
            Assert.NotNull(instance);
        }

        [Fact]
        public void CanSetAndGetname()
        {
            var testValue = "TestValue64781519";
            _testClass.name = testValue;
            Assert.Equal(testValue, _testClass.name);
        }

        [Fact]
        public void CanSetAndGetauthor()
        {
            var testValue = "TestValue774964464";
            _testClass.author = testValue;
            Assert.Equal(testValue, _testClass.author);
        }

        [Fact]
        public void CanSetAndGetcreationDate()
        {
            var testValue = new DateTime(430912638);
            _testClass.creationDate = testValue;
            Assert.Equal(testValue, _testClass.creationDate);
        }

        [Fact]
        public void CanSetAndGetthumbnail()
        {
            var testValue = "TestValue503647495";
            _testClass.thumbnail = testValue;
            Assert.Equal(testValue, _testClass.thumbnail);
        }

        [Fact]
        public void CanSetAndGettiles()
        {
            var testValue = new[] { new Tile(), new Tile(), new Tile() };
            _testClass.tiles = testValue;
            Assert.Equal(testValue, _testClass.tiles);
        }
    }
}