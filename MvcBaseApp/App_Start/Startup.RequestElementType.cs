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
        public void ConfigureRequestElementStructureTypes(IAppBuilder app)
        {
            var entities = new MedlicenseEntities();
            var states = entities.RequestElemStructureType.ToList();

            foreach (var state in states)
            {
                switch (state.CODE)
                {
                    case "ELEMENT":
                        Const.RequestElementStructureTypeId.Element = state.Id;
                        break;
                    case "GROUP":
                        Const.RequestElementStructureTypeId.Group = state.Id;
                        break;
                    case "SUBGROUP":
                        Const.RequestElementStructureTypeId.SubGroup = state.Id;
                        break;
                }
            }
        }
    }
}