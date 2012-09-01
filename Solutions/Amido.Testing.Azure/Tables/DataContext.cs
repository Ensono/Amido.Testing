using System;
using System.Data.Services.Client;
using System.Diagnostics;
using System.Linq;
using Amido.Testing.Dbc;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Amido.Testing.Azure.Tables
{
    /// <summary>
    /// Table storage data context.
    /// </summary>
    /// <typeparam name="T">The table storage entity type.</typeparam>
    public class DataContext<T> : TableServiceContext where T : TableServiceEntity
    {
        /// <summary>
        /// Constructs the data context.
        /// </summary>
        /// <param name="tableSettings">The <see cref="TableSettings"/>.</param>
        public DataContext(TableSettings tableSettings)
            : base(GetCloudStorageAccount(tableSettings).TableEndpoint.AbsoluteUri,
                GetCloudStorageAccount(tableSettings).Credentials)
        {
            var cloudClient = GetCloudStorageAccount(tableSettings);
            var tableClient = cloudClient.CreateCloudTableClient();
            tableClient.CreateTableIfNotExist(typeof(T).Name);
            IgnoreResourceNotFoundException = true;
        }

        /// <summary>
        /// Returns a queryable on the table for the specified type.
        /// </summary>
        public IQueryable<T> Data
        {
            get { return CreateQuery<T>(typeof(T).Name); }
        }

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        public virtual void Delete(T entity)
        {
            Contract.Requires(entity != null, "The entity cannot be null.");
            DeleteObject(entity);
        }

        /// <summary>
        /// Inserts an entity.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        public virtual void Insert(T entity)
        {
            Contract.Requires(entity != null, "The entity cannot be null.");
            AddObject(typeof(T).Name, entity);
        }

        /// <summary>
        /// Updates an entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        public virtual void Update(T entity)
        {
            Contract.Requires(entity != null, "The entity cannot be null.");
            UpdateObject(entity);
        }

        /// <summary>
        /// Saves pending changes in the context.
        /// </summary>
        public virtual void Save()
        {
            SaveChanges(SaveChangesOptions.ReplaceOnUpdate);
        }

        public virtual void AttachEntity(T entity)
        {
            Contract.Requires(entity != null, "The entity cannot be null.");
            AttachTo(typeof(T).Name, entity);
        }

        public virtual void DetachEntity(T entity)
        {
            Contract.Requires(entity != null, "The entity cannot be null.");
            Detach(entity);
        }

        public static CloudStorageAccount GetCloudStorageAccount(TableSettings tableSettings)
        {
            Contract.Assert(!string.IsNullOrEmpty(tableSettings.AccountName));

            try
            {
                return new CloudStorageAccount(
                    new StorageCredentialsAccountAndKey(
                        tableSettings.AccountName, tableSettings.AccountKey), tableSettings.UseHttps);
            }
            catch (Exception error)
            {
                Trace.TraceError(error.ToString());
                throw new InvalidOperationException("Unable to find cloud storage account", error);
            }
        }
    }
}
