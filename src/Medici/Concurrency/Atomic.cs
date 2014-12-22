using System;

namespace Medici.Concurrency
{
    public sealed class Atomic<T> where T : class
    {
        private T _item;
        private volatile bool _isNull = true;
        private readonly object _lock = new object();

        public Atomic() { }

        public Atomic(T item)
        {
            _isNull = (item == null);
            _item = item;
        }

        public bool IsNull
        {
            get { return _isNull; }
        }

        public T Get()
        {
            lock (_lock)
            {
                return _item;
            }
        }

        public void Set(T value)
        {
            lock (_lock)
            {
                _isNull = (value == null);
                _item = value;
            }
        }

        public void Update<TOutput>(Func<T, TOutput> updater) where TOutput : class, T
        {
            lock (_lock)
            {
                var value = updater(_item);
                _isNull = (value == null);
                _item = (T)value;
            }
        }

    }

}
