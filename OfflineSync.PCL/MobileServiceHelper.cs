using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using OfflineSync.PCL.Model;

namespace OfflineSync.PCL
{
    public class MobileServiceHelper
    {
        public MobileServiceClient Client { get; set; }

        public MobileServiceHelper()
        {
            Client = new MobileServiceClient(
                "https://todosamplednl.azurewebsites.net");
        }

        public async Task SyncAsync()
        {
            var table = Client.GetSyncTable<TodoItem>();
            try
            {
                await Client.SyncContext.PushAsync();
                await
                    table.PullAsync("todoItems" + Client.CurrentUser.UserId,
                        table.Where(x => x.UserId == Client.CurrentUser.UserId));
            }
            catch (MobileServicePushFailedException ex)
            {
                Debug.WriteLine(ex.PushResult.Status);
            }
        }

        public  async Task InitializeDatabaseAsync()
        {
            if (!Client.SyncContext.IsInitialized)
            {
                var store = new MobileServiceSQLiteStore("todos.db");
                store.DefineTable<TodoItem>();
                await Client.SyncContext.InitializeAsync(store, new SyncHandling());
            }
        }

        public void SetLoggedUser(string userId)
        {
            MobileServiceUser user = new MobileServiceUser(userId);
            Client.CurrentUser = user;
        }

        public async Task<IEnumerable<TodoItem>> GetItemsAsync()
        {
            var table = Client.GetSyncTable<TodoItem>();
            var items = await table.Where(x => x.UserId == Client.CurrentUser.UserId).ToEnumerableAsync();
            return items;
        }

        public async Task AddItemAsync(TodoItem item)
        {
            var table = Client.GetSyncTable<TodoItem>();
            await table.InsertAsync(item);
        }

        public async Task UpdateItemAsync(TodoItem item)
        {
            var table = Client.GetSyncTable<TodoItem>();
            await table.UpdateAsync(item);
        }

        public async Task DeleteItemAsync(TodoItem item)
        {
            var table = Client.GetSyncTable<TodoItem>();
            await table.DeleteAsync(item);
        }

    }
}
