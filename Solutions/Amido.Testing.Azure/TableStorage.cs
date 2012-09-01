using Amido.Testing.Azure.Tables;
using Amido.Testing.Dbc;
using Microsoft.WindowsAzure.StorageClient;

namespace Amido.Testing.Azure
{
    public class TableStorage
    {
       public static EntityBatch<TEntity> Batch<TEntity>(TableSettings tableSettings, string entitySetName) where TEntity : TableServiceEntity
       {
           Contract.Requires(tableSettings != null, "The table settings cannot be null.");
           Contract.Requires(!string.IsNullOrWhiteSpace(entitySetName), "The entity set name cannot be null or empty.");

           return new EntityBatch<TEntity>(tableSettings, entitySetName); 
       }
    }
}
