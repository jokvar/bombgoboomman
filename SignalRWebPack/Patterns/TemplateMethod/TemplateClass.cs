using SignalRWebPack.Logic;
using SignalRWebPack.Models;
using SignalRWebPack.Patterns.Iterator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SignalRWebPack.Patterns.TemplateMethod
{
    public abstract class BaseTemplateClass<T> where T : IIterable
    {
        public virtual void AddById(string connectionId, T item, bool forceThreadSafe = true, bool logResult = false) { }
    }

    public abstract class TemplateClass<T> : BaseTemplateClass<T> where T : IIterable
    {
        protected IIterator<T> queue;

        public virtual bool IdIsValid(string connectionId)
        {
            return SessionManager.Instance.GetPlayerSession(connectionId) != null;
        }
        public abstract bool ItemIsValid(T item);
        public abstract void Log(bool idIsValid, bool itemIsValid);
        public abstract void InjectConnectionIntoItem(string connectionId, T item);
        public abstract void Lock();
        public abstract void Unlock();
        public abstract IIterator<T> Iterator();
        public sealed override void AddById(string connectionId, T item, bool forceThreadSafe = true, bool logResult = false)
        {
            bool idIsValid = IdIsValid(connectionId);
            bool itemIsValid = ItemIsValid(item);
            if (idIsValid && itemIsValid)
            {
                InjectConnectionIntoItem(connectionId, item);
                if (forceThreadSafe) Lock();
                queue.Add(item);
                if (forceThreadSafe) Unlock();
            }
            if (logResult) Log(idIsValid, itemIsValid);
        }

        public abstract T ReadOne();

        public void FlushQueue(int count = 0, bool removeLastEntriesFirst = false)
        {
            Lock();
            if (count == 0)
            {
                queue.Empty();
            }
            for (;count > 0; count--)
            {
                if (removeLastEntriesFirst)
                {
                    queue.RemoveLast();
                }
                else
                {
                    queue.RemoveFirst();
                }
            }
            Unlock();
        }

        public abstract T Find(string connectionId);

        public T FindById(string connectionId)
        {
            bool idIsValid = IdIsValid(connectionId);
            if (idIsValid)
            {
                try
                {
                    Lock();
                    T result = Find(connectionId);
                    return result;
                }
                catch (Exception) {}
                finally
                {
                    Unlock();
                }
            }
            return default;
        }
    }
}
