namespace SignalRWebPackTests.Patterns.TemplateMethod
{
    using SignalRWebPack.Patterns.TemplateMethod;
    using System;
    using Xunit;
    using SignalRWebPack.Models;
    using SignalRWebPack.Logic;

    public class TemplateInputManagerTests
    {
        private TemplateInputManager<PlayerAction> _testClass;

        public TemplateInputManagerTests()
        {
            _testClass = new TemplateInputManager<PlayerAction>();
        }

        [Fact]
        public void Testmanager()
        {
            SessionManager.Instance.GetSession(null).RegisterPlayer(new Player("a", "down", 0, 0));
            SessionManager.Instance.GetSession(null).RegisterPlayer(new Player("a", "up", 0, 0));
            var templateInputManager = new TemplateInputManager<PlayerAction>();
            templateInputManager.AddById("down", new PlayerAction(ActionEnums.Down));
            templateInputManager.AddById("up", new PlayerAction(ActionEnums.Up));
            var result = templateInputManager.FindById("up");
            Assert.Equal(ActionEnums.Up, result.action);
            Assert.Equal(2, templateInputManager.StackSize);
            var result2 = templateInputManager.ReadOne();
            Assert.IsType<PlayerAction>(result2);
            Assert.NotNull(result2);
            Assert.Equal(1, templateInputManager.StackSize);
        }
    }
}