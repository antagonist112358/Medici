using System;

namespace Medici.Concurrency
{
    public interface IFuture
    {
        bool Cancel();

        bool Cancel(bool interruptThread);

        bool IsCancelled { get; }

        bool IsCompleted { get; }

        void Wait();

        bool Wait(int milliseconds);

        bool Wait(TimeSpan timeout);
    }

    public interface IFuture<T> : IFuture, IAwaitable<T>
    {
        Option<Maybe<T>> Value { get; }
    }
}
