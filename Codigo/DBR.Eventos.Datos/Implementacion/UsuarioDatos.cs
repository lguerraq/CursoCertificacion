using DBR.Eventos.Datos.Base;
using System.Data.Entity;
using System.Collections.Generic;
using DBR.Evento.Modelo.Response;
using System.Linq;
using DBR.Evento.Modelo.Request;
using DBR.Eventos.Comun;
using DBR.Evento.Modelo;
using System;
using System.Data.SqlClient;
using System.Data;

namespace DBR.Eventos.Datos.Implementacion
{
    public class UsuarioDatos : BaseAcceso
    {
        public UsuarioDatos(DbContext context) : base(context)
        {
        }
        public List<UsuarioResponse> ListUsuario()
        {
            using (var context=new DBRContext())
            {
                var query = (from u in context.Usuario
                             join ut in context.UsuarioTipo on u.IdUsuarioTipo equals ut.IdUsuarioTipo
                             where u.Estado==EstadoActivo.Activo
                             select new UsuarioResponse
                             {
                                 IdUsuario = u.IdUsuario,
                                 Login = u.Login,
                                 Password = u.Password,
                                 Nombres = u.Nombres,
                                 ApellidoPaterno = u.ApellidoPaterno,
                                 ApellidoMaterno = u.ApellidoMaterno,
                                 IdUsuarioTipo = u.IdUsuarioTipo,
                                 UsuarioTipo = ut.Descripcion,
                                 Correo=u.Correo
                             });
                return query.ToList();
            }
        }
        public Paged<UsuarioResponse> ListUsuarioPaged(PageRequest page)
        {
            Paged<UsuarioResponse> obj = new Paged<UsuarioResponse>();

            using (var context = new DBRContext())
            {

                page.search.value = page.search.value == null ? "" : page.search.value;

                var p1 = new SqlParameter { ParameterName = "search", Value = page.search.value, SqlDbType = SqlDbType.VarChar };
                var p2 = new SqlParameter { ParameterName = "start", Value = page.start, SqlDbType = SqlDbType.Int };
                var p3 = new SqlParameter { ParameterName = "length", Value = page.length, SqlDbType = SqlDbType.Int };

                var response = context.Database.SqlQuery<UsuarioResponse>("dbo.UspListUsuarioPaged @search, @start, @length", p1, p2, p3).ToList();
                var Total = response.Count() == 0 ? 0 : response[0].TotalRegistros;

                obj.data = response;
                obj.recordsTotal = Total;
                obj.recordsFiltered = Total;
                return obj;
            }

        }
        public Result SaveUsuario(UsuarioRequest request,UsuarioLogin user)
        {
            var result = new Result();
            using (var context = new DBRContext())
            {

                var query = (from u in context.Usuario
                             where u.Estado==EstadoActivo.Activo
                             && u.Login==request.Login
                             select u.IdUsuario).ToList();

                if (query.Count() > 0)
                {
                    result.IsSuccess = false;
                    result.Message = Message.UsuarioExiste;
                    return result;
                }

                var query2 = (from u in context.Usuario
                              where u.Estado == EstadoActivo.Activo
                              && u.NumeroDocumento == request.NumeroDocumento
                              select u.IdUsuario).ToList();

                if (query2.Count() > 0)
                {
                    result.IsSuccess = false;
                    result.Message = Message.UsuarioExisteDni;
                    return result;
                }

                var obj = new Usuario();
                obj.Login = request.Login;
                obj.Password = request.Password;
                obj.NumeroDocumento = request.NumeroDocumento;
                obj.Nombres = request.Nombres;
                obj.ApellidoPaterno = request.ApellidoPaterno;
                obj.ApellidoMaterno = request.ApellidoMaterno;
                obj.IdUsuarioTipo = request.IdUsuarioTipo;
                obj.UsuarioCreacion = user.IdUsuario;
                obj.Correo = request.Correo;
                obj.Estado = EstadoActivo.Activo;
                obj.FechaCreacion = DateTime.Now;

                context.Usuario.Add(obj);

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoGuardar;
                result.Codigo = obj.IdUsuario;
                return result;
            }
        }
        public Result UpdateUsuario(UsuarioRequest request, UsuarioLogin user)
        {
            var result = new Result();
            using (var context = new DBRContext())
            {

                var query = (from u in context.Usuario
                             where u.Estado == EstadoActivo.Activo
                             && u.Login == request.Login
                             && u.IdUsuario != request.IdUsuario
                             select u.IdUsuario).ToList();

                if (query.Count() > 0)
                {
                    result.IsSuccess = false;
                    result.Message = Message.UsuarioExiste;
                    return result;
                }

                var query2 = (from u in context.Usuario
                             where u.Estado == EstadoActivo.Activo
                             && u.NumeroDocumento == request.NumeroDocumento
                             && u.IdUsuario != request.IdUsuario
                             select u.IdUsuario).ToList();

                if (query2.Count() > 0)
                {
                    result.IsSuccess = false;
                    result.Message = Message.UsuarioExisteDni;
                    return result;
                }

                var obj = (from u in context.Usuario
                           where u.Estado == EstadoActivo.Activo
                           && u.IdUsuario == request.IdUsuario
                           select u).FirstOrDefault();

                obj.Login = request.Login;
                obj.Password = request.Password;
                obj.NumeroDocumento = request.NumeroDocumento;
                obj.Nombres = request.Nombres;
                obj.ApellidoPaterno = request.ApellidoPaterno;
                obj.ApellidoMaterno = request.ApellidoMaterno;
                obj.IdUsuarioTipo = request.IdUsuarioTipo;
                obj.UsuarioModificacion = user.IdUsuario;
                obj.Correo = request.Correo;
                obj.Estado = EstadoActivo.Activo;
                obj.FechaModificacion = DateTime.Now;

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoActualizar;
                result.Codigo = obj.IdUsuario;

                return result;
            }
        }
        public Result DeleteUsuario(UsuarioRequest request, UsuarioLogin user)
        {
            var result = new Result();
            using (var context = new DBRContext())
            {

                var obj = (from u in context.Usuario
                           where u.Estado == EstadoActivo.Activo
                           && u.IdUsuario == request.IdUsuario
                           select u).FirstOrDefault();

                obj.Estado = EstadoActivo.Inactivo;
                obj.UsuarioModificacion = user.IdUsuario;
                obj.FechaModificacion = DateTime.Now;

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoEliminar;

                return result;
            }
        }
        public UsuarioResponse GetUsuario(UsuarioRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from u in context.Usuario
                             join ut in context.UsuarioTipo on u.IdUsuarioTipo equals ut.IdUsuarioTipo
                             where u.Estado == EstadoActivo.Activo && u.IdUsuario == request.IdUsuario
                             select new UsuarioResponse
                             {
                                 IdUsuario = u.IdUsuario,
                                 Login = u.Login,
                                 Password = u.Password,
                                 NumeroDocumento = u.NumeroDocumento,
                                 Nombres = u.Nombres,
                                 ApellidoPaterno = u.ApellidoPaterno,
                                 ApellidoMaterno = u.ApellidoMaterno,
                                 IdUsuarioTipo = u.IdUsuarioTipo,
                                 Correo = u.Correo,
                                 UsuarioTipo = ut.Descripcion
                             });
                return query.FirstOrDefault();
            }
        }
        public List<UsuarioResponse> BuscarUsuarioXlogin(UsuarioRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from u in context.Usuario
                             join p in context.Persona on u.NumeroDocumento equals p.NumeroDocumento
                             into ts
                             from t in ts.DefaultIfEmpty()
                             where u.Login == request.Login
                             && u.Estado == EstadoActivo.Activo
                             select new UsuarioResponse
                             {
                                 IdUsuario = u.IdUsuario,
                                 Login = u.Login,
                                 Password = u.Password,
                                 Nombres = u.Nombres,
                                 IdPersona = t.IdPersona,
                                 ApellidoPaterno = u.ApellidoPaterno,
                                 ApellidoMaterno = u.ApellidoMaterno,
                                 IdUsuarioTipo = (int)u.IdUsuarioTipo,
                                 NumeroDocumento = u.NumeroDocumento,
                                 Correo = u.Correo,
                                 UltimoAcceso = (from ua in context.UsuarioHistorico orderby ua.FechaCreacion descending where ua.IdUsuario == u.IdUsuario select ua.FechaCreacion).FirstOrDefault(),
                                 UltimaActividad = (from ua in context.UsuarioActividad orderby ua.FechaCreacion descending where ua.IdUsuario == u.IdUsuario select ua.FechaCreacion).FirstOrDefault()
                             });
                return query.ToList();
            }
        }
        public UsuarioResponse BuscarUsuarioXdni(UsuarioRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from u in context.Usuario
                             join p in context.Persona on u.NumeroDocumento equals p.NumeroDocumento
                             into ts
                             from t in ts.DefaultIfEmpty()
                             where u.NumeroDocumento == request.NumeroDocumento
                             && u.Estado == EstadoActivo.Activo
                             select new UsuarioResponse
                             {
                                 IdUsuario = u.IdUsuario,
                                 Login = u.Login,
                                 Password = u.Password,
                                 Nombres = u.Nombres,
                                 IdPersona = t.IdPersona,
                                 ApellidoPaterno = u.ApellidoPaterno,
                                 ApellidoMaterno = u.ApellidoMaterno,
                                 IdUsuarioTipo = (int)u.IdUsuarioTipo,
                                 NumeroDocumento = u.NumeroDocumento,
                                 Correo = u.Correo                                
                             }).FirstOrDefault();
                return query;
            }
        }
        public List<UsuarioResponse> BuscarUsuarioParticipanteXlogin(UsuarioRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from u in context.Usuario
                             join p in context.Persona on u.NumeroDocumento equals p.NumeroDocumento
                             where u.Login == request.Login
                             && u.Estado == EstadoActivo.Activo
                             && p.Estado == EstadoActivo.Activo
                             && u.IdUsuarioTipo == 4//Solo usuarios peticipantes
                             select new UsuarioResponse
                             {
                                 IdUsuario = u.IdUsuario,
                                 Login = u.Login,
                                 Password = u.Password,
                                 Nombres = p.Nombres,
                                 IdPersona = p.IdPersona,
                                 ApellidoPaterno = p.ApellidoPaterno,
                                 ApellidoMaterno = p.ApellidoMaterno,
                                 IdUsuarioTipo = (int)u.IdUsuarioTipo,
                                 NumeroDocumento = u.NumeroDocumento,
                                 Correo = u.Correo,
                                 UltimoAcceso = (from ua in context.UsuarioHistorico orderby ua.FechaCreacion descending where ua.IdUsuario == u.IdUsuario select ua.FechaCreacion).FirstOrDefault(),
                                 UltimaActividad = (from ua in context.UsuarioActividad orderby ua.FechaCreacion descending where ua.IdUsuario == u.IdUsuario select ua.FechaCreacion).FirstOrDefault()
                             });
                return query.ToList();
            }
        }
        public List<OpcionResponse> ListOpcionesByRol(OpcionRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from u in context.Opcion
                             join or in context.OpcionUsuarioTipo on u.IdOpcion equals or.IdOpcion
                             where or.IdUsuarioTipo == request.IdUsuarioTipo
                             && u.Estado == EstadoRegistro.Activo
                             orderby u.Orden ascending
                             select new OpcionResponse
                             {
                                 Id = u.IdOpcion,
                                 IdUsuarioTipo = or.IdUsuarioTipo,
                                 IdPadre = u.IdPadre,
                                 Icono = u.Icono,
                                 Descripcion = u.Descipcion,
                                 UrlDescripcion = u.UrlDescripcion,
                                 Orden = (int)u.Orden
                             });
                return query.ToList();
            }
        }
        public Result UpdatePasswordUsuario(UsuarioRequest request,UsuarioLogin user)
        {
            Result result = new Result();
            using (var context=new DBRContext())
            {
                var obj = (from u in context.Usuario
                           where u.Estado==EstadoRegistro.Activo
                           && u.IdUsuario == user.IdUsuario
                           select u).FirstOrDefault();

                obj.Password = request.NewPassword;
                obj.FechaModificacion = DateTime.Now;
                obj.UsuarioModificacion = user.IdUsuario;

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoActualizar;

                return result;
            }
        }
        //Virtual
        public List<ComboResponse> ListUsuarioSinAsignar(EventoUsuarioRequest request)
        {
            using (var context = new DBRContext())
            {
                var IdUsuarioTipo = Convert.ToInt32(Perfil.Participante);
                var query = (from u in context.Usuario
                             where u.Estado == EstadoActivo.Activo
                             && u.IdUsuarioTipo == IdUsuarioTipo
                             select new ComboResponse
                             {
                                 Value = u.IdUsuario.ToString(),
                                 Descripcion = u.Nombres + " " + u.ApellidoPaterno + " " + u.ApellidoMaterno
                             });
                return query.ToList();
            }
        }
        //UsuarioHistorico
        public Result SaveUsuarioHistorico(UsuarioRequest request)
        {
            try
            {
                var result = new Result();
                using (var context = new DBRContext())
                {

                    var obj = new UsuarioHistorico();
                    obj.IdUsuario = request.IdUsuario;
                    obj.FechaCreacion = DateTime.Now;

                    context.UsuarioHistorico.Add(obj);

                    result.IsSuccess = context.SaveChanges() > 0;
                    result.Message = Message.ExitoGuardar;

                    return result;
                }
            }
            catch (Exception)
            {
                var result = new Result();
                return result;
            }
            
        }
        public Result SaveUsuarioAcceso(UsuarioLogin user)
        {
            try
            {
                var result = new Result();
                using (var context = new DBRContext())
                {

                    var obj = new UsuarioActividad();
                    obj.IdUsuario = user.IdUsuario;
                    obj.FechaCreacion = DateTime.Now;
                    obj.Token = user.Token;

                    context.UsuarioActividad.Add(obj);

                    result.IsSuccess = context.SaveChanges() > 0;
                    result.Message = Message.ExitoGuardar;

                    return result;
                }
            }
            catch (Exception)
            {
                var result = new Result();
                return result;
            }

        }
        public UsuarioActividadResponse GetUsuarioAcceso(UsuarioLogin user)
        {
            using (var context = new DBRContext())
            {

                var obj = (from ua in context.UsuarioActividad
                           orderby ua.FechaCreacion descending
                           where ua.IdUsuario == user.IdUsuario
                           select new UsuarioActividadResponse
                           {
                               IdUsuarioActividad = ua.IdUsuarioActividad,
                               IdUsuario = ua.IdUsuario,
                               FechaCreacion = ua.FechaCreacion,
                               Token = ua.Token
                           }).FirstOrDefault();
                return obj;
            }

        }
        public Result DeleteUsuarioAcceso(UsuarioLogin user)
        {
            try
            {
                var result = new Result();
                using (var context = new DBRContext())
                {

                    var obj1 = (from ua in context.UsuarioActividad
                                where ua.IdUsuario == user.IdUsuario
                                && ua.Token == user.Token
                                select ua).FirstOrDefault();

                    context.UsuarioActividad.Remove(obj1);
                    result.IsSuccess = context.SaveChanges() > 0;
                    result.Message = Message.ExitoGuardar;

                    return result;
                }
            }
            catch (Exception)
            {
                var result = new Result();
                return result;
            }

        }
    }
}
