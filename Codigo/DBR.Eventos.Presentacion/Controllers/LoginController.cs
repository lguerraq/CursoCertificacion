using DBR.Evento.Modelo;
using DBR.Evento.Modelo.Request;
using DBR.Evento.Modelo.Response;
using DBR.Eventos.Comun;
using DBR.Eventos.Negocio.Implementacion;
using DBR.Eventos.Presentacion.Controllers.Base;
using DBR.Eventos.Presentacion.Helpes;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DBR.Eventos.Presentacion.Controllers
{
    public class LoginController : BaseController
    {
        UsuarioNegocio _usuarioNegocio = new UsuarioNegocio();

        public ActionResult Index()
        {            
            ViewBag.NumeroRelease = Configuracion.NumeroRelease;
            ViewBag.LoginOut = "";
            var faCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (faCookie != null)
            {
                Session.Clear();
                Session.Abandon();

                Session.Abandon();
                FormsAuthentication.SignOut();

                HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Remove(cookie.ToString());
                }
            }
            return View();
        }
        public ActionResult SessionOut()
        {
            ViewBag.NumeroRelease = Configuracion.NumeroRelease;
            ViewBag.LoginOut = "Se ha iniciado sesion en otro dispositivo";
            return View("Index");
        }
        public ActionResult AccesoDenegado()
        {
            return ValidarSesion(System.Reflection.MethodBase.GetCurrentMethod());
        }
        public ActionResult ListUsuario()
        {           
            var response = _usuarioNegocio.ListUsuario();
            return Json(response,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ValidarUsuario(UsuarioRequest request)
        {
            request.Login = request.Login.Trim();
            request.Password = request.Password.Trim();

            Result result = new Result();

            if (!ValidarReCapcha(request.Capcha))
            {
                result.IsSuccess = false;
                result.Message = Message.ErrorCapcha;
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            var response = _usuarioNegocio.BuscarUsuarioXlogin(request);
            if (response.Count()==0)
            {
                result.Message = Message.DatosIncorrectos;
                result.IsSuccess = false;
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            if (response[0].Password != request.Password)
            {
                result.Message = Message.DatosIncorrectos;
                result.IsSuccess = false;
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            response[0].Token = Guid.NewGuid().ToString();

            //Creamos Cookies sesion
            CrearSession(response[0]);

            //Creamo Cookies Menu
            CrearMenu(response[0]);

            //GUARDA UN HISTORICO
            request.IdUsuario = response[0].IdUsuario;
            var responseHis = _usuarioNegocio.SaveUsuarioHistorico(request);
            UsuarioLogin userRequest = new UsuarioLogin();
            userRequest.IdUsuario = response[0].IdUsuario;
            userRequest.Token = response[0].Token;
            var ressponseAc = _usuarioNegocio.SaveUsuarioAcceso(userRequest);

            result.IsSuccess = true;
            result.Message = Message.DatosCorrectos;

            if(response[0].IdUsuarioTipo == 4)
            {
                result.Informacion = Url.Action("Index", "Cursos", new { area = "Aula" });
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CambiarPassword(UsuarioRequest usuario)
        {
            Result resul = new Result();

            if (Session[NameSession.Password].ToString() != usuario.OldPassword)
            {
                resul.IsSuccess = false;
                resul.Message = Message.ErrorOldPassword;
                return Json(resul, JsonRequestBehavior.AllowGet);
            }
            if (Session[NameSession.Password].ToString() == usuario.NewPassword)
            {
                resul.IsSuccess = false;
                resul.Message = Message.ErrorMismoPassword;
                return Json(resul, JsonRequestBehavior.AllowGet);
            }
            if (!ValidarConplejidadPassword(usuario.NewPassword))
            {
                resul.IsSuccess = false;
                resul.Message = Message.ErrorConplejidadPassword;
                return Json(resul, JsonRequestBehavior.AllowGet);
            }
            resul = _usuarioNegocio.UpdatePasswordUsuario(usuario, getUser());

            if (resul.IsSuccess)
            {
                Session[NameSession.Password] = usuario.NewPassword;
            }
            return Json(resul, JsonRequestBehavior.AllowGet);
        }     
        public ActionResult CerrarSession()
        {

            Session.Clear();
            Session.Abandon();

            Session.Abandon();
            FormsAuthentication.SignOut();

            HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Remove(cookie.ToString());
            }

            return RedirectToAction("Index", "Login");
        }
        public ActionResult CerrarSesionAutomatica()
        {
            Session.Clear();
            Session.Abandon();

            Session.Abandon();
            FormsAuthentication.SignOut();

            HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Remove(cookie.ToString());
            }

            return RedirectToAction("SessionOut", "Login");
        }
        [HttpGet]
        public ActionResult ValidarSesionActiva()
        {
            Result result = new Result();
            result.IsSuccess = false;
            if (Session[NameSession.IdUsuario] != null)
            {
                result.IsSuccess = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ValidarUsuarioActividad()
        {
            Result result = new Result();
            var user = getUser();
            if (user == null)
            {
                result.IsSuccess = false;
            }else
            {
                var response = _usuarioNegocio.GetUsuarioAcceso(user);
                if (response != null)
                {
                    if (user.Token == response.Token)
                    {
                        result.IsSuccess = true;
                    }
                    else
                    {
                        result.IsSuccess = false;
                    }
                }
                else
                {
                    result.IsSuccess = false;
                }
            }          
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region CREAR SESSION
        private void CrearSession(UsuarioResponse response)
        {          
            string userData = JsonConvert.SerializeObject(response);
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, response.IdUsuario.ToString(), DateTime.Now, DateTime.Now.AddDays(1), false, userData);

            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            faCookie.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(faCookie);

        }
        private void CrearMenu(UsuarioResponse response)
        {
            OpcionRequest requestOpcion = new OpcionRequest();
            requestOpcion.IdUsuarioTipo = response.IdUsuarioTipo;
            var Opciones = _usuarioNegocio.ListOpcionesByRol(requestOpcion);

            string optionData = JsonConvert.SerializeObject(Opciones);
            HttpCookie faCookie = new HttpCookie(NameCookies._OPTIONS, AESEncrytDecry.EncryptStringAES(optionData, Configuracion.TokenEncriptado));
            faCookie.Expires = DateTime.Now.AddYears(1);
            ControllerContext.HttpContext.Response.SetCookie(faCookie);
        }
        #endregion
    }
}