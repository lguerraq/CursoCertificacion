using System.Data.Entity;
using DBR.Eventos.Datos.Base;
using System.Linq;
using DBR.Evento.Modelo.Response;
using DBR.Evento.Modelo.Request;
using DBR.Evento.Modelo;
using DBR.Eventos.Comun;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace DBR.Eventos.Datos.Implementacion
{
    public class VirtualDatos : BaseAcceso
    {
        public VirtualDatos(DbContext context) : base(context)
        {
        }
        //VirtualContenido
        public VirtualContenidoResponse GetContenidoVirtualByEvento(VirtualContenidoRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from cv in context.VirtualContenido
                             where cv.IdEvento == request.IdEvento
                             select new VirtualContenidoResponse
                             {
                                 IdVirtualContenido = cv.IdVirtualContenido,
                                 IdEvento = cv.IdEvento,
                                 Contenido = cv.Contenido
                             });

                return query.FirstOrDefault();

            }
        }
        public Result SaveVirtualContenido(VirtualContenidoRequest request,UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = new VirtualContenido();
                obj.IdEvento = request.IdEvento;
                obj.Contenido = request.Contenido;
                obj.Estado = EstadoActivo.Activo;
                obj.FechaCreacion = DateTime.Now;
                obj.UsuarioCreacion = user.IdUsuario;

                context.VirtualContenido.Add(obj);

                result.IsSuccess = context.SaveChanges() > 0;
                result.Codigo = obj.IdVirtualContenido;
                result.Message = Message.ExitoGuardar;

                return result;
            }
        }
        public Result UpdateVirtualContenido(VirtualContenidoRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = (from vc in context.VirtualContenido
                           where vc.Estado == EstadoActivo.Activo
                           && vc.IdVirtualContenido == request.IdVirtualContenido
                           select vc).FirstOrDefault();

                obj.IdEvento = request.IdEvento;
                obj.Contenido = request.Contenido;
                obj.Estado = EstadoActivo.Activo;
                obj.FechaCreacion = DateTime.Now;
                obj.UsuarioCreacion = user.IdUsuario;

                result.IsSuccess = context.SaveChanges() > 0;
                result.Codigo = obj.IdVirtualContenido;
                result.Message = Message.ExitoActualizar;

                return result;
            }
        }
        //VirtualVideo
        public List<VirtualVideoResponse> ListVirtualVideoByEvento(VirtualContenidoRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from vv in context.VirtualVideo
                             where vv.Estado == EstadoActivo.Activo
                             && vv.IdEvento == request.IdEvento
                             select new VirtualVideoResponse
                             {
                                 IdVirtualVideo = vv.IdVirtualVideo,
                                 IdEvento = vv.IdEvento,
                                 Url = vv.Url
                             });
                return query.ToList();
            }
        }
        public Result SaveVirtualVideo(VirtualVideoRequest request,UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj =new VirtualVideo();
                obj.IdEvento = request.IdEvento;
                obj.Url = request.Url;
                obj.Estado = EstadoActivo.Activo;
                obj.FechaCreacion = DateTime.Now;
                obj.UsuarioCreacion = user.IdUsuario;

                context.VirtualVideo.Add(obj);

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoGuardar;

                return result;
            }
        }
        public VirtualVideoResponse GetVirtualVideo(VirtualVideoRequest request)
        {
            using (var context = new DBRContext())
            {
                var obj = (from vv in context.VirtualVideo
                           where vv.Estado == EstadoRegistro.Activo
                           && vv.IdVirtualVideo == request.IdVirtualVideo
                           select new VirtualVideoResponse
                           {
                               IdVirtualVideo = vv.IdVirtualVideo,
                               IdEvento = vv.IdEvento,
                               Url = vv.Url
                           }).FirstOrDefault();
                return obj;
            }
        }
        public Result UpdateVirtualVideo(VirtualVideoRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = (from vv in context.VirtualVideo
                           where vv.Estado == EstadoRegistro.Activo
                           && vv.IdVirtualVideo == request.IdVirtualVideo
                           select vv).FirstOrDefault();

                obj.IdEvento = request.IdEvento;
                obj.Url = request.Url;
                obj.FechaModificacion = DateTime.Now;
                obj.UsuarioModificacion = user.IdUsuario;

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoActualizar;

                return result;
            }
        }
        public Result DeleteVirtualVideo(VirtualVideoRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = (from vv in context.VirtualVideo
                           where vv.Estado == EstadoRegistro.Activo
                           && vv.IdVirtualVideo == request.IdVirtualVideo
                           select vv).FirstOrDefault();

                obj.Estado = EstadoActivo.Inactivo;
                obj.FechaModificacion = DateTime.Now;
                obj.UsuarioModificacion = user.IdUsuario;

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoEliminar;

                return result;
            }
        }
        //EventousuarioEventoVideo
        public List<VirtualVideoResponse> ListEventoUsuarioVirtualVideoByEvento(VirtualContenidoRequest request)
        {
            using (var context = new DBRContext())
            {
                var p1 = new SqlParameter { ParameterName = "IdEvento", Value = request.IdEvento, SqlDbType = SqlDbType.VarChar };
                var p2 = new SqlParameter { ParameterName = "IdEventoUsuario", Value = request.IdEventoUsuario, SqlDbType = SqlDbType.Int };

                var response = context.Database.SqlQuery<VirtualVideoResponse>("dbo.ListEventoUsuarioVirtualVideoByEvento @IdEvento, @IdEventoUsuario", p1, p2).ToList();
                return response;
            }
        }
        public List<VirtualVideoResponse> ListVirtualVideoByUsuarioByEvento(VirtualContenidoRequest request,UsuarioLogin user)
        {
            using (var context = new DBRContext())
            {
                //if (user.IdUsuarioTipo == Convert.ToInt32(Perfil.Administrador) || user.IdUsuarioTipo == Convert.ToInt32(Perfil.EditorContenido))
                //{
                //    var query = (from vv in context.VirtualVideo
                //                 where vv.Estado == EstadoActivo.Activo
                //                 && vv.IdEvento == request.IdEvento
                //                 select new VirtualVideoResponse
                //                 {
                //                     IdVirtualVideo = vv.IdVirtualVideo,
                //                     IdEvento = vv.IdEvento,
                //                     Url = vv.Url
                //                 });
                //    return query.ToList();
                //}
                //else
                //{
                //    var query = (from vv in context.VirtualVideo
                //                 join euvv in context.EventoUsuarioVirtualVideo on vv.IdVirtualVideo equals euvv.IdVirtualVideo
                //                 join eu in context.EventoUsuario on euvv.IdEventoUsuario equals eu.IdEventoUsuario
                //                 where vv.Estado == EstadoActivo.Activo
                //                 && euvv.Estado == EstadoActivo.Activo
                //                 && vv.IdEvento == request.IdEvento
                //                 && eu.IdUsuario == user.IdUsuario
                //                 select new VirtualVideoResponse
                //                 {
                //                     IdVirtualVideo = vv.IdVirtualVideo,
                //                     IdEvento = vv.IdEvento,
                //                     Url = vv.Url
                //                 });
                //    return query.ToList();
                //}
                var p1 = new SqlParameter { ParameterName = "IdEvento", Value = request.IdEvento, SqlDbType = SqlDbType.Int };
                var p2 = new SqlParameter { ParameterName = "IdUsuario", Value = user.IdUsuario, SqlDbType = SqlDbType.Int };

                var response = context.Database.SqlQuery<VirtualVideoResponse>("dbo.ListVirtualVideoByUsuarioByEvento @IdEvento, @IdUsuario", p1, p2).ToList();
                return response;

            }
        }
    }
}
