using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Amido.Testing.Azure
{
    public static class Configuration
    {
        public static void SetConfigurationSettingPublisher()
        {
            CloudStorageAccount.SetConfigurationSettingPublisher((configName, configSetter) =>
            {
                configSetter(RoleEnvironment.GetConfigurationSettingValue(configName));
                RoleEnvironment.Changed += (anotherSender, arg) =>
                {
                    if (arg.Changes.OfType<RoleEnvironmentConfigurationSettingChange>()
                    .Any(change => (change.ConfigurationSettingName == configName)))
                    {
                        if (!configSetter(RoleEnvironment.GetConfigurationSettingValue(configName)))
                        {
                            RoleEnvironment.RequestRecycle();
                        }
                    }
                };
            });
        }
    }
}
