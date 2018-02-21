using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(badmintonclubService.Startup))]

namespace badmintonclubService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}