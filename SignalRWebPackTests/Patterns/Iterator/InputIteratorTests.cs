namespace SignalRWebPackTests.Patterns.Iterator
{
    using SignalRWebPack.Patterns.Iterator;
    using System;
    using Xunit;
    using SignalRWebPack.Models;

    public class InputIteratorTests
    {
        private InputIterator _testClass;

        public InputIteratorTests()
        {
            _testClass = new InputIterator();
        }

        [Fact]
        //for(Iterator i=var.iterator(); i.hasNext(); ) {
        //    Object obj = i.next();
        //}
        public void TestIteration()
        {
            var InputIterator = new InputIterator();
            InputIterator.Add(new PlayerAction(ActionEnums.Down, "down"));
            InputIterator.Add(new PlayerAction(ActionEnums.Up, "up"));
            for (InputIterator i = InputIterator.Iterator(); i.HasNext;)
            {
                PlayerAction action = i.Next();
                var id = action.PlayerId;
            }
            var test = InputIterator.RemoveFirst();
            Assert.NotNull(test);
        }

    }
}