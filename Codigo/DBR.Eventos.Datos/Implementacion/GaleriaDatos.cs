using System.Data.Entity;
using DBR.Eventos.Datos.Base;
using System.Linq;
using DBR.Evento.Modelo;
using DBR.Evento.Modelo.Request;
using DBR.Eventos.Comun;
using System;
using DBR.Evento.Modelo.Response;
using System.Collections.Generic;

namespace DBR.Eventos.Datos.Implementacion
{
    public class GaleriaDatos : BaseAcceso
    {
        public GaleriaDatos(DbContext context) : base(context)
        {
        }
        public Paged<GaleriaResponse> ListGaleria(PageRequest page)
        {
            using (var context = new DBRContext())
            {
                var query = (from e in context.Galeria
                             where e.Estado == EstadoRegistro.Activo
                             select new GaleriaResponse
                             {
                                 IdGaleria = e.IdGaleria,
                                 Descripcion=e.Descripcion,
                                 Nombre = e.Nombre,
                                 Activo = (bool)e.Activo
                             });

                return Paginate(page, query, q => q.IdGaleria);
            }
        }
        public List<GaleriaResponse> ListGaleriaActivos()
        {
            using (var context = new DBRContext())
            {
                var query = (from e in context.Galeria
                             where e.Estado == EstadoRegistro.Activo
                             select new GaleriaResponse
                             {
                                 IdGaleria = e.IdGaleria,
                                 Descripcion = e.Descripcion,
                                 Nombre = e.Nombre,
                                 Activo = (bool)e.Activo
                             });

                return query.ToList();
            }
        }
        public Result SaveGaleria(GaleriaRequest request,UsuarioLogin user)
        {
            Result result = new Result();
            using (var context=new DBRContext())
            {
                var obj = new Galeria();
                obj.Descripcion = request.Descripcion;
                obj.Nombre = request.Nombre;
                obj.Activo = EstadoActivo.Activo;
                obj.Estado = EstadoRegistro.Activo;
                obj.UsuarioCreacion = user.IdUsuario;
                obj.FechaCreacion = DateTime.Now;

                context.Galeria.Add(obj);

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
                return result;
            }
        }
        public Result DeleteGaleria(GaleriaRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = (from g in context.Galeria
                           where g.IdGaleria == request.IdGaleria
                           && g.Estado == EstadoRegistro.Activo
                           select g).FirstOrDefault();

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
                return result;
            }
        }
        public Result UpdateActivoGaleria(GaleriaRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = (from g in context.Galeria
                           where g.IdGaleria == request.IdGaleria
                           && g.Estado == EstadoRegistro.Activo
                           select g).FirstOrDefault();

                obj.Activo = request.Activo;
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
                return result;
            }
        }

        #region METODOS AVAS CONSULTORES
        public List<GaleriaResponse> ListGaleriaActivosAvas()
        {
            using (var context = new DBRContext())
            {
                var query = (from e in context.Galeria
                             where e.Estado == EstadoRegistro.Activo
                             && e.Activo == true
                             orderby e.IdGaleria descending
                             select new GaleriaResponse
                             {
                                 IdGaleria = e.IdGaleria,
                                 Descripcion = e.Descripcion,
                                 Nombre = e.Nombre,
                                 Activo = (bool)e.Activo
                             });
                return query.ToList();
            }
        }
        #endregion

        #region METODOS 100% INGENIEROS
        public List<GaleriaResponse> ListUltimosGaleriaActivos()
        {
            using (var context = new DBRContext())
            {
                var query = (from e in context.Galeria
                             where e.Estado == EstadoRegistro.Activo
                             && e.Activo == true
                             orderby e.IdGaleria descending
                             select new GaleriaResponse
                             {
                                 IdGaleria = e.IdGaleria,
                                 Descripcion = e.Descripcion,
                                 Nombre = e.Nombre,
                                 Activo = (bool)e.Activo
                             }).Take(20);

                return query.ToList();
            }
        }
        public List<GaleriaResponse> ListAllGaleriaActivos()
        {
            using (var context = new DBRContext())
            {
                var query = (from e in context.Galeria
                             where e.Estado == EstadoRegistro.Activo
                             && e.Activo == true
                             orderby e.IdGaleria descending
                             select new GaleriaResponse
                             {
                                 IdGaleria = e.IdGaleria,
                                 Descripcion = e.Descripcion,
                                 Nombre = e.Nombre,
                                 Activo = (bool)e.Activo
                             }).Skip(20);

                return query.ToList();
            }
        }
        #endregion
    }
}
