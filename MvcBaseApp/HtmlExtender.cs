using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using MvcBaseApp.Controllers;

namespace MvcBaseApp
{
    public static class HtmlExtender
    {
        public static bool HasRoleView(this HtmlHelper helper, HttpSessionStateBase session, string controlName)
        {
            return true;
            var allControls = (Lookup<string, int>)session[InPageAutorizeController.UI_ROLES_KEY];
            var availableTypes = allControls[controlName];
            if (availableTypes.Any(x => x != (int)WebControlAccessType.None))
                return true;
            return false;
        }

        public static bool HasRoleEnable(this HtmlHelper helper, HttpSessionStateBase session, string controlName)
        {
            return true;
            var allControls = (Lookup<string, int>)session[InPageAutorizeController.UI_ROLES_KEY];
            var availableTypes = allControls[controlName];
            if (availableTypes.Any(x => x == (int)WebControlAccessType.Enabled))
                return true;
            return false;
        }

        public static MvcHtmlString MyTextBoxFor<T, TProp>(this HtmlHelper<T> html,Expression<Func<T, TProp>> expr)
        {
            return MvcHtmlString.Create("");
        }

    }

}