using SignalRWebPack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.Iterator
{
    public class SessionIterator : IIterator<Session>
    {
        private List<Session> _queue;
        private int index;
        private bool nextReturnsCurrent;
        private readonly object __lock = new object();
        public bool HasNext => Peek() != null;
        public SessionIterator()
        {
            _queue = new List<Session>();
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

        public Session First()
        {
            lock (__lock)
            {
                return _queue.FirstOrDefault();
            }
        }

        public Session Last()
        {
            lock (__lock)
            {
                return _queue.LastOrDefault();
            }
        }

        public Session Next()
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

        public Session Peek()
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

        public void Add(Session item)
        {
            lock (__lock)
            {
                _queue.Add(item);
            }
        }

        public Session Remove(Session item)
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

        public Session RemoveFirst()
        {
            return Remove(First());
        }

        public Session RemoveLast()
        {
            return Remove(Last());
        }

        public SessionIterator Iterator()
        {
            index = 0;
            nextReturnsCurrent = true;
            return this;
        }

        SessionIterator IIterator<Session>.SessionIterator()
        {
            return Iterator();
        }

        public InputIterator InputIterator()
        {
            throw new NotImplementedException();
        }
        public MessageIterator MessageIterator()
        {
            throw new NotImplementedException();
        }
    }
}
