using System.Data.Services.Client;
using Microsoft.WindowsAzure.StorageClient;

namespace Amido.Testing.Azure.Tables
{
    /// <summary>
    /// A table data context used for interacting with table storage.
    /// </summary>
    /// <typeparam name="T">The table storage entity type.</typeparam>
    public class BatchDataContext<T> : DataContext<T> where T : TableServiceEntity
    {
        /// <summary>
        /// Constructs the data context.
        /// </summary>
        /// <param name="tableSettings">The <see cref="TableSettings"/>.</param>
        public BatchDataContext(TableSettings tableSettings)
            : base(tableSettings)
        {
            MergeOption = MergeOption.PreserveChanges;
        }

        /// <summary>
        /// Saves the context savings using batching.
        /// </summary>
        public override void Save()
        {
            SaveChangesWithRetries(SaveChangesOptions.Batch);
        }
    }
}
