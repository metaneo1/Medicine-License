using System.Linq;
using DataModel;
using DataModel.Const;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using MvcBaseApp.Controllers;
using Owin;

namespace MvcBaseApp
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureFiles(IAppBuilder app)
        {
            Const.FilePath.PATH = System.Configuration.ConfigurationManager.AppSettings["FilePath"];
        }
    }
}