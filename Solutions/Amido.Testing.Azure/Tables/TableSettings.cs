namespace Amido.Testing.Azure.Tables
{
    /// <summary>
    /// Settings class used for accessing table storage.
    /// </summary>
    public class TableSettings
    {
        /// <summary>
        /// The storage account name.
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// The storage account key.
        /// </summary>
        public string AccountKey { get; set; }

        /// <summary>
        /// Use https.
        /// </summary>
        public bool UseHttps { get; set; }

        /// <summary>
        /// Constructs a <see cref="TableSettings"/>.
        /// </summary>
        /// <param name="accountName">The storage account name.</param>
        /// <param name="accountKey">The storage account key.</param>
        /// <param name="useHttps">Use https.</param>
        public TableSettings(string accountName, string accountKey, bool useHttps)
        {
            AccountName = accountName;
            AccountKey = accountKey;
            UseHttps = useHttps;
        }
    }
}
