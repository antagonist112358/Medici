using System;

namespace Medici
{
    public sealed class WeakReference<T> where T : class
    {
        private readonly WeakReference _refInternal;
        private readonly Type _refType;

        public WeakReference(T item, bool trackResurrection = false)
        {
            Ensure.That(item, "item").IsNotNull();

            _refInternal = new WeakReference(item, trackResurrection);
            _refType = typeof(T);
        }

        public new Type GetType()
        {
            return _refType;
        }

        public override string ToString()
        {
            return _refInternal.Target.ToString();
        }

        public bool IsAlive
        {
            get { return _refInternal.IsAlive; }
        }

        public T Target
        {
            get { return (T)_refInternal.Target; }
        }

        public bool TrackResurrection
        {
            get { return _refInternal.TrackResurrection; }
        }

        public static implicit operator WeakReference<T>(T item)
        {
            return new WeakReference<T>(item, false);
        }
    }
}
