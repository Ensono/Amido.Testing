using System;
using System.Diagnostics;
using Amido.Testing.Azure;
using Amido.Testing.Azure.Blobs;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using NUnit.Framework;

namespace Amido.Testing.Tests.Azure
{
    public class BlobStorageTests
    {
        private CloudBlob leaseBlob;
        private CloudBlobContainer container;
        private LeaseBlockBlobSettings blobSettings;

        [SetUp]
        public void SetUp()
        {
            blobSettings = new LeaseBlockBlobSettings
            {
                ConnectionString = "UseDevelopmentStorage=true",
                ContainerName = "test" + Guid.NewGuid().ToString("N"),
                BlobPath = "lease.blob",
                RetryCount = 2,
                RetryInterval = TimeSpan.FromMilliseconds(250)
            };
            var storageAccount = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            var client = storageAccount.CreateCloudBlobClient();
            container = client.GetContainerReference(blobSettings.ContainerName);
            container.CreateIfNotExist();
            leaseBlob = container.GetBlobReference(blobSettings.BlobPath);
            leaseBlob.UploadByteArray(new byte[0]);
        }

        [TearDown]
        public void TearDown()
        {
            container.Delete();
        }

        [TestFixture]
        public class When_successfully_aquiring_the_lease : BlobStorageTests
        {
            [SetUp]
            public void BecauseOf()
            {
                BlobStorage.AquireLease(blobSettings);
            }

            [Test]
            public void It_should_prevent_a_new_lease_being_aquired()
            {
                Assert.Throws<StorageClientException>(() => leaseBlob.AcquireLease(null, null));
            }
        }

        [TestFixture]
        public class When_failing_to_aquiring_the_lease : BlobStorageTests
        {
            [SetUp]
            public void BecauseOf()
            {
                leaseBlob.AcquireLease(null, null);
            }

            [Test]
            public void It_should_not_create_a_lease_id()
            {
                var leaseId = BlobStorage.AquireLease(blobSettings);
                Assert.IsNull(leaseId);
            }
        }

        [TestFixture]
        public class When_releasing_the_lease : BlobStorageTests
        {
            [SetUp]
            public void BecauseOf()
            {
                var leaseId = BlobStorage.AquireLease(blobSettings);
                BlobStorage.ReleaseLease(blobSettings, leaseId);
            }

            [Test]
            public void It_should_be_re_acquired()
            {
                var leaseId = leaseBlob.AcquireLease(null, null);
                Assert.IsNotNull(leaseId);
            }
        }
    }

    [SetUpFixture]
    public class SetUpFixture
    {
        [SetUp]
        public void SetUp()
        {
            var process = Process.Start(@"C:\Program Files\Microsoft SDKs\Windows Azure\Emulator\csrun", "/devstore");
            if (process != null)
            {
                process.WaitForExit();
            }
            else
            {
                throw new ApplicationException("Unable to start storage emulator.");
            }
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var proc in Process.GetProcessesByName("csmonitor"))
            {
                proc.Kill();
            }
        }
    }
}
