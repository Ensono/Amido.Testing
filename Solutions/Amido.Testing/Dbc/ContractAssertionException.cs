using System;
using System.Runtime.Serialization;

namespace Amido.Testing.Dbc
{
    /// <summary>
    /// Exception thrown when an assert contract check fails
    /// </summary>
    [Serializable]
    public class ContractAssertionException : Exception
    {
        public ContractAssertionException() : base()
        {
        }

        public ContractAssertionException(string message)
            : base(message)
        {
        }

        protected ContractAssertionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
