using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TianHuiWeb.Startup))]
namespace TianHuiWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
