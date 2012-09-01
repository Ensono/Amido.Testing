namespace Amido.Testing.Azure.Blobs
{
    public class CopyBlockBlobSettings
    {
        public string DestinationContainerName { get; set; }
        public string BlobSourcePath { get; set; }
        public string BlobDestinationPath { get; set; }
        public string BlobStorageSource { get; set; }
        public string BlobStorageSourceKey { get; set; }
        public string BlobStorageDestination { get; set; }
        public string BlobStorageDestinationKey { get; set; }
        public bool UseHttps { get; set; }
    }
}
