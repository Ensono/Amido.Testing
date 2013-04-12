namespace Amido.Testing.Azure.Blobs
{
    /// <summary>
    /// The settings required for deleting a container.
    /// </summary>
    public class DeleteContainerSettings
    {
        /// <summary>
        /// The container name.
        /// </summary>
        public string ContainerName { get; set; }

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
