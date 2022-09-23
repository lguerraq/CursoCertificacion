using DBR.Eventos.Negocio.Implementacion;
using System.Linq;
using System.Web.Mvc;

namespace DBR.Eventos.Presentacion.Controllers
{
    public class RegistroController : Controller
    {
        EventoNegocio _eventoNegocio = new EventoNegocio();
        GeneralNegocio _generalNegocio = new GeneralNegocio();
        public ActionResult Index(int? Id)
        {
            var EventosActivos = _eventoNegocio.ListEventoActivos();
            if (Id != null)
            {
                EventosActivos = EventosActivos.Where(x => x.IdEvento == Id).ToList();
            }
            var Ocupacion = _generalNegocio.ListTipoCombo("OCUPACION");
            var Profesion = _generalNegocio.ListProfesion();
            ViewBag.EventosActivos = EventosActivos;
            ViewBag.Ocupacion = Ocupacion;
            ViewBag.Profesion = Profesion;
            ViewBag.NumeroRelease = Configuracion.NumeroRelease;
            return View();
        }
    }
}