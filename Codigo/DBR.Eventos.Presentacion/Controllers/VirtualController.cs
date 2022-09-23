using DBR.Evento.Modelo;
using DBR.Evento.Modelo.Request;
using DBR.Evento.Modelo.Response;
using DBR.Eventos.Comun;
using DBR.Eventos.Negocio.Implementacion;
using DBR.Eventos.Presentacion.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DBR.Eventos.Presentacion.Controllers
{
    public class VirtualController : BaseController
    {
        VirtualNegocio _virtualNegocio = new VirtualNegocio();
        EventoNegocio _eventoNegocio = new EventoNegocio();
        UsuarioNegocio _usuarioNegocio = new UsuarioNegocio();
        public ActionResult Configuracion()
        {
            return ValidarSesionOpciones(System.Reflection.MethodBase.GetCurrentMethod());
        }
        public ActionResult AccesoUsuario()
        {
            return ValidarSesionOpciones(System.Reflection.MethodBase.GetCurrentMethod());
        }
        public ActionResult ContenidoVirtual()
        {
            return ValidarSesionOpciones(System.Reflection.MethodBase.GetCurrentMethod());
        }
        public ActionResult ContenidoEvento(string Id)
        {
            if (Session[NameSession.IdUsuario] != null)
            {
                if (Id != null)
                {
                    EventoRequest request = new EventoRequest();
                    request.rowid = Guid.Parse(Id);
                    var evento = _eventoNegocio.GetEventoByRowId(request, getUser());
                    if (evento != null)
                    {
                        ViewBag.IdEvento = evento.IdEvento;
                        ViewBag.Evento = evento.NombreEvento;
                    }
                    else
                    {
                        ViewBag.IdEvento = 0;
                        ViewBag.Evento = "NO SE ENCONTRO EVENTO ACTIVO";
                    }
                }
                else
                {
                    ViewBag.IdEvento = 0;
                    ViewBag.Evento = "";
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
            
        }


        #region Metodos Configuracion
        //Virtual contenido
        [HttpPost]
        public ActionResult ListAllEventoCombo()
        {
            var response = _eventoNegocio.ListAllEventoCombo();
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetContenidoVirtualByEvento(VirtualContenidoRequest request)
        {
            var response = _virtualNegocio.GetContenidoVirtualByEvento(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveVirtualContenido(VirtualContenidoRequest request)
        {          
            try
            {
                var response = new Result();
                if (request.IdVirtualContenido == 0)
                {
                    response = _virtualNegocio.SaveVirtualContenido(request, getUser());
                }
                else
                {
                    response = _virtualNegocio.UpdateVirtualContenido(request, getUser());
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Result result = new Result();
                result.IsSuccess = false;
                result.Message = Message.ErrorNoControlado;
                result.MessageExeption = ex.Message.ToString();
                result.StackTrace = ex.StackTrace.ToString();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        //VirtualVideo
        [HttpPost]
        public ActionResult ListVirtualVideoByEvento(VirtualContenidoRequest request)
        {
            var response = _virtualNegocio.ListVirtualVideoByEvento(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveVirtualVideo(VirtualVideoRequest request)
        {           
            try
            {
                var response = new Result();
                if (request.IdVirtualVideo == 0)
                {
                    response = _virtualNegocio.SaveVirtualVideo(request, getUser());
                }else
                {
                    response = _virtualNegocio.UpdateVirtualVideo(request, getUser());
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Result result = new Result();
                result.IsSuccess = false;
                result.Message = Message.ErrorNoControlado;
                result.MessageExeption = ex.Message.ToString();
                result.StackTrace = ex.StackTrace.ToString();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult GetVirtualVideo(VirtualVideoRequest request)
        {
            var response = _virtualNegocio.GetVirtualVideo(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteVirtualVideo(VirtualVideoRequest request)
        {           
            try
            {
                var response = _virtualNegocio.DeleteVirtualVideo(request, getUser());
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Result result = new Result();
                result.IsSuccess = false;
                result.Message = Message.ErrorNoControlado;
                result.MessageExeption = ex.Message.ToString();
                result.StackTrace = ex.StackTrace.ToString();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Metodos Asignacion
        [HttpPost]
        public ActionResult ListEventoAsignacionPaged(PageRequest page)
        {
            var response = _eventoNegocio.ListEventoAsignacionPaged(page);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ListEventoUsuarioAsignadoPaged(PageRequest page, EventoUsuarioRequest request)
        {
            var response = _eventoNegocio.ListEventoUsuarioAsignadoPaged(page, request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveEventoUsuario(EventoUsuarioRequest request, List<int> requestVideos)
        {
            try
            {
                requestVideos = requestVideos ?? new List<int>();
                var response = new Result();
                if (request.IdEventoUsuario == 0)
                {
                    response = _eventoNegocio.SaveEventoUsuario(request, getUser(), requestVideos);
                }
                else
                {
                    response = _eventoNegocio.UpdateEventoUsuario(request, getUser(), requestVideos);
                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Result result = new Result();
                result.IsSuccess = false;
                result.Message = Message.ErrorNoControlado;
                result.MessageExeption = ex.Message.ToString();
                result.StackTrace = ex.StackTrace.ToString();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DeleteEventoUsuario(EventoUsuarioRequest request)
        {            
            try
            {
                var response = _eventoNegocio.DeleteEventoUsuario(request, getUser());
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Result result = new Result();
                result.IsSuccess = false;
                result.Message = Message.ErrorNoControlado;
                result.MessageExeption = ex.Message.ToString();
                result.StackTrace = ex.StackTrace.ToString();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult ListUsuarioSinAsignar(EventoUsuarioRequest request)
        {
            var response = _usuarioNegocio.ListUsuarioSinAsignar(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ListEventoUsuarioVirtualVideoByEvento(VirtualContenidoRequest request)
        {
            var response = _virtualNegocio.ListEventoUsuarioVirtualVideoByEvento(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Metodos ContenidoVirtual
        [HttpPost]
        public ActionResult ListEventoUsuarioPaged(PageRequest page)
        {
            var response = _eventoNegocio.ListEventoUsuarioPaged(page, getUser());
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ContenidoEvento

        [HttpPost]
        public ActionResult ListVirtualVideoByUsuarioByEvento(VirtualContenidoRequest request)
        {
            var response = _virtualNegocio.ListVirtualVideoByUsuarioByEvento(request, getUser());
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}