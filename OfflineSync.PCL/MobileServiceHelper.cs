using System;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using OfflineSync.PCL.Model;

namespace OfflineSync.PCL
{
    public class MobileServiceHelper
    {
        public MobileServiceClient Client { get; set; }

        public string Path { get; }

        public MobileServiceHelper(string path)
        {
            Client = new MobileServiceClient(
                "https://test-azureappservice.azurewebsites.net");
            this.Path = path;
        }

        public async Task SyncAsync()
        {
            var table = Client.GetSyncTable<ToDoItem>();
            try
            {
                await Client.SyncContext.PushAsync();
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
            if (!Client.SyncContext.IsInitialized)
            {
                var store = new MobileServiceSQLiteStore(Path);
                store.DefineTable<ToDoItem>();
                await Client.SyncContext.InitializeAsync(store);
            }
        }

        public void SetLoggedUser(string userId)
        {
            MobileServiceUser user = new MobileServiceUser(userId);
            Client.CurrentUser = user;
        }

        public async Task<IEnumerable<ToDoItem>> GetItemsAsync()
        {
            var table = Client.GetSyncTable<ToDoItem>();
            var items = await table.ToEnumerableAsync();
            return items;
        }

        public async Task AddItemAsync(ToDoItem item)
        {
            var table = Client.GetSyncTable<ToDoItem>();
            await table.InsertAsync(item);
        }

        public async Task UpdateItemAsync(ToDoItem item)
        {
            var table = Client.GetSyncTable<ToDoItem>();
            await table.UpdateAsync(item);
        }

        public async Task DeleteItemAsync(ToDoItem item)
        {
            var table = Client.GetSyncTable<ToDoItem>();
            await table.DeleteAsync(item);
        }
    }
}
