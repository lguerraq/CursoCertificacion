using DBR.Evento.Modelo;
using DBR.Evento.Modelo.Request;
using DBR.Evento.Modelo.Response;
using DBR.Eventos.Negocio.Implementacion;
using DBR.Eventos.Presentacion.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using DBR.Eventos.Comun;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using OfficeOpenXml;
using TGS.GISSAT.Web.Helpes;
using Rotativa;
using System.IO.Compression;

namespace DBR.Eventos.Presentacion.Controllers
{
    public class MantenimientoController : BaseController
    {
        UsuarioNegocio _usuarioNegocio = new UsuarioNegocio();
        EventoNegocio _eventoNegocio = new EventoNegocio();
        InscripcionNegocio _inscripcionNegocio = new InscripcionNegocio();
        PersonaNegocio _personaNegocio = new PersonaNegocio();
        GeneralNegocio _generalNegocio = new GeneralNegocio();
        CorreoNegocio _correoNegocio = new CorreoNegocio();
        GaleriaNegocio _galeriaNegocio = new GaleriaNegocio();
        PortadaNegocio _PortadaNegocio = new PortadaNegocio();
        SucesoNegocio _SucesoNegocio = new SucesoNegocio();
        DocumentoNegocio _DocumentoNegocio = new DocumentoNegocio();
        EmpresaNegocio _EmpresaNegocio = new EmpresaNegocio();
        public ActionResult Usuario()
        {
            return ValidarSesionOpciones(System.Reflection.MethodBase.GetCurrentMethod());
        }
        //Tabla Eventos
        public ActionResult Curso()
        {
            return ValidarSesionOpciones(System.Reflection.MethodBase.GetCurrentMethod());
        }
        //Tabla Suceso
        public ActionResult Evento()
        {
            return ValidarSesionOpciones(System.Reflection.MethodBase.GetCurrentMethod());
        }
        public ActionResult Modulos(int idCurso)
        {
            ViewBag.DescripcionHijo = "Lista de módulos";
            ViewBag.DescripcionPadre = "Eventos";
            ViewBag.IdEvento = idCurso;
            var curso = _eventoNegocio.GetEvento(new EventoRequest { IdEvento = idCurso });
            ViewBag.Titulo = curso.NombreEvento;
            var response = _eventoNegocio.ListModulo(new ModuloRequest { IdEvento = idCurso });
            return ValidarSesion(System.Reflection.MethodBase.GetCurrentMethod(), response);
        }
        public ActionResult Inscripciones()
        {
            return ValidarSesionOpciones(System.Reflection.MethodBase.GetCurrentMethod());
        }
        public ActionResult Personas()
        {
            return ValidarSesionOpciones(System.Reflection.MethodBase.GetCurrentMethod());
        }
        public ActionResult Galeria()
        {
            var rutaGaleria = Configuracion.urlFileServerVer + Configuracion.CodigoEmpresa + "/" + NombreCarpeta.Galeria + "/";
            ViewBag.rutaGaleria = rutaGaleria;
            return ValidarSesionOpciones(System.Reflection.MethodBase.GetCurrentMethod());
        }
        public ActionResult Correo()
        {
            return ValidarSesionOpciones(System.Reflection.MethodBase.GetCurrentMethod());
        }
        [HttpGet]
        public ActionResult FileServerBrowse()
        {
            string FileServer = Server.MapPath(Url.Content("~/DocumentosCK/"));
            if (!System.IO.Directory.Exists(FileServer))
            {
                System.IO.Directory.CreateDirectory(FileServer);
            }
            var dir = new DirectoryInfo(FileServer);
            ViewBag.Files = dir.GetFiles();
            return View();
        }
        public ActionResult Reportes()
        {
            return ValidarSesionOpciones(System.Reflection.MethodBase.GetCurrentMethod());
        }
        public ActionResult Portada()
        {
            var rutaPortada = Configuracion.urlFileServerVer + Configuracion.CodigoEmpresa + "/" + NombreCarpeta.Portada + "/";
            ViewBag.rutaPortada = rutaPortada;
            return ValidarSesionOpciones(System.Reflection.MethodBase.GetCurrentMethod());
        }
        public ActionResult CertificadoPosterior(int IdEvento, decimal? Promedio)
        {
            ModuloRequest request = new ModuloRequest();
            request.IdEvento = IdEvento;
            var Modulos = _eventoNegocio.ListModulo(request);

            EventoRequest requestEvento = new EventoRequest();
            requestEvento.IdEvento = IdEvento;
            var Evento = _eventoNegocio.GetEvento(requestEvento);

            ViewBag.Modulos = Modulos;
            ViewBag.Evento = Evento;
            ViewBag.Promedio = Promedio;
            return View();
        }
        public ActionResult Empresas()
        {
            return ValidarSesionOpciones(System.Reflection.MethodBase.GetCurrentMethod());
        }
        public ActionResult Documentos()
        {
            return ValidarSesionOpciones(System.Reflection.MethodBase.GetCurrentMethod());
        }
        public ActionResult DocumentosUsuario()
        {
            return ValidarSesionOpciones(System.Reflection.MethodBase.GetCurrentMethod());
        }

