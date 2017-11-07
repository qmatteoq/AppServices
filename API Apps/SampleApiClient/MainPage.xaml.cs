using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Rest;
using SampleApiClient.Models;

namespace SampleApiClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await RefreshAsync();
        }

        public async Task RefreshAsync()
        {
            try
            {
                NewsApiCodemotion2016 apiClient =
                    new NewsApiCodemotion2016(new Uri("https://newsapicodemotion2016.azurewebsites.net"), new BasicAuthenticationCredentials());
                IList<FeedItem> items = await apiClient.ApiNewsGetAsync();
                Items.ItemsSource = items;
            }
            catch (HttpOperationException exc)
            {
                Debug.WriteLine(exc.Message);
            }
        }

        private async void OnRefresh(object sender, RoutedEventArgs e)
        {
            await RefreshAsync();
        }
    }
}

