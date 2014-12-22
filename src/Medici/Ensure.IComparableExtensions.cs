using System;

namespace Medici
{
    public static class EnsureComparableExtensions
    {
        public static Param<T> IsInRangeInclusive<T>(this Param<T> param, T from, T to) where T : IComparable
        {
            if (param.NotNull && (param.Value.CompareTo(from) < 0 || param.Value.CompareTo(to) > 0))
            {
                throw ExGen.Build<ArgumentOutOfRangeException>(param.Name, param.Value,
                    String.Format("Argument value must be in the range {0} to {1} (inclusive)", from, to));
            }

            return param;
        }

        public static Param<T> IsInRangeExclusive<T>(this Param<T> param, T from, T to) where T : IComparable
        {
            if (param.NotNull && (param.Value.CompareTo(from) <= 0 || param.Value.CompareTo(to) >= 0))
            {
                throw new ArgumentOutOfRangeException(param.Name, param.Value, String.Format("Argument value must be in the range {0} to {1} (exclusive)", from, to));
            }

            return param;
        }

        public static Param<T> IsGreaterThan<T>(this Param<T> param, T comparedTo) where T : IComparable
        {
            if (param.NotNull && param.Value.CompareTo(comparedTo) <= 0)
            {
                throw new ArgumentOutOfRangeException(param.Name, param.Value, String.Format("Argument value must be greater than {0}", comparedTo));
            }

            return param;
        }

        public static Param<T> IsLessThan<T>(this Param<T> param, T comparedTo) where T : IComparable
        {
            if (param.NotNull && param.Value.CompareTo(comparedTo) > 0)
            {
                throw new ArgumentOutOfRangeException(param.Name, param.Value, String.Format("Argument value must be less than {0}", comparedTo));
            }

            return param;
        }
    }
}
