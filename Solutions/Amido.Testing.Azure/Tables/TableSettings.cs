namespace Amido.Testing.Azure.Tables
{
    public class TableSettings
    {
        public string AccountName { get; set; }
        public string AccountKey { get; set; }
        public bool UseHttps { get; set; }

        public TableSettings(string accountName, string accountKey, bool useHttps)
        {
            AccountName = accountName;
            AccountKey = accountKey;
            UseHttps = useHttps;
        }
    }
}
