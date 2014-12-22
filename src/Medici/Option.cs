using System;

namespace Medici
{
    public sealed class Option
    {
        public static readonly Option None = new Option();

        private Option() { }

        public static Option<T> Some<T>(T t)
        {
            return Option<T>.Some(t);
        }
    }

    public struct Option<T>
    {
        private readonly bool _hasValue;
        private readonly T _value;

        private Option(T value)
        {
            _hasValue = true;
            _value = value;
        }

        public bool IsEmpty
        {
            get { return !_hasValue; }
        }

        public T GetOrElse(Func<T> elseFunc)
        {
            return IsEmpty ? elseFunc() : _value;
        }

        public T Value
        {
            get
            {
                if (_hasValue)                    
                    return _value;

                throw ExGen.Build<MediciException>("Tried to get value in empty Option<{0}>", typeof(T).Name);
            }
        }

        public static Option<T> Empty
        {
            get
            {
                return new Option<T>();
            }
        }

        public static Option<T> Some(T t)
        {
            return new Option<T>(t);
        }

        public static implicit operator Option<T>(Option empty)
        {
            return Empty;
        }
    }
}
