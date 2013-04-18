using System;

namespace Amido.Testing.Azure.Blobs
{
    /// <summary>
    /// The settings required for copying block blobs from one account and container to another.
    /// </summary>
    public abstract class BlockBlobSettings
    {
        /// <summary>
        /// The destination container name.
        /// </summary>
        public string DestinationContainerName { get; set; }

        /// <summary>
        /// The destination storage path.
        /// </summary>
        public string BlobDestinationPath { get; set; }

        /// <summary>
        /// The destination storage account name.
        /// </summary>
        public string BlobStorageDestination { get; set; }

        /// <summary>
        /// The destination storage account key.
        /// </summary>
        public string BlobStorageDestinationKey { get; set; }

        /// <summary>
        /// Specifies whether to use https.
        /// </summary>
        public bool UseHttps { get; set; }
    }

    /// <summary>
    /// The settings required for copying block blobs from one account and container to another.
    /// </summary>
    /// 
    public class CopyBlockBlobSettings : BlockBlobSettings
    {
        /// <summary>
        /// The source storage path.
        /// </summary>
        public string BlobSourcePath { get; set; }

        /// <summary>
        /// The source storage account name.
        /// </summary>
        public string BlobStorageSource { get; set; }

        /// <summary>
        /// The source storage account key.
        /// </summary>
        public string BlobStorageSourceKey { get; set; }
    }

    /// <summary>
    /// The settings required for copying block blobs from one account and container to another.
    /// </summary>
    /// 
    public class UploadBlockBlobSettings : BlockBlobSettings
    {
        /// <summary>
        /// The raw data to upload (takes precedence over string data).
        /// </summary>
        public byte[] RawData { get; set; }

        /// <summary>
        /// The string data to upload.
        /// </summary>
        public string StringData { get; set; }
    }

    public class DownloadBlockBlobSettings
    {
        public string BlobStorage { get; set; }
        public string BlobStorageKey { get; set; }
        public bool UseHttps { get; set; }
        public string ContainerName { get; set; }
        public string BlobPath { get; set; }
    }

    public class LeaseBlockBlobSettings
    {
        public string BlobStorage { get; set; }
        public string BlobStorageKey { get; set; }
        public bool UseHttps { get; set; }
        public string ContainerName { get; set; }
        public string BlobPath { get; set; }
        public TimeSpan? LeaseTime { get; set; }
        public int RetryCount { get; set; }
        public TimeSpan RetryInterval { get; set; }
    }
}
