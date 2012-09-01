using System;
using System.Runtime.Serialization;

namespace Amido.Testing.Dbc
{
    /// <summary>
    /// Exception thrown when a precondition contract check fails
    /// </summary>
    [Serializable]
    public class PreconditionException : Exception
    {
        public PreconditionException() : base()
        {
        }

        public PreconditionException(string message)
            : base(message)
        {
        }

        protected PreconditionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
