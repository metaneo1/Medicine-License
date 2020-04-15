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
        public void ConfigureRequestTypes(IAppBuilder app)
        {
            var entities = new MedlicenseEntities();
            var states = entities.RequestState.ToList();

            foreach (var state in states)
            {
                switch (state.CODE)
                {
                    case "BACKLOG":
                        Const.RequestStateId.Unassigned = state.Id;
                        break;
                    case "PROGRESS":
                        Const.RequestStateId.InProgress = state.Id;
                        break;
                    case "REOPEN":
                        Const.RequestStateId.Reopen = state.Id;
                        break;
                    case "FINISHED":
                        Const.RequestStateId.Finished = state.Id;
                        break;
                }
            }
        }
    }
}