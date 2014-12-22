using System;

namespace Medici
{
    public static class TupleExtensions
    {
        public static void To<T1>(this Tuple<T1> tuple, out T1 t1)
        {
            Ensure.That(tuple, "tuple").IsNotNull();
            t1 = tuple.Item1;
        }
        public static void To<T1, T2>(this Tuple<T1, T2> tuple, out T1 t1, out T2 t2)
        {
            Ensure.That(tuple, "tuple").IsNotNull();
            t1 = tuple.Item1;
            t2 = tuple.Item2;
        }

        public static void To<T1, T2, T3>(this Tuple<T1, T2, T3> tuple, out T1 t1, out T2 t2, out T3 t3)
        {
            Ensure.That(tuple, "tuple").IsNotNull();
            t1 = tuple.Item1;
            t2 = tuple.Item2;
            t3 = tuple.Item3;
        }

    }
}
