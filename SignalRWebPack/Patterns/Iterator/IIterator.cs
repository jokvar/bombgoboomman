namespace SignalRWebPack.Patterns.Iterator
{
    public interface IIterator<T> where T : IIterable
    {
        public bool HasNext => Peek() != null;
        public int Count => GetCount();
        public int GetCount();
        public T First();
        public T Last();
        public T Next();
        public T Peek();
        public void Add(T item);
        public T Remove(T item);
        public T RemoveFirst() => Remove(First());
        public T RemoveLast() => Remove(Last());
        public void Empty();
        public object GetLock();
    }
}
