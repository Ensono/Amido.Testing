using System;
using System.Runtime.Serialization;

namespace Amido.Testing.Dbc
{
    /// <summary>
    /// Exception thrown when a postcondition contract check fails
    /// </summary>
    [Serializable]
    public class PostconditionException : Exception
    {
        public PostconditionException() : base()
        {
        }

        public PostconditionException(string message)
            : base(message)
        {
        }

        protected PostconditionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
