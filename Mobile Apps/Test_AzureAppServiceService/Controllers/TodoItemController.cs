using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
using Test_AzureAppServiceService.DataObjects;
using Test_AzureAppServiceService.Models;

namespace Test_AzureAppServiceService.Controllers
{
    public class TodoItemController : TableController<TodoItem>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            Test_AzureAppServiceContext context = new Test_AzureAppServiceContext();
            DomainManager = new EntityDomainManager<TodoItem>(context, Request);
        }

        // GET tables/TodoItem
        [Authorize]
        public async Task<IQueryable<TodoItem>> GetAllTodoItems()
        {
            var query = Query();
            var credentials = await this.User.GetAppServiceIdentityAsync<FacebookCredentials>(this.Request);
            return query.Where(x => x.UserId == credentials.UserId);
        }

        // GET tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<TodoItem> GetTodoItem(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<TodoItem> PatchTodoItem(string id, Delta<TodoItem> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/TodoItem
        [Authorize]
        public async Task<IHttpActionResult> PostTodoItem(TodoItem item)
        {
            var credentials = await this.User.GetAppServiceIdentityAsync<FacebookCredentials>(this.Request);
            item.UserId = credentials.UserId;

            //var fbRequestUrl = "https://graph.facebook.com/me?access_token=" + credentials.AccessToken;

            //// Create an HttpClient request.
            //var client = new System.Net.Http.HttpClient();

            //// Request the current user info from Facebook.
            //var resp = await client.GetAsync(fbRequestUrl);
            //resp.EnsureSuccessStatusCode();

            //// Do something here with the Facebook user information.
            //var fbInfo = await resp.Content.ReadAsStringAsync();

            TodoItem current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/TodoItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteTodoItem(string id)
        {
            return DeleteAsync(id);
        }
    }
}