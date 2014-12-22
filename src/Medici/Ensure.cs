using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Medici
{
    public static class Ensure
    {
        public static Param<T> That<T>(T value, string name)
        {
            return new StringParam<T>(value, name);
        }

        public static Param<T> That<T>(T value, Expression<Func<T>> reference)
        {
            return new ExpressionParam<T>(value, reference);
        }

        internal static class ExFactory
        {
            [DebuggerStepThrough]
            internal static Exception Create(string message, string paramName, params object[] args)
            {
                return new ArgumentException(String.Format(message, args));
            }
        }
    }
}
