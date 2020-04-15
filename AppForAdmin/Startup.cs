using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcBaseApp.Startup))]
namespace MvcBaseApp
{
    public partial class Startup
    {
        public const string THEME = "Office2010Blue";
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
