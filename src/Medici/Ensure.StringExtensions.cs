using System;

namespace Medici
{
    public static class EnsureStringExtensions
    {
        public static Param<string> IsNotNullOrEmpty(this Param<string> param)
        {
            if (String.IsNullOrEmpty(param.Value))
            {
                throw Ensure.ExFactory.Create(ErrorMessages.ArgumentCannotBeEmpty, param.Name);
            }

            return param;
        }

        public static Param<string> IsNotNullOrWhitespace(this Param<string> param)
        {
            if (String.IsNullOrWhiteSpace(param.Value))
            {
                throw Ensure.ExFactory.Create(ErrorMessages.ArgumentCannotBeEmpty, param.Name);
            }

            return param;
        }
    }
}
