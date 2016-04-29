using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Test_AzureAppServiceService.Startup))]

namespace Test_AzureAppServiceService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}