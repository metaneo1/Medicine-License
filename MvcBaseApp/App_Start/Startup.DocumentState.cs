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
        public void ConfigureDocumentStates(IAppBuilder app)
        {
            var entities = new MedlicenseEntities();
            var states = entities.DocumentState.ToList();

            foreach (var state in states)
            {
                switch (state.CODE)
                {
                    case "BACKLOG":
                        Const.DocumentStateId.Backlog = state.Id;
                        break;
                    case "PROGRESS":
                        Const.DocumentStateId.InProgress = state.Id;
                        break;
                    case "REOPEN":
                        Const.DocumentStateId.Reopened = state.Id;
                        break;
                    case "ACCEPT":
                        Const.DocumentStateId.Accepted = state.Id;
                        break;
                }
            }
        }
    }
}