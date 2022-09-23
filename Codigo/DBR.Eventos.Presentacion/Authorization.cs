using DBR.Eventos.Comun;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DBR.Eventos.Presentacion
{
    public class Authorization : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string url = filterContext.HttpContext.Request.RawUrl;
            if (HttpContext.Current.Session[NameSession.IdUsuario] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                                new RouteValueDictionary(
                                    new
                                    {
                                        controller = "Login",
                                        action = "Index",
                                        returnUrl = url
                                    }));
            }
        }
    }
}