        #region Metodos Usuario
        [HttpPost]
        public ActionResult ListUsuarioTipo()
        {
            var response = _generalNegocio.ListTipoUsuarioCombo();
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ListUsuarioPaged(PageRequest page)
        {
            var response = _usuarioNegocio.ListUsuarioPaged(page);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveUsuario(UsuarioRequest request)
        {
            
            try
            {
                var response = new Result();
                if (!ValidarConplejidadPassword(request.Password))
                {
                    response.IsSuccess = false;
                    response.Message = Message.ErrorConplejidadPassword;
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
             
                if (request.IdUsuario == 0)
                {
                    response = _usuarioNegocio.SaveUsuario(request, getUser());
                }
                else
                {
                    response = _usuarioNegocio.UpdateUsuario(request, getUser());
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
        public ActionResult DeleteUsuario(UsuarioRequest request)
        {          
            try
            {
                var response = _usuarioNegocio.DeleteUsuario(request, getUser());
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
        public ActionResult GetUsuario(UsuarioRequest request)
        {
            var response = _usuarioNegocio.GetUsuario(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult RenviarCredenciales(UsuarioRequest request)
        {
            request.Login = request.Login.Trim();
            Result result = new Result();
            var response = _usuarioNegocio.BuscarUsuarioXlogin(request);

            if (response[0].Correo == null || response[0].Correo == "")
            {
                result.Message = Message.UsuarioPersona;
                result.IsSuccess = false;
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            var smtp = new SmtpClient(Configuracion.host, Configuracion.port);
            smtp.Credentials = new NetworkCredential(Configuracion.userName, Configuracion.password);
            smtp.EnableSsl = Configuracion.EnableSsl;
            var nombrePersona = response[0].Nombres + " " + response[0].ApellidoPaterno + " " + response[0].ApellidoMaterno;
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(Configuracion.EmailRobot, Configuracion.NameEmailRobot);
                mail.Priority = MailPriority.Normal;
                mail.To.Add(response[0].Correo);
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.Subject = "Recuperar contraseña";
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Body = ObtieneDatosCorreo(nombrePersona, response[0].Login, response[0].Password);
                smtp.Send(mail);
                mail.Dispose();

                result.Message = string.Format("Hemos enviado los credenciales a: {0}</b>", response[0].Correo);
                result.IsSuccess = true;
            }
            catch (Exception)
            {
                result.Message = Message.ErrorNoControlado;
                result.IsSuccess = false;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        private string ObtieneDatosCorreo(string Nombre, string Usuario, string Password)
        {
            string html = "<!DOCTYPE html PUBLIC ' -//W3C//DTD XHTML 1.0 Transitional//EN''http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" +
            "<html xmlns='http://www.w3.org/1999/xhtml'>" +
            "<head>" +
            "<meta http-equiv='Content-Type' content='text/html; charset=utf-8' />" +
            "<style type='text / css'> " +
            "body {margin: 0; padding: 0; min-width: 100%!important;}" +
            ".content {width: 100%; max-width: 600px;}" +
            "</style>" +
            "</head>" +
            "<body bgcolor='#fff'> " +
            "<table width='100%' bgcolor ='#fff' border ='0' cellpadding ='0' cellspacing ='0' > " +
            "<tr>" +
            "<td>" +
            "<table class='content' align ='center cellpadding ='0' cellspacing ='0' border ='0'>" +
            "<tr><td style='font-family:Calibri; color:#696158; font-size:14px;'>Estimado(a): " + Nombre + "</br>De acuerdo a tu solicitud, te adjuntamos Usuario y Clave para que puedas ingresar al sistema</br></td></tr>" +
            "<tr><td style='font-family:Calibri; color:#696158; font-size:14px;'>Usuario: <b>" + Usuario + "</b></td></tr>" +
            "<tr><td style='font-family:Calibri; color:#696158; font-size:14px;'>Clave: <b>" + Password + "</b></br></br><p style=\"font-family: Calibri;\">Este correo es generado por un robot; no responda a este remitente.</p></td></tr></table></td></tr></table>" +
            "</table>" +
            "</td>" +
            "</tr>" +
            "</table>" +
            "</body>" +
            "</html>";

            return html;
        }
        #endregion

        #region Metodos capacitaciones
        [HttpPost]
        public ActionResult ListSucesoPaginado(PageRequest page1)
        {
            PageRequest page = new PageRequest();
            page.PageNumber = (page1.start / page1.length) + 1;
            page.PageSize = page1.length;
            page.search = page1.search;
            page.Order = "DESC";

            var response = _SucesoNegocio.ListSucesoPaginado(page);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveSuceso(HttpPostedFileBase[] documento1, FormCollection collection)
        {
            SucesoRequest request = new SucesoRequest();
            request.IdSuceso = Convert.ToInt32(collection["IdSuceso"]);
            request.NombreSuceso = collection["NombreSuceso"].ToString();
            request.Descripcion = collection["Descripcion"].ToString();
            request.Fecha = Convert.ToDateTime(collection["Fecha"].ToString());
            request.Horas = Convert.ToInt32(collection["Horas"].ToString());
           

            string nombreFoto = Guid.NewGuid() + ".JPG";
            
            string pathDocumentos = Configuracion.urlFileServer + Configuracion.CodigoEmpresa + "/" + NombreCarpeta.Suceso + "/";

            if (documento1 != null)
                request.ImagenSuceso = nombreFoto;

            var response = new Result();

            if (request.IdSuceso == 0)
            {
                response = _SucesoNegocio.SaveSuceso(request, getUser());
            }
            else
            {
                response = _SucesoNegocio.UpdateSuceso(request, getUser());
            }

            if (response.IsSuccess)
            {
                try
                {
                    if (!Directory.Exists(pathDocumentos))
                    {
                        Directory.CreateDirectory(pathDocumentos);
                    }
                    if (documento1 != null)
                    {
                        var documentoFoto = documento1[0];
                        documentoFoto.SaveAs(pathDocumentos + nombreFoto);
                        ResizeImageWidth(pathDocumentos + nombreFoto, pathDocumentos + "360_" + nombreFoto, 360);
                    }
                }
                catch (Exception ex)
                {
                    response.IsSuccess = false;
                    response.Message = Message.ErrorGuardarDocumentos;
                    response.MessageExeption = ex.Message;
                }
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateSuceso(SucesoRequest request)
        {
            var response = _SucesoNegocio.UpdateSuceso(request, getUser());
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteSuceso(SucesoRequest request)
        {
            var response = _SucesoNegocio.DeleteSuceso(request, getUser());
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateEstadoSuceso(SucesoRequest request)
        {
            var response = _SucesoNegocio.UpdateEstadoSuceso(request, getUser());
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetSuceso(SucesoRequest request)
        {
            var response = _SucesoNegocio.GetSuceso(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Metodos Cursos Antiguo Evento
        public ActionResult ValoresInicialesEvento()
        {
            var TipoCurso = _generalNegocio.ListTipoCombo("TIPO CURSO");
            var TipoTema = _generalNegocio.ListTipoCombo("TIPO TEMA");
            var TipoModalidad = _generalNegocio.ListTipoCombo("MODALIDAD");
            return Json(new
            {
                TipoCurso,
                TipoTema,
                TipoModalidad
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ListEvento(PageRequest page1)
        {
            PageRequest page = new PageRequest();
            page.PageNumber = (page1.start / page1.length) + 1;
            page.PageSize = page1.length;
            page.search = page1.search;
            page.Order = "DESC";

            var response = _eventoNegocio.ListEvento(page);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveEvento(HttpPostedFileBase[] documento1, HttpPostedFileBase[] documento2, HttpPostedFileBase[] documento3, HttpPostedFileBase[] documento4, HttpPostedFileBase[] documento5, FormCollection collection)
        {
            EventoRequest request = new EventoRequest();
            request.IdEvento = Convert.ToInt32(collection["IdEvento"]);
            request.Tipo = Convert.ToInt32(collection["Tipo"]);
            request.Modalidad = Convert.ToInt32(collection["Modalidad"]);
            request.NombreEvento = collection["NombreEvento"].ToString();
            request.Descripcion = collection["Descripcion"].ToString();
            request.Expositor = collection["Expositor"].ToString();
            request.Fecha = Convert.ToDateTime(collection["Fecha"].ToString());
            request.Horas = Convert.ToInt32(collection["Horas"].ToString());
            request.Costo = collection["Costo"].ToString();
            request.CostoValor = Convert.ToDecimal(collection["CostoValor"].ToString());
            request.NotaAprobatoria = Convert.ToDecimal(collection["NotaAprobatoria"].ToString());
            if (collection["CostoValorPromocional"] != null && collection["CostoValorPromocional"] != "")
            {
                request.CostoValorPromocional = Convert.ToDecimal(collection["CostoValorPromocional"].ToString());
            }
            var ListaTemas = collection["ListaTemas"];
            request.ListTemas = ListaTemas.Split(',').Select(Int32.Parse).ToList();
            request.DetallarCertificado = Convert.ToBoolean(collection["DetallarCertificado"]);
            request.GenerarCertificado = Convert.ToBoolean(collection["GenerarCertificado"]);

            string nombreFoto = Guid.NewGuid() + ".JPG";
            string nombreFotocheck = Guid.NewGuid() + ".PDF";
            string nombreCertificado = Guid.NewGuid() + ".PDF";
            string nombreCertificadoImprimir = Guid.NewGuid() + ".PDF";
            string nombreCertificadoExpositor = Guid.NewGuid() + ".PDF";
            string pathDocumentos = Configuracion.urlFileServer + Configuracion.CodigoEmpresa + "/" + NombreCarpeta.Evento + "/";

            if (documento1 != null)
                request.ImagenEvento = nombreFoto;
            if (documento2 != null)
                request.DocumentoFotocheck = nombreFotocheck;
            if (documento3 != null)
                request.DocumentoCertificado = nombreCertificado;
            if (documento4 != null)
                request.DocumentoCertificadoImprimir = nombreCertificadoImprimir;
            if (documento5 != null)
                request.DocumentoCertificadoExpositor = nombreCertificadoExpositor;

            var response = new Result();

            if (request.IdEvento == 0)
            {
                response = _eventoNegocio.SaveEvento(request, getUser());
            }
            else
            {
                response = _eventoNegocio.UpdateEvento(request, getUser());
            }

            if (response.IsSuccess)
            {
                try
                {
                    if (!Directory.Exists(pathDocumentos))
                    {
                        Directory.CreateDirectory(pathDocumentos);
                    }
                    if (documento1 != null)
                    {
                        var documentoFoto = documento1[0];
                        documentoFoto.SaveAs(pathDocumentos + nombreFoto);
                        ResizeImageWidth(pathDocumentos + nombreFoto, pathDocumentos + "360_" + nombreFoto, 360);
                    }

                    if (documento2 != null)
                    {
                        var documentoFotocheck = documento2[0];
                        documentoFotocheck.SaveAs(pathDocumentos + nombreFotocheck);
                    }

                    if (documento3 != null)
                    {
                        var documentoCertificado = documento3[0];
                        documentoCertificado.SaveAs(pathDocumentos + nombreCertificado);
                    }
                    if (documento4 != null)
                    {
                        var documentoCertificadoImprimir = documento4[0];
                        documentoCertificadoImprimir.SaveAs(pathDocumentos + nombreCertificadoImprimir);
                    }
                    if (documento5 != null)
                    {
                        var documentoCertificadoExpositor = documento5[0];
                        documentoCertificadoExpositor.SaveAs(pathDocumentos + nombreCertificadoExpositor);
                    }

                }
                catch (Exception ex)
                {
                    response.IsSuccess = false;
                    response.Message = Message.ErrorGuardarDocumentos;
                    response.MessageExeption = ex.Message;
                }
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateEvento(EventoRequest request)
        {
            var response = _eventoNegocio.UpdateEvento(request, getUser());
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteEvento(EventoRequest request)
        {
            var response = _eventoNegocio.DeleteEvento(request, getUser());
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateEstadoEvento(EventoRequest request)
        {
            var response = _eventoNegocio.UpdateEstadoEvento(request, getUser());
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetEvento(EventoRequest request)
        {
            var response = _eventoNegocio.GetEvento(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Metodos Modulos

        [HttpPost]
        public ActionResult ValoresInicialesModulo()
        {
            var TipoLeccion = _generalNegocio.ListTipoCombo("TIPO LECCION");
            return Json(new
            {
                TipoLeccion
            }
            , JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ListModuloPaged(PageRequest page, ModuloRequest request)
        {
            var response = _eventoNegocio.ListModuloPaged(page, request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveModulo(ModuloRequest request)
        {
            var response = request.IdModulo == 0 ? _eventoNegocio.SaveModulo(request, getUser()) : _eventoNegocio.UpdateModulo(request, getUser());
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteModulo(ModuloRequest request)
        {
            var response = _eventoNegocio.DeleteModulo(request, getUser());
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetModulo(ModuloRequest request)
        {
            var response = _eventoNegocio.GetModulo(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Metodos Lecciones

        //Modulo
        [HttpPost]
        public ActionResult ListLeccionesPaged(PageRequest page, LeccionRequest request)
        {
            var response = _eventoNegocio.ListLeccionPaged(page, request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveLeccion(LeccionRequest request)
        {
            var response = request.IdLeccion == 0 ? _eventoNegocio.SaveLeccion(request, getUser()) : _eventoNegocio.UpdateLeccion(request, getUser());
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteLeccion(LeccionRequest request)
        {
            var response = _eventoNegocio.DeleteLeccion(request, getUser());
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetLeccion(LeccionRequest request)
        {
            var response = _eventoNegocio.GetLeccion(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Metodos inscripciones
        [HttpPost]
        public ActionResult ListEventoCombo()
        {
            var response = _eventoNegocio.ListAllEventoCombo();
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ValoresIniciales()
        {
            var EstadoPago = _generalNegocio.ListTipoCombo("ESTADO PAG");
            var TipoPago = _generalNegocio.ListTipoCombo("TIPO PAGO");
            var Ocupacion = _generalNegocio.ListTipoCombo("OCUPACION");
            var Modalidad = _generalNegocio.ListTipoCombo("MODALIDAD");
            var TipoCurso = _generalNegocio.ListTipoCombo("TIPO CURSO");
            var Profesion = _generalNegocio.ListProfesion();
            var Evento = _eventoNegocio.ListAllEventoCombo();
            var Pais = _generalNegocio.ListPaisCombo();
            return Json(new
            {
                EstadoPago,
                TipoPago,
                Ocupacion,
                Profesion,
                Evento,
                Modalidad,
                Pais,
                TipoCurso
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ListInscripcion(string IdEvento)
        {
            InscripcionRequest request = new InscripcionRequest();
            if (IdEvento == null || IdEvento == "")
            {
                request.IdEvento = 0;
            }
            else
            {
                request.IdEvento = Convert.ToInt32(IdEvento);
            }

            PageRequest page = new PageRequest();
            page.PageNumber = (Convert.ToInt32(Request.Form.GetValues("start")[0]) / Convert.ToInt32(Request.Form.GetValues("length")[0])) + 1;
            page.PageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
            page.Order = "ASC";
            page.search.value = Request.Form.GetValues("search[value]")[0];

            var response = _inscripcionNegocio.ListInscripcion(page, request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetPersonaXdni(PersonaRequest request)
        {
            var response = _personaNegocio.GetPersonaXdni(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetPersonaXcip(PersonaRequest request)
        {
            var response = _personaNegocio.GetPersonaXcip(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveInscripcion(InscripcionRequest request1, PersonaRequest request2)
        {
            var response = new Result();
            if (!ValidarCorreo(request2.Correo))
            {
                response.IsSuccess = false;
                response.Message = Message.ErrorDeFormatoCorreo;
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            if (request1.IdInscripcion == 0)
            {
                response = _inscripcionNegocio.SaveInscripcion(request1, request2, getUser());
            }
            else
            {
                response = _inscripcionNegocio.UpdateInscripcion(request1, request2, getUser());
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }        
        [HttpPost]
        public ActionResult DeleteInscripcion(InscripcionRequest request)
        {
            var response = _inscripcionNegocio.DeleteInscripcion(request, getUser());
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetInscripcion(InscripcionRequest request)
        {
            var response = _inscripcionNegocio.GetInscripcion(request);
            return Json(response, JsonRequestBehavior.AllowGet); ;
        }
        [HttpPost]
        public ActionResult UpdateEntregaCertificadoInscripcion(InscripcionRequest request)
        {
            var response = _inscripcionNegocio.UpdateEntregaCertificadoInscripcion(request, getUser());
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult VerPdfCertificado(InscripcionRequest request, EventoRequest documentos, PersonaRequest persona)
        {
            var pathDocumentos = Configuracion.urlFileServer + Configuracion.CodigoEmpresa + "/" + NombreCarpeta.Evento + "/";

            string IdUsuarioString = getUser().IdUsuario.ToString();
            string FechaHora = DateTime.Now.ToString("ddMMyyyy_HHmmss") + "_" + IdUsuarioString;

            string rutaView = Server.MapPath(Url.Content("~/pdfjs/web/"));

            //Elimimos PDFs Basura
            var Fichero = System.IO.Directory.GetFiles(rutaView);

            foreach (var item in Fichero)
            {
                if (item.LastIndexOf(IdUsuarioString + ".PDF") > 0)
                {
                    if (System.IO.File.Exists(item))
                    {
                        System.IO.File.Delete(item);
                    }
                }
            }

            string barcodeValue = request.rowguid;
            var NombreCertificado = persona.TipoOcupacionAbreviatura + " " + persona.Nombres + " " + persona.ApellidoPaterno + " " + persona.ApellidoMaterno;
            var CodigoBarras = "";
            if (request.NumeroCertificado > 0)
            {
                CodigoBarras = request.IdInscripcion.ToString() + request.IdEvento.ToString() + request.IdPersona.ToString();
                CodigoBarras = CodigoBarras.Length >= 16 ? CodigoBarras : CodigoBarras.PadRight(16, '0');
            }
            else
            {
                CodigoBarras = barcodeValue.Substring(0, 5).ToUpper() + request.IdInscripcion.ToString();
            }

            ModuloRequest requestM = new ModuloRequest();
            requestM.IdEvento = documentos.IdEvento;
            var Modulos = new List<ModuloResponse>();
            if (request.TipoInscripcion == 1)
            {
                if (documentos.DetallarCertificado)
                {
                    Modulos = _eventoNegocio.ListModulo(requestM);
                }
            }

            string NombreDocumentoSalida = persona.ApellidoPaterno.Trim() + "_" + persona.ApellidoMaterno.Trim() + "_" + persona.Nombres.Trim() + "_" + FechaHora + ".PDF";
            string NombreDocumentoSalida1 = persona.ApellidoPaterno.Trim() + "_" + persona.ApellidoMaterno.Trim() + "_" + persona.Nombres.Trim() + "1_" + FechaHora + ".PDF";
            string NombreDocumentoSalida2 = persona.ApellidoPaterno.Trim() + "_" + persona.ApellidoMaterno.Trim() + "_" + persona.Nombres.Trim() + "2_" + FechaHora + ".PDF";

            string ruta = pathDocumentos + documentos.DocumentoCertificadoImprimir;
            byte[] pdf = System.IO.File.ReadAllBytes(ruta);

            MemoryStream ms = new MemoryStream();
            ms.Write(pdf, 0, pdf.Length);

            PdfReader reader = new PdfReader(pdf);
            //int n = reader.NumberOfPages;
            iTextSharp.text.Rectangle psize = reader.GetPageSizeWithRotation(1);

            Document document = new Document(psize);
            PdfWriter writer = null;
            if (Modulos.Count() > 0)
            {
                writer = PdfWriter.GetInstance(document, new System.IO.FileStream(rutaView + NombreDocumentoSalida1, FileMode.Create));
            }else
            {
                writer = PdfWriter.GetInstance(document, new System.IO.FileStream(rutaView + NombreDocumentoSalida, FileMode.Create));
            }
            
            document.Open();
            PdfContentByte cb = writer.DirectContent;

            document.NewPage();

            PdfImportedPage importedPage = writer.GetImportedPage(reader, 1);
            switch (psize.Rotation)
            {
                case 0:
                    cb.AddTemplate(importedPage, 1, 0, 0, 1, 0, 0); //Gira 0 grados
                    break;
                case 90:
                    cb.AddTemplate(importedPage, 0, -1, 1, 0, 0, psize.Height); //Gira 90 grados

                    break;
                case 180:
                    cb.AddTemplate(importedPage, -1, 0, 0, -1, psize.Width, psize.Height); //Gira 180 grados
                    break;
                case 270:
                    cb.AddTemplate(importedPage, 0, 1, -1, 0, psize.Width, 0); //Gira 270 grados
                    break;
                default:
                    cb.AddTemplate(importedPage, 1, 0, 0, 1, 0, 0);
                    break;
            }

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            BaseFont bf2 = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            cb.BeginText();

                          
            cb.SetFontAndSize(bf, 26);
            float ancho = reader.GetPageSizeWithRotation(1).Width;
            float tam = NombreCertificado.Length * 16.10f;
            float ubicacion = (ancho - tam) / 2;


            cb.SetColorFill(new BaseColor(10, 16, 102));
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, NombreCertificado, ubicacion, reader.GetPageSizeWithRotation(1).Height - 235f, 0);
            cb.SetFontAndSize(bf2, 13);
            cb.SetColorFill(BaseColor.BLACK);
            if (request.NumeroCertificado > 0)
            {
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, CodigoBarras, (ancho - 125) / 2, reader.GetPageSizeWithRotation(1).Height - 48f, 0);
            }else
            {
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, CodigoBarras, (ancho - 90) / 2, reader.GetPageSizeWithRotation(1).Height - 46f, 0);
            }
            
            cb.EndText();

            Stream memoryStream = new MemoryStream();

            
            Barcode128 barcodeImg = new Barcode128();
            barcodeImg.CodeType = Barcode.CODE128;
            barcodeImg.ChecksumText = true;
            barcodeImg.GenerateChecksum = true;
            barcodeImg.StartStopText = true;
            barcodeImg.Code = CodigoBarras;
            var img = barcodeImg.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White);

            iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(img, BaseColor.WHITE);
            imagen.BorderWidth = 0;
            imagen.Alignment = Element.ALIGN_RIGHT;
            float percentage = 0.0f;
            percentage = 150 / imagen.Width;
            imagen.ScalePercent(percentage * 95);
            float posisionX = (ancho - 150) / 2;
            imagen.SetAbsolutePosition(posisionX, 518);
            cb.AddImage(imagen);

            document.Close();

            

            var response = new Result();
            if (Modulos.Count() > 0)
            {
                //var TamañoPDF = GuardarCertificadoPosterior(documentos.IdEvento, request.Nota, rutaView + NombreDocumentoSalida2);
                // Inicializamos el documento PDF
                Document doc = new Document(PageSize.A4.Rotate(), 50, 50, 30, 30);
                PdfWriter.GetInstance(doc, new FileStream(rutaView + NombreDocumentoSalida2, FileMode.Create));
                doc.Open();
                // Creamos un titulo personalizado con tamaño de fuente 18 y color Azul
                Paragraph title = new Paragraph();
                title.Font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, 20f, new BaseColor(10, 46, 123));
                title.Add(Modulos[0].NombreEvento);
                title.Alignment = Element.ALIGN_CENTER;
                title.SetLeading(1f, 1f);
                doc.Add(title);
                // Agregamos un parrafo vacio como separacion.
                doc.Add(new Paragraph(" "));

                // Empezamos a crear la tabla, definimos una tabla de 6 columnas
                PdfPTable table = new PdfPTable(2);
                // Esta es la primera fila
                table.WidthPercentage = 100f;
                float[] widths = new float[] { 5f, 2f };
                table.SetWidths(widths);
                PdfPCell cell1 = new PdfPCell(new Phrase("MÓDULOS", FontFactory.GetFont(BaseFont.HELVETICA_BOLD, 15f, BaseColor.BLACK)));
                cell1.HorizontalAlignment = 1;
                cell1.BorderWidthBottom = 0;
                cell1.Padding = 5;

                PdfPCell cell2 = new PdfPCell(new Phrase("PLANA DOCENTE", FontFactory.GetFont(BaseFont.HELVETICA_BOLD, 15f, BaseColor.BLACK)));
                cell2.HorizontalAlignment = 1;
                cell2.BorderWidthBottom = 0;               
                cell2.Padding = 5;

                table.AddCell(cell1);
                table.AddCell(cell2);
                // Segunda fila
                var TotalHoras = 0;
                foreach (var item in Modulos)
                {
                    PdfPCell cell11 = new PdfPCell(new Phrase(item.Nombre, FontFactory.GetFont(BaseFont.HELVETICA_BOLD, 11f, BaseColor.BLACK)));
                    cell11.Padding = 5;
                    cell11.PaddingBottom = 0;
                    cell11.BorderWidthBottom = 0;
                    table.AddCell(cell11);                   

                    PdfPCell cell13 = new PdfPCell(new Phrase(item.Expositor, FontFactory.GetFont(BaseFont.HELVETICA, 11f, BaseColor.BLACK)));
                    cell13.Padding = 5;
                    cell13.Rowspan = 2;
                    cell13.PaddingTop = 0;
                    cell13.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell13);

                    PdfPCell cell12 = new PdfPCell(new Phrase(item.Descripcion, FontFactory.GetFont(BaseFont.HELVETICA, 11f, BaseColor.BLACK)));
                    cell12.Padding = 5;
                    cell12.PaddingTop = 0;
                    cell12.BorderWidthTop = 0;
                    table.AddCell(cell12);

                    TotalHoras = TotalHoras + (int)item.Horas;
                }
                PdfPCell cell3 = new PdfPCell(new Phrase("Promedio Final: " + request.Nota, FontFactory.GetFont(BaseFont.HELVETICA_BOLD, 12f, BaseColor.BLACK)));
                cell3.HorizontalAlignment = 1;
                cell3.BorderWidthTop = 0;

                PdfPCell cell4 = new PdfPCell(new Phrase("Total: " + TotalHoras.ToString() + " Horas académicas", FontFactory.GetFont(BaseFont.HELVETICA_BOLD, 12f, BaseColor.BLACK)));
                cell4.HorizontalAlignment = 1;

                table.AddCell(cell3);
                table.AddCell(cell4);

                doc.Add(table);
            
                doc.Close();

                var Urls = new List<string>();
                Urls.Add(rutaView + NombreDocumentoSalida1);
                Urls.Add(rutaView + NombreDocumentoSalida2);

                response = UnirArchivos(rutaView + NombreDocumentoSalida, Urls);
                response.Message = NombreDocumentoSalida;
            }else
            {
                response.IsSuccess = true;
                response.Message = NombreDocumentoSalida;
            }
            
            return Json(response, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult CargarPdfCertificadoUtomatico(InscripcionRequest request, EventoRequest documentos, PersonaRequest persona)
        {
            var pathDocumentos = Configuracion.urlFileServer + Configuracion.CodigoEmpresa + "/" + NombreCarpeta.Evento + "/";
            string IdUsuarioString = getUser().IdUsuario.ToString();
            string FechaHora = DateTime.Now.ToString("ddMMyyyy_HHmmss") + "_" + IdUsuarioString;

            string rutaView = Server.MapPath(Url.Content("~/pdfjs/web/"));

            //Elimimos PDFs Basura
            var Fichero = System.IO.Directory.GetFiles(rutaView);

            foreach (var item in Fichero)
            {
                if (item.LastIndexOf(IdUsuarioString + ".PDF") > 0)
                {
                    if (System.IO.File.Exists(item))
                    {
                        System.IO.File.Delete(item);
                    }
                }
            }

            string barcodeValue = request.rowguid;
            var NombreCertificado = persona.TipoOcupacionAbreviatura + " " + persona.Nombres + " " + persona.ApellidoPaterno + " " + persona.ApellidoMaterno;
            var CodigoBarras = "";
            if (request.NumeroCertificado > 0)
            {
                CodigoBarras = request.IdInscripcion.ToString() + request.IdEvento.ToString() + request.IdPersona.ToString();
                CodigoBarras = CodigoBarras.Length >= 16 ? CodigoBarras : CodigoBarras.PadRight(16, '0');
            }
            else
            {
                CodigoBarras = barcodeValue.Substring(0, 5).ToUpper() + request.IdInscripcion.ToString();
            }

            ModuloRequest requestM = new ModuloRequest();
            requestM.IdEvento = documentos.IdEvento;
            var Modulos = new List<ModuloResponse>();
            if (request.TipoInscripcion == 1)
            {
                if (documentos.DetallarCertificado)
                {
                    Modulos = _eventoNegocio.ListModulo(requestM);
                }
            }

            string NombreDocumentoSalida = persona.ApellidoPaterno.Trim() + "_" + persona.ApellidoMaterno.Trim() + "_" + persona.Nombres.Trim() + "_" + FechaHora + ".PDF";
            string NombreDocumentoSalida1 = persona.ApellidoPaterno.Trim() + "_" + persona.ApellidoMaterno.Trim() + "_" + persona.Nombres.Trim() + "1_" + FechaHora + ".PDF";
            string NombreDocumentoSalida2 = persona.ApellidoPaterno.Trim() + "_" + persona.ApellidoMaterno.Trim() + "_" + persona.Nombres.Trim() + "2_" + FechaHora + ".PDF";

            string ruta = pathDocumentos + documentos.DocumentoCertificadoImprimir;
            byte[] pdf = System.IO.File.ReadAllBytes(ruta);

            MemoryStream ms = new MemoryStream();
            ms.Write(pdf, 0, pdf.Length);

            PdfReader reader = new PdfReader(pdf);
            //int n = reader.NumberOfPages;
            iTextSharp.text.Rectangle psize = reader.GetPageSizeWithRotation(1);

            Document document = new Document(psize);
            PdfWriter writer = null;
            if (Modulos.Count() > 0)
            {
                writer = PdfWriter.GetInstance(document, new System.IO.FileStream(rutaView + NombreDocumentoSalida1, FileMode.Create));
            }
            else
            {
                writer = PdfWriter.GetInstance(document, new System.IO.FileStream(rutaView + NombreDocumentoSalida, FileMode.Create));
            }

            document.Open();
            PdfContentByte cb = writer.DirectContent;

            document.NewPage();

            PdfImportedPage importedPage = writer.GetImportedPage(reader, 1);
            switch (psize.Rotation)
            {
                case 0:
                    cb.AddTemplate(importedPage, 1, 0, 0, 1, 0, 0); //Gira 0 grados
                    break;
                case 90:
                    cb.AddTemplate(importedPage, 0, -1, 1, 0, 0, psize.Height); //Gira 90 grados

                    break;
                case 180:
                    cb.AddTemplate(importedPage, -1, 0, 0, -1, psize.Width, psize.Height); //Gira 180 grados
                    break;
                case 270:
                    cb.AddTemplate(importedPage, 0, 1, -1, 0, psize.Width, 0); //Gira 270 grados
                    break;
                default:
                    cb.AddTemplate(importedPage, 1, 0, 0, 1, 0, 0);
                    break;
            }

            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            BaseFont bf2 = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            cb.BeginText();


            cb.SetFontAndSize(bf, 26);
            float ancho = reader.GetPageSizeWithRotation(1).Width;
            float tam = NombreCertificado.Length * 16.10f;
            float ubicacion = (ancho - tam) / 2;


            cb.SetColorFill(new BaseColor(10, 16, 102));
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, NombreCertificado, ubicacion, reader.GetPageSizeWithRotation(1).Height - 235f, 0);
            cb.SetFontAndSize(bf2, 13);
            cb.SetColorFill(BaseColor.BLACK);
            if (request.NumeroCertificado > 0)
            {
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, CodigoBarras, (ancho - 125) / 2, reader.GetPageSizeWithRotation(1).Height - 48f, 0);
            }
            else
            {
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, CodigoBarras, (ancho - 90) / 2, reader.GetPageSizeWithRotation(1).Height - 46f, 0);
            }

            cb.EndText();

            Stream memoryStream = new MemoryStream();


            Barcode128 barcodeImg = new Barcode128();
            barcodeImg.CodeType = Barcode.CODE128;
            barcodeImg.ChecksumText = true;
            barcodeImg.GenerateChecksum = true;
            barcodeImg.StartStopText = true;
            barcodeImg.Code = CodigoBarras;
            var img = barcodeImg.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White);

            iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(img, BaseColor.WHITE);
            imagen.BorderWidth = 0;
            imagen.Alignment = Element.ALIGN_RIGHT;
            float percentage = 0.0f;
            percentage = 150 / imagen.Width;
            imagen.ScalePercent(percentage * 95);
            float posisionX = (ancho - 150) / 2;
            imagen.SetAbsolutePosition(posisionX, 518);
            cb.AddImage(imagen);

            document.Close();



            var response = new Result();
            if (Modulos.Count() > 0)
            {
                //var TamañoPDF = GuardarCertificadoPosterior(documentos.IdEvento, request.Nota, rutaView + NombreDocumentoSalida2);
                // Inicializamos el documento PDF
                Document doc = new Document(PageSize.A4.Rotate(), 50, 50, 30, 30);
                PdfWriter.GetInstance(doc, new FileStream(rutaView + NombreDocumentoSalida2, FileMode.Create));
                doc.Open();
                // Creamos un titulo personalizado con tamaño de fuente 18 y color Azul
                Paragraph title = new Paragraph();
                title.Font = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, 20f, new BaseColor(10, 46, 123));
                title.Add(Modulos[0].NombreEvento);
                title.Alignment = Element.ALIGN_CENTER;
                title.SetLeading(1f, 1f);
                doc.Add(title);
                // Agregamos un parrafo vacio como separacion.
                doc.Add(new Paragraph(" "));

                // Empezamos a crear la tabla, definimos una tabla de 6 columnas
                PdfPTable table = new PdfPTable(2);
                // Esta es la primera fila
                table.WidthPercentage = 100f;
                float[] widths = new float[] { 5f, 2f };
                table.SetWidths(widths);
                PdfPCell cell1 = new PdfPCell(new Phrase("MÓDULOS", FontFactory.GetFont(BaseFont.HELVETICA_BOLD, 15f, BaseColor.BLACK)));
                cell1.HorizontalAlignment = 1;
                cell1.BorderWidthBottom = 0;
                cell1.Padding = 5;

                PdfPCell cell2 = new PdfPCell(new Phrase("PLANA DOCENTE", FontFactory.GetFont(BaseFont.HELVETICA_BOLD, 15f, BaseColor.BLACK)));
                cell2.HorizontalAlignment = 1;
                cell2.BorderWidthBottom = 0;
                cell2.Padding = 5;

                table.AddCell(cell1);
                table.AddCell(cell2);
                // Segunda fila
                var TotalHoras = 0;
                foreach (var item in Modulos)
                {
                    PdfPCell cell11 = new PdfPCell(new Phrase(item.Nombre, FontFactory.GetFont(BaseFont.HELVETICA_BOLD, 11f, BaseColor.BLACK)));
                    cell11.Padding = 5;
                    cell11.PaddingBottom = 0;
                    cell11.BorderWidthBottom = 0;
                    table.AddCell(cell11);

                    PdfPCell cell13 = new PdfPCell(new Phrase(item.Expositor, FontFactory.GetFont(BaseFont.HELVETICA, 11f, BaseColor.BLACK)));
                    cell13.Padding = 5;
                    cell13.Rowspan = 2;
                    cell13.PaddingTop = 0;
                    cell13.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table.AddCell(cell13);

                    PdfPCell cell12 = new PdfPCell(new Phrase(item.Descripcion, FontFactory.GetFont(BaseFont.HELVETICA, 11f, BaseColor.BLACK)));
                    cell12.Padding = 5;
                    cell12.PaddingTop = 0;
                    cell12.BorderWidthTop = 0;
                    table.AddCell(cell12);

                    TotalHoras = TotalHoras + (int)item.Horas;
                }
                PdfPCell cell3 = new PdfPCell(new Phrase("Promedio Final: " + request.Nota, FontFactory.GetFont(BaseFont.HELVETICA_BOLD, 12f, BaseColor.BLACK)));
                cell3.HorizontalAlignment = 1;
                cell3.BorderWidthTop = 0;

                PdfPCell cell4 = new PdfPCell(new Phrase("Total: " + TotalHoras.ToString() + " Horas académicas", FontFactory.GetFont(BaseFont.HELVETICA_BOLD, 12f, BaseColor.BLACK)));
                cell4.HorizontalAlignment = 1;

                table.AddCell(cell3);
                table.AddCell(cell4);

                doc.Add(table);

                doc.Close();

                var Urls = new List<string>();
                Urls.Add(rutaView + NombreDocumentoSalida1);
                Urls.Add(rutaView + NombreDocumentoSalida2);

                response = UnirArchivos(rutaView + NombreDocumentoSalida, Urls);
                response.Message = NombreDocumentoSalida;
            }
            else
            {
                response.IsSuccess = true;
                response.Message = NombreDocumentoSalida;
            }

            //MOVEMOS EL CERTIFICADO CREADO
            InscripcionRequest request1 = new InscripcionRequest();
            request1.IdInscripcion = request.IdInscripcion;
            string NombreCertificado1 = Guid.NewGuid() + ".PDF";
            var pathDocumentos1 = Configuracion.urlFileServer + Configuracion.CodigoEmpresa + "/" + NombreCarpeta.EventoCertificadoFirmado + "/";
            if (!System.IO.Directory.Exists(pathDocumentos1))
            {
                System.IO.Directory.CreateDirectory(pathDocumentos1);
            }
            System.IO.File.Copy(rutaView+NombreDocumentoSalida, pathDocumentos1 + NombreCertificado1);
            request1.Certificado = NombreCertificado1;

            response = _inscripcionNegocio.UpdateInscripcionCertificado(request1, getUser());

            return Json(response, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public FileResult DescargarPersonasInscritas(int IdEvento, string NombreEvento)
        {
            string plantilla = Server.MapPath(Url.Content("~/Plantillas/PERSONAS_INSCRIPCION.xlsx"));
            string targetPath = plantilla.Replace("PERSONAS_INSCRIPCION.xlsx", "PERSONAS_INSCRIPCION" + getUser().IdUsuario + ".xlsx");
            System.IO.File.Copy(plantilla, targetPath, true);

            InscripcionRequest request = new InscripcionRequest();
            request.IdEvento = IdEvento;
            var ListPersonas = _inscripcionNegocio.ListAllInscripcion(request);

            using (var package = new ExcelPackage(new FileInfo(targetPath)))
            {
                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet[1];

                workSheet.Cells[2, 2].Value = NombreEvento;

                for (int i = 0; i < ListPersonas.Count; i++)
                {
                    //Data
                    workSheet.Cells[i + 5, 1].Value = i + 1;
                    workSheet.Cells[i + 5, 2].Value = ListPersonas[i].NumeroDocumento;
                    workSheet.Cells[i + 5, 3].Value = ListPersonas[i].CIP;
                    workSheet.Cells[i + 5, 4].Value = ListPersonas[i].Nombres;
                    workSheet.Cells[i + 5, 5].Value = ListPersonas[i].ApellidoPaterno;
                    workSheet.Cells[i + 5, 6].Value = ListPersonas[i].ApellidoMaterno;
                    workSheet.Cells[i + 5, 7].Value = ListPersonas[i].Celular;
                    workSheet.Cells[i + 5, 8].Value = ListPersonas[i].Correo;
                    workSheet.Cells[i + 5, 9].Value = ListPersonas[i].DescripcionProfesion;
                    workSheet.Cells[i + 5, 10].Value = ListPersonas[i].TipoOcupacionNombre;
                    workSheet.Cells[i + 5, 11].Value = ListPersonas[i].DescripcionOcupacion;
                    workSheet.Cells[i + 5, 12].Value = ListPersonas[i].NombreEstadoPago;
                    workSheet.Cells[i + 5, 13].Value = ListPersonas[i].NombreTipoPago;
                    workSheet.Cells[i + 5, 14].Value = ListPersonas[i].Modalidad;
                    workSheet.Cells[i + 5, 15].Value = ListPersonas[i].Monto;

                    workSheet.Cells[i + 5, 16].Value = ListPersonas[i].NombrePais;
                    workSheet.Cells[i + 5, 17].Value = ListPersonas[i].Ciudad;

                    workSheet.Cells[i + 5, 18].Value = ListPersonas[i].NombreBanco;
                    workSheet.Cells[i + 5, 19].Value = ListPersonas[i].FechaOperacion==null?"": ((DateTime)ListPersonas[i].FechaOperacion).ToString("dd/MM/yyyy");
                    workSheet.Cells[i + 5, 20].Value = ListPersonas[i].NumeroOperacion;
                    workSheet.Cells[i + 5, 21].Value = ListPersonas[i].FechaRegistro.ToString("dd/MM/yyyy HH:mm"); ;
                    //Bordes
                    workSheet.Cells[i + 5, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 5, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 5, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 5, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 5, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 5, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 5, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 5, 8].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 5, 9].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 5, 10].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 5, 11].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 5, 12].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 5, 13].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 5, 14].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 5, 15].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 5, 16].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 5, 17].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 5, 18].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 5, 19].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 5, 20].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 5, 21].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                }

                package.Save();
            }

            string FolderFile = plantilla;
            if (System.IO.File.Exists(FolderFile))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(targetPath);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "PERSONAS_INSCRIPCION.xlsx");
            }
            return null;
        }
        [HttpPost]
        public ActionResult SaveCertificadoFirmado(HttpPostedFileBase[] documento, FormCollection collection)
        {
            InscripcionRequest request = new InscripcionRequest();
            request.IdInscripcion = Convert.ToInt32(collection["IdInscripcion"]);

            string NombreCertificado = Guid.NewGuid() + ".PDF";
            var pathDocumentos = Configuracion.urlFileServer + Configuracion.CodigoEmpresa + "/" + NombreCarpeta.EventoCertificadoFirmado + "/";

            request.Certificado = NombreCertificado;

            var response = new Result();

            response = _inscripcionNegocio.UpdateInscripcionCertificado(request, getUser());

            if (response.IsSuccess)
            {
                try
                {
                    if (documento != null)
                    {
                        var documentoFotocheck = documento[0];
                        if (!System.IO.Directory.Exists(pathDocumentos))
                        {
                            System.IO.Directory.CreateDirectory(pathDocumentos);
                        }
                        documentoFotocheck.SaveAs(pathDocumentos + NombreCertificado);
                    }

                }
                catch (Exception ex)
                {
                    response.IsSuccess = false;
                    response.Message = Message.ErrorGuardarDocumentos;
                    response.MessageExeption = ex.Message;
                }
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public int GuardarCertificadoPosterior(int IdEvento, decimal? Promedio, string Path)
        {
            var TampañoPdf = 0;
            var Pdf = new ActionAsPdf("CertificadoPosterior", new
            {
                IdEvento = IdEvento,
                Promedio = Promedio
            })
            {
                PageMargins = { Top = 12, Bottom = 12, Left = 12, Right = 12 },
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageSize = Rotativa.Options.Size.A4
            };
            var byteArray = Pdf.BuildPdf(ControllerContext);
            TampañoPdf = byteArray.Length;
            var fileStream = new FileStream(Path, FileMode.Create, FileAccess.Write);
            fileStream.Write(byteArray, 0, byteArray.Length);
            fileStream.Close();
            return TampañoPdf;
        }
        [HttpPost]
        public ActionResult SaveCorreoInscritos(InscripcionRequest request1)
        {
            var result = new Result();
            var requestCorreo = new EventoCorreoRequest() { IdEvento = request1.IdEvento };
            var response = _eventoNegocio.GeEventoCorreoByIdEvento(requestCorreo);
            if (response == null)
            {
                result.IsSuccess = false;
                result.Message = Message.ErrorEnviarCorre;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var user = getUser();
                var ListPesonas = _inscripcionNegocio.ListAllInscripcion(request1);

                var smtp = new SmtpClient(Configuracion.host, Configuracion.port);
                smtp.Credentials = new NetworkCredential(Configuracion.userName, Configuracion.password);
                smtp.EnableSsl = Configuracion.EnableSsl;

                var Cont1 = 0;
                var Cont2 = 0;
                //response.Mensaje = GetPlantillaMensajeEmpresa(response.Mensaje, Configuracion.Email, false);
                for (int i = 0; i < ListPesonas.Count(); i++)
                {
                    try
                    {
                        var nombrePersona = ListPesonas[i].Nombres + " " + ListPesonas[i].ApellidoPaterno + " " + ListPesonas[i].ApellidoMaterno;
                        MailMessage mail = new MailMessage();
                        mail.From = new MailAddress(response.Origen, response.NombreOrigen);
                        mail.Priority = MailPriority.Normal;
                        mail.To.Add(ListPesonas[i].Correo);
                        mail.SubjectEncoding = System.Text.Encoding.UTF8;
                        mail.Subject = response.Asunto;
                        mail.BodyEncoding = System.Text.Encoding.UTF8;
                        mail.IsBodyHtml = true;                        
                        try
                        {
                            mail.Body = string.Format(response.Mensaje, nombrePersona);
                        }
                        catch (Exception)
                        {
                            mail.Body = response.Mensaje;
                        }
                        smtp.Send(mail);
                        mail.Dispose();

                        var obj = new CorreoDifusionRequest();
                        obj.IdEvento = request1.IdEvento;
                        obj.IdPersona = ListPesonas[i].IdPersona;
                        obj.Correo = ListPesonas[i].Correo;
                        obj.Estado = 1;
                        obj.UsuarioCreacion = user.IdUsuario;
                        obj.FechaCreacion = DateTime.Now;
                        obj.NumeroEnvio = 1;

                        var r = _correoNegocio.SaveCorreoDifusion(obj, user);

                        Cont1++;
                        Session[NameSession.CorreosEnviados] = Cont1;
                    }
                    catch (Exception ex)
                    {
                        var obj = new CorreoDifusionRequest();
                        obj.IdEvento = request1.IdEvento;
                        obj.IdPersona = ListPesonas[i].IdPersona;
                        obj.Correo = ListPesonas[i].Correo;
                        obj.Estado = 2;
                        obj.ErrorMensaje = ex.Message;
                        obj.ErrorStackTrace = ex.StackTrace;
                        obj.UsuarioCreacion = user.IdUsuario;
                        obj.FechaCreacion = DateTime.Now;
                        obj.NumeroEnvio = -1;

                        var r = _correoNegocio.SaveCorreoDifusion(obj, user);

                        Cont2++;
                        Session[NameSession.CorreosSinEnviar] = Cont2;
                    }

                }

                smtp.Dispose();

                result.IsSuccess = true;

                if (Session[NameSession.CorreosEnviados] != null && Session[NameSession.CorreosSinEnviar] != null)
                {
                    result.Message = "Correos enviados: " + Session[NameSession.CorreosEnviados].ToString() + "</br> Correos fallidos: " + Session[NameSession.CorreosSinEnviar].ToString();
                }
                else
                {
                    if (Session[NameSession.CorreosEnviados] == null && Session[NameSession.CorreosSinEnviar] != null)
                    {
                        result.Message = "Correos enviados: 0 </br> Correos fallidos: " + Session[NameSession.CorreosSinEnviar].ToString();
                    }
                    if (Session[NameSession.CorreosEnviados] != null && Session[NameSession.CorreosSinEnviar] == null)
                    {
                        result.Message = "Correos enviados: " + Session[NameSession.CorreosEnviados].ToString() + "</br> Correos fallidos: 0";
                    }
                }

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveCorreoInscritosIndividual(InscripcionRequest request1,PersonaRequest request)
        {
            var result = new Result();
            var requestCorreo = new EventoCorreoRequest() { IdEvento = request1.IdEvento };
            var response = _eventoNegocio.GeEventoCorreoByIdEvento(requestCorreo);
            if (response == null)
            {
                result.IsSuccess = false;
                result.Message = Message.ErrorEnviarCorre;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var user = getUser();
                var smtp = new SmtpClient(Configuracion.host, Configuracion.port);
                smtp.Credentials = new NetworkCredential(Configuracion.userName, Configuracion.password);
                smtp.EnableSsl = Configuracion.EnableSsl;
                //response.Mensaje = GetPlantillaMensajeEmpresa(response.Mensaje, Configuracion.Email, false);
                try
                {
                    var nombrePersona = request.NombreCompleto;
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(response.Origen, response.NombreOrigen);
                    mail.Priority = MailPriority.Normal;
                    mail.To.Add(request.Correo);
                    mail.SubjectEncoding = System.Text.Encoding.UTF8;
                    mail.Subject = response.Asunto;
                    mail.BodyEncoding = System.Text.Encoding.UTF8;
                    mail.IsBodyHtml = true;
                    try
                    {
                        mail.Body = string.Format(response.Mensaje, nombrePersona);
                    }
                    catch (Exception)
                    {
                        mail.Body = response.Mensaje;
                    }
                    smtp.Send(mail);
                    mail.Dispose();

                    var obj = new CorreoDifusionRequest();
                    obj.IdEvento = request1.IdEvento;
                    obj.IdPersona = request.IdPersona;
                    obj.Correo = request.Correo;
                    obj.Estado = 1;
                    obj.UsuarioCreacion = user.IdUsuario;
                    obj.FechaCreacion = DateTime.Now;
                    obj.NumeroEnvio = 1;

                    var r = _correoNegocio.SaveCorreoDifusion(obj, user);

                    result.IsSuccess = true;
                    result.Message = "Correo enviado con éxito";
                }
                catch (Exception ex)
                {
                    var obj = new CorreoDifusionRequest();
                    obj.IdEvento = request1.IdEvento;
                    obj.IdPersona = request.IdPersona;
                    obj.Correo = request.Correo;
                    obj.Estado = 2;
                    obj.ErrorMensaje = ex.Message;
                    obj.ErrorStackTrace = ex.StackTrace;
                    obj.UsuarioCreacion = user.IdUsuario;
                    obj.FechaCreacion = DateTime.Now;
                    obj.NumeroEnvio = -1;

                    var r = _correoNegocio.SaveCorreoDifusion(obj, user);

                    result.IsSuccess = false;
                    result.Message = "No se pudo enviar el correo";
                    result.MessageExeption = ex.Message;
                }

                smtp.Dispose();             
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
                return Json(result, JsonRequestBehavior.AllowGet);
            }           
        }
        [HttpPost]
        public ActionResult GetEventoUsuario(EventoUsuarioRequest request)
        {
            var response = _eventoNegocio.GetEventoUsuario(request);
            if (response == null)
            {
                return Json("NULL", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }
        //EventoCorroe
        [HttpPost]
        public ActionResult SaveEventoCorreo(EventoCorreoRequest request)
        {
            if (request.IdEventoCorreo == 0)
            {
                var response = _eventoNegocio.SaveEventoCorreo(request, getUser());
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var response = _eventoNegocio.UpdateCorreo(request, getUser());
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            
        }
        [HttpPost]
        public ActionResult GeEventoCorreoByIdEvento(EventoCorreoRequest request)
        {
            var response = _eventoNegocio.GeEventoCorreoByIdEvento(request);
            if (response == null)
            {
                return Json("NULL", JsonRequestBehavior.AllowGet);
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveUsuarioAutomativo(PersonaRequest request)
        {
            try
            {
                var response = new Result();

                var persona = _personaNegocio.GetPersona(request);
                UsuarioRequest requestUser = new UsuarioRequest();
                requestUser.Password = GetRandonPassword();
                requestUser.Login = persona.NumeroDocumento;
                requestUser.NumeroDocumento = persona.NumeroDocumento;
                requestUser.Nombres = persona.Nombres;
                requestUser.ApellidoPaterno = persona.ApellidoPaterno;
                requestUser.ApellidoMaterno = persona.ApellidoMaterno;
                requestUser.IdUsuarioTipo = (int)Perfil.Participante;
                response = _usuarioNegocio.SaveUsuario(requestUser, getUser());
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

        #region Metodos Persona
        [HttpPost]
        public ActionResult ValoresInicialesPersona()
        {
            var Profesion = _generalNegocio.ListProfesion();
            var Ocupacion = _generalNegocio.ListTipoCombo("OCUPACION");
            var Pais = _generalNegocio.ListPaisCombo();
            return Json(new
            {
                Ocupacion,
                Profesion,
                Pais
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ListPersonaPaged()
        {
            PersonaRequest request = new PersonaRequest();

            PageRequest page = new PageRequest();
            page.start = Convert.ToInt32(Request.Form.GetValues("start")[0]);
            page.length = Convert.ToInt32(Request.Form.GetValues("length")[0]);
            page.search.value = Request.Form.GetValues("search[value]")[0];

            var response = _personaNegocio.ListPersonaPaged(page, request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SavePersona(PersonaRequest request)
        {
            Result response = new Result();

            if (!ValidarCorreo(request.Correo))
            {
                response.IsSuccess = false;
                response.Message = Message.ErrorDeFormatoCorreo;
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            if (request.IdPersona == 0)
            {
                response = _personaNegocio.SavePersona(request, getUser());
            }
            else
            {
                response = _personaNegocio.UpdatePersona(request, getUser());
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeletePersona(PersonaRequest request)
        {
            var response = _personaNegocio.DeletePersona(request, getUser());
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DescargarPlantilla()
        {
            string pathDocumentos = Server.MapPath(Url.Content("~/Plantillas/"));

            string NombrePlantilla = "PLANTILLA_PERSONAS.xlsx";

            if (System.IO.File.Exists(pathDocumentos + NombrePlantilla))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(pathDocumentos + NombrePlantilla);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, NombrePlantilla);
            }
            return null;
        }
        public ActionResult DescargarPlantillaErrores()
        {
            string pathDocumentos = Server.MapPath(Url.Content("~/Plantillas/"));

            string NombrePlantilla = "PLANTILLA_PERSONAS_ERRORES.xlsx";

            if (System.IO.File.Exists(pathDocumentos + getUser().IdUsuario.ToString() + NombrePlantilla))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(pathDocumentos + getUser().IdUsuario.ToString() + NombrePlantilla);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, NombrePlantilla);
            }
            return null;
        }
        [HttpPost]
        public ActionResult SavePersonaMasivo(HttpPostedFileBase[] documento)
        {

            string pathDocumentos = Server.MapPath(Url.Content("~/Plantillas/"));
            var Ocupacion = _generalNegocio.ListTipoCombo("OCUPACION");
            var Profesion = _generalNegocio.ListProfesion();

            Result result = new Result();

            try
            {
                var documentoCertificadoImprimir = documento[0];
                documentoCertificadoImprimir.SaveAs(pathDocumentos + getUser().IdUsuario.ToString() + "PLANTILLA_PERSONAS_ERRORES.xlsx");
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = Message.ErrorGuardarPlantilla;
                result.MessageExeption = ex.Message;
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            try
            {
                int Total;
                var MessageError = new List<Errores>();
                var ListaPersona = new List<PersonaRequest>();
                using (var package = new ExcelPackage(new FileInfo(pathDocumentos + getUser().IdUsuario.ToString() + "PLANTILLA_PERSONAS_ERRORES.xlsx")))
                {
                    var currentSheet = package.Workbook.Worksheets;
                    var workSheet = currentSheet[1];
                    var noOfCol = workSheet.Dimension.End.Column;
                    var noOfRow = workSheet.Dimension.End.Row;
                    Total = noOfRow - 1;

                    for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                    {
                        for (int j = 2; j <= noOfCol; j++)
                        {
                            workSheet.Cells[rowIterator, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                        }

                        if (
                            workSheet.Cells[rowIterator, (int)FormatoColumnPersona.NumeroDocumento].Value == null ||
                            //workSheet.Cells[rowIterator, (int)FormatoColumnPersona.Cip].Value == null ||
                            workSheet.Cells[rowIterator, (int)FormatoColumnPersona.Nombres].Value == null ||
                            workSheet.Cells[rowIterator, (int)FormatoColumnPersona.ApellidoPaterno].Value == null ||
                            workSheet.Cells[rowIterator, (int)FormatoColumnPersona.ApellidoMaterno].Value == null ||
                            workSheet.Cells[rowIterator, (int)FormatoColumnPersona.Celular].Value == null ||
                            workSheet.Cells[rowIterator, (int)FormatoColumnPersona.Correo].Value == null ||
                            workSheet.Cells[rowIterator, (int)FormatoColumnPersona.Profesion].Value == null ||
                            workSheet.Cells[rowIterator, (int)FormatoColumnPersona.OficioOcupacion].Value == null
                            //workSheet.Cells[rowIterator, (int)FormatoColumnPersona.OficioOcupacionDescripcion].Value == null
                            )
                        {
                            for (int j = 2; j <= noOfCol - 1; j++)
                            {
                                if (workSheet.Cells[rowIterator, j].Value == null && j != (int)FormatoColumnPersona.Cip && j != (int)FormatoColumnPersona.OficioOcupacionDescripcion)
                                    workSheet.Cells[rowIterator, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium, System.Drawing.Color.Red);
                            }

                            MessageError.Add(new Errores { Mensaje = MessageExcel.ErrorInformacionVacia, TipoError = 1, Posicion = (rowIterator - 1) });
                        }
                        else
                        {
                            var row = new PersonaRequest();
                            //row.Numero = rowIterator - 1;
                            if (workSheet.Cells[rowIterator, (int)FormatoColumnPersona.NumeroDocumento].Value.ToString().Trim().Length < 8)
                            {
                                row.NumeroDocumento = workSheet.Cells[rowIterator, (int)FormatoColumnPersona.NumeroDocumento].Value.ToString().Trim().PadLeft(8, '0');
                            }
                            else
                            {
                                row.NumeroDocumento = workSheet.Cells[rowIterator, (int)FormatoColumnPersona.NumeroDocumento].Value.ToString().ToUpper().Trim();
                            }
                            if (workSheet.Cells[rowIterator, (int)FormatoColumnPersona.Cip].Value != null)
                            {
                                row.CIP = workSheet.Cells[rowIterator, (int)FormatoColumnPersona.Cip].Value.ToString().Trim();
                            }
                            row.Nombres = workSheet.Cells[rowIterator, (int)FormatoColumnPersona.Nombres].Value.ToString().ToUpper().Trim();
                            row.ApellidoPaterno = workSheet.Cells[rowIterator, (int)FormatoColumnPersona.ApellidoPaterno].Value.ToString().Trim().ToUpper();
                            row.ApellidoMaterno = workSheet.Cells[rowIterator, (int)FormatoColumnPersona.ApellidoMaterno].Value.ToString();
                            row.Celular = workSheet.Cells[rowIterator, (int)FormatoColumnPersona.Celular].Value.ToString();
                            row.Correo = workSheet.Cells[rowIterator, (int)FormatoColumnPersona.Correo].Value.ToString().Trim().ToUpper();

                            var ProfesionTexto = workSheet.Cells[rowIterator, (int)FormatoColumnPersona.Profesion].Value.ToString().Trim().ToUpper();
                            var ProfesionExiste = Profesion.Where(x => x.Descripcion == ProfesionTexto).ToList();
                            if (ProfesionExiste.Count() > 0)
                            {
                                row.IdProfesion = Convert.ToInt32(ProfesionExiste[0].Value);
                            }
                            else
                            {
                                row.IdProfesion = Convert.ToInt32(Ocupacion.Where(x => x.Descripcion == "OTROS").First().Value);
                            }

                            var TipoOcupacionTexto = workSheet.Cells[rowIterator, (int)FormatoColumnPersona.OficioOcupacion].Value.ToString().Trim().ToUpper();
                            var OcupacionExiste = Ocupacion.Where(x => x.Descripcion == TipoOcupacionTexto).ToList();
                            if (OcupacionExiste.Count() > 0)
                            {
                                row.TipoOcupacion = Convert.ToInt32(OcupacionExiste[0].Value);
                            }
                            else
                            {
                                row.TipoOcupacion = Convert.ToInt32(Ocupacion.Where(x => x.Descripcion == "OTRO").First().Value);
                            }
                            if (workSheet.Cells[rowIterator, (int)FormatoColumnPersona.OficioOcupacionDescripcion].Value != null)
                            {
                                row.DescripcionOcupacion = workSheet.Cells[rowIterator, (int)FormatoColumnPersona.OficioOcupacionDescripcion].Value.ToString().Trim().ToUpper();
                            }
                            bool validar = true;

                            if (row.NumeroDocumento == "00000000")
                            {
                                workSheet.Cells[rowIterator, (int)FormatoColumnPersona.NumeroDocumento].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium, System.Drawing.Color.Red);
                                MessageError.Add(new Errores { Mensaje = MessageExcel.ErrorInformacionVacia, TipoError = 1, Posicion = (rowIterator - 1) });
                                validar = false;
                            }

                            if (!ValidarCorreo(row.Correo))
                            {
                                workSheet.Cells[rowIterator, (int)FormatoColumnPersona.Correo].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium, System.Drawing.Color.Red);
                                MessageError.Add(new Errores { Mensaje = MessageExcel.ErrorCorreo, TipoError = 2, Posicion = (rowIterator - 1) });
                                validar = false;
                            }
                            if (validar)
                            {
                                ListaPersona.Add(row);
                            }

                        }
                    }
                    package.Save();
                }

                string MensajeLogTotal = "";
                string MensajeError1 = "";
                string MensajeError2 = "";
                foreach (var item in MessageError)
                {
                    switch (item.TipoError)
                    {
                        case 1:
                            MensajeError1 = MensajeError1 + ", " + item.Posicion;
                            break;
                        case 2:
                            MensajeError2 = MensajeError2 + ", " + item.Posicion;
                            break;
                        default:
                            break;
                    }
                }
                MensajeLogTotal = "<u><b>ENCONTRADOS CON ERROR (" + MessageError.Count + ")</b></u>\n";
                if (MensajeError1 != "")
                {
                    MensajeLogTotal = MensajeLogTotal + "<b>" + MessageExcel.ErrorInformacionVacia + " en la(s) siguiente(s) fila(s):</b> " + MensajeError1.Substring(2) + "\n";
                }
                if (MensajeError2 != "")
                {
                    MensajeLogTotal = MensajeLogTotal + "<b>" + MessageExcel.ErrorCorreo + " en la(s) siguiente(s) fila(s):</b> " + MensajeError2.Substring(2) + "\n";
                }

                if (MessageError.Count == 0)
                {
                    result = _personaNegocio.SaveUpdatePersonaMasivo(ListaPersona, getUser());
                    result.Codigo = 1;
                }
                else
                {
                    result.Codigo = 0;
                    result.IsSuccess = true;
                    result.Message = MensajeLogTotal;
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = Message.ErrorProcesarPlantilla;
                result.MessageExeption = ex.Message;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public FileResult DescargarPersonas()
        {
            string plantilla = Server.MapPath(Url.Content("~/Plantillas/PERSONAS.xlsx"));
            string targetPath = plantilla.Replace("PERSONAS.xlsx", "PERSONAS" + getUser().IdUsuario + ".xlsx");
            System.IO.File.Copy(plantilla, targetPath, true);

            var ListPersonas = _personaNegocio.ListAllPersona();

            using (var package = new ExcelPackage(new FileInfo(targetPath)))
            {
                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet[1];

                for (int i = 0; i < ListPersonas.Count; i++)
                {
                    //Data
                    workSheet.Cells[i + 2, 1].Value = i + 1;
                    workSheet.Cells[i + 2, 2].Value = ListPersonas[i].NumeroDocumento;
                    workSheet.Cells[i + 2, 3].Value = ListPersonas[i].CIP;
                    workSheet.Cells[i + 2, 4].Value = ListPersonas[i].Nombres;
                    workSheet.Cells[i + 2, 5].Value = ListPersonas[i].ApellidoPaterno;
                    workSheet.Cells[i + 2, 6].Value = ListPersonas[i].ApellidoMaterno;
                    workSheet.Cells[i + 2, 7].Value = ListPersonas[i].Celular;
                    workSheet.Cells[i + 2, 8].Value = ListPersonas[i].Correo;
                    workSheet.Cells[i + 2, 9].Value = ListPersonas[i].DescripcionProfesion;
                    workSheet.Cells[i + 2, 10].Value = ListPersonas[i].TipoOcupacionNombre;
                    workSheet.Cells[i + 2, 11].Value = ListPersonas[i].DescripcionOcupacion;
                    workSheet.Cells[i + 2, 12].Value = ListPersonas[i].NombrePais;
                    workSheet.Cells[i + 2, 13].Value = ListPersonas[i].Ciudad;
                    //Bordes
                    workSheet.Cells[i + 2, 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 2, 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 2, 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 2, 4].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 2, 5].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 2, 6].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 2, 7].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 2, 8].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 2, 9].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 2, 10].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 2, 11].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 2, 12].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                    workSheet.Cells[i + 2, 13].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin, System.Drawing.Color.Black);
                }

                package.Save();
            }

            string FolderFile = plantilla;
            if (System.IO.File.Exists(FolderFile))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(targetPath);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "PERSONAS.xlsx");
            }
            return null;
        }
        #endregion

        #region Metodos galeria
        [HttpPost]
        public ActionResult ListGaleria(GaleriaRequest request)
        {
            PageRequest page = new PageRequest();
            page.PageNumber = (Convert.ToInt32(Request.Form.GetValues("start")[0]) / Convert.ToInt32(Request.Form.GetValues("length")[0])) + 1;
            page.PageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
            page.Order = "ASC";

            var response = _galeriaNegocio.ListGaleria(page);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ListGaleriaActivos()
        {
            var response = _galeriaNegocio.ListGaleriaActivos();
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveGaleria(HttpPostedFileBase[] documento, FormCollection collection)
        {
            GaleriaRequest request = new GaleriaRequest();
            request.Descripcion = collection["Descripcion"].ToString();

            var foto = documento[0];
            string Nombre = foto.FileName.ToUpper();
            string Extension = Nombre.Substring(Nombre.LastIndexOf('.') + 1);
            string NombreSinExtesion = Nombre.Substring(0, Nombre.LastIndexOf('.'));
            string CarpetaGenerar = Configuracion.urlFileServer + Configuracion.CodigoEmpresa + "/" + NombreCarpeta.Galeria + "/";

            if (!System.IO.Directory.Exists(CarpetaGenerar))
            {
                System.IO.Directory.CreateDirectory(CarpetaGenerar);
            }
            request.Nombre = Guid.NewGuid().ToString() + "." + Extension;

            var response = _galeriaNegocio.SaveGaleria(request, getUser());

            if (response.IsSuccess)
            {
                foto.SaveAs(CarpetaGenerar + request.Nombre);
                ResizeImage(CarpetaGenerar + request.Nombre, CarpetaGenerar + request.Nombre, 600, 600);
                ResizeImageHeight(CarpetaGenerar + request.Nombre, CarpetaGenerar + "SM_" + request.Nombre, 360, 360);

            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteGaleria(GaleriaRequest request)
        {
            var response = _galeriaNegocio.DeleteGaleria(request, getUser());
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateActivoGaleria(GaleriaRequest request)
        {
            var response = _galeriaNegocio.UpdateActivoGaleria(request, getUser());
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Metodos correo
        [HttpPost]
        public ActionResult ValoresInicialesCorreo()
        {
            var Profesion = _generalNegocio.ListProfesion();
            return Json(new
            {
                Profesion
            }
            , JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ListCorreoPaged()
        {
            PageRequest page = new PageRequest();
            page.start = Convert.ToInt32(Request.Form.GetValues("start")[0]);
            page.length = Convert.ToInt32(Request.Form.GetValues("length")[0]);
            page.search.value = Request.Form.GetValues("search[value]")[0];
            var response = _correoNegocio.ListCorreoPaged(page);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveCorreo(CorreoRequest request)
        {
            var response = new Result();
            if (!ValidarCorreo(request.Origen))
            {
                response.IsSuccess = false;
                response.Message = Message.ErrorDeFormatoCorreo;
            }
            if (request.IdCorreo == 0)
            {
                response = _correoNegocio.SaveCorreo(request, getUser());
            }
            else
            {
                response = _correoNegocio.UpdateCorreo(request, getUser());
            }
            
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteCorreo(CorreoRequest request, UsuarioLogin user)
        {
            var response = _correoNegocio.DeleteCorreo(request, user);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetCorreo(CorreoRequest request)
        {
            var response = _correoNegocio.GetCorreo(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EnviarCorreoMasivo(CorreoRequest request)
        {
            Result result = new Result();
            try
            {
                var user = getUser();
                var ListPesonas = _personaNegocio.ListAllPersonaCorreos();
                var Correo = _correoNegocio.GetCorreo(request);
                request.NumeroEnvio = Correo.NumeroEnvio + 1;
                var Result = _correoNegocio.UpdateCorreoEnvio(request, user);

                var smtp = new SmtpClient(Configuracion.host, Configuracion.port);
                smtp.Credentials = new NetworkCredential(Configuracion.userName, Configuracion.password);
                smtp.EnableSsl = Configuracion.EnableSsl;

                var Cont1 = 0;
                var Cont2 = 0;
                Correo.Mensaje = GetPlantillaMensajeEmpresa(Correo.Mensaje, Correo.Origen, false);
                for (int i = 0; i < ListPesonas.Count(); i++)
                {
                    try
                    {
                        var nombrePersona = ListPesonas[i].Nombres + " " + ListPesonas[i].ApellidoPaterno + " " + ListPesonas[i].ApellidoMaterno;
                        MailMessage mail = new MailMessage();
                        mail.From = new MailAddress(Correo.Origen, Correo.NombreOrigen);
                        mail.Priority = MailPriority.Normal;
                        mail.To.Add(ListPesonas[i].Correo);
                        mail.SubjectEncoding = System.Text.Encoding.UTF8;
                        mail.Subject = Correo.Asunto;
                        mail.BodyEncoding = System.Text.Encoding.UTF8;
                        mail.IsBodyHtml = true;
                        try
                        {
                            mail.Body = string.Format(Correo.Mensaje, nombrePersona);
                        }
                        catch (Exception)
                        {
                            mail.Body = Correo.Mensaje;
                        }
                        smtp.Send(mail);
                        mail.Dispose();

                        var obj = new CorreoDifusionRequest();
                        obj.IdCorreo = Correo.IdCorreo;
                        obj.IdPersona = ListPesonas[i].IdPersona;
                        obj.Correo = ListPesonas[i].Correo;
                        obj.Estado = 1;
                        obj.UsuarioCreacion = user.IdUsuario;
                        obj.FechaCreacion = DateTime.Now;
                        obj.NumeroEnvio = Correo.NumeroEnvio + 1;

                        var r = _correoNegocio.SaveCorreoDifusion(obj, user);
                        Cont1++;
                        Session[NameSession.CorreosEnviados] = Cont1;
                    }
                    catch (Exception ex)
                    {
                        var obj = new CorreoDifusionRequest();
                        obj.IdCorreo = Correo.IdCorreo;
                        obj.IdPersona = ListPesonas[i].IdPersona;
                        obj.Correo = ListPesonas[i].Correo;
                        obj.Estado = 2;
                        obj.ErrorMensaje = ex.Message;
                        obj.ErrorStackTrace = ex.StackTrace;
                        obj.UsuarioCreacion = user.IdUsuario;
                        obj.FechaCreacion = DateTime.Now;
                        obj.NumeroEnvio = Correo.NumeroEnvio + 1;

                        var r = _correoNegocio.SaveCorreoDifusion(obj, user);

                        Cont2++;
                        Session[NameSession.CorreosSinEnviar] = Cont2;
                    }

                }

                smtp.Dispose();

                result.IsSuccess = true;

                if (Session[NameSession.CorreosEnviados] != null && Session[NameSession.CorreosSinEnviar] != null)
                {
                    result.Message = "Correos enviados: " + Session[NameSession.CorreosEnviados].ToString() + "</br> Correos fallidos: " + Session[NameSession.CorreosSinEnviar].ToString();
                }
                else
                {
                    if (Session[NameSession.CorreosEnviados] == null && Session[NameSession.CorreosSinEnviar] != null)
                    {
                        result.Message = "Correos enviados: 0 </br> Correos fallidos: " + Session[NameSession.CorreosSinEnviar].ToString();
                    }
                    if (Session[NameSession.CorreosEnviados] != null && Session[NameSession.CorreosSinEnviar] == null)
                    {
                        result.Message = "Correos enviados: " + Session[NameSession.CorreosEnviados].ToString() + "</br> Correos fallidos: 0";
                    }
                }

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EnviarCorreoMasivoFaltantes(CorreoRequest request)
        {
            Result result = new Result();
            try
            {
                var user = getUser();
                var ListPesonas = _personaNegocio.ListAllPersonaCorreosFaltantes(request);
                var Correo = _correoNegocio.GetCorreo(request);

                
                if (ListPesonas.Count > 0)
                {
                    var smtp = new SmtpClient(Configuracion.host, Configuracion.port);
                    smtp.Credentials = new NetworkCredential(Configuracion.userName, Configuracion.password);
                    smtp.EnableSsl = Configuracion.EnableSsl;

                    var Cont1 = 0;
                    var Cont2 = 0;
                    Correo.Mensaje = GetPlantillaMensajeEmpresa(Correo.Mensaje, Correo.Origen, false);
                    for (int i = 0; i < ListPesonas.Count(); i++)
                    {
                        try
                        {
                            var nombrePersona = ListPesonas[i].Nombres + " " + ListPesonas[i].ApellidoPaterno + " " + ListPesonas[i].ApellidoMaterno;
                            MailMessage mail = new MailMessage();
                            mail.From = new MailAddress(Correo.Origen, Correo.NombreOrigen);
                            mail.Priority = MailPriority.Normal;
                            mail.To.Add(ListPesonas[i].Correo);
                            mail.SubjectEncoding = System.Text.Encoding.UTF8;
                            mail.Subject = Correo.Asunto;
                            mail.BodyEncoding = System.Text.Encoding.UTF8;
                            mail.IsBodyHtml = true;
                            try
                            {
                                mail.Body = string.Format(Correo.Mensaje, nombrePersona);
                            }
                            catch (Exception)
                            {
                                mail.Body = Correo.Mensaje;
                            }
                            smtp.Send(mail);
                            mail.Dispose();

                            var obj = new CorreoDifusionRequest();
                            obj.IdCorreo = Correo.IdCorreo;
                            obj.IdPersona = ListPesonas[i].IdPersona;
                            obj.Correo = ListPesonas[i].Correo;
                            obj.Estado = 1;
                            obj.UsuarioCreacion = user.IdUsuario;
                            obj.FechaCreacion = DateTime.Now;
                            obj.NumeroEnvio = Correo.NumeroEnvio;

                            var r = _correoNegocio.SaveCorreoDifusion(obj, user);
                            Cont1++;
                            Session[NameSession.CorreosEnviados] = Cont1;
                        }
                        catch (Exception ex)
                        {
                            var obj = new CorreoDifusionRequest();
                            obj.IdCorreo = Correo.IdCorreo;
                            obj.IdPersona = ListPesonas[i].IdPersona;
                            obj.Correo = ListPesonas[i].Correo;
                            obj.Estado = 2;
                            obj.ErrorMensaje = ex.Message;
                            obj.ErrorStackTrace = ex.StackTrace;
                            obj.UsuarioCreacion = user.IdUsuario;
                            obj.FechaCreacion = DateTime.Now;
                            obj.NumeroEnvio = Correo.NumeroEnvio;

                            var r = _correoNegocio.SaveCorreoDifusion(obj, user);

                            Cont2++;
                            Session[NameSession.CorreosSinEnviar] = Cont2;
                        }
                    }

                    smtp.Dispose();

                    result.IsSuccess = true;

                    if (Session[NameSession.CorreosEnviados] != null && Session[NameSession.CorreosSinEnviar] != null)
                    {
                        result.Message = "Correos enviados: " + Session[NameSession.CorreosEnviados].ToString() + "</br> Correos fallidos: " + Session[NameSession.CorreosSinEnviar].ToString();
                    }
                    else
                    {
                        if (Session[NameSession.CorreosEnviados] == null && Session[NameSession.CorreosSinEnviar] != null)
                        {
                            result.Message = "Correos enviados: 0 </br> Correos fallidos: " + Session[NameSession.CorreosSinEnviar].ToString();
                        }
                        if (Session[NameSession.CorreosEnviados] != null && Session[NameSession.CorreosSinEnviar] == null)
                        {
                            result.Message = "Correos enviados: " + Session[NameSession.CorreosEnviados].ToString() + "</br> Correos fallidos: 0";
                        }
                    }
                }else
                {
                    result.IsSuccess = true;
                    result.Message = "No hay correos pendientes por enviar";
                }

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EnviarCorreoMasivoPorProfesion(CorreoRequest request, List<int?> IdsProfesion)
        {
            Result result = new Result();
            try
            {
                var user = getUser();
                var ListPesonas = _personaNegocio.ListAllPersonaCorreosPorProfesion(request, IdsProfesion);
                var Correo = _correoNegocio.GetCorreo(request);
                request.NumeroEnvio = Correo.NumeroEnvio + 1;
                var Result = _correoNegocio.UpdateCorreoEnvio(request, user);

                if (ListPesonas.Count > 0)
                {
                    var smtp = new SmtpClient(Configuracion.host, Configuracion.port);
                    smtp.Credentials = new NetworkCredential(Configuracion.userName, Configuracion.password);
                    smtp.EnableSsl = Configuracion.EnableSsl;

                    var Cont1 = 0;
                    var Cont2 = 0;
                    Correo.Mensaje = GetPlantillaMensajeEmpresa(Correo.Mensaje, Correo.Origen, false);
                    for (int i = 0; i < ListPesonas.Count(); i++)
                    {
                        try
                        {
                            var nombrePersona = ListPesonas[i].Nombres + " " + ListPesonas[i].ApellidoPaterno + " " + ListPesonas[i].ApellidoMaterno;
                            MailMessage mail = new MailMessage();
                            mail.From = new MailAddress(Correo.Origen, Correo.NombreOrigen);
                            mail.Priority = MailPriority.Normal;
                            mail.To.Add(ListPesonas[i].Correo);
                            mail.SubjectEncoding = System.Text.Encoding.UTF8;
                            mail.Subject = Correo.Asunto;
                            mail.BodyEncoding = System.Text.Encoding.UTF8;
                            mail.IsBodyHtml = true;
                            try
                            {
                                mail.Body = string.Format(Correo.Mensaje, nombrePersona);
                            }
                            catch (Exception)
                            {
                                mail.Body = Correo.Mensaje;
                            }
                            smtp.Send(mail);
                            mail.Dispose();

                            var obj = new CorreoDifusionRequest();
                            obj.IdCorreo = Correo.IdCorreo;
                            obj.IdPersona = ListPesonas[i].IdPersona;
                            obj.Correo = ListPesonas[i].Correo;
                            obj.Estado = 1;
                            obj.UsuarioCreacion = user.IdUsuario;
                            obj.FechaCreacion = DateTime.Now;
                            obj.NumeroEnvio = Correo.NumeroEnvio;

                            var r = _correoNegocio.SaveCorreoDifusion(obj, user);
                            Cont1++;
                            Session[NameSession.CorreosEnviados] = Cont1;
                        }
                        catch (Exception ex)
                        {
                            var obj = new CorreoDifusionRequest();
                            obj.IdCorreo = Correo.IdCorreo;
                            obj.IdPersona = ListPesonas[i].IdPersona;
                            obj.Correo = ListPesonas[i].Correo;
                            obj.Estado = 2;
                            obj.ErrorMensaje = ex.Message;
                            obj.ErrorStackTrace = ex.StackTrace;
                            obj.UsuarioCreacion = user.IdUsuario;
                            obj.FechaCreacion = DateTime.Now;
                            obj.NumeroEnvio = Correo.NumeroEnvio;

                            var r = _correoNegocio.SaveCorreoDifusion(obj, user);

                            Cont2++;
                            Session[NameSession.CorreosSinEnviar] = Cont2;
                        }
                    }

                    smtp.Dispose();

                    result.IsSuccess = true;

                    if (Session[NameSession.CorreosEnviados] != null && Session[NameSession.CorreosSinEnviar] != null)
                    {
                        result.Message = "Correos enviados: " + Session[NameSession.CorreosEnviados].ToString() + "</br> Correos fallidos: " + Session[NameSession.CorreosSinEnviar].ToString();
                    }
                    else
                    {
                        if (Session[NameSession.CorreosEnviados] == null && Session[NameSession.CorreosSinEnviar] != null)
                        {
                            result.Message = "Correos enviados: 0 </br> Correos fallidos: " + Session[NameSession.CorreosSinEnviar].ToString();
                        }
                        if (Session[NameSession.CorreosEnviados] != null && Session[NameSession.CorreosSinEnviar] == null)
                        {
                            result.Message = "Correos enviados: " + Session[NameSession.CorreosEnviados].ToString() + "</br> Correos fallidos: 0";
                        }
                    }
                }
                else
                {
                    result.IsSuccess = true;
                    result.Message = "No hay correos pendientes por enviar";
                }

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EnviarCorreoIndividual(CorreoRequest request, PersonaRequest Persona)
        {
            Result result = new Result();
            int IdPersona = 0;
            try
            {
                var user = getUser();

                var Correo = _correoNegocio.GetCorreo(request);

                var persona = _personaNegocio.GetPersonaXdni(Persona);

                if (persona.Count > 0)
                {
                    IdPersona = persona[0].IdPersona;
                }

                var smtp = new SmtpClient(Configuracion.host, Configuracion.port);
                smtp.Credentials = new NetworkCredential(Configuracion.userName, Configuracion.password);
                smtp.EnableSsl = Configuracion.EnableSsl;
                Correo.Mensaje = GetPlantillaMensajeEmpresa(Correo.Mensaje, Correo.Origen, false);
                try
                {
                    var nombrePersona = Persona.Nombres + " " + Persona.ApellidoPaterno + " " + Persona.ApellidoMaterno;
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(Correo.Origen, Correo.NombreOrigen);
                    mail.Priority = MailPriority.Normal;
                    mail.To.Add(Persona.Correo);
                    mail.SubjectEncoding = System.Text.Encoding.UTF8;
                    mail.Subject = Correo.Asunto;
                    mail.BodyEncoding = System.Text.Encoding.UTF8;
                    mail.IsBodyHtml = true;
                    try
                    {
                        mail.Body = string.Format(Correo.Mensaje, nombrePersona);
                    }
                    catch (Exception)
                    {
                        mail.Body = Correo.Mensaje;
                    }
                    smtp.Send(mail);
                    mail.Dispose();

                    var obj = new CorreoDifusionRequest();
                    obj.IdCorreo = Correo.IdCorreo;
                    obj.IdPersona = IdPersona;
                    obj.Correo = Persona.Correo;
                    obj.Estado = 1;
                    obj.UsuarioCreacion = user.IdUsuario;
                    obj.FechaCreacion = DateTime.Now;
                    obj.NumeroEnvio = Correo.NumeroEnvio + 1;

                    var r = _correoNegocio.SaveCorreoDifusion(obj, user);
                }
                catch (Exception ex)
                {
                    var obj = new CorreoDifusionRequest();
                    obj.IdCorreo = Correo.IdCorreo;
                    obj.IdPersona = IdPersona;
                    obj.Correo = Persona.Correo;
                    obj.Estado = 2;
                    obj.ErrorMensaje = ex.Message;
                    obj.ErrorStackTrace = ex.StackTrace;
                    obj.UsuarioCreacion = user.IdUsuario;
                    obj.FechaCreacion = DateTime.Now;
                    obj.NumeroEnvio = Correo.NumeroEnvio + 1;

                    var r = _correoNegocio.SaveCorreoDifusion(obj, user);
                }

                smtp.Dispose();

                result.IsSuccess = true;
                result.Message = Message.ExitoCorreoEnviado;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ConsultarEnvios()
        {
            CorreoRequest request = new CorreoRequest();
            ConsultaEnvioResponse response = new ConsultaEnvioResponse();
            request.NumeroEnvio = 0;
            response.Total = 0;
            //if (Session[NameSession.NumeroEnvioCorreo] != null)
            //{
            //    request.NumeroEnvio = Convert.ToInt32(Session[NameSession.NumeroEnvioCorreo]);
            //}
            //if (Session[NameSession.TotalCorreos] != null)
            //{
            //    response.Total = Convert.ToInt32(Session[NameSession.TotalCorreos]);
            //}
            response.Enviados = _correoNegocio.ConsultarEnvios(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveFileServer(HttpPostedFileBase[] documento)
        {
            Result result = new Result();

            HttpPostedFileBase doc = null;

            try
            {
                if (documento != null)
                {
                    doc = documento[0];
                    var val = doc.FileName.Split('.');
                    var extension = val[val.Length - 1];
                    string Nombre = Guid.NewGuid().ToString() + "." + extension;
                    string FileServer = Server.MapPath(Url.Content("~/Documentos/"));
                    string FileServerVisualizar = ConfigurationManager.AppSettings["urlFileServerVisualizar"];

                    doc.SaveAs(FileServer + Nombre);

                    result.IsSuccess = true;
                    result.Message = FileServerVisualizar + Nombre;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Ocurrio un problema al generar link.";
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = Message.ErrorNoControlado;
                result.MessageExeption = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveFileServerCkEditor(HttpPostedFileBase[] upload)
        {
            Result result = new Result();

            HttpPostedFileBase doc = null;

            try
            {
                if (upload != null)
                {
                    doc = upload[0];
                    var val = doc.FileName.Split('.');
                    var extension = val[val.Length - 1];
                    string Nombre = Guid.NewGuid().ToString() + "." + extension;
                    string FileServer = Server.MapPath(Url.Content("~/DocumentosCK/"));
                    string FileServerVisualizar = ConfigurationManager.AppSettings["urlFileServerVisualizarCkEditor"];
                    if (!System.IO.Directory.Exists(FileServer))
                    {
                        System.IO.Directory.CreateDirectory(FileServer);
                    }
                    doc.SaveAs(FileServer + Nombre);

                    result.IsSuccess = true;
                    result.Message = FileServerVisualizar + Nombre;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "Ocurrio un problema al generar link.";
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = Message.ErrorNoControlado;
                result.MessageExeption = ex.Message;
            }

            return Json(new { uploaded=true, url= result.Message });
        }
        #endregion

        #region  Metodos portada
        [HttpPost]
        public ActionResult ListPortadaPaginado(PageRequest page)
        {
            var response = _PortadaNegocio.ListPortadaPaginado(page);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SavePortada(HttpPostedFileBase[] documento, FormCollection collection)
        {
            PortadaRequest request = new PortadaRequest();
            request.IdPortada = Convert.ToInt32(collection["IdPortada"]);
            request.Descripcion = collection["Descripcion"].ToString();
            if (collection["SubTitulo1"] != null)
            {
                request.SubTitulo1 = collection["SubTitulo1"].ToString();
            }
            if (collection["SubTitulo2"] != null)
            {
                request.SubTitulo2 = collection["SubTitulo2"].ToString();
            }          
            request.NombreImagen = collection["NombreImagen"].ToString();
            if (collection["TextoEnlace"] != null && collection["TextoEnlace"] != "")
                request.TextoEnlace = collection["TextoEnlace"].ToString();
            if (collection["UrlEnlace"] != null && collection["UrlEnlace"] != "")
                request.UrlEnlace = collection["UrlEnlace"].ToString();

            request.left = Convert.ToDecimal(collection["left"].ToString());
            request.top = Convert.ToDecimal(collection["top"].ToString());
            //Tamaño del lienzo          
            request.widthCropper = Convert.ToDecimal(collection["widthCropper"].ToString());
            request.heightCropper = Convert.ToDecimal(collection["heightCropper"].ToString());

            //Tamaño de la imagen
            request.naturalWidth = Convert.ToInt32(collection["naturalWidth"].ToString());
            request.naturalHeight = Convert.ToInt32(collection["naturalHeight"].ToString());

            //Tamaño recorte
            request.width = Convert.ToDecimal(collection["width"].ToString());
            request.height = Convert.ToDecimal(collection["height"].ToString());

            string CarpetaGenerar = Configuracion.urlFileServer + Configuracion.CodigoEmpresa + "/" + NombreCarpeta.Portada + "/";

            if (!System.IO.Directory.Exists(CarpetaGenerar))
            {
                System.IO.Directory.CreateDirectory(CarpetaGenerar);
            }
            if (documento != null)
            {
                if (!ValidateHelper.isImage(documento[0]))
                {
                    Result result = new Result();
                    result.IsSuccess = false;
                    result.Message = Message.DocumentoNoPermitido;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                var extension = Path.GetExtension(documento[0].FileName).ToUpper();
                request.NombreImagen = Guid.NewGuid().ToString() + extension;
            }

            var response = new Result();
            string pathImagen = CarpetaGenerar + request.NombreImagen;

            if (request.IdPortada == 0)
            {
                response = _PortadaNegocio.SavePortada(request, getUser());
                if (response.IsSuccess)
                {
                    documento[0].SaveAs(pathImagen);
                }
            }
            else
            {
                response = _PortadaNegocio.UpdatePortada(request, getUser());
                if (response.IsSuccess)
                {
                    if (documento != null)
                    {
                        documento[0].SaveAs(pathImagen);
                    }
                }
            }

            int left, top, width, height;

            if (request.naturalWidth < request.widthCropper)
            {
                request.naturalWidth = (int)request.widthCropper;
                ResizeImageWidth(pathImagen, pathImagen, request.naturalWidth);

                left = (int)request.left;
                top = (int)request.top;

                width = (int)request.width;
                height = (int)request.height;
            }
            else
            {
                left = (int)(request.left * request.naturalWidth / request.widthCropper);
                top = (int)(request.top * request.naturalHeight / request.heightCropper);

                width = (int)(request.width * request.naturalWidth / request.widthCropper);
                height = (int)(request.height * request.naturalHeight / request.heightCropper);
            }

            Crop(pathImagen, pathImagen, width, height, left, top);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeletePortada(PortadaRequest request)
        {
            var response = _PortadaNegocio.DeletePortada(request, getUser());
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetPortada(PortadaRequest request)
        {
            var response = _PortadaNegocio.GetPortada(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Medodos Empresas        
        [HttpPost]
        public ActionResult ListEmpresaPaginado(PageRequest page)
        {
            var response = _EmpresaNegocio.ListEmpresaPaginado(page);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SaveEmpresa(EmpresaRequest request, UsuarioRequest requestuser)
        {
            requestuser.Password = GetRandonPassword();
            var response = request.IdEmpresa == 0 ? _EmpresaNegocio.SaveEmpresa(request, requestuser, getUser()) : _EmpresaNegocio.UpdateEmpresa(request, requestuser, getUser());
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteEmpresa(EmpresaRequest request)
        {
            var response = _EmpresaNegocio.DeleteEmpresa(request, getUser());
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetEmpresa(EmpresaRequest request)
        {
            var response = _EmpresaNegocio.GetEmpresa(request);
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion


        public static string ResizeImage(string strImgPath, string strImgOutputPath, int iWidth, int iHeight)
        {
            try
            {
                bool mismaImagen = strImgPath.Equals(strImgOutputPath);
                if (mismaImagen)
                {
                    strImgOutputPath = strImgPath + "___.JPG";
                }

                string[] extensiones = {
                                   ".JPG",
                                   ".PNG",
                                   ".BMP",
                                   ".GIF"
                               };

                if (!extensiones.Contains(System.IO.Path.GetExtension(strImgPath)))
                    throw new Exception("Extensión no soportada");

                //Lee el fichero en un stream
                System.IO.Stream mystream = null;

                if (strImgPath.StartsWith("http"))
                {
                    HttpWebRequest wreq = (HttpWebRequest)WebRequest.Create(strImgPath);
                    HttpWebResponse wresp = (HttpWebResponse)wreq.GetResponse();
                    mystream = wresp.GetResponseStream();
                }
                else
                    mystream = System.IO.File.OpenRead(strImgPath);

                // Cargo la imágen
                System.Drawing.Bitmap imgToResize = new System.Drawing.Bitmap(mystream);

                System.Drawing.Size size = new System.Drawing.Size(iWidth, iHeight);

                int sourceWidth = imgToResize.Width;
                int sourceHeight = imgToResize.Height;

                float nPercent = 0;
                float nPercentW = 0;
                float nPercentH = 0;

                nPercentW = ((float)size.Width / (float)sourceWidth);
                nPercentH = ((float)size.Height / (float)sourceHeight);

                if (nPercentH < nPercentW)
                    //nPercent = nPercentH;
                    nPercent = nPercentW;
                else
                    //nPercent = nPercentW;
                    nPercent = nPercentH;
                //nPercent = nPercentW;

                int destWidth = (int)(sourceWidth * nPercent);
                int destHeight = (int)(sourceHeight * nPercent);

                System.Drawing.Bitmap b = new System.Drawing.Bitmap(destWidth, destHeight);
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage((System.Drawing.Image)b);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
                g.Dispose();

                // We will store the correct image codec in this object
                System.Drawing.Imaging.ImageCodecInfo ici = GetEncoderInfo("image/jpeg"); ;
                // This will specify the image quality to the encoder
                System.Drawing.Imaging.EncoderParameter epQuality = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 99L);
                // Store the quality parameter in the list of encoder parameters
                System.Drawing.Imaging.EncoderParameters eps = new System.Drawing.Imaging.EncoderParameters(1);
                eps.Param[0] = epQuality;
                b.Save(strImgOutputPath, ici, eps);

                imgToResize.Dispose();
                mystream.Close();
                mystream.Dispose();
                b.Dispose();
                g.Dispose();

                if (mismaImagen)
                {
                    System.IO.File.Delete(strImgPath);
                    System.IO.File.Move(strImgOutputPath, strImgPath);
                }

                return strImgPath;
            }
            catch
            {
                throw;
            }
        }
        public static string ResizeImageHeight(string strImgPath, string strImgOutputPath, int iWidth, int iHeight)
        {
            try
            {
                bool mismaImagen = strImgPath.Equals(strImgOutputPath);
                if (mismaImagen)
                {
                    strImgOutputPath = strImgPath + "___.JPG";
                }

                string[] extensiones = {
                                   ".JPG",
                                   ".PNG",
                                   ".BMP",
                                   ".GIF"
                               };

                if (!extensiones.Contains(System.IO.Path.GetExtension(strImgPath)))
                    throw new Exception("Extensión no soportada");

                //Lee el fichero en un stream
                System.IO.Stream mystream = null;

                if (strImgPath.StartsWith("http"))
                {
                    HttpWebRequest wreq = (HttpWebRequest)WebRequest.Create(strImgPath);
                    HttpWebResponse wresp = (HttpWebResponse)wreq.GetResponse();
                    mystream = wresp.GetResponseStream();
                }
                else
                    mystream = System.IO.File.OpenRead(strImgPath);

                // Cargo la imágen
                System.Drawing.Bitmap imgToResize = new System.Drawing.Bitmap(mystream);

                System.Drawing.Size size = new System.Drawing.Size(iWidth, iHeight);

                int sourceWidth = imgToResize.Width;
                int sourceHeight = imgToResize.Height;

                float nPercent = 0;
                float nPercentW = 0;
                float nPercentH = 0;

                nPercentW = ((float)size.Width / (float)sourceWidth);
                nPercentH = ((float)size.Height / (float)sourceHeight);

                nPercent = nPercentW;

                int destWidth = (int)(sourceWidth * nPercent);
                int destHeight = (int)(sourceHeight * nPercent);

                System.Drawing.Bitmap b = new System.Drawing.Bitmap(destWidth, destHeight);
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage((System.Drawing.Image)b);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
                g.Dispose();

                // We will store the correct image codec in this object
                System.Drawing.Imaging.ImageCodecInfo ici = GetEncoderInfo("image/jpeg"); ;
                // This will specify the image quality to the encoder
                System.Drawing.Imaging.EncoderParameter epQuality = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 99L);
                // Store the quality parameter in the list of encoder parameters
                System.Drawing.Imaging.EncoderParameters eps = new System.Drawing.Imaging.EncoderParameters(1);
                eps.Param[0] = epQuality;
                b.Save(strImgOutputPath, ici, eps);

                imgToResize.Dispose();
                mystream.Close();
                mystream.Dispose();
                b.Dispose();
                g.Dispose();

                if (mismaImagen)
                {
                    System.IO.File.Delete(strImgPath);
                    System.IO.File.Move(strImgOutputPath, strImgPath);
                }

                return strImgPath;
            }
            catch
            {
                throw;
            }
        }
        public static string ResizeImageSinProporcion(string strImgPath, string strImgOutputPath, int iWidth, int iHeight)
        {
            try
            {
                bool mismaImagen = strImgPath.Equals(strImgOutputPath);
                if (mismaImagen)
                {
                    strImgOutputPath = strImgPath + "___.JPG";
                }

                string[] extensiones = {
                                   ".JPG",
                                   ".PNG",
                                   ".BMP",
                                   ".GIF"
                               };

                if (!extensiones.Contains(System.IO.Path.GetExtension(strImgPath)))
                    throw new Exception("Extensión no soportada");

                //Lee el fichero en un stream
                System.IO.Stream mystream = null;

                if (strImgPath.StartsWith("http"))
                {
                    HttpWebRequest wreq = (HttpWebRequest)WebRequest.Create(strImgPath);
                    HttpWebResponse wresp = (HttpWebResponse)wreq.GetResponse();
                    mystream = wresp.GetResponseStream();
                }
                else
                    mystream = System.IO.File.OpenRead(strImgPath);

                // Cargo la imágen
                System.Drawing.Bitmap imgToResize = new System.Drawing.Bitmap(mystream);

                System.Drawing.Size size = new System.Drawing.Size(iWidth, iHeight);

                int destWidth = size.Width;
                int destHeight = size.Height;

                System.Drawing.Bitmap b = new System.Drawing.Bitmap(destWidth, destHeight);
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage((System.Drawing.Image)b);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
                g.Dispose();

                // We will store the correct image codec in this object
                System.Drawing.Imaging.ImageCodecInfo ici = GetEncoderInfo("image/jpeg"); ;
                // This will specify the image quality to the encoder
                System.Drawing.Imaging.EncoderParameter epQuality = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 99L);
                // Store the quality parameter in the list of encoder parameters
                System.Drawing.Imaging.EncoderParameters eps = new System.Drawing.Imaging.EncoderParameters(1);
                eps.Param[0] = epQuality;
                b.Save(strImgOutputPath, ici, eps);

                imgToResize.Dispose();
                mystream.Close();
                mystream.Dispose();
                b.Dispose();
                g.Dispose();

                if (mismaImagen)
                {
                    System.IO.File.Delete(strImgPath);
                    System.IO.File.Move(strImgOutputPath, strImgPath);
                }

                return strImgPath;
            }
            catch
            {
                throw;
            }
        }
        private static System.Drawing.Imaging.ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            System.Drawing.Imaging.ImageCodecInfo[] encoders;
            encoders = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
        public void debug(string msg, string NombreDocumento)
        {
            try
            {
                string m_debugPath = @"D:\PROYECTOS\INGENIEROS TFS\DBR.Eventos.Presentacion\Documentos\";

                using (FileStream fs = new FileStream(m_debugPath + NombreDocumento + DateTime.Now.ToString("ddMMyyyy") + ".txt", FileMode.OpenOrCreate, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.BaseStream.Seek(0, SeekOrigin.End);
                        sw.WriteLine(msg);
                        sw.Flush();
                        sw.Close();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

    }
}