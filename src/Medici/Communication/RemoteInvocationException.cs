using System;

namespace Medici.Communication
{
    [Serializable]
    public class RemoteInvocationException : MediciException
    {
        private readonly string _path;
        private readonly string _invocationTargetType;

        public RemoteInvocationException(Type targetType, string path, string message, Exception innerEx)
            : base(message, innerEx)
        {
            _invocationTargetType = targetType.FullName;
            _path = path; 
        }

        public override string Message
        {
            get
            {
                return String.Format("Remote Invocation Exception ([{0}] targeting {1}) - {2}",
                    _invocationTargetType, _path, base.Message);
            }
        }

        public string SourcePath
        {
            get { return _path; }
        }
    }
}
