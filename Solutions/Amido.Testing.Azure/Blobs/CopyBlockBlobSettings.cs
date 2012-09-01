﻿namespace Amido.Testing.Azure.Blobs
{
    /// <summary>
    /// The settings required for copying block blobs from one account and container to another.
    /// </summary>
    public class CopyBlockBlobSettings
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
}
