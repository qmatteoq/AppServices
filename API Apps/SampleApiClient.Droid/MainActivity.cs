using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using SampleApiClient.Models;
using Microsoft.Rest;
using System;
using System.Linq;
using System.Threading.Tasks;
using Android.Views;

namespace SampleApiClient.Droid
{
    [Activity(Label = "SampleApiClient.Droid", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private Button refresNewsButton;

        private ListView listNews;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            listNews = this.FindViewById<ListView>(Resource.Id.News);

            await RefreshAsync();
        }

        [Java.Interop.Export()]
        public async void RefreshNews(View view)
        {
            await RefreshAsync();
        }

        public async Task RefreshAsync()
        {
            NewsApiCodemotion2016 apiClient =
                new NewsApiCodemotion2016(new Uri("https://newsapicodemotion2016.azurewebsites.net"), new BasicAuthenticationCredentials());
            IList<FeedItem> items = await apiClient.ApiNewsGetAsync();
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, items.Select(x => x.Title).ToList());
            listNews.Adapter = adapter;
        }
    }
}

