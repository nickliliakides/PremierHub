using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PremierHub.Startup))]
namespace PremierHub
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
