using Amido.Testing.Azure.Tables;
using Amido.Testing.Dbc;
using Microsoft.WindowsAzure.StorageClient;

namespace Amido.Testing.Azure
{
    /// <summary>
    /// Helper class for working with table storage.
    /// </summary>
    public static class TableStorage
    {
        /// <summary>
        /// Create a context for batching operations within table storage.
        /// </summary>
        /// <typeparam name="TEntity">The table storage entity type.</typeparam>
        /// <param name="tableSettings">The <see cref="TableSettings"/>.</param>
        /// <param name="entitySetName">The entity set name - i.e the name of the table within table storage.</param>
        /// <returns>Returns a new context.</returns>
       public static EntityBatch<TEntity> Batch<TEntity>(TableSettings tableSettings, string entitySetName) where TEntity : TableServiceEntity
       {
           Contract.Requires(tableSettings != null, "The table settings cannot be null.");
           Contract.Requires(!string.IsNullOrWhiteSpace(entitySetName), "The entity set name cannot be null or empty.");

           return new EntityBatch<TEntity>(tableSettings, entitySetName); 
       }
    }
}
