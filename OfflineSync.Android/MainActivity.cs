using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Widget;
using Microsoft.WindowsAzure.MobileServices;
using OfflineSync.Droid;
using OfflineSync.PCL;

namespace OfflineSync.Droid
{
    [Activity(Label = "OfflineSync.Droid", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private MobileServiceHelper _helper;

        private string userId;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _helper = new MobileServiceHelper();

            await LoginAsync();

            await _helper.InitializeDatabaseAsync();

            await _helper.SyncAsync();

            var items = await _helper.GetItemsAsync();
            List<string> notes = items.Select(x => x.Text).ToList();

            ListView listView = this.FindViewById<ListView>(Resource.Id.Items);
            listView.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, notes);
            listView.ItemClick += ListView_ItemClick;
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async Task LoginAsync()
        {
            MobileServiceUser user =
                await _helper.Client.LoginAsync(this, MobileServiceAuthenticationProvider.Twitter);
            _helper.SetLoggedUser(user.UserId);
            userId = user.UserId;
        }
    }
}

