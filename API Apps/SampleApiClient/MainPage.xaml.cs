using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Rest;
using Microsoft.WindowsAzure.MobileServices;
using SampleApiClient.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SampleApiClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MobileServiceClient client;
        private string token;

        public MainPage()
        {
            this.InitializeComponent();
            client = new MobileServiceClient("https://newsapicodemotion2016.azurewebsites.net");
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await RefreshAsync();
        }

        public async Task LoginAsync()
        {
            var mobileServiceUser = await client.LoginAsync(MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory);
            token = mobileServiceUser.MobileServiceAuthenticationToken;

        }

        public async Task RefreshAsync()
        {
            try
            {
                NewsApiCodemotion2016 apiClient =
                    new NewsApiCodemotion2016(new Uri("https://newsapicodemotion2016.azurewebsites.net"));
                apiClient.HttpClient.DefaultRequestHeaders.Add("X-ZUMO-AUTH", token);
                IList<FeedItem> items = await apiClient.News.GetAsync();
                Items.ItemsSource = items;
            }
            catch (HttpOperationException exc)
            {
                Debug.WriteLine(exc.Message);
            }
        }

        private async void OnLogin(object sender, RoutedEventArgs e)
        {
            await LoginAsync();
        }

        private async void OnRefresh(object sender, RoutedEventArgs e)
        {
            await RefreshAsync();
        }
    }
}

