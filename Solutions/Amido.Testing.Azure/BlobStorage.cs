using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Amido.Testing.Azure.Blobs;
using Amido.Testing.Dbc;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Amido.Testing.Azure
{
    /// <summary>
    /// Helper class for Azure blob storage.
    /// </summary>
    public static class BlobStorage
    {
        /// <summary>
        /// Copies a blob from one account and container to another.
        /// </summary>
        /// <param name="copyBlockBlobSettings">A <see cref="CopyBlockBlobSettings"/>.</param>
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

            destinationBlob.StartCopyFromBlob(sourceBlob.Uri);

            MonitorCopy(destinationBlob.Container);
        }

        /// <summary>
        ///  Taken from: http://blogs.msdn.com/b/windowsazurestorage/archive/2012/06/12/introducing-asynchronous-cross-account-copy-blob.aspx
        /// </summary>
        /// <param name="destContainer">The container to monitor</param>
        public static void MonitorCopy(CloudBlobContainer destContainer)
        {
            var pendingCopy = true;

            while (pendingCopy)
            {
                var destBlobList = destContainer.ListBlobs(true, BlobListingDetails.Copy);

                foreach (var destBlob in destBlobList.Select(dest => dest as CloudBlob))
                {
                    if (destBlob.CopyState == null)
                    {
                        Debug.WriteLine("BlobStorage.MonitorCopy: CopyState is null. Small sleep, then we assume it's done!");
                        Thread.Sleep(4000);
                        return;
                    }

                    switch (destBlob.CopyState.Status)
                    {
                        case CopyStatus.Failed:
                        case CopyStatus.Aborted:
                            Debug.WriteLine("BlobStorage.MonitorCopy: Copy Failed or Aborted; restarting copy");
                            destBlob.StartCopyFromBlob(destBlob.CopyState.Source);
                            break;
                        case CopyStatus.Success:
                            pendingCopy = false;
                            break;
                    }
                }

                Thread.Sleep(1000);
            }
        }
        
        /// <summary>
        /// Downloads a blob from a container.
        /// </summary>
        /// <param name="downloadBlockBlobSettings">A <see cref="DownloadBlockBlobSettings"/>.</param>
        public static MemoryStream DownloadBlockBlob(DownloadBlockBlobSettings downloadBlockBlobSettings)
        {
            Contract.Requires(downloadBlockBlobSettings != null, "The copy block blob settings cannot be null.");

            var storageAccount = new CloudStorageAccount(new StorageCredentialsAccountAndKey(downloadBlockBlobSettings.BlobStorage, downloadBlockBlobSettings.BlobStorageKey), downloadBlockBlobSettings.UseHttps);
            var client = storageAccount.CreateCloudBlobClient();
            var container = client.GetContainerReference(downloadBlockBlobSettings.ContainerName);
            var blockBlob = container.GetBlockBlobReference(downloadBlockBlobSettings.BlobPath);
            using (var memoryStream = new MemoryStream())
            {
                blockBlob.DownloadToStream(memoryStream);
                return memoryStream;
            }
        }

        /// <summary>
        /// Uploads data (raw bytes or string data) to a container.
        /// </summary>
        /// <param name="uploadBlockBlobSettings">A <see cref="UploadBlockBlobSettings"/>.</param>
        public static void UploadBlockBlob(UploadBlockBlobSettings uploadBlockBlobSettings)
        {
            Contract.Requires(uploadBlockBlobSettings != null, "The upload block blob settings cannot be null.");

            var destinationStorageAccount = new CloudStorageAccount(new StorageCredentialsAccountAndKey(uploadBlockBlobSettings.BlobStorageDestination, uploadBlockBlobSettings.BlobStorageDestinationKey), uploadBlockBlobSettings.UseHttps);
            var destinationClient = destinationStorageAccount.CreateCloudBlobClient();
            var destinationContainer = destinationClient.GetContainerReference(uploadBlockBlobSettings.DestinationContainerName);

            try
            {
                destinationContainer.CreateIfNotExist();
            }
            catch
            {
                // do nothing, create if not exists blows up if it already exists... nice.
            }

            CloudBlob destinationBlob = destinationClient.GetBlockBlobReference(uploadBlockBlobSettings.BlobDestinationPath);

            if (uploadBlockBlobSettings.RawData != null)
            {
                destinationBlob.UploadByteArray(uploadBlockBlobSettings.RawData);
            }
            else if (uploadBlockBlobSettings.StringData != null)
            {
                destinationBlob.UploadText(uploadBlockBlobSettings.StringData);
            }
        }

        /// <summary>
        /// Deletes a container.
        /// </summary>
        /// <param name="deleteContainerSettings">A <see cref="ContainerSettings"/>.</param>
        public static void DeleteContainer(ContainerSettings deleteContainerSettings)
        {
            Contract.Requires(deleteContainerSettings != null, "The delete container settings cannot be null.");

            var storageAccount = new CloudStorageAccount(new StorageCredentialsAccountAndKey(deleteContainerSettings.BlobStorageDestination, deleteContainerSettings.BlobStorageDestinationKey), true);

            var client = storageAccount.CreateCloudBlobClient();

            var blobContainer = client.GetContainerReference(deleteContainerSettings.ContainerName);
            
            blobContainer.Delete(AccessCondition.GenerateEmptyCondition(), null);
        }

        public static bool ContainerExists(ContainerSettings containerSettings)
        {
            Contract.Requires(containerSettings != null, "The container settings cannot be null.");

            var storageAccount =
                new CloudStorageAccount(
                    new StorageCredentialsAccountAndKey(containerSettings.BlobStorageDestination,
                                                        containerSettings.BlobStorageDestinationKey), true);

            var client = storageAccount.CreateCloudBlobClient();

            var blobContainer = client.GetContainerReference(containerSettings.ContainerName);

            if (containerSettings.SubContainerName != null)
                blobContainer = blobContainer.GetDirectoryReference(containerSettings.SubContainerName).Container;

            try
            {
                blobContainer.FetchAttributes();
                return true;
            }
            catch (StorageClientException ex)
            {
                return false;
            }
        }
    }
}
