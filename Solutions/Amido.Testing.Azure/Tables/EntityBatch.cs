using System.Linq;
using Microsoft.WindowsAzure.StorageClient;

namespace Amido.Testing.Azure.Tables
{
    /// <summary>
    /// A data context used for accessing table storage and saving in batches.
    /// </summary>
    /// <typeparam name="TEntity">The table storage entity type.</typeparam>
    public class EntityBatch<TEntity> where TEntity : TableServiceEntity
    {
        private readonly string entitySetName;
        private readonly BatchDataContext<TEntity> context;

        /// <summary>
        /// Constructs the data context.
        /// </summary>
        /// <param name="tableSettings">The <see cref="TableSettings"/>.</param>
        /// <param name="entitySetName">The name of the table in table storage.</param>
        public EntityBatch(TableSettings tableSettings, string entitySetName)
        {
            this.entitySetName = entitySetName;
            context = new BatchDataContext<TEntity>(tableSettings);
        }

        /// <summary>
        /// Returns a queryable on the table for the specified type.
        /// </summary>
        public IQueryable<TEntity> Data
        {
            get { return context.Data; }
        }

        /// <summary>
        /// Inserts an entity.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        public EntityBatch<TEntity> Insert(TEntity entity)
        {
            context.AddObject(entitySetName, entity);
            return this;
        }

        /// <summary>
        /// Updates an entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        public EntityBatch<TEntity> Update(TEntity entity)
        {
            context.UpdateObject(entity);
            return this;
        }

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public EntityBatch<TEntity> Delete(TEntity entity)
        {
            context.DeleteObject(entity);
            return this;
        }

        /// <summary>
        /// Saves pending changes in the context.
        /// </summary>
        public void Save()
        {
            context.Save();
        }
    }
}
