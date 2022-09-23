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
    public class DocumentoDatos : BaseAcceso
    {
        public DocumentoDatos(DbContext context) : base(context)
        {

        }
        public List<DocumentoResponse> ListDocumentoByIdDocumento(DocumentoRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from e in context.Documento
                             where e.Estado == EstadoRegistro.Activo
                             && e.IdDocumentoPadre == request.IdDocumentoPadre
                             orderby e.Tipo ascending, e.NombreOriginal ascending
                             select new DocumentoResponse
                             {
                                 IdDocumento = e.IdDocumento,
                                 IdDocumentoPadre = e.IdDocumentoPadre,
                                 IdEmpresa = e.IdEmpresa,
                                 Tipo = e.Tipo,
                                 Nombre = e.Nombre,
                                 NombreOriginal = e.NombreOriginal,
                                 Extension = e.Extension.ToUpper(),
                                 Tamaño = e.Tamaño,
                                 EstadoDocumento = e.EstadoDocumento,
                                 FechaCreacion = e.FechaCreacion,
                                 FechaModificacion = e.FechaModificacion,
                                 FechaDescarga = e.FechaDescarga
                             }).ToList();

                for (int i = 0; i < query.Count; i++)
                {
                    if (query[i].Tipo == 1) // Tipo carpeta
                    {
                        query[i].ListDocumentos = ListDocumentoByIdDocumento(new DocumentoRequest() { IdDocumentoPadre = query[i].IdDocumento });
                    }
                }
                return query;
            }
        }
        public List<DocumentoResponse> ListDocumentoByIdDocumentoEliminados(DocumentoRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from e in context.Documento
                             where e.Estado == EstadoRegistro.Inactivo
                             && e.IdDocumentoPadre == request.IdDocumentoPadre
                             orderby e.Tipo ascending, e.NombreOriginal ascending
                             select new DocumentoResponse
                             {
                                 IdDocumento = e.IdDocumento,
                                 IdDocumentoPadre = e.IdDocumentoPadre,
                                 IdEmpresa = e.IdEmpresa,
                                 Tipo = e.Tipo,
                                 Nombre = e.Nombre,
                                 NombreOriginal = e.NombreOriginal,
                                 Extension = e.Extension.ToUpper(),
                                 Tamaño = e.Tamaño,
                                 EstadoDocumento = e.EstadoDocumento,
                                 FechaCreacion = e.FechaCreacion,
                                 FechaModificacion = e.FechaModificacion,
                                 FechaDescarga = e.FechaDescarga
                             }).ToList();

                for (int i = 0; i < query.Count; i++)
                {
                    if (query[i].Tipo == 1) // Tipo carpeta
                    {
                        query[i].ListDocumentos = ListDocumentoByIdDocumentoEliminados(new DocumentoRequest() { IdDocumentoPadre = query[i].IdDocumento });
                    }
                }
                return query;
            }
        }
        public Paged<DocumentoResponse> ListDocumentoPaginado(DocumentoRequest request)
        {
            Paged<DocumentoResponse> response = new Paged<DocumentoResponse>();
            using (var context = new DBRContext())
            {
                var query = (from e in context.Documento
                             where e.Estado == !request.Estado
                             && e.IdDocumentoPadre == request.IdDocumentoPadre
                             && e.IdEmpresa == request.IdEmpresa
                             orderby e.Tipo ascending, e.NombreOriginal ascending
                             select new DocumentoResponse
                             {
                                 IdDocumento = e.IdDocumento,
                                 IdDocumentoPadre = e.IdDocumentoPadre,
                                 IdEmpresa = e.IdEmpresa,
                                 Tipo = e.Tipo,
                                 Nombre = e.Nombre,
                                 NombreOriginal = e.NombreOriginal,
                                 Extension = e.Extension.ToUpper(),
                                 Tamaño = e.Tamaño,
                                 EstadoDocumento = e.EstadoDocumento,
                                 Estado = e.Estado,
                                 FechaCreacion = e.FechaCreacion,
                                 FechaModificacion = e.FechaModificacion,
                                 FechaDescarga = e.FechaDescarga
                             }).ToList();

                if (request.IdDocumentoPadre > 0)
                {
                    var padre= (from e in context.Documento
                                join d1 in context.Documento on e.IdDocumentoPadre equals d1.IdDocumento into left1
                                from lefts1 in left1.DefaultIfEmpty()
                                join d2 in context.Documento on lefts1.IdDocumentoPadre equals d2.IdDocumento into left2
                                from lefts2 in left2.DefaultIfEmpty()
                                join d3 in context.Documento on lefts2.IdDocumentoPadre equals d3.IdDocumento into left3
                                from lefts3 in left3.DefaultIfEmpty()
                                join d4 in context.Documento on lefts3.IdDocumentoPadre equals d4.IdDocumento into left4
                                from lefts4 in left4.DefaultIfEmpty()
                                join d5 in context.Documento on lefts4.IdDocumentoPadre equals d5.IdDocumento into left5
                                from lefts5 in left5.DefaultIfEmpty()
                                join d6 in context.Documento on lefts5.IdDocumentoPadre equals d6.IdDocumento into left6
                                from lefts6 in left6.DefaultIfEmpty()
                                join d7 in context.Documento on lefts6.IdDocumentoPadre equals d7.IdDocumento into left7
                                from lefts7 in left7.DefaultIfEmpty()
                                where e.IdDocumento == request.IdDocumentoPadre
                                select new DocumentoResponse
                                {
                                    IdDocumento = (int)e.IdDocumentoPadre,
                                    IdDocumentoPadre = e.IdDocumentoPadre,
                                    IdEmpresa = e.IdEmpresa,
                                    Tipo = 0,
                                    Nombre = e.NombreOriginal,
                                    NombreOriginal =     
                                    (lefts7.NombreOriginal == null ? "" : ".." + @"\") +
                                    (lefts6.NombreOriginal == null ? "" : "<a title='" + lefts6.NombreOriginal + "' onclick='Funciones.VerCarpeta(" + lefts6.IdDocumento + ")' class='folder-personalizado'>" + lefts6.NombreOriginal + "</a>" + @"\") +
                                    (lefts5.NombreOriginal == null ? "" : "<a title='" + lefts5.NombreOriginal + "' onclick='Funciones.VerCarpeta(" + lefts5.IdDocumento + ")' class='folder-personalizado'>" + lefts5.NombreOriginal + "</a>" + @"\") +
                                    (lefts4.NombreOriginal == null ? "" : "<a title='" + lefts4.NombreOriginal + "' onclick='Funciones.VerCarpeta(" + lefts4.IdDocumento + ")' class='folder-personalizado'>" + lefts4.NombreOriginal + "</a>" + @"\") +
                                    (lefts3.NombreOriginal == null ? "" : "<a title='" + lefts3.NombreOriginal + "' onclick='Funciones.VerCarpeta(" + lefts3.IdDocumento + ")' class='folder-personalizado'>" + lefts3.NombreOriginal + "</a>" + @"\") +
                                    (lefts2.NombreOriginal == null ? "" : "<a title='" + lefts2.NombreOriginal + "' onclick='Funciones.VerCarpeta(" + lefts2.IdDocumento + ")' class='folder-personalizado'>" + lefts2.NombreOriginal + "</a>" + @"\") +
                                    (lefts1.NombreOriginal == null ? "" : "<a title='" + lefts1.NombreOriginal + "' onclick='Funciones.VerCarpeta(" + lefts1.IdDocumento + ")' class='folder-personalizado'>" + lefts1.NombreOriginal + "</a>" + @"\") +
                                    e.NombreOriginal
                                }).ToList();

                    padre.AddRange(query);

                    response.recordsFiltered = padre.Count();
                    response.recordsTotal = padre.Count();
                    response.data = padre;
                    return response;
                }
                else
                {
                    response.recordsFiltered = query.Count();
                    response.recordsTotal = query.Count();
                    response.data = query;
                    return response;
                }

                
            }
        }
        public Result SaveDocumento(DocumentoRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var Documento = (from d in context.Documento
                                 where d.Estado == EstadoActivo.Activo
                                 && d.IdDocumentoPadre == request.IdDocumentoPadre
                                 && d.IdEmpresa == request.IdEmpresa
                                 && d.NombreOriginal == request.NombreOriginal
                                 select new { d.IdDocumento}).FirstOrDefault();

                if (Documento != null)
                {
                    result.IsSuccess = false;
                    result.Message = request.Tipo == 1 ? Message.ExisteCarpeta : Message.ExisteDocumento;
                    return result;
                }

                var obj = new Documento();
                obj.IdDocumentoPadre = request.IdDocumentoPadre;
                obj.IdEmpresa = request.IdEmpresa;
                obj.Tipo = request.Tipo;
                obj.Nombre = request.Nombre;
                obj.NombreOriginal = request.NombreOriginal;
                obj.Extension = request.Extension;
                obj.Tamaño = request.Tamaño;
                obj.EstadoDocumento = 1;
                obj.Estado = EstadoRegistro.Activo;
                obj.UsuarioCreacion = user.IdUsuario;
                obj.FechaCreacion = DateTime.Now;
                obj.UsuarioModificacion = user.IdUsuario;
                obj.FechaModificacion = DateTime.Now;
                obj.rowid = Guid.NewGuid();

                context.Documento.Add(obj);

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoGuardar;
                return result;
            }
        }
        public Result UpdateDocumento(DocumentoRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var Documento = (from d in context.Documento
                                 where d.Estado == EstadoActivo.Activo
                                 && d.IdDocumentoPadre == request.IdDocumentoPadre
                                 && d.IdEmpresa == request.IdEmpresa
                                 && d.NombreOriginal == request.NombreOriginal
                                 && d.IdDocumento != request.IdDocumento
                                 select new { d.IdDocumento }).FirstOrDefault();

                if (Documento != null)
                {
                    result.IsSuccess = false;
                    result.Message = request.Tipo == 1 ? Message.ExisteCarpeta : Message.ExisteDocumento;
                    return result;
                }

                var obj = context.Documento.Find(request.IdDocumento);

                var OldDocumento = obj.Nombre;

                obj.IdDocumentoPadre = request.IdDocumentoPadre;
                obj.IdEmpresa = request.IdEmpresa;
                obj.Tipo = request.Tipo;
                obj.Nombre = request.Nombre;
                obj.NombreOriginal = request.NombreOriginal;
                obj.Extension = request.Extension;
                obj.Tamaño = request.Tamaño;
                obj.EstadoDocumento = request.EstadoDocumento;
                obj.UsuarioModificacion = user.IdUsuario;
                obj.FechaModificacion = DateTime.Now;

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoActualizar;
                result.Informacion = OldDocumento;
                return result;
            }
        }
        public Result DeleteDocumento(DocumentoRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = context.Documento.Find(request.IdDocumento);
                
                if (obj.Tipo == 1)
                {
                    request.IdDocumentoPadre = request.IdDocumento;
                    var ListDocumentos = ListDocumentoByIdDocumento(request);
                    DeleteDocumentoRecursivo(ListDocumentos, user);
                }

                obj.Estado = EstadoRegistro.Inactivo;
                obj.UsuarioModificacion = user.IdUsuario;
                obj.FechaModificacion = DateTime.Now;

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoEliminar;
                return result;
            }
        }
        private void DeleteDocumentoRecursivo(List<DocumentoResponse> ListDocumentos, UsuarioLogin user)
        {
            using (var context = new DBRContext())
            {
                foreach (var item in ListDocumentos)
                {
                    var obj1 = context.Documento.Find(item.IdDocumento);
                    obj1.Estado = EstadoRegistro.Inactivo;
                    obj1.UsuarioModificacion = user.IdUsuario;
                    obj1.FechaModificacion = DateTime.Now;

                    context.SaveChanges();

                    if (item.ListDocumentos != null)
                    {
                        DeleteDocumentoRecursivo(item.ListDocumentos, user);
                    }
                }
            }    
        }
        public DocumentoResponse GetDocumento(DocumentoRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from e in context.Documento
                             where e.Estado == EstadoRegistro.Activo
                             && e.IdDocumento == request.IdDocumento
                             select new DocumentoResponse
                             {
                                 IdDocumento = e.IdDocumento,
                                 IdDocumentoPadre = e.IdDocumentoPadre,
                                 IdEmpresa = e.IdEmpresa,
                                 Tipo = e.Tipo,
                                 Nombre = e.Nombre,
                                 NombreOriginal = e.NombreOriginal,
                                 Tamaño = e.Tamaño,
                                 EstadoDocumento = e.EstadoDocumento,
                                 FechaCreacion = e.FechaCreacion,
                                 FechaModificacion = e.FechaModificacion
                             }).FirstOrDefault();
                return query;
            }
        }
        public Result DeleteFisicoDocumento(DocumentoRequest request, UsuarioLogin user, string pathDocumentos)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = context.Documento.Find(request.IdDocumento);           

                if (obj.Tipo == 1)
                {
                    request.IdDocumentoPadre = request.IdDocumento;
                    var ListDocumento = ListDocumentoByIdDocumentoEliminados(request);
                    DeleteFisicoDocumentoRecursivo(ListDocumento, user, pathDocumentos);
                }
                if (obj.Tipo == 2)
                {
                    if (System.IO.File.Exists(pathDocumentos + obj.Nombre))
                    {
                        System.IO.File.Delete(pathDocumentos + obj.Nombre);
                    }
                }

                var objh = new DocumentoHistorico();
                objh.IdDocumento = obj.IdDocumento;
                objh.IdDocumentoPadre = obj.IdDocumentoPadre;
                objh.IdEmpresa = obj.IdEmpresa;
                objh.Tipo = obj.Tipo;
                objh.Nombre = obj.Nombre;
                objh.NombreOriginal = obj.NombreOriginal;
                objh.Extension = obj.Extension;
                objh.Tamaño = obj.Tamaño;
                objh.EstadoDocumento = obj.EstadoDocumento;
                objh.Estado = obj.Estado;
                objh.UsuarioCreacion = obj.UsuarioCreacion;
                objh.FechaCreacion = obj.FechaCreacion;
                objh.UsuarioModificacion = user.IdUsuario;
                objh.FechaModificacion = DateTime.Now;
                objh.rowid = obj.rowid;

                context.DocumentoHistorico.Add(objh);

                context.Documento.Remove(obj);

                result.IsSuccess = context.SaveChanges() >= 0;
                result.Message = Message.ExitoEliminar;
                return result;
            }
        }
        private void DeleteFisicoDocumentoRecursivo(List<DocumentoResponse> ListDocumento, UsuarioLogin user, string pathDocumentos)
        {
            using (var context = new DBRContext())
            {
                foreach (var item in ListDocumento)
                {
                    var obj = context.Documento.Find(item.IdDocumento);

                    if (obj.Tipo == 2)
                    {
                        if (System.IO.File.Exists(pathDocumentos + item.Nombre))
                        {
                            System.IO.File.Delete(pathDocumentos + item.Nombre);
                        }
                    }

                    if (item.ListDocumentos != null)
                    {
                        DeleteFisicoDocumentoRecursivo(item.ListDocumentos, user, pathDocumentos);
                    }

                    var objh = new DocumentoHistorico();
                    objh.IdDocumento = obj.IdDocumento;
                    objh.IdDocumentoPadre = obj.IdDocumentoPadre;
                    objh.IdEmpresa = obj.IdEmpresa;
                    objh.Tipo = obj.Tipo;
                    objh.Nombre = obj.Nombre;
                    objh.NombreOriginal = obj.NombreOriginal;
                    objh.Extension = obj.Extension;
                    objh.Tamaño = obj.Tamaño;
                    objh.EstadoDocumento = obj.EstadoDocumento;
                    objh.Estado = obj.Estado;
                    objh.UsuarioCreacion = obj.UsuarioCreacion;
                    objh.FechaCreacion = obj.FechaCreacion;
                    objh.UsuarioModificacion = user.IdUsuario;
                    objh.FechaModificacion = DateTime.Now;
                    objh.rowid = obj.rowid;

                    context.DocumentoHistorico.Add(objh);

                    context.Documento.Remove(obj);
                    context.SaveChanges();    
                }
            }  
        }
        public Result RestoreDocumento(DocumentoRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = context.Documento.Find(request.IdDocumento);

                if (obj.IdDocumentoPadre > 0)
                {
                    request.IdDocumento = (int)obj.IdDocumentoPadre;
                    RestoreDocumentoRecursivo(request, user);
                }

                obj.Estado = EstadoRegistro.Activo;
                obj.UsuarioModificacion = user.IdUsuario;
                obj.FechaModificacion = DateTime.Now;

                result.IsSuccess = context.SaveChanges() >= 0;
                result.Message = Message.ExitoActualizar;
                return result;
            }
        }
        private void RestoreDocumentoRecursivo(DocumentoRequest request, UsuarioLogin user)
        {
            using (var context = new DBRContext())
            {
                var obj = context.Documento.Find(request.IdDocumento);

                if (obj.Estado == EstadoRegistro.Inactivo)
                {
                    if (obj.IdDocumentoPadre > 0)
                    {
                        request.IdDocumento = (int)obj.IdDocumentoPadre;
                        RestoreDocumentoRecursivo(request, user);
                    }

                    obj.Estado = EstadoRegistro.Activo;
                    obj.UsuarioModificacion = user.IdUsuario;
                    obj.FechaModificacion = DateTime.Now;

                    context.SaveChanges();
                }
            }
        }
        public long GetSizeFolder()
        {
            using (var context = new DBRContext())
            {
                var query = (from e in context.Documento
                              where e.Tamaño != null
                              select e.Tamaño).ToList();
                long Tamaño = 0;

                for (int i = 0; i < query.Count; i++)
                {
                    Tamaño += query[i] ?? 0;
                }
                return Tamaño;
            }
        }
        //VISTA DE USUARIO
        public Paged<DocumentoResponse> ListDocumentoUsuarioPaginado(DocumentoRequest request, UsuarioLogin user)
        {
            Paged<DocumentoResponse> response = new Paged<DocumentoResponse>();
            using (var context = new DBRContext())
            {
                var query = (from e in context.Documento
                             join em in context.Empresa on e.IdEmpresa equals em.IdEmpresa
                             where e.Estado == EstadoRegistro.Activo
                             && em.Estado == EstadoRegistro.Activo
                             && e.IdDocumentoPadre == request.IdDocumentoPadre
                             && em.IdUsuario == user.IdUsuario
                             orderby e.Tipo ascending, e.NombreOriginal ascending
                             select new DocumentoResponse
                             {
                                 IdDocumento = e.IdDocumento,
                                 IdDocumentoPadre = e.IdDocumentoPadre,
                                 IdEmpresa = e.IdEmpresa,
                                 Tipo = e.Tipo,
                                 Nombre = e.Nombre,
                                 NombreOriginal = e.NombreOriginal,
                                 Extension = e.Extension.ToUpper(),
                                 Tamaño = e.Tamaño,
                                 EstadoDocumento = e.EstadoDocumento,
                                 FechaCreacion = e.FechaCreacion,
                                 FechaModificacion = e.FechaModificacion
                             }).ToList();

                if (request.IdDocumentoPadre > 0)
                {
                    var padre = (from e in context.Documento
                                 join d1 in context.Documento on e.IdDocumentoPadre equals d1.IdDocumento into left1
                                 from lefts1 in left1.DefaultIfEmpty()
                                 join d2 in context.Documento on lefts1.IdDocumentoPadre equals d2.IdDocumento into left2
                                 from lefts2 in left2.DefaultIfEmpty()
                                 join d3 in context.Documento on lefts2.IdDocumentoPadre equals d3.IdDocumento into left3
                                 from lefts3 in left3.DefaultIfEmpty()
                                 join d4 in context.Documento on lefts3.IdDocumentoPadre equals d4.IdDocumento into left4
                                 from lefts4 in left4.DefaultIfEmpty()
                                 join d5 in context.Documento on lefts4.IdDocumentoPadre equals d5.IdDocumento into left5
                                 from lefts5 in left5.DefaultIfEmpty()
                                 join d6 in context.Documento on lefts5.IdDocumentoPadre equals d6.IdDocumento into left6
                                 from lefts6 in left6.DefaultIfEmpty()
                                 join d7 in context.Documento on lefts6.IdDocumentoPadre equals d7.IdDocumento into left7
                                 from lefts7 in left7.DefaultIfEmpty()
                                 where e.IdDocumento == request.IdDocumentoPadre
                                 select new DocumentoResponse
                                 {
                                     IdDocumento = (int)e.IdDocumentoPadre,
                                     IdDocumentoPadre = e.IdDocumentoPadre,
                                     IdEmpresa = e.IdEmpresa,
                                     Tipo = 0,
                                     Nombre = e.NombreOriginal,
                                     NombreOriginal =
                                     (lefts7.NombreOriginal == null ? "" : ".." + @"\") +
                                     (lefts6.NombreOriginal == null ? "" : "<a title='" + lefts6.NombreOriginal + "' onclick='Funciones.VerCarpeta(" + lefts6.IdDocumento + ")' class='folder-personalizado'>" + lefts6.NombreOriginal + "</a>" + @"\") +
                                     (lefts5.NombreOriginal == null ? "" : "<a title='" + lefts5.NombreOriginal + "' onclick='Funciones.VerCarpeta(" + lefts5.IdDocumento + ")' class='folder-personalizado'>" + lefts5.NombreOriginal + "</a>" + @"\") +
                                     (lefts4.NombreOriginal == null ? "" : "<a title='" + lefts4.NombreOriginal + "' onclick='Funciones.VerCarpeta(" + lefts4.IdDocumento + ")' class='folder-personalizado'>" + lefts4.NombreOriginal + "</a>" + @"\") +
                                     (lefts3.NombreOriginal == null ? "" : "<a title='" + lefts3.NombreOriginal + "' onclick='Funciones.VerCarpeta(" + lefts3.IdDocumento + ")' class='folder-personalizado'>" + lefts3.NombreOriginal + "</a>" + @"\") +
                                     (lefts2.NombreOriginal == null ? "" : "<a title='" + lefts2.NombreOriginal + "' onclick='Funciones.VerCarpeta(" + lefts2.IdDocumento + ")' class='folder-personalizado'>" + lefts2.NombreOriginal + "</a>" + @"\") +
                                     (lefts1.NombreOriginal == null ? "" : "<a title='" + lefts1.NombreOriginal + "' onclick='Funciones.VerCarpeta(" + lefts1.IdDocumento + ")' class='folder-personalizado'>" + lefts1.NombreOriginal + "</a>" + @"\") +
                                     e.NombreOriginal
                                 }).ToList();

                    padre.AddRange(query);

                    response.recordsFiltered = padre.Count();
                    response.recordsTotal = padre.Count();
                    response.data = padre;
                    return response;
                }
                else
                {
                    response.recordsFiltered = query.Count();
                    response.recordsTotal = query.Count();
                    response.data = query;
                    return response;
                }


            }
        }
        public DocumentoResponse GetDocumentoUsuario(DocumentoRequest request, UsuarioLogin user)
        {
            using (var context = new DBRContext())
            {
                var query = (from e in context.Documento
                             join em in context.Empresa on e.IdEmpresa equals em.IdEmpresa
                             where e.Estado == EstadoRegistro.Activo
                             && e.IdDocumento == request.IdDocumento
                             && em.IdUsuario == user.IdUsuario
                             select new DocumentoResponse
                             {
                                 IdDocumento = e.IdDocumento,
                                 IdDocumentoPadre = e.IdDocumentoPadre,
                                 IdEmpresa = e.IdEmpresa,
                                 Tipo = e.Tipo,
                                 Nombre = e.Nombre,
                                 NombreOriginal = e.NombreOriginal,
                                 Tamaño = e.Tamaño,
                                 EstadoDocumento = e.EstadoDocumento,
                                 FechaCreacion = e.FechaCreacion,
                                 FechaModificacion = e.FechaModificacion
                             }).FirstOrDefault();
                return query;
            }
        }
        public Result UpdateEstadoDocumento(DocumentoRequest request)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = context.Documento.Find(request.IdDocumento);
                if (obj.EstadoDocumento == 1)
                {
                    obj.FechaDescarga = DateTime.Now;
                }
                obj.EstadoDocumento = 2;

                result.IsSuccess = context.SaveChanges() >= 0;
                result.Message = Message.ExitoActualizar;
                return result;
            }
        }
    }
}
