using System.Linq;
using Amido.Testing.Azure.Blobs;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using Amido.Testing.Dbc;

namespace Amido.Testing.Azure
{
    public static class BlobStorage
    {
        public static void CopyBlockBlob(CopyBlockBlobSettings copyBlockBlobSettings)
        {
            Contract.Requires(copyBlockBlobSettings != null, "The copy block blob settings cannot be null.");

            var sourceStorageAccount = new CloudStorageAccount(new StorageCredentialsAccountAndKey(copyBlockBlobSettings.BlobStorageSource, copyBlockBlobSettings.BlobStorageSourceKey), copyBlockBlobSettings.UseHttps);
            var sourceClient = sourceStorageAccount.CreateCloudBlobClient();
            var destinationStorageAccount = new CloudStorageAccount(new StorageCredentialsAccountAndKey(copyBlockBlobSettings.BlobStorageDestination, copyBlockBlobSettings.BlobStorageDestinationKey), copyBlockBlobSettings.UseHttps);
            var destinationClient = destinationStorageAccount.CreateCloudBlobClient();
            var destinationContainer = destinationClient.GetContainerReference(copyBlockBlobSettings.DestinationContainerName);

            try
            {
                destinationContainer.CreateIfNotExist();
            }
            catch
            {
                // do nothing, create if not exists blows up if it already exists... nice.
            }

            var sourceBlob = sourceClient.GetBlockBlobReference(copyBlockBlobSettings.BlobSourcePath);
            sourceBlob.FetchAttributes();

            CloudBlob destinationBlob = destinationClient.GetBlockBlobReference(copyBlockBlobSettings.BlobDestinationPath);

            try
            {
                destinationBlob.StartCopyFromBlob(sourceBlob.Uri);
            }
            catch (StorageClientException ex)
            {
                throw;
            }
        }

        public static void DeleteContainer(DeleteContainerSettings deleteContainerSettings)
        {
            Contract.Requires(deleteContainerSettings != null, "The delete container settings cannot be null.");

            var storageAccount = new CloudStorageAccount(new StorageCredentialsAccountAndKey(deleteContainerSettings.BlobStorageDestination, deleteContainerSettings.BlobStorageDestinationKey), true);

            var client = storageAccount.CreateCloudBlobClient();

            var blobContainer = client.GetContainerReference(deleteContainerSettings.ContainerName);
            try
            {
                blobContainer.Delete(AccessCondition.GenerateEmptyCondition(), null);
            }
            catch (StorageClientException ex)
            {
                throw;
            }
        }
    }
}
