namespace Amido.Azure.Storage.BlobStorage.Account
{
    /// <summary>
    /// Class that represents a connection to a Windows Azure blob repository based upon a connection string.
    /// </summary>
    public class AccountConnection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountConnection" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public AccountConnection(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString { get; private set; }
    }
}