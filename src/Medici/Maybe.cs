using System;
using System.Collections.Generic;

namespace Medici
{
    public struct Maybe<T> : IEnumerable<T>
    {
        #region Iterator Implementation

        private class PossibleEnumerator : IEnumerator<T>
        {
            private readonly Maybe<T> _possible;
            private bool _hasMoved;

            public PossibleEnumerator(ref Maybe<T> possible)
            {
                Current = default(T);
                _possible = possible;
            }

            public T Current { get; private set; }

            public void Dispose()
            {
                // No-op
            }

            object System.Collections.IEnumerator.Current
            {
                get { return Current; }
            }

            public bool MoveNext()
            {
                if (_hasMoved)
                    return false;

                _hasMoved = true;

                if (_possible.HasError) return false;

                Current = _possible.Value;
                return true;
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        private readonly T _value;
        private readonly Exception _error;
        private readonly bool _hasError;

        public Maybe(T value)
        {
            _value = value;
            _error = null;
            _hasError = false;
        }

        public Maybe(Exception error)
        {
            _error = error;
            _value = default(T);
            _hasError = true;
        }

        public bool HasError
        {
            get { return _hasError; }
        }

        public bool HasValue
        {
            get { return !_hasError; }
        }

        public Exception Error
        {
            get { return _error; }
        }

        [System.Diagnostics.Contracts.Pure]
        public T Value
        {
            get
            {
                if (_hasError)
                    throw ExGen.Build<MediciException>(ErrorMessages.MaybeDoesNotHaveValue, typeof (T).Name);

                return _value;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new PossibleEnumerator(ref this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new PossibleEnumerator(ref this);
        }
    }

    public static class Maybe
    {
        public static Maybe<T> Try<T>(Func<T> generator)
        {
            try
            {
                return new Maybe<T>(generator());
            }
            catch (Exception ex)
            {
                return new Maybe<T>(ex);
            }
        }
    }

}
