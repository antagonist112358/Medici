using System;

namespace Medici
{
    public class ValueCase<T, V>
    {
        private readonly T _target;
        internal V Returned;
        internal bool HasMatched;

        internal ValueCase(T target)
        {
            _target = target;
            Returned = default(V);
            HasMatched = false;
        }

        internal T Value 
        {
            get { return _target; }
        }
    }

    public class UnitCase<T>
    {
        private readonly T _target;
        internal bool HasMatched;

        internal UnitCase(T target)
        {
            _target = target;
            HasMatched = false;
        }

        internal T Value 
        {
            get { return _target; }
        }

    }

    public sealed class UnitCaseDecorator<T>
    {
        private UnitCase<T> _case;
        private readonly Func<T, bool> _match;
        private readonly Action<T> _action;

        public UnitCaseDecorator(UnitCase<T> uCase, Func<T, bool> match, Action<T> action)
        {
            _case = uCase;
            _match = match;
            _action = action;
        }

        public void DoMatch()
        {
            // Skip if we have already matched
            if (_case.HasMatched)
                return;

            if (_match(_case.Value))
            {
                _action(_case.Value);
                _case.HasMatched = true;
            }
        }
    }

    public sealed class ValueCaseDecorator<T, V>
    {
        private ValueCase<T, V> _case;
        private readonly Func<T, bool> _match;
        private readonly Func<T, V> _func;

        public ValueCaseDecorator(ValueCase<T, V> vCase, Func<T, bool> match, Func<T, V> func)
        {
            _case = vCase;
            _match = match;
            _func = func;
        }

        public void DoMatch()
        {
            // Skip if we have already matched
            if (_case.HasMatched)
                return;

            if (_match(_case.Value))
            {
                _case.Returned = _func(_case.Value);
                _case.HasMatched = true;
            }
        }
    }

    public sealed class ConditionalUnitCase<T>
    {
        private readonly UnitCase<T> _case;
        private readonly Func<T, bool> _expr;

        internal ConditionalUnitCase(UnitCase<T> @case, Func<T, bool> expr)
        {
            _case = @case;
            _expr = expr;
        }

        public bool Check()
        {
            if (_case.HasMatched)
                return false;

            return _expr(_case.Value);
        }

        internal UnitCase<T> Case
        {
            get { return _case; }
        }
    }

    public static class CaseExtensions
    {        
        public static UnitCase<T> Then<T>(this ConditionalUnitCase<T> testCase, Action<T> action)
        {
            new UnitCaseDecorator<T>(testCase.Case, _ => testCase.Check(), action)
            .DoMatch();

            return testCase.Case;
        }

        public static ConditionalUnitCase<T> ValueIs<T>(this UnitCase<T> @case, T testValue) where T : struct
        {
            return new ConditionalUnitCase<T>(@case, t => (testValue.Equals(t)));
        }

        public static UnitCase<T> Case<T, U>(this UnitCase<T> input, Action<U> action) where T : class where U : class, T
        {
            new UnitCaseDecorator<T>(input, i => ((i as U) != null), a => action((U)a))
            .DoMatch();

            return input;
        }

        public static UnitCase<T> Case<T, U>(this UnitCase<T> input, Func<T, bool> whenClause, Action<U> action)
            where T : class
            where U : class, T
        {
            new UnitCaseDecorator<T>(input, i => ((i as U) != null) && whenClause(i), a => action((U)a))
            .DoMatch();

            return input;
        }


        public static void Default<T>(this UnitCase<T> defaultCase, Action action)
        {
            new UnitCaseDecorator<T>(defaultCase, _ => true, _ => action())
            .DoMatch();
        }
    }

    public static class PatternMatcher
    {
        public static void Match<T>(this Option<T> value, Action<UnitCase<Option<T>>> cases)
        {
            var uCase = new UnitCase<Option<T>>(value);
            cases(uCase);
            if (!uCase.HasMatched)
                throw ExGen.Build<MediciException>("Match not found in PatternMatch.");
        }

        public static V Match<T, V>(this Option<T> value, Action<ValueCase<Option<T>, V>> cases)
        {
            var match = new ValueCase<Option<T>, V>(value);
            cases(match);
            
            if (match.HasMatched)
                return match.Returned;
            
            throw ExGen.Build<MediciException>("Match not found in PatternMatch.");
        }

        public static void Match<T>(this T value, Action<UnitCase<T>> cases)
        {            
            var uCase = new UnitCase<T>(value);
            cases(uCase);
            if (!uCase.HasMatched)
                throw ExGen.Build<MediciException>("Match not found in PatternMatch.");
        }

    }

}
