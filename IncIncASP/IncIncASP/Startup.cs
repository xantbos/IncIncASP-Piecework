using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IncIncASP.Startup))]
namespace IncIncASP
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
