using DBR.Evento.Modelo;
using DBR.Evento.Modelo.Response;
using DBR.Eventos.Comun;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Linq;
using System.Web.Security;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using System.Web;
using System.IO;
using System.Net.Http;
using DBR.Eventos.Presentacion.Helpes;

namespace DBR.Eventos.Presentacion.Controllers.Base
{
    public class BaseController : Controller
    {
        public UsuarioLogin getUser()
        {
            UsuarioLogin user = new UsuarioLogin();

            var faCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (faCookie != null)
            {
                FormsAuthenticationTicket _authTicket = FormsAuthentication.Decrypt(faCookie.Value);
                UsuarioResponse _UsuarioResponse = JsonConvert.DeserializeObject<UsuarioResponse>(_authTicket.UserData);

                user.IdUsuario = _UsuarioResponse.IdUsuario;
                user.NombreUsuario = _UsuarioResponse.Nombres + " " + _UsuarioResponse.ApellidoPaterno + " " + _UsuarioResponse.ApellidoMaterno;
                user.IdUsuarioTipo = (int)_UsuarioResponse.IdUsuarioTipo;
                user.Token = _UsuarioResponse.Token;

                Session[NameSession.IdUsuario] = _UsuarioResponse.IdUsuario;
                Session[NameSession.NombreUsuario] = _UsuarioResponse.Nombres + " " + _UsuarioResponse.ApellidoPaterno + " " + _UsuarioResponse.ApellidoMaterno;
                Session[NameSession.IdUsuarioTipo] = _UsuarioResponse.IdUsuarioTipo;
                Session[NameSession.Password] = _UsuarioResponse.Password;

                Session[NameSession.IdPersona] = _UsuarioResponse.IdPersona;
                Session[NameSession.Nombres] = _UsuarioResponse.Nombres;
                Session[NameSession.ApellidoPaterno] = _UsuarioResponse.ApellidoPaterno;
                Session[NameSession.ApellidoMaterno] = _UsuarioResponse.ApellidoMaterno;
                Session[NameSession.NumeroDocumento] = _UsuarioResponse.NumeroDocumento;
                Session[NameSession.Correo] = _UsuarioResponse.Correo;

                if (_UsuarioResponse.UltimoAcceso != null)
                {
                    DateTime UltimoAcceso = (DateTime)_UsuarioResponse.UltimoAcceso;
                    Session[NameSession.UltimoAcceso] = UltimoAcceso.AddHours(Configuracion.DiferenciaZona).ToString("dd/MM/yyyy hh:mm tt");
                }
                else
                {
                    Session[NameSession.UltimoAcceso] = "00/00/0000 00:00";
                }
                return user;
            }
            else
            {
                return null;
            }

            //user.IdUsuario = Convert.ToInt32(Session[NameSession.IdUsuario]);
            //user.NombreUsuario = Session[NameSession.NombreUsuario].ToString();
            //user.IdUsuarioTipo = Convert.ToInt32(Session[NameSession.IdUsuarioTipo]);
            //return user;
        }
        public List<OpcionResponse> getOptions()
        {
            List<OpcionResponse> _options = new List<OpcionResponse>();
            var faCookie = ControllerContext.HttpContext.Request.Cookies[NameCookies._OPTIONS];
            if (faCookie != null)
            {
                _options = JsonConvert.DeserializeObject<List<OpcionResponse>>(AESEncrytDecry.DecryptStringAES(faCookie.Value, Configuracion.TokenEncriptado));
            }
            return _options;
        }
        public ActionResult ValidarSesion(System.Reflection.MethodBase GetCurrentMethod)
        {
            if (getUser() != null)
            {
                var urlMetodo = GetCurrentMethod.ReflectedType.Name.Replace("Controller", "/") + GetCurrentMethod.Name;

                List<OpcionResponse> menu = getOptions();

                ViewBag.DescripcionPadre = GetCurrentMethod.Name;
                ViewBag.DescripcionHijo = GetCurrentMethod.Name;

                ViewBag.NumeroRelease = Configuracion.NumeroRelease;
                return View();
            } else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult ValidarSesion<T>(System.Reflection.MethodBase GetCurrentMethod, T data)
        {
            if (getUser() != null)
            {
                var urlMetodo = GetCurrentMethod.ReflectedType.Name.Replace("Controller", "/") + GetCurrentMethod.Name;

                List<OpcionResponse> menu = getOptions();

                ViewBag.DescripcionPadre = GetCurrentMethod.Name;
                ViewBag.DescripcionHijo = GetCurrentMethod.Name;

                ViewBag.NumeroRelease = Configuracion.NumeroRelease;
                return View(data);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
        public ActionResult ValidarSesionOpciones(System.Reflection.MethodBase GetCurrentMethod)
        {
            if (getUser() != null)
            {
                var urlMetodo = GetCurrentMethod.ReflectedType.Name.Replace("Controller", "/") + GetCurrentMethod.Name;


                List<OpcionResponse> menu = getOptions();

                if (menu == null)
                {
                    return RedirectToAction("AccesoDenegado", "Login", new { area = "" });
                }

                var existeMenu = menu.Where(x => x.UrlDescripcion == urlMetodo).ToList();

                if (existeMenu.Count == 0)
                {
                    urlMetodo = "Aula/" + urlMetodo;

                    existeMenu = menu.Where(x => x.UrlDescripcion == urlMetodo).ToList();

                    if (existeMenu.Count == 0)
                    {
                        return RedirectToAction("AccesoDenegado", "Login", new { area = "" });
                    }
                }
                else
                {
                    var padre = menu.Where(x => x.Id == existeMenu[0].IdPadre).ToList();

                    if (padre.Count > 0)
                    {
                        ViewBag.DescripcionPadre = padre[0].Descripcion;
                        ViewBag.DescripcionHijo = existeMenu[0].Descripcion;
                    }
                    else
                    {
                        ViewBag.DescripcionPadre = "Inicio";
                        ViewBag.DescripcionHijo = existeMenu[0].Descripcion;
                    }

                }

                ViewBag.NumeroRelease = Configuracion.NumeroRelease;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login", new { area = "" });
            }
        }
        public bool ValidarReCapcha(string Capcha)
        {
            string fullimagepath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"LogError");
            CapturaErrores log = new CapturaErrores();
            try
            {
                using (var client = new HttpClient())
                {
                    var values = new Dictionary<string, string>
                    {
                        {"secret", Configuracion.reCapchaKeySecret},
                        {"response", Capcha},
                        {"remoteip", Request.UserHostAddress}
                    };

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

                    var content = new FormUrlEncodedContent(values);
                    var verify = client.PostAsync("https://www.google.com/recaptcha/api/siteverify", content).Result;
                    var captchaResponseJson = verify.Content.ReadAsStringAsync().Result;
                    log.WriteLogRecapcha(captchaResponseJson, fullimagepath);
                    var captchaResult = JsonConvert.DeserializeObject<ResponseCapcha>(captchaResponseJson);
                    return captchaResult.success && captchaResult.score >= 0.3;
                }
            }
            catch (Exception ex)
            {
                log.WriteLogRecapcha(ex.Message, fullimagepath);
                return false;
            }
        }
        public bool ValidarCorreo(string email)
        {
            string expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, string.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool ValidarConplejidadPassword(string password)
        {
            string expresion;
            expresion = @"(?=^.{8,15}$)(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?!.*\s).*$";
            if (Regex.IsMatch(password, expresion))
            {
                if (Regex.Replace(password, expresion, string.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public static string ResizeImageWidth(string strImgPath, string strImgOutputPath, int iWidth)
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
                                   ".JPEG",
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
                    System.Net.HttpWebRequest wreq = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(strImgPath);
                    System.Net.HttpWebResponse wresp = (System.Net.HttpWebResponse)wreq.GetResponse();
                    mystream = wresp.GetResponseStream();
                }
                else
                    mystream = System.IO.File.OpenRead(strImgPath);

                // Cargo la imágen
                System.Drawing.Bitmap imgToResize = new System.Drawing.Bitmap(mystream);

                //System.Drawing.Size size = new System.Drawing.Size(iWidth, iWidth);

                int sourceWidth = imgToResize.Width;
                int sourceHeight = imgToResize.Height;

                float nPercent = 0;
                float nPercentW = 0;

                nPercentW = ((float)iWidth / (float)sourceWidth);

                nPercent = nPercentW;

                int destWidth = iWidth;
                int destHeight = (int)(sourceHeight * nPercent);

                System.Drawing.Bitmap b = new System.Drawing.Bitmap(destWidth, destHeight);
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage((System.Drawing.Image)b);
                //Para agregar fondo blanco al lienzo
                //g.Clear(System.Drawing.Color.White);

                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
                g.Dispose();

                // We will store the correct image codec in this object
                System.Drawing.Imaging.ImageCodecInfo ici = GetEncoderInfo("image/png"); ;
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
        public void Crop(string strImgPath, string strImgOutputPath, int width, int height, int x, int y)
        {
            bool mismaImagen = strImgPath.Equals(strImgOutputPath);
            if (mismaImagen)
            {
                strImgOutputPath = strImgPath + "___.PNG";
            }
            //System.Drawing.Image source = new System.Drawing.Bitmap(strImgPath);
            //System.Drawing.Image source2 = new System.Drawing.Image.FromFile(strImgPath); 

            WebClient wc = new WebClient();
            byte[] bytes = wc.DownloadData(strImgPath);
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytes);
            System.Drawing.Image source = System.Drawing.Image.FromStream(ms);


            System.Drawing.Rectangle section = new System.Drawing.Rectangle(new System.Drawing.Point(x, y), new System.Drawing.Size(width, height));
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(section.Width, section.Height);

            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp);

            g.DrawImage(source, 0, 0, section, System.Drawing.GraphicsUnit.Pixel);

            // Almacenaremos el códec de imagen correcto en este objeto
            System.Drawing.Imaging.ImageCodecInfo ici = GetEncoderInfo("image/png"); ;
            // Esto especificará la calidad de imagen al codificador
            System.Drawing.Imaging.EncoderParameter epQuality = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 99L);
            // Almacene el parámetro de calidad en la lista de parámetros del codificador
            System.Drawing.Imaging.EncoderParameters eps = new System.Drawing.Imaging.EncoderParameters(1);
            eps.Param[0] = epQuality;
            bmp.Save(strImgOutputPath, ici, eps);
            g.Dispose();
            source.Dispose();
            bmp.Dispose();

            if (mismaImagen)
            {
                System.IO.File.Delete(strImgPath);
                System.IO.File.Move(strImgOutputPath, strImgPath);
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
        //Metodo para unir pdfs
        public Result UnirArchivos(string strFileTarget, List<string> lstFiles)
        {
            Result blnMerged = new Result();
            iTextSharp.text.pdf.PdfReader reader = null;
            iTextSharp.text.Document sourceDocument = null;
            iTextSharp.text.pdf.PdfCopy pdfCopyProvider = null;
            iTextSharp.text.pdf.PdfImportedPage importedPage;


            sourceDocument = new iTextSharp.text.Document();
            System.IO.FileStream fileStream = new System.IO.FileStream(strFileTarget, System.IO.FileMode.Create);
            pdfCopyProvider = new iTextSharp.text.pdf.PdfCopy(sourceDocument, fileStream);

            sourceDocument.Open();
            try
            {
                //Bucle a través de la lista de archivos
                for (int f = 0; f < lstFiles.Count; f++)
                {
                    int pages = get_pageCcountAlt(lstFiles[f]);
                    reader = new iTextSharp.text.pdf.PdfReader(lstFiles[f]);
                    //Añadir páginas del archivo actual
                    for (int i = 1; i <= pages; i++)
                    {
                        importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                        pdfCopyProvider.AddPage(importedPage);
                    }
                    reader.Close();
                }
                sourceDocument.Close();
                blnMerged.IsSuccess = true;
            }
            catch (Exception ex)
            {
                blnMerged.IsSuccess = false;
                blnMerged.MessageExeption = ex.Message;
            }
            return blnMerged;
        }
        private int get_pageCcountAlt(string pdf)
        {
            byte[] bytesSLIMediosPaginados = System.IO.File.ReadAllBytes(pdf);
            iTextSharp.text.pdf.PdfReader readerSLIMediosPaginados = new iTextSharp.text.pdf.PdfReader(bytesSLIMediosPaginados);
            int NumberPages = readerSLIMediosPaginados.NumberOfPages;
            return NumberPages;
        }
        //Metodos para plantilas de correos
        public string GetPlantillaMensajeEmpresa(string Mensaje, string CorreoSalidad, bool EsRobot)
        {
            var plantilla =
                "<table border='0' style='background-color:#d8d8d8; border-spacing:0; width:100%'>" +
                    "<tbody>" +
                        "<tr>" +
                            "<td align='center' style='padding:0px'>" +
                              "<table style='background-color:#ffffff; max-width:700px; width:100%'>" +
                                 "<tbody>" +
                                    "<tr>" +
                                        "<td style='padding:10px'>" + Mensaje + "</td>" +
                                    "</tr>" +
                                 "</tbody>" +
                              "</table>" +
                            "</td>" +
                        "</tr>";
            if (EsRobot)
            {
                plantilla +=

                        "<tr>" +
                            "<td align='center' style='padding:0px'>" +
                              "<table style='background-color:#ffffff; max-width:700px; width:100%'>" +
                                 "<tbody>" +
                                    "<tr>" +
                                        "<td style='padding:10px;font-weight:700;font-size:16px;'>🤖 Este correo es generado por un robot; no responda a este remitente.</td>" +
                                    "</tr>" +
                                 "</tbody>" +
                              "</table>" +
                            "</td>" +
                        "</tr>";
            }
            plantilla +=
                "</tbody>" +
            "</table>";

            if (Configuracion.CodigoEmpresa == 1)
            {
                plantilla+=
                    "<table border='0' style='background-color:#d8d8d8; border-spacing:0; width:100%'>" +
                        "<tbody>" +
                            "<tr>" +
                                "<td align='center' style='padding:0px'>" +
                                "<table style='background-color:#ffffff; text-align:center; width:100%'>" +
                                    "<tbody>" +
                                        "<tr>" +
                                            "<td align='center' style='color:#999999; font-family:Helvetica,Arial,sans-serif; padding:5px'><span style='font-size:14px; line-height:18px'><img alt='' src='http://www.ingenierosx100.org/images/Correo/arbol_1308-36471.jpg' /> <span style='color:#007417; font-size:11px'>Protege nuestro medio ambiente. No imprimas este email de no ser necesario.</span> </span></td>" +
                                        "</tr>" +
                                    "</tbody>" +
                                "</table>" +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td align='center' style='padding:0px'>" +
                                "<table style='background-color:#333333; height:100px; text-align:center; width:100%'>" +
                                    "<tbody>" +
                                        "<tr>" +
                                            "<td style='height:100px; text-align:center; width:50%'>" +
                                            "<h2><strong><a href='http://www.ingenierosx100.org/' style='color:#FFF;text-decoration:none;font-family:Arial;' target='_blank'>100% INGENIEROS</a></strong></h2>" +
                                            "</td>" +
                                            "<td style='height:100px; text-align:center; width:50%'><a href='https://www.facebook.com/ingenierosx100/' target='_blank' title='Facebook'><img alt='Facebook' src='http://www.ingenierosx100.org/images/Correo/fb.png' style='width:50px' /> </a> <a href='https://api.whatsapp.com/send?phone=51947852211&amp;text=&amp;source=&amp;data=' target='_blank' title='(+51)947852211'> <img alt='Whatsapp' src='http://www.ingenierosx100.org/images/Correo/ws.png' style='width:50px' /> </a></td>" +
                                        "</tr>" +
                                    "</tbody>" +
                                "</table>" +
                                "</td>" +
                            "</tr>" +
                            "<tr>" +
                                "<td align='center' style='padding:0px'>" +
                                    "<table style='background-color:#d8d8d8; max-width:700px; width:100%'>" +
                                        "<tbody>" +
                                            "<tr>" +
                                                "<td align='center' style='color:#999999; font-family:Helvetica,Arial,sans-serif; font-size:12px; padding:5px'>" +
                                                "<p>De acuerdo con la Ley N&deg; 28493 y su Reglamento, le informamos que este mensaje ha sido enviado por 100% INGENIEROS, con domicilio Mz. G - Lote 14 - Urb. Las Capullanas &middot; Trujillo - Per&uacute;.</p>" +
                                                "<p>La direcci&oacute;n de correo electr&oacute;nico de contacto para efectos de este env&iacute;o es "+ CorreoSalidad + "</p>" +
                                                "</td>" +
                                            "</tr>" +
                                        "</tbody>" +
                                    "</table>" +
                                    "<table style='background-color:#FFF;width:100%;border-spacing: 0;' border='0'>" +
                                        "<tbody>" +
                                            "<tr>" +
                                                "<td align='center' style='padding:0px;'>" +
                                                    "<table style='background-color:#FFF; max-width:700px; width:100%;'>" +
                                                        "<tbody>" +
                                                            "<tr>" +
                                                                "<td align='center' style ='padding:5px;color:#999999;font-family:Helvetica,Arial,sans-serif;'>" +
                                                                    "¿Deseas dejar de recibir estos emails? <a href='https://www.ingenierosx100.org/web/Desafiliacion'>Click aquí</a>" +
                                                                "</td>" +
                                                            "</tr>" +
                                                            "<tr>" +
                                                                "<td align='center' style='padding:5px;color:#999999;font-family:Helvetica,Arial,sans-serif;'>" +
                                                                    "100 % INGENIEROS · Mz.G - Lote 14 - Urb.Las Capullanas · Trujillo - Perú" +
                                                                "</td>" +
                                                            "</tr>" +
                                                        "</tbody>" +
                                                    "</table>" +
                                                "</td>" +
                                            "</tr>" +
                                        "</tbody>" +
                                    "</table>" +
                              "</td>" +
                            "</tr>" +
                        "</tbody>" +
                    "</table>";
            }
            if (Configuracion.CodigoEmpresa == 2)
            {
                plantilla +=
                    "<table border='0' style='background-color:#d8d8d8; border-spacing:0; width:100%'>" +
                        "<tbody>" +
                            "<tr>" +
                                "<td align='center' style='padding:0px'>" +
                                "<table style='background-color:#ffffff; text-align:center; width:100%'>" +
                                    "<tbody>" +
                                        "<tr>" +
                                            "<td align='center' style='color:#999999; font-family:Helvetica,Arial,sans-serif; padding:5px'><span style='font-size:14px; line-height:18px'><img alt='' src='http://www.ingenierosx100.org/images/Correo/arbol_1308-36471.jpg' /> <span style='color:#007417; font-size:11px'>Protege nuestro medio ambiente. No imprimas este email de no ser necesario.</span> </span></td>" +
                                        "</tr>" +
                                    "</tbody>" +
                                "</table>" +
                                "</td>" +
                            "</tr>" +
                        "</tbody>" +
                    "</table>";
            }
            return plantilla;
        }
        //Crear contraseña aleartoria
        public string GetRandonPassword()
        {
            Random rdn = new Random();
            string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890%$#@";
            int longitud = caracteres.Length;
            char letra;
            int longitudContrasenia = 10;
            string contraseniaAleatoria = string.Empty;
            for (int i = 0; i < longitudContrasenia; i++)
            {
                letra = caracteres[rdn.Next(longitud)];
                contraseniaAleatoria += letra.ToString();
            }
            return contraseniaAleatoria;
        }
    }
}