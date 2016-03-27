using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.ServiceBus.Notifications;
using NotificationHubSample.Server.Models;
using System.Collections.ObjectModel;

namespace NotificationHubSample.Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<DeviceRegistration> devices;

        private const string ConnectionString = "Endpoint=sb://windows10samples.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=DlbQZoLHpq49BNJbP9YmkRVPoN4jqCfnJZwt+vAHU24=";

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override async void OnActivated(EventArgs e)
        {
            await RefreshDevices();
        }

        private async Task RefreshDevices()
        {
            NotificationHubClient client = NotificationHubClient.CreateClientFromConnectionString(ConnectionString, "uwpsample");
            CollectionQueryResult<RegistrationDescription> results = await client.GetAllRegistrationsAsync(0);
            var devicesList = results.Select(x => new DeviceRegistration
            {
                RegistrationId = x.RegistrationId,
                ExpirationTime = x.ExpirationTime.Value,
                Tags = RetrieveTags(x.Tags)
            });

            devices = new ObservableCollection<DeviceRegistration>(devicesList);
            Devices.ItemsSource = devices;
        }

        private string RetrieveTags(ISet<string> tags)
        {
            StringBuilder builder = new StringBuilder();
            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    builder.Append($"{tag},");
                }

                return builder.ToString();
            }

            return string.Empty;
        }

        private async void OnSendNativeNotification(object sender, RoutedEventArgs e)
        {
            string xml = @"<toast>
                <visual>
                <binding template=""ToastGeneric"">
                    <text>Hello insiders!</text>
                    <text>This is a notification from Notification Hub</text>
                </binding>
                </visual>
            </toast>";

            NotificationHubClient client = NotificationHubClient.CreateClientFromConnectionString(ConnectionString, "uwpsample");
            await client.SendWindowsNativeNotificationAsync(xml);
        }

        private async void OnDeleteDeviceClicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            DeviceRegistration device = (button.DataContext) as DeviceRegistration;

            NotificationHubClient client = NotificationHubClient.CreateClientFromConnectionString(ConnectionString, "uwpsample");
            await client.DeleteRegistrationAsync(device.RegistrationId);
            devices.Remove(device);
        }

        private async void OnRefreshClicked(object sender, RoutedEventArgs e)
        {
            await RefreshDevices();
        }

        private async void OnSendTemplateNotification(object sender, RoutedEventArgs e)
        {
            NotificationHubClient client = NotificationHubClient.CreateClientFromConnectionString(ConnectionString, "uwpsample");
            Dictionary<string, string> properties = new Dictionary<string, string>();
            properties.Add("title", "Template notification");
            properties.Add("message", "This is a template notification");
            await client.SendTemplateNotificationAsync(properties);
        }
    }
}
