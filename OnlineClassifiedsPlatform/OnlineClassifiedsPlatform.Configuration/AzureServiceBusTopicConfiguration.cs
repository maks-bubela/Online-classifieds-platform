using OnlineClassifiedsPlatform.Configuration;

namespace OnlineClassifiedsPlatform.Configuration
{
    public class AzureServiceBusTopicConfiguration
    {
        private const string NotifyUserTopicPathName = "NotifyUsersTopicPath";
        public string NotifyUserTopicPath { get; private set; }
        private AzureServiceBusTopicConfiguration(string notifyUserTopicPath)
        {
            NotifyUserTopicPath = notifyUserTopicPath;
        }
        public static AzureServiceBusTopicConfiguration CreateFromConfigurations()
        {
            return new AzureServiceBusTopicConfiguration(
                notifyUserTopicPath: AppSettings.CreateFromConfigurations(NotifyUserTopicPathName).SettingsValue);
        }
    }
}
