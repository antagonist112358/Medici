using System;
using System.Diagnostics;

namespace Medici
{
    /// <summary>
    /// Helper class for creating exceptions.
    /// </summary>
    [DebuggerStepThrough]    
    public static class ExGen
    {
        public static TException Build<TException>(string message, params object[] formatArgs) where TException : Exception
        {
            return Build<TException>(message, null, formatArgs);
        }

        public static TException Build<TException>(params object[] args) where TException : Exception
        {
            Exception toBuild = (Exception)Activator.CreateInstance(typeof(TException), args);
            return (TException)toBuild;
        }

        public static TException Build<TException>(string message, Exception innerEx, params object[] formatArgs) where TException : Exception
        {
            string formattedMessage = String.Format(message, formatArgs);
            Exception toBuild = null;

            if (innerEx != null)
                toBuild = (Exception)Activator.CreateInstance(typeof(TException), formattedMessage, innerEx);
            else
                toBuild = (Exception)Activator.CreateInstance(typeof(TException), formattedMessage);

            return (TException)toBuild;
        }

        #region Generic Exception Builders

        public static TException Build<T1, TException>(T1 arg1) where TException : Exception
        {
            throw Build<TException>(new object[] { arg1 });
        }

        public static TException Build<T1, T2, TException>(T1 arg1, T2 arg2) where TException : Exception
        {
            throw Build<TException>(new object[] { arg1, arg2 });
        }

        public static TException Build<T1, T2, T3, TException>(T1 arg1, T2 arg2, T3 arg3) where TException : Exception
        {
            throw Build<TException>(new object[] { arg1, arg2, arg3 });
        }

        public static TException Build<T1, T2, T3, T4, TException>(T1 arg1, T2 arg2, T3 arg3, T4 arg4) where TException : Exception
        {
            throw Build<TException>(new object[] { arg1, arg2, arg3, arg4 });
        }

        public static TException Build<T1, T2, T3, T4, T5, TException>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) where TException : Exception
        {
            throw Build<TException>(new object[] { arg1, arg2, arg3, arg4, arg5 });
        }

        public static TException Build<T1, T2, T3, T4, T5, T6, TException>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) where TException : Exception
        {
            throw Build<TException>(new object[] { arg1, arg2, arg3, arg4, arg5, arg6 });
        }

        #endregion

    }
}