using System;
using System.Data.Services.Client;
using System.Diagnostics;
using System.Linq;
using Amido.Testing.Dbc;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Amido.Testing.Azure.Tables
{
    public class DataContext<T> : TableServiceContext where T : TableServiceEntity
    {

        public DataContext(TableSettings tableSettings)
            : base(GetCloudStorageAccount(tableSettings).TableEndpoint.AbsoluteUri,
                GetCloudStorageAccount(tableSettings).Credentials)
        {
            var cloudClient = GetCloudStorageAccount(tableSettings);
            var tableClient = cloudClient.CreateCloudTableClient();
            tableClient.CreateTableIfNotExist(typeof(T).Name);
            IgnoreResourceNotFoundException = true;
        }

        public IQueryable<T> Data
        {
            get { return CreateQuery<T>(typeof(T).Name); }
        }

        public virtual void Delete(T entity)
        {
            Contract.Requires(entity != null, "The entity cannot be null.");
            DeleteObject(entity);
        }

        public virtual void Insert(T entity)
        {
            Contract.Requires(entity != null, "The entity cannot be null.");
            AddObject(typeof(T).Name, entity);
        }

        public virtual void Update(T entity)
        {
            Contract.Requires(entity != null, "The entity cannot be null.");
            UpdateObject(entity);
        }

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
