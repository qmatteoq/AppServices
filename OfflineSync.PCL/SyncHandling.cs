using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json.Linq;

namespace OfflineSync.PCL
{
    public class SyncHandling: IMobileServiceSyncHandler
    {
        public async Task OnPushCompleteAsync(MobileServicePushCompletionResult result)
        {
            Debug.WriteLine("Push completed");
            await Task.FromResult<object>(null);
        }

        public async Task<JObject> ExecuteTableOperationAsync(IMobileServiceTableOperation operation)
        {
            Debug.WriteLine("Execute table");
            JObject result = null;
            try
            {
                result = await operation.ExecuteAsync();
            }
            catch (MobileServiceInvalidOperationException exc)
            {
                return await Task.FromResult<JObject>(null);
            }

            return result;
        }
    }
}
