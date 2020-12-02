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
        private readonly object __lock = new object();
        public InputIterator()
        {
            _queue = new List<PlayerAction>();
            index = 0;
        }

        public InputIterator Iterator()
        {
            index = 0;
            return this;
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

        //for(Iterator i=var.iterator(); i.hasNext(); ) {
        //    Object obj = i.next();
        //}
        public PlayerAction Next()
        {
            lock (__lock)
            {
                index++;
                if (index >= _queue.Count)
                {
                    index = 0;
                    return null;
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
                    return null;
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
                _queue.Remove(item);
                return item;
            }
        }

        public void Empty()
        {
            lock (__lock)
            {
                _queue.Clear();
            }
        }
    }
}
