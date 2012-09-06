using System;
using System.ComponentModel;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.LoadTesting;

namespace Amido.Testing.LoadTest.Plugins
{
    public class DynamicUserCountPlugin : ILoadTestPlugin, IDisposable
    {
        private static Microsoft.VisualStudio.TestTools.LoadTesting.LoadTest loadTest;
        public static int userLoad;
        private bool serviceIsOnline = true;
        private Task task;

        [DefaultValue(60)]
        [Description("Starting Current Load")]
        public static int UserLoad
        {
            get { return userLoad; }
            set { userLoad = value; }
        }

        public void Initialize(Microsoft.VisualStudio.TestTools.LoadTesting.LoadTest loadTest)
        {
            DynamicUserCountPlugin.loadTest = loadTest;
            task = Task.Factory.StartNew(HostService);
            DynamicUserCountPlugin.loadTest.Heartbeat += new EventHandler<HeartbeatEventArgs>(LoadTestHeartBeat);
        }

        private void HostService()
        {
            var baseAddress = new Uri("http://localhost/pluginService");
            using (var host = new ServiceHost(typeof(PluginService), baseAddress))
            {
                host.Open();
                while (serviceIsOnline)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private void LoadTestHeartBeat(object sender, HeartbeatEventArgs e)
        {
            if (!e.IsWarmupComplete)
                return;

            loadTest.Scenarios[0].CurrentLoad = userLoad;
        }

        public void Dispose()
        {
            serviceIsOnline = false;
            task.Wait();
        }
    }

    [ServiceContract]
    public interface IPluginService
    {
        [OperationContract]
        void SetVirtualUserLoad(int virtualUsers);
    }

    public class PluginService : IPluginService
    {
        public void SetVirtualUserLoad(int virtualUsers)
        {
            DynamicUserCountPlugin.UserLoad = virtualUsers;
        }
    }
}