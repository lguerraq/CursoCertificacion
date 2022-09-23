using DBR.Evento.Modelo;
using DBR.Evento.Modelo.Request;
using DBR.Evento.Modelo.Response;
using DBR.Eventos.Comun;
using DBR.Eventos.Negocio.Implementacion;
using DBR.Eventos.Presentacion.Controllers.Base;
using DBR.Eventos.Presentacion.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DBR.Eventos.Presentacion.Areas.Aula.Controllers
{
    public class CursosController : BaseController
    {
        EventoNegocio _eventoNegocio = new EventoNegocio();
        CuestionarioNegocio _cuestionarioNegocio = new CuestionarioNegocio();
        VirtualNegocio _virtualNegocio = new VirtualNegocio();
        InscripcionNegocio _inscripcionNegocio = new InscripcionNegocio();

        #region Vistas

        public ActionResult Index()
        {
            var user = getUser();
            ViewBag.NumeroRelease = Configuracion.NumeroRelease;
            if (user != null)
            {
                var data = _eventoNegocio.ListEventoUsuario(user);
                var ultimosCursos = _eventoNegocio.ListEventoActivos().OrderByDescending(c => c.Fecha).Take(3).ToList();

                foreach (var item in ultimosCursos)
                {
                    var video = new VirtualNegocio().ListVirtualVideoByEvento(new VirtualContenidoRequest { IdEvento = item.IdEvento }).FirstOrDefault();
                    if (video != null)
                    {
                        item.UrlVideo = video.Url;
                    }
                }

                var model = new CursoDashboardViewModel
                {
                    TotalCursos = data.Count,
                    NuevosCursos = data.Where(c => c.Fecha >= DateTime.Now.AddDays(-30)).Count(),
                    UltimosCursos = ultimosCursos
                };

                return ValidarSesion<CursoDashboardViewModel>(System.Reflection.MethodBase.GetCurrentMethod(), model);
            }
            else
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }
        }
        public ActionResult Administrar()
        {
            return ValidarSesionOpciones(System.Reflection.MethodBase.GetCurrentMethod());
        }
        public ActionResult Matriculados()
        {
            var user = getUser();
            ViewBag.NumeroRelease = Configuracion.NumeroRelease;
            if (user != null)
            {
                var data = _eventoNegocio.ListEventoUsuario(user);
                return ValidarSesion<List<EventoResponse>>(System.Reflection.MethodBase.GetCurrentMethod(), data);
            }
            else
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }
        }
        public ActionResult Detalle(string id)
        {
            var user = getUser();
            ViewBag.NumeroRelease = Configuracion.NumeroRelease;
            if (Session[NameSession.IdUsuario] != null)
            {
                if (id != null)
                {
                    EventoRequest request = new EventoRequest();
                    request.rowid = Guid.Parse(id);
                    var evento = _eventoNegocio.GetEventoByRowId(request, getUser());
                    if (evento != null)
                    {
                        ViewBag.RowId = id;
                        ViewBag.IdEvento = evento.IdEvento;
                        ViewBag.Evento = evento.NombreEvento;
                        ViewBag.FechaUltimoAcceso = evento.FechaUltimoAcceso ?? DateTime.Now;
                        ViewBag.Descripcion = evento.Descripcion;

                        var inscritos = _eventoNegocio.ListEventoUsuarioAsignado(new EventoUsuarioRequest { IdEvento = evento.IdEvento });
                        ViewBag.Inscritos = inscritos.Count;

                        _eventoNegocio.UpdateEventoAccedidoUsuario(new EventoUsuarioRequest { IdEvento = evento.IdEvento }, user);

                        var modulos = _eventoNegocio.ListModuloWithLecciones(new ModuloRequest { IdEvento = evento.IdEvento });

                        var video = new VirtualNegocio().ListVirtualVideoByEvento(new VirtualContenidoRequest { IdEvento = evento.IdEvento }).FirstOrDefault();
                        var viewModel = new CursoDetalleViewModel
                        {
                            UrlVideo = video == null ? "" : video.Url,
                            CantidadLecciones = modulos.Sum(m => m.Lecciones.Count()),
                            CantidadHoras = modulos.Sum(m => m.Lecciones.Sum(l => l.Duracion)) / 60,
                            Modulos = modulos
                        };

                        return View(viewModel);
                    }
                    else
                    {
                        ViewBag.RowId = id;
                        ViewBag.FechaUltimoAcceso = DateTime.Now;
                        ViewBag.Descripcion = "";
                        ViewBag.Inscritos = 0;

                        ViewBag.IdEvento = 0;
                        ViewBag.Evento = "NO SE ENCONTRO EVENTO ACTIVO";
                    }
                }
                else
                {
                    ViewBag.RowId = id;
                    ViewBag.FechaUltimoAcceso = DateTime.Now;
                    ViewBag.Descripcion = "";
                    ViewBag.Inscritos = 0;

                    ViewBag.IdEvento = 0;
                    ViewBag.Evento = "";
                }
                return View(new CursoDetalleViewModel { Modulos = new List<ModuloResponse>() });
            }
            else
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }
        }
        public ActionResult DetalleHtml(string IdEvento)
        {
            var user = getUser();
            ViewBag.NumeroRelease = Configuracion.NumeroRelease;
            if (Session[NameSession.IdUsuario] != null)
            {
                if (IdEvento != null)
                {
                    VirtualContenidoRequest request = new VirtualContenidoRequest();
                    request.IdEvento = Convert.ToInt32(IdEvento);
                    var response = _virtualNegocio.GetContenidoVirtualByEvento(request);
                    ViewBag.Contenido = response == null ? "" : response.Contenido;
                    return View();
                }
                else
                {
                    ViewBag.IdEvento = 0;
                    ViewBag.Evento = "";
                }
                return View(new CursoDetalleViewModel { Modulos = new List<ModuloResponse>() });
            }
            else
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }
        }
        public ActionResult VerLeccion(int? id, string rowId)
        {
            var user = getUser();
            ViewBag.NumeroRelease = Configuracion.NumeroRelease;
            if (Session[NameSession.IdUsuario] != null)
            {
                if (id != null)
                {
                    var request = new LeccionRequest();
                    request.IdLeccion = id.Value;
                    var leccion = _eventoNegocio.GetLeccionByEventoActivo(request);                   
                    if (leccion != null)
                    {
                        ViewBag.IdEvento = leccion.IdEvento;
                        ViewBag.RowId = rowId;
                        ViewBag.IdLeccion = leccion.IdLeccion;
                        ViewBag.Leccion = leccion.Nombre;

                        var leccionRequest = new LeccionRequest { IdModulo = leccion.IdModulo.Value, Orden = leccion.Orden, IdEvento = leccion.IdEvento };
                        var leccionAnterior = _eventoNegocio.GetPrevLeccion(leccionRequest);
                        var leccionSiguiente = _eventoNegocio.GetNextLeccion(leccionRequest);

                        if (leccionAnterior != null)
                        {
                            ViewBag.LeccionAnterior = leccionAnterior;
                        }

                        if (leccionSiguiente != null)
                        {
                            ViewBag.LeccionSiguiente = leccionSiguiente;
                        }

                        if (leccion.Tipo == 3) // Evaluación
                        {
                            var cuestionario = _cuestionarioNegocio.GetCuestionario(new CuestionarioRequest { IdLeccion = leccion.IdLeccion });
                            var evaluacion = _cuestionarioNegocio.GetEvaluacion(new EvaluacionRequest { IdCuestionario = cuestionario.IdCuestionario, IdUsuario = user.IdUsuario, IdEvento = leccion.IdEvento });
                            var Preguntas = _cuestionarioNegocio.GetCuestionarioCompleto(new CuestionarioRequest { IdLeccion = leccion.IdLeccion }, getUser());

                            if (evaluacion != null && !(bool)evaluacion.Abierto)
                            {
                                ViewBag.Intento = evaluacion.Intento + 1;
                                ViewBag.Nota = evaluacion.Nota;
                                ViewBag.Abierto = (bool)evaluacion.Abierto;
                                return View("VerResultado", Preguntas);
                            }

                            if (evaluacion != null && evaluacion.Intento == 3)
                            {
                                ViewBag.Abierto = (bool)evaluacion.Abierto;
                                ViewBag.Intento = evaluacion.Intento + 1;
                                ViewBag.Nota = evaluacion.Nota;
                                return View("VerResultado", Preguntas);
                            }
                            else
                            {                                
                                if (evaluacion == null)
                                {
                                    ViewBag.Intento = 1;
                                    ViewBag.Nota = 0;
                                }
                                else
                                {
                                    ViewBag.Intento = evaluacion.Intento + 1;
                                    ViewBag.Nota = evaluacion.Nota;
                                }
                                return View("VerEvaluacion", Preguntas);
                            }
                        }
                        else {
                            return View(leccion);
                        }
                    }
                    else
                    {
                        return RedirectToAction("Matriculados", "Cursos", new { area = "Aula" });
                    }
                }
                else
                {
                    return RedirectToAction("Matriculados", "Cursos", new { area = "Aula" });
                }
            }
            else
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }
        }
        public ActionResult DetalleLeccionHtml(int Id)
        {
            var user = getUser();
            if (Session[NameSession.IdUsuario] != null)
            {
                var request = new LeccionRequest();
                request.IdLeccion = Id;
                var leccion = _eventoNegocio.GetLeccion(request);
                ViewBag.Descripcion = leccion.Descripcion;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }
        }
        public ActionResult Agregar()
        {
            return View();
        }
        public ActionResult Setting()
        {
            var user = getUser();
            ViewBag.NumeroRelease = Configuracion.NumeroRelease;
            return View();
        }
        #endregion


        #region Metodos Evento

        [HttpPost]
        public ActionResult ListEvento()
        {
            PageRequest page = new PageRequest();
            page.PageNumber = (Convert.ToInt32(Request.Form.GetValues("start")[0]) / Convert.ToInt32(Request.Form.GetValues("length")[0])) + 1;
            page.PageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
            page.Order = "DESC";

            var response = _eventoNegocio.ListEvento(page);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveCurso(FormCollection collection)
        {
            EventoRequest request = new EventoRequest();
            request.IdEvento = Convert.ToInt32(collection["IdEvento"]);
            request.NombreEvento = collection["NombreEvento"].ToString();
            request.Expositor = collection["Expositor"].ToString();
            request.Fecha = Convert.ToDateTime(collection["Fecha"].ToString());
            request.Horas = Convert.ToInt32(collection["Horas"].ToString());
            request.Costo = collection["Costo"].ToString();

            Result response;

            if (request.IdEvento == 0)
            {
                response = _eventoNegocio.SaveEvento(request, getUser());
            }
            else
            {
                response = _eventoNegocio.UpdateEvento(request, getUser());
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
        public ActionResult UpdateCursoFiles(HttpPostedFileBase[] documento1, HttpPostedFileBase[] documento2, HttpPostedFileBase[] documento3, HttpPostedFileBase[] documento4, HttpPostedFileBase[] documento5, FormCollection collection)
        {
            EventoRequest request = new EventoRequest();
            request.IdEvento = Convert.ToInt32(collection["IdEvento"]);

            string nombreFoto = Guid.NewGuid() + ".JPG";
            string nombreFotocheck = Guid.NewGuid() + ".PDF";
            string nombreCertificado = Guid.NewGuid() + ".PDF";
            string nombreCertificadoImprimir = Guid.NewGuid() + ".PDF";
            string nombreCertificadoExpositor = Guid.NewGuid() + ".PDF";
            string pathDocumentos = Server.MapPath(Url.Content("~/Documentos/"));

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

            Result response;

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
                    if (documento1 != null)
                    {
                        var documentoFoto = documento1[0];
                        documentoFoto.SaveAs(pathDocumentos + nombreFoto);
                        ResizeImage(pathDocumentos + nombreFoto, pathDocumentos + nombreFoto, 600, 300);
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
        //Modulo
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
        #region GenerarCertificado
        [HttpPost]
        public ActionResult CargarPdfCertificadoUtomatico(EventoRequest documentos)
        {
            var response = new Result();
            try
            {
                InscripcionRequest requestGet = new InscripcionRequest();
                requestGet.IdEvento = documentos.IdEvento;

                var EventoInscripcion = _eventoNegocio.GetEventoInscripcion(requestGet, getUser());

                if (!(EventoInscripcion.GenerarCertificado ?? false))
                {
                    response.IsSuccess = false;
                    response.Message = Message.GeneracionCertificado;
                    return Json(response, JsonRequestBehavior.AllowGet);
                }

                if (EventoInscripcion.Certificado != null)
                {
                    var pathDocumentosExiste = Configuracion.urlFileServer + Configuracion.CodigoEmpresa + "/" + NombreCarpeta.EventoCertificadoFirmado + "/";
                    var rutaDoc = pathDocumentosExiste + EventoInscripcion.Certificado;
                    if (System.IO.File.Exists(rutaDoc))
                    {
                        //Cerramos la inscripción
                        var cerrar = _eventoNegocio.CerrarEventoUsuario(requestGet, getUser());

                        response.IsSuccess = true;
                        response.Message = EventoInscripcion.Certificado;
                        return Json(response, JsonRequestBehavior.AllowGet);
                    }
                }
                if (EventoInscripcion.Nota == null)
                {
                    var validarExamen = _eventoNegocio.ValidarFinalizacionEvento(requestGet, getUser());
                    if (!validarExamen.IsSuccess)
                    {
                        return Json(validarExamen, JsonRequestBehavior.AllowGet);
                    }
                    EventoInscripcion.Nota = validarExamen.ResultNota;
                }                                       
                InscripcionRequest request = new InscripcionRequest();
                request.IdInscripcion = EventoInscripcion.IdInscripcion;
                request.IdEvento = EventoInscripcion.IdEvento;
                request.IdPersona = EventoInscripcion.IdPersona;
                request.TipoInscripcion = EventoInscripcion.TipoInscripcion;
                request.Nota = EventoInscripcion.Nota;

                PersonaRequest persona = new PersonaRequest();
                persona.TipoOcupacionAbreviatura = EventoInscripcion.TipoOcupacionAbreviatura;
                persona.Nombres = EventoInscripcion.Nombres;
                persona.ApellidoPaterno = EventoInscripcion.ApellidoPaterno;
                persona.ApellidoMaterno = EventoInscripcion.ApellidoMaterno;


                var pathDocumentos = Configuracion.urlFileServer + Configuracion.CodigoEmpresa + "/" + NombreCarpeta.Evento + "/";
                string IdUsuarioString = getUser().IdUsuario.ToString();
                string FechaHora = DateTime.Now.ToString("ddMMyyyy_HHmmss") + "_" + IdUsuarioString;

                string rutaView = pathDocumentos + "/TemCertificados/";
                if (!System.IO.Directory.Exists(rutaView))
                {
                    System.IO.Directory.CreateDirectory(rutaView);
                }

                var NombreCertificado = persona.TipoOcupacionAbreviatura + " " + persona.Nombres + " " + persona.ApellidoPaterno + " " + persona.ApellidoMaterno;
                var CodigoBarras = "";
                CodigoBarras = request.IdInscripcion.ToString() + request.IdEvento.ToString() + request.IdPersona.ToString();
                CodigoBarras = CodigoBarras.Length >= 16 ? CodigoBarras : CodigoBarras.PadRight(16, '0');

                ModuloRequest requestM = new ModuloRequest();
                requestM.IdEvento = documentos.IdEvento;
                var Modulos = new List<ModuloResponse>();
                if (request.TipoInscripcion == 1)
                {
                    if (EventoInscripcion.DetallarCertificado ?? false)
                    {
                        Modulos = _eventoNegocio.ListModulo(requestM);
                    }
                }

                string NombreDocumentoSalida = persona.ApellidoPaterno.Trim() + "_" + persona.ApellidoMaterno.Trim() + "_" + persona.Nombres.Trim() + "_" + FechaHora + ".PDF";
                string NombreDocumentoSalida1 = persona.ApellidoPaterno.Trim() + "_" + persona.ApellidoMaterno.Trim() + "_" + persona.Nombres.Trim() + "1_" + FechaHora + ".PDF";
                string NombreDocumentoSalida2 = persona.ApellidoPaterno.Trim() + "_" + persona.ApellidoMaterno.Trim() + "_" + persona.Nombres.Trim() + "2_" + FechaHora + ".PDF";

                string ruta = pathDocumentos + EventoInscripcion.NombreCertificadoImprimir;
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
                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, CodigoBarras, (ancho - 125) / 2, reader.GetPageSizeWithRotation(1).Height - 48f, 0);

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
                    string textoNota = "";
                    if (request.Nota != null && request.Nota != 0)
                    {
                        textoNota = "Promedio Final: " + request.Nota;
                    }
                    PdfPCell cell3 = new PdfPCell(new Phrase(textoNota, FontFactory.GetFont(BaseFont.HELVETICA_BOLD, 12f, BaseColor.BLACK)));
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
                System.IO.File.Copy(rutaView + NombreDocumentoSalida, pathDocumentos1 + NombreCertificado1);
                request1.Certificado = NombreCertificado1;

                response = _inscripcionNegocio.UpdateInscripcionCertificado(request1, getUser());
                if (response.IsSuccess)
                {
                    response.Message = NombreCertificado1;
                }                
            }
            catch (Exception ex)
            {
                response.Message = Message.ErrorNoControlado;
                response.MessageExeption = ex.Message ;
                response.IsSuccess = false;
            }
            return Json(response, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public FileResult DescargarPdfGenerado(string NombreDocumento)
        {
            var pathDocumentos1 = Configuracion.urlFileServer + Configuracion.CodigoEmpresa + "/" + NombreCarpeta.EventoCertificadoFirmado + "/";
            var rutaDoc = pathDocumentos1 + NombreDocumento;
            if (System.IO.File.Exists(rutaDoc))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(rutaDoc);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, getUser().NombreUsuario + ".pdf");
            }
            return null;
        }
        #endregion
        [HttpPost]
        public ActionResult SaveEvaluacion(int idCuestionario, List<RespuestaRequest> request, int IdEvento)
        {
            Result response;
            var evaluacionRequest = new EvaluacionRequest
            {
                IdCuestionario = idCuestionario,
                Respuestas = request,
                IdEvento = IdEvento
            };

            response = _cuestionarioNegocio.SaveEvaluacion(evaluacionRequest, getUser());

            return Json(response, JsonRequestBehavior.AllowGet);
        }

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
    }
}