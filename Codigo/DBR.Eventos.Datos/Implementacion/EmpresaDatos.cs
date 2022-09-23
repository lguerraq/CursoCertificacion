using DBR.Evento.Modelo;
using DBR.Evento.Modelo.Request;
using DBR.Evento.Modelo.Response;
using DBR.Eventos.Comun;
using DBR.Eventos.Datos.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DBR.Eventos.Datos.Implementacion
{
    public class EmpresaDatos : BaseAcceso
    {
        public EmpresaDatos(DbContext context) : base(context)
        {

        }
        public List<ComboResponse> ListEmpresaCombo()
        {
            using (var context = new DBRContext())
            {
                var query = (from e in context.Empresa
                             join u in context.Usuario on e.IdUsuario equals u.IdUsuario
                             where e.Estado == EstadoRegistro.Activo
                             && u.Estado == EstadoRegistro.Activo
                             select new ComboResponse
                             {
                                 Value = e.IdEmpresa.ToString(),
                                 Descripcion = e.NombreComercial,
                                 Informacion = u.Correo
                             }).ToList();
                return query;
            }
        }
        public Paged<EmpresaResponse> ListEmpresaPaginado(PageRequest page)
        {
            page.search.value = page.search.value ?? "";
            page.Order = "ASC";
            using (var context = new DBRContext())
            {
                var query = (from e in context.Empresa
                             join u in context.Usuario on e.IdUsuario equals u.IdUsuario
                             where e.Estado == EstadoRegistro.Activo
                             && u.Estado == EstadoRegistro.Activo
                             && (
                                 e.Ruc.StartsWith(page.search.value) ||
                                 e.RazonSocial.StartsWith(page.search.value) ||
                                 e.NombreComercial.StartsWith(page.search.value)
                             )
                             select new EmpresaResponse
                             {
                                 IdEmpresa = e.IdEmpresa,
                                 Ruc = e.Ruc,
                                 IdUsuario = e.IdUsuario,
                                 NombreComercial = e.NombreComercial,
                                 RazonSocial = e.RazonSocial,
                                 DireccionFiscal = e.DireccionFiscal,
                                 Frecuencia = e.Frecuencia,
                                 Responsable = u.Nombres + " " + u.ApellidoMaterno + " " + u.ApellidoPaterno
                             });

                return PaginateNew(page, query, q => q.IdEmpresa);
            }
        }
        public Result SaveEmpresa(EmpresaRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {

                var Empresa = (from e in context.Empresa
                               where e.Ruc == request.Ruc
                               && e.Estado == EstadoRegistro.Activo
                               select new { e.IdEmpresa }).FirstOrDefault();

                if (Empresa != null)
                {
                    result.IsSuccess = false;
                    result.Message = Message.ErrorRucRegistrado;

                    return result;
                }

                Empresa = (from e in context.Empresa
                               where e.IdUsuario == request.IdUsuario
                               && e.Estado == EstadoRegistro.Activo
                               select new { e.IdEmpresa }).FirstOrDefault();

                if (Empresa != null)
                {
                    result.IsSuccess = false;
                    result.Message = Message.ErrorUsuarioConEmpresa;

                    return result;
                }

                var obj = new Empresa();
                obj.IdUsuario = request.IdUsuario;
                obj.Ruc = request.Ruc;
                obj.RazonSocial = request.RazonSocial;
                obj.NombreComercial = request.NombreComercial;
                obj.DireccionFiscal = request.DireccionFiscal;
                obj.Logo = request.Logo;
                obj.Frecuencia = request.Frecuencia;
                obj.Estado = EstadoRegistro.Activo;
                obj.UsuarioCreacion = user.IdUsuario;
                obj.FechaCreacion = DateTime.Now;
                obj.UsuarioModificacion = user.IdUsuario;
                obj.FechaModificacion = DateTime.Now;
                obj.rowid = Guid.NewGuid();

                context.Empresa.Add(obj);

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoGuardar;
                return result;
            }
        }
        public Result UpdateEmpresa(EmpresaRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var Empresa = (from e in context.Empresa
                               where e.Ruc == request.Ruc
                               && e.Estado == EstadoRegistro.Activo
                               && e.IdEmpresa != request.IdEmpresa
                               select new { e.IdEmpresa }).FirstOrDefault();

                if (Empresa != null)
                {
                    result.IsSuccess = false;
                    result.Message = Message.ErrorRucRegistrado;

                    return result;
                }

                Empresa = (from e in context.Empresa
                           where e.IdUsuario == request.IdUsuario
                           && e.Estado == EstadoRegistro.Activo
                           && e.IdEmpresa != request.IdEmpresa
                           select new { e.IdEmpresa }).FirstOrDefault();

                if (Empresa != null)
                {
                    result.IsSuccess = false;
                    result.Message = Message.ErrorUsuarioConEmpresa;

                    return result;
                }

                var obj = context.Empresa.Find(request.IdEmpresa);
                obj.IdUsuario = request.IdUsuario;
                obj.Ruc = request.Ruc;
                obj.RazonSocial = request.RazonSocial;
                obj.NombreComercial = request.NombreComercial;
                obj.DireccionFiscal = request.DireccionFiscal;
                obj.Logo = request.Logo;
                obj.Frecuencia = request.Frecuencia;
                obj.UsuarioModificacion = user.IdUsuario;
                obj.FechaModificacion = DateTime.Now;

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoActualizar;
                return result;
            }
        }
        public Result DeleteEmpresa(EmpresaRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = context.Empresa.Find(request.IdEmpresa);
                obj.Estado = EstadoRegistro.Inactivo;
                obj.UsuarioModificacion = user.IdUsuario;
                obj.FechaModificacion = DateTime.Now;

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoEliminar;
                return result;
            }
        }
        public EmpresaResponse GetEmpresa(EmpresaRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from e in context.Empresa
                             join u in context.Usuario on e.IdUsuario equals u.IdUsuario
                             where e.Estado == EstadoRegistro.Activo
                             && e.IdEmpresa == request.IdEmpresa
                             select new EmpresaResponse
                             {
                                 IdEmpresa = e.IdEmpresa,
                                 Ruc = e.Ruc,
                                 IdUsuario = e.IdUsuario,
                                 NombreComercial = e.NombreComercial,
                                 RazonSocial = e.RazonSocial,
                                 DireccionFiscal = e.DireccionFiscal,
                                 Frecuencia = e.Frecuencia,
                                 //Usuario
                                 Usuario = u.Login,
                                 NumeroDocumento = u.NumeroDocumento,
                                 Nombres = u.Nombres,
                                 ApellidoPaterno = u.ApellidoPaterno,
                                 ApellidoMaterno = u.ApellidoMaterno,
                                 Correo = u.Correo
                             }).FirstOrDefault();
                return query;
            }
        }
        public EmpresaResponse GetEmpresaByUsuario(UsuarioLogin user)
        {
            using (var context = new DBRContext())
            {
                var query = (from e in context.Empresa
                             join u in context.Usuario on e.IdUsuario equals u.IdUsuario
                             where e.Estado == EstadoRegistro.Activo
                             && e.IdUsuario == user.IdUsuario
                             select new EmpresaResponse
                             {
                                 IdEmpresa = e.IdEmpresa,
                                 Ruc = e.Ruc,
                                 IdUsuario = e.IdUsuario,
                                 NombreComercial = e.NombreComercial,
                                 RazonSocial = e.RazonSocial,
                                 DireccionFiscal = e.DireccionFiscal,
                                 Frecuencia = e.Frecuencia,
                                 //Usuario
                                 Usuario = u.Login,
                                 NumeroDocumento = u.NumeroDocumento,
                                 Nombres = u.Nombres,
                                 ApellidoPaterno = u.ApellidoPaterno,
                                 ApellidoMaterno = u.ApellidoMaterno
                             }).FirstOrDefault();
                return query;
            }
        }
    }
}
