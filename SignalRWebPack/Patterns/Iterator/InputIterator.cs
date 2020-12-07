using SignalRWebPack.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.Iterator
{
    public class InputIterator : IIterator<PlayerAction>
    {
        private List<PlayerAction> _queue;
        private int index;
        private bool nextReturnsCurrent;
        private readonly object __lock = new object();
        public bool HasNext => Peek() != null;
        public InputIterator()
        {
            _queue = new List<PlayerAction>();
            index = 0;
            nextReturnsCurrent = true;
        }

        public object GetLock() => __lock;

        public int GetCount()
        {
            lock (__lock)
            {
                return _queue.Count;
            }
        }

        public PlayerAction First()
        {
            lock (__lock)
            {
                return _queue.FirstOrDefault();
            }
        }

        public PlayerAction Last()
        {
            lock (__lock)
            {
                return _queue.LastOrDefault();
            }
        }

        public PlayerAction Next()
        {
            lock (__lock)
            {
                if (nextReturnsCurrent)
                {
                    nextReturnsCurrent = false;
                    return _queue[index];
                }
                index++;
                if (index >= _queue.Count)
                {
                    index = 0;
                    return default;
                }
                return _queue[index];
            }
        }

        public PlayerAction Peek()
        {
            lock (__lock)
            {
                if (index + 1 >= _queue.Count)
                {
                    index = 0;
                    return default;
                }
                return _queue[index + 1];
            }
        }

        public void Add(PlayerAction item)
        {
            lock (__lock)
            {
                _queue.Add(item);
            }
        }

        public PlayerAction Remove(PlayerAction item)
        {
            lock (__lock)
            {
                if (_queue.Remove(item))
                {
                    return item;
                }
                return default;
            }
        }

        public void Empty()
        {
            lock (__lock)
            {
                _queue.Clear();
            }
        }

        public PlayerAction RemoveFirst()
        {
            return Remove(First());
        }

        public PlayerAction RemoveLast()
        {
            return Remove(Last());
        }

        public InputIterator Iterator()
        {
            index = 0;
            nextReturnsCurrent = true;
            return this;
        }

        InputIterator IIterator<PlayerAction>.InputIterator()
        {
            return Iterator();
        }

        public SessionIterator SessionIterator()
        {
            throw new NotImplementedException();
        }

        public MessageIterator MessageIterator()
        {
            throw new NotImplementedException();
        }
    }
}
