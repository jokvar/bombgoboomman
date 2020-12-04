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
        public virtual void AddById(string id, T item, bool forceThreadSafe = true, bool logResult = false) { }
    }

    public abstract class TemplateClass<T> : BaseTemplateClass<T> where T : IIterable
    {
        protected IIterator<T> queue;

        public virtual bool IdIsValid(string id)
        {
            return SessionManager.Instance.GetPlayerSession(id) != null;
        }
        public abstract bool ItemIsValid(T item);
        public abstract void Log(bool idIsValid, bool itemIsValid);
        public abstract void InjectConnectionIntoItem(string id, T item);
        public abstract void Lock();
        public abstract void Unlock();
        public abstract IIterator<T> Iterator();
        public sealed override void AddById(string id, T item, bool forceThreadSafe = true, bool logResult = false)
        {
            bool idIsValid = IdIsValid(id);
            bool itemIsValid = ItemIsValid(item);
            if (idIsValid && itemIsValid)
            {
                InjectConnectionIntoItem(id, item);
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

        /// <summary>
        /// Non-thread safe. Use FindById() instead.
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns></returns>
        public abstract T Find(string id);

        public T FindById(string id)
        {
            bool idIsValid = IdIsValid(id);
            if (idIsValid)
            {
                try
                {
                    Lock();
                    T result = Find(id);
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
