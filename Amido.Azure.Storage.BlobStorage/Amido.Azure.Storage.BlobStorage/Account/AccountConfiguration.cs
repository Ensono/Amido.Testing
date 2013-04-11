namespace Amido.Azure.Storage.BlobStorage.Account
{
    /// <summary>
    /// /// Class that represents a connection to a Windows Azure blob repository based upon an account name.
    /// </summary>
    public class AccountConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountConfiguration" /> class.
        /// </summary>
        /// <param name="accountName">Name of the account.</param>
        public AccountConfiguration(string accountName)
        {
            AccountName = accountName;
        }

        /// <summary>
        /// Gets the name of the account.
        /// </summary>
        /// <value>The name of the account.</value>
        public string AccountName { get; private set; }
    }
}