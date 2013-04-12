namespace Amido.Testing.Azure.Blobs
{
    /// <summary>
    /// The settings required for referencing a container.
    /// </summary>
    public class ContainerSettings
    {
        /// <summary>
        /// The container name.
        /// </summary>
        public string ContainerName { get; set; }

        /// <summary>
        /// The sub-container name.
        /// </summary>
        public string SubContainerName { get; set; }

        /// <summary>
        /// The storage account name.
        /// </summary>
        public string BlobStorageDestination { get; set; }

        /// <summary>
        /// The storage account key.
        /// </summary>
        public string BlobStorageDestinationKey { get; set; }
    }
}
