using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Online_Store.Startup))]
namespace Online_Store
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
