namespace SignalRWebPackTests.Patterns.Observer
{
    using SignalRWebPack.Patterns.Observer;
    using SignalRWebPack.Models;
    using System;
    using Xunit;
    using Moq;

    public class LivesObserverTests
    {
        private LivesObserver _testClass;

        public LivesObserverTests()
        {
            _testClass = new LivesObserver();
        }

        [Fact]
        public void CanConstruct()
        {
            var instance = new LivesObserver();
            Assert.NotNull(instance);
        }

        //[Fact]
        //public void CanCallUpdate()
        //{
        //    Session testSession = new Session();
        //    Player p = new Player("test", "id", 2, 2);
        //    Player pp = new Player("testtest", "id", 2, 2);
        //    testSession.Players.Add(p);
        //    testSession.Players.Add(pp);
        //    testSession.LastPlayerDamaged = p;
        //    testSession.Notify();
        //    Tuple<string, Message> message = testSession.ReadOneMessage();
        //    Message m = message.Item2;
        //    String s = m.Content;
        //    Assert.Equal("<b>test</b> has taken damage! Remaining lives: <b>3</b>", s);
        //}

        [Fact]
        public void CannotCallUpdateWithNoPlayers()
        {
            Session testSession = new Session();
            Assert.Throws<NullReferenceException>(() => testSession.Notify());
        }
        [Fact]
        public void CannotCallUpdateWithNoLastDamaged()
        {
            Session testSession = new Session();
            Player p = new Player("test", "id", 2, 2);
            testSession.Players.Add(p);
            Assert.Throws<NullReferenceException>(() => testSession.Notify());
        }
        [Fact]
        public void CanCallHasGameEndend()
        {
            Session testSession = new Session();
            Player p = new Player("", "id", 2, 2);
            p.lives = 0;
            testSession.Players.Add(p);
            testSession.LastPlayerDamaged = p;
            testSession.Notify();
            Assert.True(testSession.HasGameEnded);
        }
    }
}