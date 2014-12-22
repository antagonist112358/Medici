using System;

namespace Medici
{
    public static class MaybeMatchExtensions
    {
        public static UnitCase<Maybe<T>> Result<T>(this UnitCase<Maybe<T>> maybe, Action<T> action)
        {
            new UnitCaseDecorator<Maybe<T>>(maybe, opt => opt.HasValue, o => action(o.Value))
            .DoMatch();

            return maybe;
        }

        public static void Error<T>(this UnitCase<Maybe<T>> maybe, Action<Exception> action)
        {
            new UnitCaseDecorator<Maybe<T>>(maybe, m => m.HasError, o => action(o.Error))
            .DoMatch();
        }
    }
}
