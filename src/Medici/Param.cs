using System;
using System.Linq.Expressions;

namespace Medici
{
    public abstract class Param<T>
    {
        private readonly T _value;

        internal Param(T value)
        {
            _value = value;
        }

        public T Value { get { return _value; } }

        public abstract string Name { get; }

        internal bool NotNull
        {
            get
            {
                if (typeof (T).IsValueType)
                    return false;
                else                
                    return (_value as object) != null;
            }
        }
    }

    internal sealed class StringParam<T> : Param<T>
    {
        private readonly string _name;

        public StringParam(T value, string name)
            : base(value)
        {
            if (String.IsNullOrEmpty(name)) { throw new ArgumentException(ErrorMessages.ParameterNameMissing, "name"); }
            _name = name;
        }

        public override string Name
        {
            get { return _name; }
        }
    }

    internal sealed class ExpressionParam<T> : Param<T>
    {
        private readonly Expression<Func<T>> _nameGenerator;

        public ExpressionParam(T value, Expression<Func<T>> nameFunc)
            : base(value)
        {
            if (nameFunc == null) { throw new ArgumentException(ErrorMessages.NameGeneratorExpressionMissing, "nameFunc"); }
            _nameGenerator = nameFunc;
        }

        public override string Name
        {
            get { return GetParameterName(_nameGenerator); }
        }

        private static string GetParameterName(LambdaExpression reference)
        {
            var member = (MemberExpression)reference.Body;
            return member.Member.Name;
        }
    }

    internal sealed class ArgumentTypeParam<T> : Param<Type> where T : class
    {
        private readonly Param<T> _wrappedParam;

        public ArgumentTypeParam(Param<T> wrapped)
            : base(wrapped.Value.GetType())
        {
            _wrappedParam = wrapped;
        }

        public override string Name
        {
            get { return _wrappedParam.Name; }
        }
    }
}