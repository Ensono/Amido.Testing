using System.Linq;
using Microsoft.WindowsAzure.StorageClient;

namespace Amido.Testing.Azure.Tables
{
    public class EntityBatch<TEntity> where TEntity : TableServiceEntity
    {
        private readonly string entitySetName;
        private readonly BatchDataContext<TEntity> context;

        public EntityBatch(TableSettings tableSettings, string entitySetName)
        {
            this.entitySetName = entitySetName;
            context = new BatchDataContext<TEntity>(tableSettings);
        }

        public IQueryable<TEntity> Data
        {
            get { return context.Data; }
        }

        public EntityBatch<TEntity> Insert(TEntity entity)
        {
            context.AddObject(entitySetName, entity);
            return this;
        }

        public EntityBatch<TEntity> Update(TEntity entity)
        {
            context.UpdateObject(entity);
            return this;
        }

        public EntityBatch<TEntity> Delete(TEntity entity)
        {
            context.DeleteObject(entity);
            return this;
        }

        public void Save()
        {
            context.Save();
        }
    }
}
