using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalRWebPack.Logic;
using SignalRWebPack.Models;

namespace SignalRWebPack.Patterns.Singleton
{
    public class InputQueueManager 
    {
        private List<Input> inputQueue;
        private readonly object queueLock = new object();
        private InputQueueManager()
        {
            inputQueue = new List<Input>();
        }

        public static InputQueueManager Instance { get; } = new InputQueueManager();

        public void AddToInputQueue(string _connectionId, PlayerAction _action)
        {
            if (SessionManager.Instance.IsPlayerAlive(_connectionId))
            {
                Input input = new Input(_connectionId, _action);
                lock (queueLock)
                {
                    inputQueue.Add(input);
                }
            }         
        }

        public Tuple<string, PlayerAction> ReadOne()
        {
            Tuple<string, PlayerAction> input;
            lock (queueLock)
            {
                if (inputQueue.Count == 0) //if empty
                {
                    return null;
                }
                input = inputQueue[0].Get();
                inputQueue.RemoveAt(0);
            }
            return input;
        }

        public void FlushInputQueue()
        {
            inputQueue = new List<Input>();
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
