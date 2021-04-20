using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LibrARRRy.Startup))]
namespace LibrARRRy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
