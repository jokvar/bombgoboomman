namespace SignalRWebPack.Patterns.Iterator
{
    public interface IIterator<T> where T : IIterable
    {
        public int GetCount();
        public T First();
        public T Last();
        public T Next();
        public T Peek();
        public void Add(T item);
        public T Remove(T item);
        public abstract T RemoveFirst();
        public abstract T RemoveLast();
        public void Empty();
        public object GetLock();
        public abstract InputIterator InputIterator();
        public abstract SessionIterator SessionIterator();
        public abstract MessageIterator MessageIterator();
    }
}
