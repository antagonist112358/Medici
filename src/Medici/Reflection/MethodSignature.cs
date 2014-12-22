using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Medici.Reflection
{
    public sealed class MethodSignature : IEquatable<MethodSignature>
    {
        private readonly Type _returnType;
        private readonly IList<Type> _parameters;
        private readonly int _hashCode;

        internal MethodSignature(MethodInfo methodInfo)
        {
            Ensure.That(methodInfo, "methodInfo").IsNotNull();

            _returnType = methodInfo.ReturnType;
            _parameters = methodInfo.GetParameters().Select(pi => pi.ParameterType).ToList();
            _hashCode = CombineHashcodes(_returnType, _parameters);
        }

        public Type ReturnType
        {
            get { return _returnType; }
        }

        public IEnumerable<Type> ParameterTypes
        {
            get { return _parameters; }
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MethodSignature)) { return false; }
            return Equals(obj as MethodSignature);
        }

        public bool Equals(MethodSignature other)
        {
            if (other == null) { return false; }
            return (this._hashCode == other._hashCode);
        }

        private static int CombineHashcodes(Type retType, IEnumerable<Type> paramTypes)
        {
            int code = 2147483647 ^ retType.GetHashCode();
            int i = 1;

            unchecked
            {
                foreach (var type in paramTypes)
                {
                    code ^= (code * i++) + type.GetHashCode();
                }

                code *= i;
            }

            return code;
        }

        public static bool operator ==(MethodSignature left, MethodSignature right)
        {
            if (left == null)
            {
                return right == null;
            }

            return (left.Equals(right));
        }

        public static bool operator !=(MethodSignature left, MethodSignature right)
        {
            if (left == null)
            {
                return right != null;
            }

            return (!left.Equals(right));
        }
    }
}
