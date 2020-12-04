using SignalRWebPack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.Iterator
{
    public class MessageIterator : IIterator<Message>
    {
        private List<Message> _queue;
        private int index;
        private bool nextReturnsCurrent;
        private readonly object __lock = new object();
        public bool HasNext => Peek() != null;
        public MessageIterator()
        {
            _queue = new List<Message>();
            index = 0;
            nextReturnsCurrent = true;
        }
        public void Add(Message item)
        {
            lock (__lock)
            {
                _queue.Add(item);
            }
        }

        public void Empty()
        {
            lock (__lock)
            {
                _queue.Clear();
            }
        }

        public Message First()
        {
            lock (__lock)
            {
                return _queue.FirstOrDefault();
            }
        }

        public int GetCount()
        {
            lock (__lock)
            {
                return _queue.Count;
            }
        }

        public object GetLock() => __lock;

        public Message Last()
        {
            lock (__lock)
            {
                return _queue.LastOrDefault();
            }
        }

        public Message Next()
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

        public Message Peek()
        {
            return First();
        }

        public Message Remove(Message item)
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

        public Message RemoveFirst()
        {
            return Remove(First());
        }

        public Message RemoveLast()
        {
            return Remove(Last());
        }
        public MessageIterator Iterator()
        {
            index = 0;
            nextReturnsCurrent = true;
            return this;
        }
        MessageIterator IIterator<Message>.MessageIterator()
        {
            return Iterator();
        }
        public SessionIterator SessionIterator()
        {
            throw new NotImplementedException();
        }
        public InputIterator InputIterator()
        {
            throw new NotImplementedException();
        }
    }
}
