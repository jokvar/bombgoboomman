namespace SignalRWebPackTests.Logic
{
    using SignalRWebPack.Logic;
    using System;
    using Xunit;
    using System.Collections.Generic;
    using SignalRWebPack.Models;
    using System.Linq;

    public class SessionManagerTests
    {

        public SessionManagerTests()
        {
        }

        //[Fact]
        //public void CanConstruct()
        //{
        //    var instance = new SessionManager();
        //    Assert.NotNull(instance);
        //}

        [Fact]
        public void CanCallGetPlayerSession()
        {
            var id = "TestValue1560000523";
            var name = "TestValue1757550007";
            var code = "code";
            var __player = new Player(name, id, 145340, 1453431);
            var session = SessionManager.Instance.GetSession(code);
            session.RegisterPlayer(__player, true);
            var result = SessionManager.Instance.GetPlayerSession(id);
            Assert.Equal(code, result.roomCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void CannotCallGetPlayerSessionWithInvalidId(string value)
        {
            Assert.Throws<ArgumentNullException>(() => SessionManager.Instance.GetPlayerSession(value));
        }

        [Fact]
        public void GetPlayerSessionPerformsMapping()
        {
            SessionManager.Instance.FlushSessions();
            var id = "TestValue1562120523";
            var name = "TestValue1893700007";
            var __player = new Player(name, id, 10, 11);
            SessionManager.Instance.GetSession(null).RegisterPlayer(__player, true);
            var session = SessionManager.Instance.GetPlayerSession(id);
            var result = session.Players.Where(p => p.id == id).FirstOrDefault();
            Assert.NotNull(result);
            Assert.Equal(name, result.name);
        }

        [Fact]
        public void CanCallIsPlayerAlive()
        {
            SessionManager.Instance.FlushSessions();
            var id = "TestValue1533333523";
            var name = "TestValue1113007";
            var __player = new Player(name, id, 10, 11) { lives = 1 };
            SessionManager.Instance.GetSession(null).RegisterPlayer(__player, true);
            Assert.True(SessionManager.Instance.IsPlayerAlive(id));
            __player.lives = 0;
            Assert.False(SessionManager.Instance.IsPlayerAlive(id));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void CannotCallIsPlayerAliveWithInvalidId(string value)
        {
            Assert.Throws<ArgumentNullException>(() => SessionManager.Instance.IsPlayerAlive(value));
        }

        [Fact]
        public void CanCallGetSession()
        {
            var code = "TestValue444441";
            var result = SessionManager.Instance.GetSession(code);
            Assert.IsType<Session>(result);
            Assert.Equal(code, result.roomCode);
        }

        [Fact]
        public void CanCallGenerateRoomCode()
        {
            var result = SessionManager.GenerateRoomCode();
            Assert.IsType<string>(result);
            Assert.Equal(4, result.Length);
        }

        [Fact]
        public void CanGetInstance()
        {
            var instance = SessionManager.Instance;
            Assert.IsType<SessionManager>(instance);
            Assert.Same(instance, SessionManager.Instance);
        }

        //[Fact]
        //public void CanGetAllSessionCodes()
        //{
        //    SessionManager.Instance.FlushSessions();
        //    List<string> codes = new List<string> { "T1", "T2", "T3" };
        //    SessionManager.Instance.GetSession(codes[0]);
        //    SessionManager.Instance.GetSession(codes[1]);
        //    SessionManager.Instance.GetSession(codes[2]);
        //    SessionManager.Instance.GetSession(codes[0]);
        //    Assert.IsType<List<string>>(SessionManager.Instance.AllSessionCodes);
        //    Assert.Equal(codes, SessionManager.Instance.AllSessionCodes);
        //}

        [Fact]
        public void CanSetAndGetActiveSessionCode()
        {
            var testValue = "TestValue1601492934";
            SessionManager.Instance.ActiveSessionCode = testValue;
            Assert.Equal(testValue, SessionManager.Instance.ActiveSessionCode);
        }

        //[Fact]
        //public void CanCallFlushSessions()
        //{
        //    SessionManager.Instance.FlushSessions();
        //    Assert.Empty(SessionManager.Instance.AllSessionCodes);
        //}
    }
}