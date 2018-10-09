using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RMA.Startup))]
namespace RMA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
