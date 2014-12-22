using System.Collections.Generic;

namespace Medici.Collections
{
    public interface IQueue<T> : IEnumerable<T>
    {
        int Count { get; }

        bool Contains(T item);

        bool IsReadOnly { get; }

        void Enqueue(T item);
        
        T Peek();
        
        T Dequeue();

        void Clear();
        
        void CopyTo(T[] array, int arrayIndex);
    }
}
