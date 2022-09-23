using System.Data.Entity;
using DBR.Eventos.Datos.Base;
using DBR.Evento.Modelo;
using DBR.Evento.Modelo.Response;
using DBR.Evento.Modelo.Request;
using System.Linq;
using DBR.Eventos.Comun;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

namespace DBR.Eventos.Datos.Implementacion
{
    public class PortadaDatos : BaseAcceso
    {
        public PortadaDatos(DbContext context) : base(context)
        {
        }
        public Paged<PortadaResponse> ListPortadaPaginado(PageRequest page)
        {
            Paged<PortadaResponse> obj = new Paged<PortadaResponse>();

            using (var context = new DBRContext())
            {
                page.search.value = page.search.value == null ? "" : page.search.value;

                var p1 = new SqlParameter { ParameterName = "search", Value = page.search.value, SqlDbType = SqlDbType.VarChar };
                var p2 = new SqlParameter { ParameterName = "start", Value = page.start, SqlDbType = SqlDbType.Int };
                var p3 = new SqlParameter { ParameterName = "length", Value = page.length, SqlDbType = SqlDbType.Int };

                var response = context.Database.SqlQuery<PortadaResponse>("dbo.UspListPortadaPaged @search, @start, @length", p1, p2, p3).ToList();
                var Total = response.Count() == 0 ? 0 : response[0].TotalRegistros;

                obj.data = response;
                obj.recordsTotal = Total;
                obj.recordsFiltered = Total;
                return obj;
            }
        }
        public List<PortadaResponse> ListPortada()
        {
            using (var context = new DBRContext())
            {
                var query = (from g in context.Portada
                             where g.Estado == EstadoRegistro.Activo
                             select new PortadaResponse
                             {
                                 IdPortada = g.IdPortada,
                                 Descripcion = g.Descripcion,
                                 NombreImagen = g.NombreImagen,
                                 SubTitulo1 = g.SubTitulo1,
                                 SubTitulo2 = g.SubTitulo2,
                                 TextoEnlace = g.TextoEnlace,
                                 UrlEnlace = g.UrlEnlace
                             }).ToList();

                return query;
            }
        }
        public Result SavePortada(PortadaRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = new Portada();
                obj.NombreImagen = request.NombreImagen;
                obj.Descripcion = request.Descripcion;
                obj.SubTitulo1 = request.SubTitulo1;
                obj.SubTitulo2 = request.SubTitulo2;
                obj.TextoEnlace = request.TextoEnlace;
                obj.UrlEnlace = request.UrlEnlace;
                obj.Estado = EstadoRegistro.Activo;
                obj.UsuarioCreacion = user.IdUsuario;
                obj.FechaCreacion = DateTime.Now;

                context.Portada.Add(obj);

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoGuardar;

                return result;
            }
        }
        public Result DeletePortada(PortadaRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = (from g in context.Portada
                           where g.IdPortada == request.IdPortada
                           && g.Estado == EstadoRegistro.Activo
                           select g).FirstOrDefault();

                obj.Estado = EstadoRegistro.Inactivo;
                obj.UsuarioModificacion = user.IdUsuario;
                obj.FechaModificacion = DateTime.Now;

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoEliminar;

                return result;
            }
        }
        public Result UpdatePortada(PortadaRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = (from g in context.Portada
                           where g.IdPortada == request.IdPortada
                           && g.Estado == EstadoRegistro.Activo
                           select g).FirstOrDefault();

                obj.NombreImagen = request.NombreImagen;
                obj.Descripcion = request.Descripcion;
                obj.SubTitulo1 = request.SubTitulo1;
                obj.SubTitulo2 = request.SubTitulo2;
                obj.TextoEnlace = request.TextoEnlace;
                obj.UrlEnlace = request.UrlEnlace;
                obj.UsuarioModificacion = user.IdUsuario;
                obj.FechaModificacion = DateTime.Now;

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoActualizar;
                return result;
            }
        }
        public PortadaResponse GetPortada(PortadaRequest request)
        {
            using (var context = new DBRContext())
            {
                var obj = (from g in context.Portada
                           where g.IdPortada == request.IdPortada
                           && g.Estado == EstadoRegistro.Activo
                           select new PortadaResponse
                           {
                               IdPortada = g.IdPortada,
                               Descripcion = g.Descripcion,
                               NombreImagen = g.NombreImagen,
                               SubTitulo1 = g.SubTitulo1,
                               SubTitulo2 = g.SubTitulo2,
                               TextoEnlace=g.TextoEnlace,
                               UrlEnlace = g.UrlEnlace
                           }).FirstOrDefault();

                return obj;
            }
        }
    }
}
