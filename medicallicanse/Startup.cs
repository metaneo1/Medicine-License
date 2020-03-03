using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(medicallicanse.Startup))]
namespace medicallicanse
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
