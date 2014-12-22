namespace Medici.Collections
{
    public sealed class DuplicateKeyException : MediciException
    {     
        public DuplicateKeyException(object key) :
            base(ErrorMessages.DuplicateKeyNotAllowed, key) { }

        public DuplicateKeyException(object key, string message) :
            base("[Duplicate Key: " + key.ToString() + "] - " + message) { }

        public DuplicateKeyException(object key, string message, params object[] args) :
            base("[Duplicate Key: " + key.ToString() + "] - " + message, args) { }
    }
}
