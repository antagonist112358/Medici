using System;

namespace Medici
{
    public static class EnsureObjectExtensions
    {
        public static Param<T> IsNotNull<T>(this Param<T> param) where T : class
        {
            if (param.Value == null)
            {
                throw new ArgumentNullException(param.Name, ErrorMessages.ArgumentCannotBeNull);
            }

            return param;
        }

        public static Param<Type> ArgumentType<T>(this Param<T> param) where T : class
        {
            return new ArgumentTypeParam<T>(param);
        }

        public static Param<T> IsNotDefaultValue<T>(this Param<T> param) where T : struct
        {
            if (param.Value.Equals(default(T)))
            {
                throw Ensure.ExFactory.Create(ErrorMessages.ArgumentCannotBeDefault, param.Name);
            }

            return param;
        }
    }
}
