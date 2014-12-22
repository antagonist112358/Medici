using System;

namespace Medici
{
    public static class EnsureTypeExtensions
    {
        public static Param<Type> IsAssignableTo(this Param<Type> param, Type targetType)
        {
            if (targetType.IsAssignableFrom(param.Value)) return param;

            if (targetType.IsInterface)
            {
                throw Ensure.ExFactory.Create(ErrorMessages.TypeNotImplementInterface,
                    param.Name, param.Value, targetType);
            }

            throw Ensure.ExFactory.Create(ErrorMessages.TypeNotInheritFromType,
                param.Name, param.Value, targetType);
        }
    }
}
