using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Models;

namespace SignalRWebPack.Logic
{


    public class InputQueueManager 
    {
        private static readonly InputQueueManager instance = new InputQueueManager();
        private List<Input> inputQueue;
        private InputQueueManager()
        {
            inputQueue = new List<Input>();
        }

        public static InputQueueManager Instance => instance;

        public void AddToInputQueue(string _connectionId, PlayerAction _action)
        {
            inputQueue.Add(new Input(_connectionId, _action));
        }

        public Tuple<string, PlayerAction> ReadOne()
        {
            if (inputQueue.Count == 0) //if empty
            {
                return null;
            }
            Tuple<string, PlayerAction> input = inputQueue[0].Get();
            inputQueue.RemoveAt(0);
            return input;
        }

        protected class Input
        {
            string connectionId;
            PlayerAction action;

            public Input(string _connectionId, PlayerAction _action)
            {
                connectionId = _connectionId;
                action = _action;
            }

            public Tuple<string, PlayerAction> Get()
            {
                return new Tuple<string, PlayerAction>(connectionId, action);
            }
        }
    }
}
