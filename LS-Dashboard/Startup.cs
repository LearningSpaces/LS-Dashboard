using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LS_Dashboard.Startup))]
namespace LS_Dashboard
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
