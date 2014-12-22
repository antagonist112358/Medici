using System;

namespace Medici
{
    public static class OptionMatchExtensions
    {
        public static UnitCase<Option<T>> Some<T>(this UnitCase<Option<T>> option, Action<T> action)
        {
            new UnitCaseDecorator<Option<T>>(option, opt => !opt.IsEmpty, o => action(o.Value))
                .DoMatch();

            return option;
        }

        public static void None<T>(this UnitCase<Option<T>> option, Action action)
        {
            new UnitCaseDecorator<Option<T>>(option, opt => opt.IsEmpty, _ => action())
                .DoMatch();
        }

        public static ValueCase<Option<T>, V> Some<T, V>(this ValueCase<Option<T>, V> option, Func<T, V> func)
        {
            new ValueCaseDecorator<Option<T>, V>(option, opt => !opt.IsEmpty, o => func(o.Value))
                .DoMatch();

            return option;
        }

        public static void None<T, V>(this ValueCase<Option<T>, V> option, Func<V> func)
        {
            new ValueCaseDecorator<Option<T>, V>(option, opt => opt.IsEmpty, _ => func())
                .DoMatch();
        }
    }
}
