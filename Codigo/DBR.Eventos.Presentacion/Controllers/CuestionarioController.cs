using DBR.Evento.Modelo.Request;
using DBR.Eventos.Negocio.Implementacion;
using DBR.Eventos.Presentacion.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBR.Eventos.Presentacion.Controllers
{
    public class CuestionarioController : BaseController
    {
        CuestionarioNegocio cuestionarioNegocio = new CuestionarioNegocio();

        public ActionResult Editar(int idLeccion, int idCurso)
        {
            var cuestionario = cuestionarioNegocio.GetCuestionario(new CuestionarioRequest { IdLeccion = idLeccion });

            ViewBag.Titulo = cuestionario.Nombre;
            ViewBag.Peso = cuestionario.Peso;
            ViewBag.DescripcionHijo = "Detalle de cuestionario";
            ViewBag.DescripcionPadre = "Cuestionarios";
            ViewBag.IdLeccion = idLeccion;
            ViewBag.IdCuestionario = cuestionario.IdCuestionario;
            ViewBag.IdCurso = idCurso;
            cuestionario.Preguntas = cuestionarioNegocio.ListPregunta(new PreguntaRequest { IdCuestionario = cuestionario.IdCuestionario });

            return ValidarSesion(System.Reflection.MethodBase.GetCurrentMethod(),cuestionario);
        }



        #region Metodos Preguntas

        //Modulo
        [HttpPost]
        public ActionResult ListPreguntaPaged(PageRequest page, PreguntaRequest request)
        {
            var response = cuestionarioNegocio.ListPreguntaPaged(page, request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SavePregunta(PreguntaRequest request)
        {
            var response = request.IdPregunta == 0 ? cuestionarioNegocio.SavePregunta(request, getUser()) : cuestionarioNegocio.UpdatePregunta(request, getUser());
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeletePregunta(PreguntaRequest request)
        {
            var response = cuestionarioNegocio.DeletePregunta(request, getUser());
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetPregunta(PreguntaRequest request)
        {
            var response = cuestionarioNegocio.GetPregunta(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}