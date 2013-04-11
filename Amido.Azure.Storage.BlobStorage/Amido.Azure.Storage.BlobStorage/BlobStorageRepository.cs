using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Amido.Azure.Storage.BlobStorage.Account;
using Amido.Azure.Storage.BlobStorage.Extensions;
using Amido.Azure.Storage.BlobStorage.RetryPolicy;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Amido.Azure.Storage.BlobStorage
{
    public class BlobStorageRepository : IBlobStorageRepository
    {
        private CloudBlobClient cloudBlobClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobStorageRepository" /> class.
        /// </summary>
        /// <param name="accountConfiguration">The account configuration.</param>
        public BlobStorageRepository(AccountConfiguration accountConfiguration)
            : this(GetCloudStorageAccountByConfigurationSetting(accountConfiguration.AccountName)) 
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobStorageRepository" /> class.
        /// </summary>
        /// <param name="accountConnection">The account connection.</param>
        public BlobStorageRepository(AccountConnection accountConnection)
            : this(GetCloudStorageAccountByConnectionString(accountConnection.ConnectionString))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlobStorageRepository" /> class.
        /// </summary>
        /// <param name="cloudStorageAccount">The cloud storage account.</param>
        private BlobStorageRepository(CloudStorageAccount cloudStorageAccount)
        {
            cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
        }

        public void DeleteDirectory(Uri uri, string containerName)
        {
            StorageRetryPolicy.RetryPolicy.ExecuteAction(() =>
            {
                var cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);

                var cloudBlockBlobs = ListBlobs(cloudBlobContainer);

                foreach (var cloudBlockBlob in cloudBlockBlobs)
                {
                    cloudBlockBlob.DeleteIfExists();
                }
            });
        }

        public IEnumerable<CloudBlockBlob> ListBlobs(CloudBlobContainer cloudBlobContainer)
        {
            return cloudBlobContainer.ListBlobs(null, true)
                .OfType<CloudBlockBlob>();
        }

        public IEnumerable<CloudBlobContainer> ListContainers()
        {
            return cloudBlobClient.ListContainers();
        }

        public string GetBlobText(Uri uri, string containerName, string blobName)
        {
            string blobText = null;

            StorageRetryPolicy.RetryPolicy.ExecuteAction(() =>
            {
                CloudBlobContainer container = cloudBlobClient.GetContainerReference(containerName);
                CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(blobName);

                using (var memoryStream = new MemoryStream())
                {
                    cloudBlockBlob.EndDownloadToStream(cloudBlockBlob.BeginDownloadToStream(memoryStream, null, null));
                    blobText = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            });

            return blobText;
        }

        public byte[] GetBlobBytes(Uri uri, string containerName, string blobName)
        {
            byte[] blobBytes = null;

            StorageRetryPolicy.RetryPolicy.ExecuteAction(() =>
            {
                CloudBlobContainer container = cloudBlobClient.GetContainerReference(containerName);
                CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(blobName);
                
                using (var memoryStream = new MemoryStream())
                {
                    cloudBlockBlob.EndDownloadToStream(cloudBlockBlob.BeginDownloadToStream(memoryStream, null, null));
                    blobBytes = memoryStream.ToByteArray();
                }
            });

            return blobBytes;
        }

        public void Save(string blobName, string containerName, byte[] bytes, string contentType = "application/octect-stream")
        {
            StorageRetryPolicy.RetryPolicy.ExecuteAction(() =>
            {
                CloudBlobContainer container = cloudBlobClient.GetContainerReference(containerName);
                CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(blobName);

                cloudBlockBlob.Properties.ContentType = contentType;

                using (var memoryStream = new MemoryStream(bytes))
                {
                    cloudBlockBlob.UploadFromStream(memoryStream);
                }
            });
        }

        private void CreateUserIdContainerIfNotExists(string containerName)
        {
            var container = cloudBlobClient.GetContainerReference(containerName);
            container.CreateIfNotExists();
        } 

        /// <summary>
        /// Gets the cloud storage account by connection string.
        /// </summary>
        /// <param name="storageConnectionString">The storage connection string.</param>
        /// <returns>CloudStorageAccount.</returns>
        /// <exception cref="System.InvalidOperationException">Unable to find cloud storage account</exception>
        private static CloudStorageAccount GetCloudStorageAccountByConnectionString(string storageConnectionString)
        {
            try
            {
                return CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (Exception error)
            {
                throw new InvalidOperationException("Unable to find cloud storage account", error);
            }
        }

        /// <summary>
        /// Gets the cloud storage account by configuration setting.
        /// </summary>
        /// <param name="configurationSetting">The configuration setting.</param>
        /// <returns>CloudStorageAccount.</returns>
        /// <exception cref="System.InvalidOperationException">Unable to find cloud storage account</exception>
        private static CloudStorageAccount GetCloudStorageAccountByConfigurationSetting(string configurationSetting)
        {
            try
            {
                return CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting(configurationSetting));
            }
            catch (Exception error)
            {
                throw new InvalidOperationException("Unable to find cloud storage account", error);
            }
        }
    }
}
