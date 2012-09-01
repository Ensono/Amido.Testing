using System.Data.Services.Client;
using Microsoft.WindowsAzure.StorageClient;

namespace Amido.Testing.Azure.Tables
{
    public class BatchDataContext<T> : DataContext<T> where T : TableServiceEntity
    {
        public BatchDataContext(TableSettings tableSettings)
            : base(tableSettings)
        {
            MergeOption = MergeOption.PreserveChanges;
        }

        public override void Save()
        {
            SaveChangesWithRetries(SaveChangesOptions.Batch);
        }
    }
}
