namespace DBR.Eventos.Datos.Implementacion
{
    using Evento.Modelo;
    using Evento.Modelo.Request;
    using Evento.Modelo.Response;
    using Comun;
    using Base;
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Collections.Generic;

    public class SucesoDatos : BaseAcceso
    {
        public SucesoDatos(DbContext context) : base(context)
        {
        }
        public Paged<SucesoResponse> ListSucesoPaginado(PageRequest page)
        {
            using (var context = new DBRContext())
            {
                var search = page.search.value ?? "";
                var query = (from e in context.Suceso
                             where e.Estado == EstadoRegistro.Activo
                             && e.NombreSuceso.StartsWith(search)
                             select new SucesoResponse
                             {
                                 IdSuceso = e.IdSuceso,
                                 NombreSuceso = e.NombreSuceso,
                                 Fecha = (DateTime)e.Fecha,
                                 Horas = (int)e.Horas,
                                 ImagenSuceso = e.ImagenSuceso,
                                 Activo = (bool)e.Activo
                             });

                return Paginate(page, query, q => q.Fecha);
            }
        }
        public Result SaveSuceso(SucesoRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = new Suceso();
                obj.NombreSuceso = request.NombreSuceso;
                obj.Descripcion = request.Descripcion;
                obj.Fecha = request.Fecha;
                obj.Horas = request.Horas;
                obj.ImagenSuceso = request.ImagenSuceso;                
                obj.Activo = false;
                obj.Estado = EstadoRegistro.Activo;
                obj.UsuarioCreacion = user.IdUsuario;
                obj.FechaCreacion = DateTime.Now;
                obj.rowid = Guid.NewGuid();

                context.Suceso.Add(obj);

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoGuardar;
           
                return result;
            }
        }
        public Result UpdateSuceso(SucesoRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = context.Suceso.Find(request.IdSuceso);
                obj.NombreSuceso = request.NombreSuceso;
                obj.Descripcion = request.Descripcion;
                obj.Fecha = request.Fecha;
                obj.Horas = request.Horas;
                if (request.ImagenSuceso != null && request.ImagenSuceso != "")
                    obj.ImagenSuceso = request.ImagenSuceso;

                obj.UsuarioModificacion = user.IdUsuario;
                obj.FechaModificacion = DateTime.Now;

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoActualizar;

                return result;
            }
        }
        public Result DeleteSuceso(SucesoRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = context.Suceso.Find(request.IdSuceso);
                obj.Estado = EstadoRegistro.Inactivo;
                obj.UsuarioModificacion = user.IdUsuario;
                obj.FechaModificacion = DateTime.Now;

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoEliminar;

                return result;
            }
        }
        public Result UpdateEstadoSuceso(SucesoRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = context.Suceso.Find(request.IdSuceso);

                obj.Activo = request.Activo;
                obj.UsuarioModificacion = user.IdUsuario;
                obj.FechaModificacion = DateTime.Now;

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoActualizar;

                return result;
            }
        }
        public SucesoResponse GetSuceso(SucesoRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from e in context.Suceso
                             where e.Estado == EstadoRegistro.Activo
                             && e.IdSuceso == request.IdSuceso
                             select new SucesoResponse
                             {
                                 IdSuceso = e.IdSuceso,
                                 NombreSuceso = e.NombreSuceso,
                                 Descripcion = e.Descripcion,
                                 Fecha = (DateTime)e.Fecha,
                                 Horas = (int)e.Horas,
                                 ImagenSuceso = e.ImagenSuceso,
                                 Activo = (bool)e.Activo
                             }).FirstOrDefault();

                return query;
            }
        }

        #region METODOS 100% INGENIEROS
        public List<SucesoResponse> ListUltimosSucesos()
        {
            using (var context = new DBRContext())
            {
                var query = (from e in context.Suceso
                             where e.Estado == EstadoRegistro.Activo
                             && e.Activo == true
                             orderby e.Fecha descending
                             select new SucesoResponse
                             {
                                 rowid = e.rowid.ToString(),
                                 IdSuceso = e.IdSuceso,
                                 NombreSuceso = e.NombreSuceso,
                                 ImagenSuceso=e.ImagenSuceso,
                                 Descripcion=e.Descripcion,
                                 Fecha = (DateTime)e.Fecha,
                                 Horas = (int)e.Horas,
                                 Activo = (bool)e.Activo,
                             }).Take(10).ToList();

                return query;
            }
        }
        public List<SucesoResponse> ListAllSucesos()
        {
            using (var context = new DBRContext())
            {
                var query = (from e in context.Suceso
                             where e.Estado == EstadoRegistro.Activo
                             && e.Activo == true
                             orderby e.Fecha descending
                             select new SucesoResponse
                             {
                                 rowid = e.rowid.ToString(),
                                 IdSuceso = e.IdSuceso,
                                 NombreSuceso = e.NombreSuceso,
                                 ImagenSuceso = e.ImagenSuceso,
                                 Descripcion = e.Descripcion,
                                 Fecha = (DateTime)e.Fecha,
                                 Horas = (int)e.Horas,
                                 Activo = (bool)e.Activo,
                             }).Skip(10).ToList();

                return query;
            }
        }
        #endregion
    }
}
