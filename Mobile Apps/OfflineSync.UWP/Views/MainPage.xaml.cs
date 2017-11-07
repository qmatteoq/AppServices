using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using OfflineSync.UWP.Model;

namespace OfflineSync.UWP.Views
{
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<TodoItem> todoItems { get; set; }

        MobileServiceClient client = new MobileServiceClient("https://appservice-sample.azurewebsites.net");

        private string userId;

        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await LoginAsync();
            await InitializeDatabaseAsync();
            await Refresh();
        }

        private async Task LoginAsync()
        {
            MobileServiceUser user = await client.LoginAsync(MobileServiceAuthenticationProvider.Facebook, true);
        }

        private async void OnAddClicked(object sender, RoutedEventArgs e)
        {
            TodoItem item = new TodoItem
            {
                Text = Name.Text,
                Complete = false,
                UserId = userId
            };

            await AddItemAsync(item);
            await SyncAsync();
            todoItems.Add(item);
        }

        private void OnNoteSelected(object sender, ItemClickEventArgs e)
        {
            TodoItem selectedItem = e.ClickedItem as TodoItem;
            Frame.Navigate(typeof(DetailView), selectedItem);
        }

        private async void OnRefreshClicked(object sender, RoutedEventArgs e)
        {
            await Refresh();
        }

        public async Task Refresh()
        {
            await SyncAsync();

            var items = await GetItemsAsync();
            todoItems = new ObservableCollection<TodoItem>(items);
            Items.ItemsSource = todoItems;
        }

        public async Task SyncAsync()
        {
            var table = client.GetSyncTable<TodoItem>();
            try
            {
                await client.SyncContext.PushAsync();
                await
                    table.PullAsync("allTodoItems", table.CreateQuery());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task InitializeDatabaseAsync()
        {
            if (!client.SyncContext.IsInitialized)
            {
                var store = new MobileServiceSQLiteStore("todo.db");
                store.DefineTable<TodoItem>();
                await client.SyncContext.InitializeAsync(store);
            }
        }

        public async Task AddItemAsync(TodoItem item)
        {
            var table = client.GetSyncTable<TodoItem>();
            await table.InsertAsync(item);
        }

        public async Task UpdateItemAsync(TodoItem item)
        {
            var table = client.GetSyncTable<TodoItem>();
            await table.UpdateAsync(item);
        }

        public async Task DeleteItemAsync(TodoItem item)
        {
            var table = client.GetSyncTable<TodoItem>();
            await table.DeleteAsync(item);
        }

        public async Task<IEnumerable<TodoItem>> GetItemsAsync()
        {
            var table = client.GetSyncTable<TodoItem>();
            var items = await table.ToEnumerableAsync();
            return items;
        }
    }
}
