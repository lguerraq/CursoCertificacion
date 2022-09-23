using DBR.Eventos.Presentacion.Controllers.Base;
using System;
using System.Web.Mvc;

namespace DBR.Eventos.Presentacion.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public ActionResult Bienvenida()
        {
            return ValidarSesion(System.Reflection.MethodBase.GetCurrentMethod());
        }
        public ActionResult HoraServidor()
        {
            var response = System.Threading.Thread.CurrentThread.CurrentCulture.NativeName;
            var localZone = TimeZone.CurrentTimeZone;         
            var Fecha = DateTime.Now;
            var zona = localZone.GetUtcOffset(Fecha);

            return Json(response + ":" + Fecha.ToString() + ":" + zona.ToString(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Error()
        {
            return View("Error");
        }
    }
}