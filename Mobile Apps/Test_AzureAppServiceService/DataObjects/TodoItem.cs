using Microsoft.Azure.Mobile.Server;

namespace Test_AzureAppServiceService.DataObjects
{
    public class TodoItem : EntityData
    {
        public string Text { get; set; }

        public bool Complete { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }
    }
}