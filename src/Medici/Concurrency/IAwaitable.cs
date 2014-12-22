using System;

namespace Medici.Concurrency
{
    public interface IAwaitable
    {
        void Await();

        bool Await(int timeoutMilliseconds);

        bool Await(TimeSpan timeout);
    }

    public interface IAwaitable<T> : IAwaitable
    {
        new T Await();

        new Option<T> Await(int timeoutMilliseconds);

        new Option<T> Await(TimeSpan timeout);
    }
}
