using System;
using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling.AzureStorage;
using Microsoft.Practices.TransientFaultHandling;

namespace Amido.Azure.Storage.BlobStorage.RetryPolicy
{
    public static class StorageRetryPolicy
    {
        public static RetryPolicy<StorageTransientErrorDetectionStrategy> RetryPolicy
        {
            get
            {
                var retryStrategy = new Incremental(5, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));

                return new RetryPolicy<StorageTransientErrorDetectionStrategy>(retryStrategy);
            }
        }
    }
}