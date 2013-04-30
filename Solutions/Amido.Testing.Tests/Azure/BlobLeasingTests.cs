using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Amido.Testing.Azure;
using Amido.Testing.Azure.Blobs;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using NUnit.Framework;

namespace Amido.Testing.Tests.Azure
{
    public abstract class BlobLeasingTests
    {
        private CloudBlob leaseBlob;
        private CloudBlobContainer container;
        private LeaseBlockBlobSettings blobSettings;
        private int maximumStopDurationEstimateSeconds;

        [SetUp]
        public void SetUp()
        {
            blobSettings = new LeaseBlockBlobSettings
            {
                ConnectionString = "UseDevelopmentStorage=true",
                ContainerName = "test" + Guid.NewGuid().ToString("N"),
                BlobPath = "lease.blob",
                ReAquirePreviousTestLease = false,
                RetryCount = 2,
                RetryInterval = TimeSpan.FromMilliseconds(250)
            };
            maximumStopDurationEstimateSeconds = 10;
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
            try
            {
                leaseBlob.BreakLease(TimeSpan.Zero);
            }
            catch
            {
            }
            container.Delete();
        }

        [TestFixture]
        public class When_successfully_aquiring_the_lease : BlobLeasingTests
        {
            [SetUp]
            public void BecauseOf()
            {
                BlobStorage.AquireLease(blobSettings, maximumStopDurationEstimateSeconds);
                leaseBlob.FetchAttributes();
            }

            [Test]
            public void It_should_prevent_a_new_lease_being_aquired()
            {
                Assert.Throws<StorageClientException>(() => leaseBlob.AcquireLease(null, null));
            }

            [Test]
            public void It_should_have_the_test_lease_metadata()
            {
                Assert.IsTrue(leaseBlob.Metadata.AllKeys.Contains("leased_for_test"));
            }
        }

        [TestFixture]
        public class When_successfully_aquiring_the_lease_twice_allowing_test_lease_to_be_ignored : BlobLeasingTests
        {
            private string leaseId;

            [SetUp]
            public void BecauseOf()
            {
                BlobStorage.AquireLease(blobSettings, maximumStopDurationEstimateSeconds);
                blobSettings.ReAquirePreviousTestLease = true;
                leaseId = BlobStorage.AquireLease(blobSettings, maximumStopDurationEstimateSeconds);
                leaseBlob.FetchAttributes();
            }

            [Test]
            public void It_should_prevent_a_new_lease_being_aquired()
            {
                Assert.Throws<StorageClientException>(() => leaseBlob.AcquireLease(null, null));
            }

            [Test]
            public void It_should_aquire_the_lease()
            {
                Assert.IsNotNull(leaseId);
            }

            [Test]
            public void It_should_have_the_test_lease_metadata()
            {
                Assert.IsTrue(leaseBlob.Metadata.AllKeys.Contains("leased_for_test"));
            }
        }

        [TestFixture]
        public class When_successfully_aquiring_the_lease_twice_preventing_test_lease_to_be_ignored : BlobLeasingTests
        {
            private string leaseId;

            [SetUp]
            public void BecauseOf()
            {
                BlobStorage.AquireLease(blobSettings, maximumStopDurationEstimateSeconds);
                leaseId = BlobStorage.AquireLease(blobSettings, maximumStopDurationEstimateSeconds);
                leaseBlob.FetchAttributes();
            }

            [Test]
            public void It_should_prevent_a_new_lease_being_aquired()
            {
                Assert.Throws<StorageClientException>(() => leaseBlob.AcquireLease(null, null));
            }

            [Test]
            public void It_should_not_aquire_the_lease()
            {
                Assert.IsNull(leaseId);
            }

            [Test]
            public void It_should_have_the_test_lease_metadata()
            {
                Assert.IsTrue(leaseBlob.Metadata.AllKeys.Contains("leased_for_test"));
            }
        }

        [TestFixture]
        public class When_successfully_aquiring_the_lease_and_waiting_less_than_the_maximum_stop_duration : BlobLeasingTests
        {
            [SetUp]
            public void BecauseOf()
            {
                BlobStorage.AquireLease(blobSettings, maximumStopDurationEstimateSeconds);
                Thread.Sleep(TimeSpan.FromSeconds(9));
                leaseBlob.FetchAttributes();
            }

            [Test]
            public void It_should_still_own_the_lease_and_have_the_test_lease_metadata()
            {
                Assert.Throws<StorageClientException>(() => leaseBlob.AcquireLease(null, null));
                Assert.IsTrue(leaseBlob.Metadata.AllKeys.Contains("leased_for_test"));
            }
        }

        [TestFixture]
        public class When_successfully_aquiring_the_lease_and_waiting_more_than_the_maximum_stop_duration : BlobLeasingTests
        {
            [SetUp]
            public void BecauseOf()
            {
                BlobStorage.AquireLease(blobSettings, maximumStopDurationEstimateSeconds);
                Thread.Sleep(TimeSpan.FromSeconds(12));
                leaseBlob.FetchAttributes();
            }

            [Test]
            public void It_should_release_the_lease_and_not_have_the_test_lease_metadata()
            {
                Assert.IsFalse(leaseBlob.Metadata.AllKeys.Contains("leased_for_test"));
                leaseBlob.AcquireLease(null, null);
            }
        }

        [TestFixture]
        public class When_failing_to_aquiring_the_lease : BlobLeasingTests
        {
            [SetUp]
            public void BecauseOf()
            {
                leaseBlob.AcquireLease(null, null);
                leaseBlob.FetchAttributes();
            }

            [Test]
            public void It_should_not_create_a_lease_id()
            {
                var leaseId = BlobStorage.AquireLease(blobSettings, maximumStopDurationEstimateSeconds);
                Assert.IsNull(leaseId);
            }

            [Test]
            public void It_should_not_have_the_test_lease_metadata()
            {
                Assert.IsFalse(leaseBlob.Metadata.AllKeys.Contains("leased_for_test"));
            }
        }

        [TestFixture]
        public class When_releasing_the_lease : BlobLeasingTests
        {
            [SetUp]
            public void BecauseOf()
            {
                var leaseId = BlobStorage.AquireLease(blobSettings, maximumStopDurationEstimateSeconds);
                BlobStorage.ReleaseLease(blobSettings, leaseId);
                leaseBlob.FetchAttributes();
            }

            [Test]
            public void It_should_be_re_acquired()
            {
                var leaseId = leaseBlob.AcquireLease(null, null);
                Assert.IsNotNull(leaseId);
            }

            [Test]
            public void It_should_not_have_the_test_lease_metadata()
            {
                Assert.IsFalse(leaseBlob.Metadata.AllKeys.Contains("leased_for_test"));
            }
        }

        [TestFixture]
        public class When_releasing_the_lease_twice : BlobLeasingTests
        {
            private string leaseId;

            [SetUp]
            public void BecauseOf()
            {
                leaseId = BlobStorage.AquireLease(blobSettings, maximumStopDurationEstimateSeconds);
                BlobStorage.ReleaseLease(blobSettings, leaseId);
            }

            [Test]
            public void It_should_not_throw()
            {
                Assert.DoesNotThrow(() => BlobStorage.ReleaseLease(blobSettings, leaseId));
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
