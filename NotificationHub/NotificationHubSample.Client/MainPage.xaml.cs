using System;
using Windows.Networking.PushNotifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.WindowsAzure.Messaging;

namespace NotificationHubSample.Client
{
    public sealed partial class MainPage : Page
    {
        private const string ConnectionString =
            "Endpoint=sb://windows10samples.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=DlbQZoLHpq49BNJbP9YmkRVPoN4jqCfnJZwt+vAHU24=";

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void OnRegisterForNativeNotifications(object sender, RoutedEventArgs e)
        {
            PushNotificationChannel channel =
                await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            if (channel != null)
            {
                string result = $"Registration successfull: the channel url is {channel.Uri}";
                Result.Text = result;
                NotificationHub hub = new NotificationHub("uwpsample", ConnectionString);
                await hub.RegisterNativeAsync(channel.Uri);
            }
        }

        private async void OnRegisterForTemplateNotifications(object sender, RoutedEventArgs e)
        {
            PushNotificationChannel channel =
                await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            if (channel != null)
            {
                string result = $"Registration successfull: the channel url is {channel.Uri}";
                Result.Text = result;
                NotificationHub hub = new NotificationHub("uwpsample", ConnectionString);
                var xml =
                    "<toast><visual><binding template=\"ToastGeneric\"><text>$(title)</text><text>$(message)</text></binding></visual></toast>";

                await hub.RegisterTemplateAsync(channel.Uri, xml, "SampleTemplate");
            }
        }
    }
}
