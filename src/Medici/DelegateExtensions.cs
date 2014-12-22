using System;
using Medici.Reflection;

namespace Medici
{
    public static class DelegateExtensions
    {
        public static MethodSignature GetSignature(this Delegate del)
        {
            Ensure.That(del, "del").IsNotNull();

            return new MethodSignature(del.Method);
        }
    }
}
