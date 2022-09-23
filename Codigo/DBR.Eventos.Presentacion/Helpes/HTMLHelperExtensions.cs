using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBR.Eventos.Presentacion
{
    public static class HMTLHelperExtensions
    {
        public static string IsSelected(this HtmlHelper html, string area = null, string controller = null, string action = null, string cssClass = null)
        {

            if (String.IsNullOrEmpty(cssClass))
                cssClass = "active";

            string currentAction = string.Empty;
            string currentController = string.Empty;
            string currentArea = string.Empty;

            if (html.ViewContext.ParentActionViewContext != null)
            {
                currentAction = (string)html.ViewContext.ParentActionViewContext.RouteData.Values["action"];
                currentController = (string)html.ViewContext.ParentActionViewContext.RouteData.Values["controller"];
                currentArea = (string)html.ViewContext.ParentActionViewContext.RouteData.DataTokens["area"];
            }
            else
            {
                currentAction = (string)html.ViewContext.RouteData.Values["action"];
                currentController = (string)html.ViewContext.RouteData.Values["controller"];
                currentArea = (string)html.ViewContext.RouteData.DataTokens["area"];
            }

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            if (String.IsNullOrEmpty(area))
                area = currentArea;

            if (controller.Split(',').Length > 1)
            {
                return controller.Split(',').Contains(currentController) && action == currentAction && area == currentArea ? cssClass : string.Empty;
            }
            else
            {
                return controller == currentController && action == currentAction && area == currentArea ?
                    cssClass : String.Empty;
            }
        }
    }
}