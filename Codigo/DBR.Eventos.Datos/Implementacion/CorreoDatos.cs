using DBR.Evento.Modelo;
using DBR.Evento.Modelo.Request;
using DBR.Evento.Modelo.Response;
using DBR.Eventos.Comun;
using DBR.Eventos.Datos.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace DBR.Eventos.Datos.Implementacion
{
    public class CorreoDatos : BaseAcceso
    {
        public CorreoDatos(DbContext context) : base(context)
        {
        }
        //Correo
        public Paged<CorreoResponse> ListCorreoPaged(PageRequest page)
        {
            Paged<CorreoResponse> obj = new Paged<CorreoResponse>();

            using (var context = new DBRContext())
            {
                page.search.value = page.search.value ?? "";
                var p1 = new SqlParameter { ParameterName = "search", Value = page.search.value, SqlDbType = SqlDbType.VarChar };
                var p2 = new SqlParameter { ParameterName = "start", Value = page.start, SqlDbType = SqlDbType.Int };
                var p3 = new SqlParameter { ParameterName = "length", Value = page.length, SqlDbType = SqlDbType.Int };

                var response = context.Database.SqlQuery<CorreoResponse>("dbo.UspListCorreoPaged @search, @start, @length", p1, p2, p3).ToList();
                var Total = response.Count() == 0 ? 0 : response[0].TotalRegistros;

                obj.data = response;
                obj.recordsTotal = Total;
                obj.recordsFiltered = Total;
                return obj;
            }
        }
        public Result SaveCorreo(CorreoRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = new Correo();
                obj.Asunto = request.Asunto;
                obj.Origen = request.Origen;
                obj.NombreOrigen = request.NombreOrigen;
                obj.Mensaje = request.Mensaje;
                obj.EstadoCorreo = EstadoCorreo.Creado;
                obj.FechaEnvio = request.FechaEnvio;
                obj.NumeroEnvio = 0;
                obj.Estado = EstadoRegistro.Activo;
                obj.UsuarioCreacion = user.IdUsuario;
                obj.FechaCreacion = DateTime.Now;

                context.Correo.Add(obj);

                if (context.SaveChanges() > 0)
                {
                    result.IsSuccess = true;
                    result.Message = Message.ExitoGuardar;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = Message.ErrorGuardar;
                };
                context.Dispose();
                return result;
            }
        }
        public Result UpdateCorreo(CorreoRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = (from c in context.Correo where c.IdCorreo == request.IdCorreo select c).FirstOrDefault();

                obj.Asunto = request.Asunto;
                obj.Origen = request.Origen;
                obj.NombreOrigen = request.NombreOrigen;
                obj.Mensaje = request.Mensaje;
                obj.UsuarioModificacion = user.IdUsuario;
                obj.FechaModificacion = DateTime.Now;

                if (context.SaveChanges() > 0)
                {
                    result.IsSuccess = true;
                    result.Message = Message.ExitoActualizar;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = Message.ErrorActualizar;
                };
                context.Dispose();
                return result;
            }
        }
        public Result DeleteCorreo(CorreoRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = (from c in context.Correo
                           where c.IdCorreo == request.IdCorreo
                           select c).FirstOrDefault();

                obj.Estado = EstadoRegistro.Inactivo;
                obj.UsuarioModificacion = user.IdUsuario;
                obj.FechaModificacion = DateTime.Now;

                if (context.SaveChanges() > 0)
                {
                    result.IsSuccess = true;
                    result.Message = Message.ExitoEliminar;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = Message.ErrorEliminar;
                };
                context.Dispose();
                return result;
            }

        }
        public Result UpdateCorreoEnvio(CorreoRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = (from c in context.Correo
                           where c.IdCorreo == request.IdCorreo
                           select c).FirstOrDefault();

                obj.NumeroEnvio = request.NumeroEnvio;
                obj.FechaEnvio = DateTime.Now;
                obj.UsuarioModificacion = user.IdUsuario;
                obj.FechaModificacion = DateTime.Now;

                if (context.SaveChanges() > 0)
                {
                    result.IsSuccess = true;
                    result.Message = Message.ExitoEliminar;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = Message.ErrorEliminar;
                };
                context.Dispose();
                return result;
            }

        }
        public CorreoResponse GetCorreo(CorreoRequest request)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var query = (from c in context.Correo
                             where c.IdCorreo == request.IdCorreo
                             select new CorreoResponse
                             {
                                 IdCorreo = c.IdCorreo,
                                 Asunto = c.Asunto,
                                 Origen = c.Origen,
                                 NombreOrigen = c.NombreOrigen,
                                 Mensaje = c.Mensaje,
                                 EstadoCorreo = (int)c.EstadoCorreo,
                                 FechaEnvio = c.FechaEnvio,
                                 NumeroEnvio = (int)c.NumeroEnvio
                             });
                return query.FirstOrDefault();
            }
        }
        public int ConsultarEnvios(CorreoRequest request)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var query = (from c in context.CorreoDifusion
                             where c.NumeroEnvio == request.NumeroEnvio
                             select c.IdCorreoDifusion).ToList();
                context.Dispose();
                return query.Count();
            }
        }
        
        //CorreoDifusion
        public Result SaveCorreoDifusion(CorreoDifusionRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = new CorreoDifusion();
                obj.IdCorreo = request.IdCorreo;
                obj.IdEvento = request.IdEvento;
                obj.IdPersona = request.IdPersona;
                obj.Correo = request.Correo;
                obj.Estado = request.Estado;
                obj.ErrorMensaje = request.ErrorMensaje;
                obj.ErrorStackTrace = request.ErrorStackTrace;
                obj.UsuarioCreacion = user.IdUsuario;
                obj.FechaCreacion = DateTime.Now;
                obj.NumeroEnvio = request.NumeroEnvio;

                context.CorreoDifusion.Add(obj);

                if (context.SaveChanges() > 0)
                {
                    result.IsSuccess = true;
                    result.Message = Message.ExitoGuardar;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = Message.ErrorGuardar;
                };
                context.Dispose();
                return result;
            }

        }

        //Reporte
        public Paged<CorreoReporteResponse> ListCorreoReportePaged(PageRequest page)
        {
            using (var context = new DBRContext())
            {
                var response = context.Database.SqlQuery<CorreoReporteResponse>("ListCantidadMensajesByMes").ToList();

                Paged<CorreoReporteResponse> obj = new Paged<CorreoReporteResponse>();
                int PageIndex = page.PageNumber - 1;
                var rowsTotal = response.Count();

                var query = response.Skip(PageIndex * page.PageSize).Take(page.PageSize);

                obj.data = query.ToList();
                obj.recordsTotal = rowsTotal;
                obj.recordsFiltered = rowsTotal;
                return obj;
            }
        }

        //Desafiliado
        public Result SaveDesafiliado(DesafiliadoRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = new Desafiliado();
                obj.Correo = request.Correo;
                obj.Valor = request.Valor;
                obj.Observacion = request.Observacion;
                obj.Estado = EstadoRegistro.Activo;
                obj.UsuarioCreacion = user.IdUsuario;
                obj.FechaCreacion = DateTime.Now;

                context.Desafiliado.Add(obj);

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoGuardar;
                
                context.Dispose();
                return result;
            }
        }
    }
}
