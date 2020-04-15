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
        public void ConfigureDocumentFormats(IAppBuilder app)
        {
            var entities = new MedlicenseEntities();
            var states = entities.Document_Format.ToList();

            foreach (var state in states)
            {
                switch (state.CODE)
                {
                    case "PDF":
                        Const.DocumentFormatId.Pdf = state.Id;
                        break;
                    case "IMAGE":
                        Const.DocumentFormatId.Image = state.Id;
                        break;
                }
            }
        }
    }
}