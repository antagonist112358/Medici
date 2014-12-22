using System;

namespace Medici
{
    public class MediciException : Exception
    {
        private readonly bool _hasInnerException = false;

        public MediciException(string message, params object[] msgFormatArgs) : 
            base(
                    (msgFormatArgs != null) ? String.Format(message, msgFormatArgs) : message
                )
        {
            _hasInnerException = false;
        }

        public MediciException(Exception innerEx, string message, params object[] msgFormatArgs) :
            base(
                    (msgFormatArgs != null) ? String.Format(message, msgFormatArgs) : message
                )
        {
            _hasInnerException = true;
        }

        public MediciException(string message) : this(message, null) { }
        public MediciException(Exception innerEx, string message) : this(innerEx, message, null) { }

        public override string Message
        {
            get
            {
                if (_hasInnerException)
                    return base.Message + Environment.NewLine +
                        "\tInner Exception: " + base.InnerException.Message;
                else
                {
                    return base.Message;
                }
            }
        }

        public override string StackTrace
        {
            get
            {
                if (_hasInnerException)
                    return base.InnerException.StackTrace;
                else
                    return base.StackTrace;
            }
        }

    }
}
