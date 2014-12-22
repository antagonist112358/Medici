using System;
using System.Reflection;

namespace Oculus.Reflection
{
    public static class ReflectionHelper
    {
        public static void SetPrivateField(object instance, string fieldName, Type instanceType, object value)
        {
            FieldInfo targetField = instanceType.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);

            targetField.SetValue(instance, value);
        }

        public static void SetPrivateStaticField(string fieldName, Type instanceType, object value)
        {
            FieldInfo targetField = instanceType.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);

            targetField.SetValue(null, value);
        }

        public static object InvokePrivateMethod(Type type, object instance, string methodName, params object[] methArgs)
        {
            MethodInfo mi = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);

            return mi.Invoke(instance, methArgs);
        }

        public static T GetPrivateField<T>(object instance, string fieldName, Type instanceType)
        {
            FieldInfo targetField = instanceType.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);

            return (T)targetField.GetValue(instance);
        }
    }
}
