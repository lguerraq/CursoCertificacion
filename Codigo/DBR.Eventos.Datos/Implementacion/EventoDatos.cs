namespace DBR.Eventos.Datos.Implementacion
{
    using System.Data.Entity;
    using Base;
    using System.Collections.Generic;
    using Evento.Modelo.Response;
    using System.Linq;
    using Comun;
    using System;
    using Evento.Modelo;
    using Evento.Modelo.Request;
    using System.Data.SqlClient;
    using System.Data;
    using System.Configuration;

    public class EventoDatos : BaseAcceso
    {
        public EventoDatos(DbContext context) : base(context)
        {
        }
        public Paged<EventoResponse> ListEvento(PageRequest page)
        {
            using (var context=new DBRContext())
            {
                var search = page.search.value ?? "";
                var query = (from e in context.Evento
                             where e.Estado == EstadoRegistro.Activo
                             && e.NombreEvento.StartsWith(search)
                             select new EventoResponse
                             {
                                 IdEvento=e.IdEvento,
                                 NombreEvento=e.NombreEvento,
                                 Expositor=e.Expositor,
                                 Fecha=(DateTime)e.Fecha,
                                 Horas=(int)e.Horas,
                                 ImagenEvento=e.ImagenEvento,
                                 DocumentoFotocheck=e.DocumentoFotocheck,
                                 DocumentoCertificado=e.DocumentoCertificado,
                                 DocumentoCertificadoImprimir = e.DocumentoCertificadoImprimir,
                                 DocumentoCertificadoExpositor = e.DocumentoCertificadoExpositor,
                                 Activo = (bool)e.Activo,
                                 Costo = e.Costo,
                                 DetallarCertificado = e.DetallarCertificado,
                                 GenerarCertificado = e.GenerarCertificado,
                                 rowid = e.rowid.ToString()
                             });

                return Paginate(page, query, q => q.Fecha);
            }
        }
        public List<ComboResponse> ListEventoCombo()
        {
            using (var context = new DBRContext())
            {
                var query = (from e in context.Evento
                             where e.Estado == EstadoRegistro.Activo
                             && e.Activo == true
                             orderby e.Fecha ascending
                             select new ComboResponse
                             {
                                 Value=e.IdEvento.ToString(),
                                 Descripcion=e.NombreEvento
                             });

                return query.ToList();
            }
        }
        public List<ComboResponse> ListAllEventoCombo()
        {
            using (var context = new DBRContext())
            {
                var query = (from e in context.Evento
                             where e.Estado == EstadoRegistro.Activo
                             orderby e.Fecha ascending
                             select new ComboResponse
                             {
                                 Value = e.IdEvento.ToString(),
                                 Descripcion = e.NombreEvento
                             });

                return query.ToList();
            }
        }
        public List<EventoResponse> ListEventoActivos()
        {
            using (var context = new DBRContext())
            {
                var query = (from e in context.Evento
                             where e.Estado == EstadoRegistro.Activo
                             && e.Activo == true
                             orderby e.Fecha ascending
                             select new EventoResponse
                             {
                                 IdEvento = e.IdEvento,
                                 NombreEvento = e.NombreEvento,
                                 Expositor = e.Expositor,
                                 Fecha = (DateTime)e.Fecha,
                                 Horas = (int)e.Horas,
                                 ImagenEvento = e.ImagenEvento,
                                 DocumentoFotocheck = e.DocumentoFotocheck,
                                 DocumentoCertificado = e.DocumentoCertificado,
                                 DocumentoCertificadoImprimir = e.DocumentoCertificadoImprimir,
                                 Descripcion = e.Descripcion,
                                 Activo = (bool)e.Activo
                             });

                return query.ToList();
            }
        }       
        public Result SaveEvento(EventoRequest request,UsuarioLogin user)
        {
            Result result = new Result();
            using (var context=new DBRContext())
            {
                var obj = new Evento();
                obj.NombreEvento = request.NombreEvento;
                obj.Descripcion = request.Descripcion;
                obj.Tipo = request.Tipo;
                obj.Modalidad = request.Modalidad;
                obj.Expositor = request.Expositor;
                obj.Fecha = request.Fecha;
                obj.Horas = request.Horas;
                obj.ImagenEvento = request.ImagenEvento;               
                obj.DocumentoFotocheck = request.DocumentoFotocheck;
                obj.DocumentoCertificado = request.DocumentoCertificado;
                obj.DocumentoCertificadoImprimir = request.DocumentoCertificadoImprimir;
                obj.DocumentoCertificadoExpositor = request.DocumentoCertificadoExpositor;
                obj.NotaAprobatoria = request.NotaAprobatoria;
                obj.Costo = request.Costo;
                obj.CostoValor = request.CostoValor;
                obj.CostoValorPromocional = request.CostoValorPromocional;
                obj.Activo = false;
                obj.DetallarCertificado = request.DetallarCertificado;
                obj.GenerarCertificado = request.GenerarCertificado;
                obj.Estado = EstadoRegistro.Activo;
                obj.UsuarioCreacion = user.IdUsuario;
                obj.FechaCreacion = DateTime.Now;
                obj.rowid = Guid.NewGuid();

                context.Evento.Add(obj);

                foreach (var item in request.ListTemas)
                {
                    var obj1 = new EventoTema();
                    obj1.IdEvento = obj.IdEvento;
                    obj1.TipoTema = item;

                    context.EventoTema.Add(obj1);
                }

                if (context.SaveChanges() > 0)
                {
                    result.IsSuccess = true;
                    result.Message = Message.ExitoGuardar;
                }
                else {
                    result.IsSuccess = false;
                    result.Message = Message.ErrorGuardar;
                };
                return result;
            }
        }
        public Result UpdateEvento(EventoRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = context.Evento.Find(request.IdEvento);
                obj.Tipo = request.Tipo;
                obj.Modalidad = request.Modalidad;
                obj.NombreEvento = request.NombreEvento;
                obj.Descripcion = request.Descripcion;
                obj.Expositor = request.Expositor;
                obj.Fecha = request.Fecha;
                obj.Horas = request.Horas;
                if (request.ImagenEvento != null && request.ImagenEvento != "")
                    obj.ImagenEvento = request.ImagenEvento;
                if (request.DocumentoFotocheck != null && request.DocumentoFotocheck != "")
                    obj.DocumentoFotocheck = request.DocumentoFotocheck;
                if (request.DocumentoCertificado != null && request.DocumentoCertificado != "")
                    obj.DocumentoCertificado = request.DocumentoCertificado;
                if (request.DocumentoCertificadoImprimir != null && request.DocumentoCertificadoImprimir != "")
                    obj.DocumentoCertificadoImprimir = request.DocumentoCertificadoImprimir;
                if (request.DocumentoCertificadoExpositor != null && request.DocumentoCertificadoExpositor != "")
                    obj.DocumentoCertificadoExpositor = request.DocumentoCertificadoExpositor;

                obj.NotaAprobatoria = request.NotaAprobatoria;
                obj.Costo = request.Costo;
                obj.CostoValor = request.CostoValor;
                obj.CostoValorPromocional = request.CostoValorPromocional;
                obj.DetallarCertificado = request.DetallarCertificado;
                obj.GenerarCertificado = request.GenerarCertificado;
                obj.UsuarioModificacion = user.IdUsuario;
                obj.FechaModificacion = DateTime.Now;

                context.Database.ExecuteSqlCommand("DELETE FROM EventoTema WHERE IdEvento=" + request.IdEvento.ToString());

                foreach (var item in request.ListTemas)
                {
                    var obj1 = new EventoTema();
                    obj1.IdEvento = obj.IdEvento;
                    obj1.TipoTema = item;

                    context.EventoTema.Add(obj1);
                }

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
        public Result UpdateCursoFiles(EventoRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = context.Evento.Find(request.IdEvento);
         
                if (request.ImagenEvento != null && request.ImagenEvento != "")
                    obj.ImagenEvento = request.ImagenEvento;
                if (request.DocumentoFotocheck != null && request.DocumentoFotocheck != "")
                    obj.DocumentoFotocheck = request.DocumentoFotocheck;
                if (request.DocumentoCertificado != null && request.DocumentoCertificado != "")
                    obj.DocumentoCertificado = request.DocumentoCertificado;
                if (request.DocumentoCertificadoImprimir != null && request.DocumentoCertificadoImprimir != "")
                    obj.DocumentoCertificadoImprimir = request.DocumentoCertificadoImprimir;
                if (request.DocumentoCertificadoExpositor != null && request.DocumentoCertificadoExpositor != "")
                    obj.DocumentoCertificadoExpositor = request.DocumentoCertificadoExpositor;

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
        public Result DeleteEvento(EventoRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var query = (from i in context.Inscripcion
                             where i.IdEvento == request.IdEvento
                             select i).ToList();
                if (query.Count > 0)
                {
                    result.IsSuccess = false;
                    result.Message = Message.ExistePersonas;
                }

                var obj = context.Evento.Find(request.IdEvento);
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
        public Result UpdateEstadoEvento(EventoRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = context.Evento.Find(request.IdEvento);

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
        public EventoResponse GetEvento(EventoRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from e in context.Evento
                             where e.Estado == EstadoRegistro.Activo
                             && e.IdEvento == request.IdEvento
                             select new EventoResponse
                             {
                                 IdEvento = e.IdEvento,
                                 Tipo = e.Tipo,
                                 Modalidad=e.Modalidad,
                                 NombreEvento = e.NombreEvento,
                                 Descripcion = e.Descripcion,
                                 Expositor = e.Expositor,
                                 Fecha = (DateTime)e.Fecha,
                                 Horas = (int)e.Horas,
                                 ImagenEvento = e.ImagenEvento,
                                 DocumentoFotocheck = e.DocumentoFotocheck,
                                 DocumentoCertificado = e.DocumentoCertificado,
                                 DocumentoCertificadoImprimir = e.DocumentoCertificadoImprimir,
                                 DocumentoCertificadoExpositor = e.DocumentoCertificadoExpositor,
                                 Activo = (bool)e.Activo,
                                 NotaAprobatoria = e.NotaAprobatoria,
                                 Costo = e.Costo,
                                 CostoValor = e.CostoValor,
                                 CostoValorPromocional = e.CostoValorPromocional,
                                 DetallarCertificado = e.DetallarCertificado,
                                 GenerarCertificado = e.GenerarCertificado,
                                 IdsTemas = (from t in context.EventoTema where t.IdEvento == e.IdEvento select (int)t.TipoTema).ToList()
                             }).FirstOrDefault();

                return query;
            }
        }
        //Virtual
        public EventoResponse GetEventoByRowId(EventoRequest request,UsuarioLogin user)
        {
            using (var context = new DBRContext())
            {
                if (user.IdUsuarioTipo == Convert.ToInt32(Perfil.Administrador) || user.IdUsuarioTipo == Convert.ToInt32(Perfil.EditorContenido))
                {
                    var query = (from e in context.Evento
                                 where e.Estado == EstadoRegistro.Activo
                                 && e.rowid == request.rowid
                                 && e.Activo == true
                                 select new EventoResponse
                                 {
                                     IdEvento = e.IdEvento,
                                     NombreEvento = e.NombreEvento,
                                     Expositor = e.Expositor,
                                     Fecha = (DateTime)e.Fecha,
                                     Horas = (int)e.Horas,
                                     ImagenEvento = e.ImagenEvento,
                                     DocumentoFotocheck = e.DocumentoFotocheck,
                                     DocumentoCertificado = e.DocumentoCertificado,
                                     DocumentoCertificadoImprimir = e.DocumentoCertificadoImprimir,
                                     Activo = (bool)e.Activo,
                                     rowid = e.rowid.ToString()
                                 }).FirstOrDefault();

                    return query;
                }
                else
                {
                    DateTime FechaHoy = DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings.Get("DiferenciaZona").ToString()));

                    var query = (from e in context.Evento
                                 join eu in context.EventoUsuario on e.IdEvento equals eu.IdEvento
                                 where e.Estado == EstadoRegistro.Activo
                                 && eu.Estado == EstadoRegistro.Activo
                                 && e.rowid == request.rowid
                                 && eu.IdUsuario == user.IdUsuario
                                 && eu.FechaInicio <= FechaHoy
                                 && eu.FechaFin >= FechaHoy
                                 && e.Activo == true
                                 select new EventoResponse
                                 {
                                     IdEvento = e.IdEvento,
                                     NombreEvento = e.NombreEvento,
                                     Descripcion = e.Descripcion,
                                     Expositor = e.Expositor,
                                     Fecha = (DateTime)e.Fecha,
                                     Horas = (int)e.Horas,
                                     ImagenEvento = e.ImagenEvento,
                                     DocumentoFotocheck = e.DocumentoFotocheck,
                                     DocumentoCertificado = e.DocumentoCertificado,
                                     DocumentoCertificadoImprimir = e.DocumentoCertificadoImprimir,
                                     Activo = (bool)e.Activo,
                                     FechaUltimoAcceso = eu.FechaUltimoAcceso,
                                     rowid = e.rowid.ToString()
                                 }).FirstOrDefault();

                    return query;
                }
                
            }
        }
        public Paged<EventoResponse> ListEventoAsignacionPaged(PageRequest page)
        {
            using (var context = new DBRContext())
            {
                var search = page.search.value ?? "";
                page.Order = "DESC";
                var query = (from e in context.Evento
                             where e.Estado == EstadoRegistro.Activo
                             && e.NombreEvento.StartsWith(search)
                             select new EventoResponse
                             {
                                 IdEvento = e.IdEvento,
                                 NombreEvento = e.NombreEvento,
                                 Expositor = e.Expositor,
                                 Fecha = (DateTime)e.Fecha,
                                 Horas = (int)e.Horas,
                                 ImagenEvento = e.ImagenEvento,
                                 DocumentoFotocheck = e.DocumentoFotocheck,
                                 DocumentoCertificado = e.DocumentoCertificado,
                                 DocumentoCertificadoImprimir = e.DocumentoCertificadoImprimir,
                                 Activo = (bool)e.Activo,
                                 rowid = e.rowid.ToString(),
                                 CantidadAsignados = (from eu in context.EventoUsuario where eu.Estado == EstadoActivo.Activo && eu.IdEvento == e.IdEvento select 1).Count()
                             });

                return PaginateNew(page, query, q => q.Fecha);
            }
        }
        public List<EventoResponse> ListEventoUsuario(UsuarioLogin user)
        {
            using (var context = new DBRContext())
            {
                if (user.IdUsuarioTipo == Convert.ToInt32(Perfil.Administrador) || user.IdUsuarioTipo == Convert.ToInt32(Perfil.EditorContenido))
                {
                    var query = (from e in context.Evento
                                 join t in context.Tipo
                                 on new { Tipo = e.Tipo, TipoCurso = "TIPO CURSO" } equals new { Tipo = t.Valor, TipoCurso = t.Grupo } into ts
                                 from t in ts.DefaultIfEmpty()
                                 where e.Estado == EstadoRegistro.Activo
                                 && e.Activo == true
                                 select new EventoResponse
                                 {
                                     IdEvento = e.IdEvento,
                                     TipoCursoNombre = t != null ? t.NombreTipo : "",
                                     NombreEvento = e.NombreEvento,
                                     Expositor = e.Expositor,
                                     Fecha = (DateTime)e.Fecha,
                                     Horas = (int)e.Horas,
                                     ImagenEvento = e.ImagenEvento,
                                     DocumentoFotocheck = e.DocumentoFotocheck,
                                     DocumentoCertificado = e.DocumentoCertificado,
                                     DocumentoCertificadoImprimir = e.DocumentoCertificadoImprimir,
                                     Activo = (bool)e.Activo,
                                     rowid = e.rowid.ToString()
                                 });

                    return query.ToList();
                }
                else
                {
                    DateTime FechaHoy = DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings.Get("DiferenciaZona").ToString()));

                    var query = (from e in context.Evento
                                 join eu in context.EventoUsuario on e.IdEvento equals eu.IdEvento
                                 join u in context.Usuario on eu.IdUsuario equals u.IdUsuario
                                 join p in context.Persona on u.NumeroDocumento equals p.NumeroDocumento
                                 join i in context.Inscripcion on p.IdPersona equals i.IdPersona
                                 join t in context.Tipo
                                 on new { Tipo = e.Tipo, TipoCurso = "TIPO CURSO" } equals new { Tipo = t.Valor, TipoCurso = t.Grupo } into ts
                                 from t in ts.DefaultIfEmpty()
                                 where e.Estado == EstadoRegistro.Activo
                                 && eu.Estado == EstadoRegistro.Activo
                                 && i.Estado == EstadoRegistro.Activo
                                 && e.IdEvento == i.IdEvento
                                 && eu.IdUsuario == user.IdUsuario
                                 && eu.FechaInicio <= FechaHoy
                                 && eu.FechaFin >= FechaHoy
                                 && e.Activo == true
                                 select new EventoResponse
                                 {
                                     IdEvento = e.IdEvento,
                                     TipoCursoNombre = t != null ? t.NombreTipo : "",
                                     NombreEvento = e.NombreEvento,
                                     Expositor = e.Expositor,
                                     Fecha = (DateTime)e.Fecha,
                                     Horas = (int)e.Horas,
                                     ImagenEvento = e.ImagenEvento,
                                     DocumentoFotocheck = e.DocumentoFotocheck,
                                     DocumentoCertificado = e.DocumentoCertificado,
                                     DocumentoCertificadoImprimir = e.DocumentoCertificadoImprimir,
                                     Activo = (bool)e.Activo,
                                     CetificadoFirmado = i.Certificado,
                                     Costo = e.Costo,
                                     rowid = e.rowid.ToString()
                                 });

                    return query.ToList();
                }
            }
        }
        public Paged<EventoResponse> ListEventoUsuarioPaged(PageRequest page,UsuarioLogin user)
        {
            using (var context = new DBRContext())
            {
                if (user.IdUsuarioTipo == Convert.ToInt32(Perfil.Administrador) || user.IdUsuarioTipo == Convert.ToInt32(Perfil.EditorContenido))
                {
                    var query = (from e in context.Evento
                                 where e.Estado == EstadoRegistro.Activo
                                 select new EventoResponse
                                 {
                                     IdEvento = e.IdEvento,
                                     NombreEvento = e.NombreEvento,
                                     Expositor = e.Expositor,
                                     Fecha = (DateTime)e.Fecha,
                                     Horas = (int)e.Horas,
                                     ImagenEvento = e.ImagenEvento,
                                     DocumentoFotocheck = e.DocumentoFotocheck,
                                     DocumentoCertificado = e.DocumentoCertificado,
                                     DocumentoCertificadoImprimir = e.DocumentoCertificadoImprimir,
                                     Activo = (bool)e.Activo,
                                     rowid = e.rowid.ToString()
                                 });

                    return PaginateNew(page, query, q => q.IdEvento);
                }
                else
                {
                    DateTime FechaHoy = DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings.Get("DiferenciaZona").ToString()));

                    var query = (from e in context.Evento
                                 join eu in context.EventoUsuario on e.IdEvento equals eu.IdEvento
                                 where e.Estado == EstadoRegistro.Activo
                                 && eu.Estado == EstadoRegistro.Activo
                                 && eu.IdUsuario == user.IdUsuario
                                 && eu.FechaInicio <= FechaHoy
                                 && eu.FechaFin >= FechaHoy
                                 select new EventoResponse
                                 {
                                     IdEvento = e.IdEvento,
                                     NombreEvento = e.NombreEvento,
                                     Expositor = e.Expositor,
                                     Fecha = (DateTime)e.Fecha,
                                     Horas = (int)e.Horas,
                                     ImagenEvento = e.ImagenEvento,
                                     DocumentoFotocheck = e.DocumentoFotocheck,
                                     DocumentoCertificado = e.DocumentoCertificado,
                                     DocumentoCertificadoImprimir = e.DocumentoCertificadoImprimir,
                                     Activo = (bool)e.Activo,
                                     rowid = e.rowid.ToString()
                                 });

                    return PaginateNew(page, query, q => q.IdEvento);
                }
            }
        }
        //EventoUsuario
        public Paged<EventoUsuarioResponse> ListEventoUsuarioAsignadoPaged(PageRequest page, EventoUsuarioRequest request)
        {
            using (var context = new DBRContext())
            {
                page.search.value = page.search.value == null ? "" : page.search.value;

                var query = (from e in context.EventoUsuario
                             join u in context.Usuario on e.IdUsuario equals u.IdUsuario
                             where e.Estado == EstadoRegistro.Activo
                             && u.Estado==EstadoActivo.Activo
                             && e.IdEvento == request.IdEvento
                             && (u.Nombres.Contains(page.search.value) || u.ApellidoPaterno.Contains(page.search.value) || u.ApellidoMaterno.Contains(page.search.value))
                             select new EventoUsuarioResponse
                             {
                                 IdEventoUsuario=e.IdEventoUsuario,
                                 IdEvento = e.IdEvento,
                                 IdUsuario=e.IdUsuario,
                                 NombreUsuario = u.Nombres + " " + u.ApellidoPaterno + " " + u.ApellidoMaterno,
                                 FechaInicio =e.FechaInicio,
                                 FechaFin=e.FechaFin
                             });

                return PaginateNew(page, query, q => q.IdEvento);
            }
        }
        public List<EventoUsuarioResponse> ListEventoUsuarioAsignado(EventoUsuarioRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from e in context.EventoUsuario
                             join u in context.Usuario on e.IdUsuario equals u.IdUsuario
                             where e.Estado == EstadoRegistro.Activo
                             && u.Estado == EstadoActivo.Activo
                             && e.IdEvento == request.IdEvento
                             select new EventoUsuarioResponse
                             {
                                 IdEventoUsuario = e.IdEventoUsuario,
                                 IdEvento = e.IdEvento,
                                 IdUsuario = e.IdUsuario,
                                 NombreUsuario = u.Nombres + " " + u.ApellidoPaterno + " " + u.ApellidoMaterno,
                                 FechaInicio = e.FechaInicio,
                                 FechaFin = e.FechaFin
                             });

                return query.ToList();
            }
        }
        public EventoUsuarioResponse GetEventoUsuario(EventoUsuarioRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from ue in context.EventoUsuario
                             where ue.Estado == EstadoActivo.Activo
                             && ue.IdEvento == request.IdEvento
                             && ue.IdUsuario == request.IdUsuario
                             select new EventoUsuarioResponse
                             {
                                 IdEventoUsuario = ue.IdEventoUsuario,
                                 IdEvento = ue.IdEvento,
                                 IdUsuario = ue.IdUsuario,
                                 FechaInicio = ue.FechaInicio,
                                 FechaFin = ue.FechaFin
                             }).FirstOrDefault();

                return query;
            }
        }
        public Result SaveEventoUsuario(EventoUsuarioRequest request, UsuarioLogin user, List<int> requestVideos)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var query = (from ue in context.EventoUsuario
                             where ue.Estado==EstadoActivo.Activo
                             && ue.IdEvento==request.IdEvento
                             && ue.IdUsuario==request.IdUsuario
                             select ue.IdEventoUsuario).ToList();

                if (query.Count() > 0)
                {
                    result.IsSuccess = false;
                    result.Message = Message.EventoUsuarioExiste;
                    return result;
                }

                var queryInscrito = (from i in context.Inscripcion
                                     join p in context.Persona on i.IdPersona equals p.IdPersona
                                     join u in context.Usuario on p.NumeroDocumento equals u.NumeroDocumento
                                     where i.Estado == EstadoActivo.Activo
                                     && p.Estado == EstadoActivo.Activo
                                     && u.Estado == EstadoActivo.Activo
                                     && u.IdUsuario == request.IdUsuario
                                     && i.IdEvento == request.IdEvento
                                     select i.IdInscripcion).ToList();

                if (queryInscrito.Count() == 0)
                {
                    result.IsSuccess = false;
                    result.Message = Message.EventoUsuarioExisteIncripto;
                    return result;
                }

                var obj = new EventoUsuario();
                obj.IdEvento = request.IdEvento;
                obj.IdUsuario = request.IdUsuario;
                obj.FechaInicio = request.FechaInicio;
                obj.FechaFin = request.FechaFin;
                obj.Abierto = true;
                obj.Estado = EstadoRegistro.Activo;
                obj.UsuarioCreacion = user.IdUsuario;
                obj.FechaCreacion = DateTime.Now;

                context.EventoUsuario.Add(obj);

                foreach (var item in requestVideos)
                {
                    var objVideo = new EventoUsuarioVirtualVideo();
                    objVideo.IdEventoUsuario = obj.IdEventoUsuario;
                    objVideo.IdVirtualVideo = item;
                    objVideo.Estado = EstadoRegistro.Activo;
                    objVideo.UsuarioCreacion = user.IdUsuario;
                    objVideo.FechaCreacion = DateTime.Now;

                    context.EventoUsuarioVirtualVideo.Add(objVideo);
                }

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoGuardar;

                return result;
            }
        }
        public Result UpdateEventoUsuario(EventoUsuarioRequest request, UsuarioLogin user, List<int> requestVideos)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        var obj = (from eu in context.EventoUsuario
                                   where eu.Estado == EstadoActivo.Activo
                                   && eu.IdEventoUsuario == request.IdEventoUsuario
                                   select eu).FirstOrDefault();

                        obj.FechaInicio = request.FechaInicio;
                        obj.FechaFin = request.FechaFin;
                        obj.UsuarioModificacion = user.IdUsuario;
                        obj.FechaModificacion = DateTime.Now;

                        context.Database.ExecuteSqlCommand("UPDATE dbo.EventoUsuarioVirtualVideo SET Estado=0,UsuarioModificacion='" + user.IdUsuario.ToString() + "',FechaModificacion=GETDATE() WHERE Estado=1 AND IdEventoUsuario=" + obj.IdEventoUsuario.ToString());

                        foreach (var item in requestVideos)
                        {
                            var objVideo = new EventoUsuarioVirtualVideo();
                            objVideo.IdEventoUsuario = obj.IdEventoUsuario;
                            objVideo.IdVirtualVideo = item;
                            objVideo.Estado = EstadoRegistro.Activo;
                            objVideo.UsuarioCreacion = user.IdUsuario;
                            objVideo.FechaCreacion = DateTime.Now;

                            context.EventoUsuarioVirtualVideo.Add(objVideo);
                        }
                        trans.Commit();
                        result.IsSuccess = context.SaveChanges() > 0;
                        result.Message = Message.ExitoActualizar;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        result.IsSuccess = false;
                        result.Message = Message.ErrorNoControlado;
                        result.MessageExeption = ex.Message;
                    }
                }              
                return result;
            }
        }
        public Result UpdateEventoAccedidoUsuario(EventoUsuarioRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                using (var trans = context.Database.BeginTransaction())
                {
                    try
                    {
                        var obj = (from eu in context.EventoUsuario
                                   where eu.Estado == EstadoActivo.Activo
                                   && eu.IdEvento == request.IdEvento
                                   && eu.IdUsuario == user.IdUsuario
                                   select eu).FirstOrDefault();

                        obj.FechaUltimoAcceso = DateTime.Now;
                        obj.UsuarioModificacion = user.IdUsuario;
                        obj.FechaModificacion = DateTime.Now;

                        trans.Commit();
                        result.IsSuccess = context.SaveChanges() > 0;
                        result.Message = Message.ExitoActualizar;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        result.IsSuccess = false;
                        result.Message = Message.ErrorNoControlado;
                        result.MessageExeption = ex.Message;
                    }
                }
                return result;
            }
        }
        public Result DeleteEventoUsuario(EventoUsuarioRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = (from eu in context.EventoUsuario
                           where eu.Estado == EstadoActivo.Activo
                           && eu.IdEventoUsuario == request.IdEventoUsuario
                           select eu).FirstOrDefault();

                obj.Estado = EstadoActivo.Inactivo;
                obj.UsuarioModificacion = user.IdUsuario;
                obj.FechaModificacion = DateTime.Now;

                context.Database.ExecuteSqlCommand("UPDATE dbo.EventoUsuarioVirtualVideo SET Estado=0,UsuarioModificacion='" + user.IdUsuario.ToString() + "',FechaModificacion=GETDATE() WHERE Estado=0 AND IdEventoUsuario=" + obj.IdEventoUsuario.ToString());

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoEliminar;

                return result;
            }
        }
        //Participacion
        public Paged<EventoResponse> ListEventosPorPersonaPaged(PageRequest page, PersonaRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from i in context.Inscripcion
                             join e in context.Evento on i.IdEvento equals e.IdEvento
                             join p in context.Persona on i.IdPersona equals p.IdPersona
                             where e.Estado == EstadoRegistro.Activo
                             && i.Estado == EstadoRegistro.Activo
                             && p.NumeroDocumento == request.NumeroDocumento
                             && i.EstadoPago == 1//Pagado
                             && i.Certificado != null
                             orderby e.Fecha ascending
                             select new EventoResponse
                             {
                                 IdEvento = e.IdEvento,
                                 IdInscripcion = i.IdInscripcion,
                                 NombreEvento = e.NombreEvento,
                                 Expositor = e.Expositor,
                                 Fecha = (DateTime)e.Fecha,
                                 Horas = (int)e.Horas,
                                 ImagenEvento = e.ImagenEvento,
                                 DocumentoFotocheck = e.DocumentoFotocheck,
                                 DocumentoCertificado = e.DocumentoCertificado,
                                 DocumentoCertificadoImprimir = e.DocumentoCertificadoImprimir,
                                 Activo = (bool)e.Activo,
                                 NombrePersona = p.Nombres + " " + p.ApellidoPaterno + " " + p.ApellidoMaterno,
                                 CetificadoFirmado = i.Certificado
                             });

                return PaginateNew(page, query, q => q.IdEvento);
            }
        }
        public Paged<EventoResponse> ListEventosPorCodigoPaged(PageRequest page, PersonaRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from i in context.Inscripcion
                             join e in context.Evento on i.IdEvento equals e.IdEvento
                             join p in context.Persona on i.IdPersona equals p.IdPersona
                             where e.Estado == EstadoRegistro.Activo
                             && i.Estado == EstadoRegistro.Activo
                             &&
                             ( 
                                (i.NumeroCertificado == 0 && (i.rowguid.ToString().Substring(0, 5) + i.IdInscripcion.ToString()).ToUpper() == request.NumeroDocumento) ||
                                (i.NumeroCertificado > 0)
                             )
                             && i.EstadoPago == 1//Pagado
                             && i.Certificado != null
                             orderby e.Fecha ascending
                             select new EventoResponse
                             {
                                 IdEvento = e.IdEvento,
                                 IdInscripcion = i.IdInscripcion,
                                 NombreEvento = e.NombreEvento,
                                 Expositor = e.Expositor,
                                 Fecha = (DateTime)e.Fecha,
                                 Horas = (int)e.Horas,
                                 ImagenEvento = e.ImagenEvento,
                                 DocumentoFotocheck = i.IdInscripcion.ToString() + i.IdEvento.ToString() + i.IdPersona.ToString(),
                                 DocumentoCertificado = e.DocumentoCertificado,
                                 DocumentoCertificadoImprimir = e.DocumentoCertificadoImprimir,
                                 Activo = (bool)e.Activo,
                                 NombrePersona = p.Nombres + " " + p.ApellidoPaterno + " " + p.ApellidoMaterno,
                                 CetificadoFirmado = i.Certificado
                             });
                var queryList = query.ToList();

                queryList = queryList.Where(x => x.DocumentoFotocheck.PadRight(16, '0') == request.NumeroDocumento).ToList();

                Paged<EventoResponse> obj = new Paged<EventoResponse>();
                int PageIndex = page.PageNumber - 1;
                var rowsTotal = queryList.Count();

                queryList = queryList.Skip(page.start).Take(page.length).ToList();

                obj.data = queryList;
                obj.recordsTotal = rowsTotal;
                obj.recordsFiltered = rowsTotal;

                return obj;
            }
        }
        //EventoCorroe
        public Result SaveEventoCorreo(EventoCorreoRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = new EventoCorreo();
                obj.IdEvento = request.IdEvento;
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

                context.EventoCorreo.Add(obj);

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
        public Result UpdateCorreo(EventoCorreoRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = (from c in context.EventoCorreo where c.IdEventoCorreo == request.IdEventoCorreo select c).FirstOrDefault();

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
        public EventoCorreoResponse GeEventoCorreoByIdEvento(EventoCorreoRequest request)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = (from c in context.EventoCorreo
                           where c.IdEvento == request.IdEvento
                           select new EventoCorreoResponse
                           {
                               IdEventoCorreo = c.IdEventoCorreo,
                               IdEvento = c.IdEvento,
                               Asunto = c.Asunto,
                               Origen = c.Origen,
                               NombreOrigen = c.NombreOrigen,
                               Mensaje = c.Mensaje
                           }).FirstOrDefault();
                return obj;
            }
        }

        #region Modulo 

        public Paged<ModuloResponse> ListModuloPaged(PageRequest page, ModuloRequest request)
        {
            Paged<ModuloResponse> obj = new Paged<ModuloResponse>();
            using (var context = new DBRContext())
            {
                page.search.value = page.search.value == null ? "" : page.search.value;

                var p0 = new SqlParameter { ParameterName = "IdEvento", Value = request.IdEvento, SqlDbType = SqlDbType.Int };
                var p1 = new SqlParameter { ParameterName = "search", Value = page.search.value, SqlDbType = SqlDbType.VarChar };
                var p2 = new SqlParameter { ParameterName = "start", Value = page.start, SqlDbType = SqlDbType.Int };
                var p3 = new SqlParameter { ParameterName = "length", Value = page.length, SqlDbType = SqlDbType.Int };

                var response = context.Database.SqlQuery<ModuloResponse>("dbo.UspListModuloByEventoPaged @IdEvento, @search, @start, @length", p0, p1, p2, p3).ToList();
                var Total = response.Count() == 0 ? 0 : response[0].TotalRegistros;

                obj.data = response;
                obj.recordsTotal = Total;
                obj.recordsFiltered = Total;
                return obj;
            }
        }
        public Result SaveModulo(ModuloRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = new Modulo();
                obj.IdEvento = request.IdEvento;
                obj.Nombre = request.Nombre;
                obj.Descripcion = request.Descripcion;
                obj.Expositor = request.Expositor;
                obj.Horas = request.Horas;
                obj.Peso = request.Peso;
                obj.Estado = EstadoRegistro.Activo;
                obj.UsuarioCreacion = user.IdUsuario;
                obj.FechaCreacion = DateTime.Now;
                obj.rowid = Guid.NewGuid();

                context.Modulo.Add(obj);

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoGuardar;

                return result;
            }
        }
        public Result UpdateModulo(ModuloRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = (from m in context.Modulo
                           where m.Estado == EstadoRegistro.Activo
                           && m.IdModulo == request.IdModulo
                           select m).FirstOrDefault();

                obj.Nombre = request.Nombre;
                obj.Descripcion = request.Descripcion;
                obj.Expositor = request.Expositor;
                obj.Horas = request.Horas;
                obj.Peso = request.Peso;
                obj.UsuarioModificacion = user.IdUsuario;
                obj.FechaModificacion = DateTime.Now;

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoActualizar;

                return result;
            }
        }
        public Result DeleteModulo(ModuloRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = (from m in context.Modulo
                           where m.Estado == EstadoRegistro.Activo
                           && m.IdModulo == request.IdModulo
                           select m).FirstOrDefault();

                obj.Estado = EstadoRegistro.Inactivo;
                obj.UsuarioModificacion = user.IdUsuario;
                obj.FechaModificacion = DateTime.Now;

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoEliminar;

                return result;
            }
        }
        public ModuloResponse GetModulo(ModuloRequest request)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = (from m in context.Modulo
                           where m.Estado == EstadoRegistro.Activo
                           && m.IdModulo == request.IdModulo
                           select new ModuloResponse
                           {
                               IdModulo = m.IdModulo,
                               IdEvento = m.IdEvento,
                               Nombre = m.Nombre,
                               Descripcion = m.Descripcion,
                               Expositor = m.Expositor,
                               Horas = m.Horas,
                               Peso = m.Peso ?? 0
                           }).FirstOrDefault();

                return obj;
            }
        }
        public List<ModuloResponse> ListModulo(ModuloRequest request)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var query = (from m in context.Modulo
                             join e in context.Evento on m.IdEvento equals e.IdEvento
                             where m.Estado == EstadoRegistro.Activo
                             && m.IdEvento == request.IdEvento
                             orderby m.IdModulo ascending
                             select new ModuloResponse
                             {
                                 IdModulo = m.IdModulo,
                                 IdEvento = m.IdEvento,
                                 Nombre = m.Nombre,
                                 Descripcion = m.Descripcion,
                                 Expositor = m.Expositor,
                                 Horas = m.Horas,
                                 Peso = m.Peso.Value,
                                 NombreEvento=e.NombreEvento
                             }).ToList();

                return query;
            }
        }

        public List<ModuloLeccionResponse> ListModuloWithLecciones(ModuloRequest request)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var query = (from m in context.Modulo
                             join l in context.Leccion on m.IdModulo equals l.IdModulo
                             where m.Estado == EstadoRegistro.Activo
                             && l.Estado == EstadoRegistro.Activo
                             && m.IdEvento == request.IdEvento
                             orderby m.IdModulo ascending
                             select new ModuloLeccionResponse
                             {
                                 IdModulo = m.IdModulo,
                                 IdEvento = m.IdEvento,
                                 Nombre = m.Nombre,
                                 Descripcion = m.Descripcion,
                                 Expositor = m.Expositor,
                                 Horas = m.Horas,
                                 Peso = m.Peso ?? 0,
                                 IdLeccion = l.IdLeccion,
                                 Tipo = l.Tipo ?? 0,
                                 NombreLeccion = l.Nombre,
                                 Duracion = l.Duracion ?? 0,
                                 Orden = (int)l.Orden
                             }).ToList();

                return query;
            }
        }

        #endregion

        #region Leccion

        public Paged<LeccionResponse> ListLeccionPaged(PageRequest page, LeccionRequest request)
        {
            Paged<LeccionResponse> obj = new Paged<LeccionResponse>();
            using (var context = new DBRContext())
            {
                page.search.value = page.search.value == null ? "" : page.search.value;

                var p0 = new SqlParameter { ParameterName = "IdModulo", Value = request.IdModulo, SqlDbType = SqlDbType.Int };
                var p1 = new SqlParameter { ParameterName = "search", Value = page.search.value, SqlDbType = SqlDbType.VarChar };
                var p2 = new SqlParameter { ParameterName = "start", Value = page.start, SqlDbType = SqlDbType.Int };
                var p3 = new SqlParameter { ParameterName = "length", Value = page.length, SqlDbType = SqlDbType.Int };

                var response = context.Database.SqlQuery<LeccionResponse>("dbo.UspListLeccionByModuloPaged @IdModulo, @search, @start, @length", p0, p1, p2, p3).ToList();
                var Total = response.Count() == 0 ? 0 : response[0].TotalRegistros;

                obj.data = response;
                obj.recordsTotal = Total;
                obj.recordsFiltered = Total;
                return obj;
            }
        }

        public Result SaveLeccion(LeccionRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = new Leccion();
                obj.IdModulo = request.IdModulo;
                obj.Tipo = request.Tipo;
                obj.Nombre = request.Nombre;
                obj.Descripcion = request.Descripcion;
                obj.Duracion = request.Duracion;
                obj.TipoUrl = request.TipoUrl;
                obj.Url = request.Url;
                obj.Orden = request.Orden;
                obj.Estado = EstadoRegistro.Activo;
                obj.UsuarioCreacion = user.IdUsuario;
                obj.FechaCreacion = DateTime.Now;

                context.Leccion.Add(obj);

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

        public Result SaveLeccionCuestionario(LeccionRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                using (var tx = context.Database.BeginTransaction())
                {
                    try
                    {
                        var obj = new Leccion();
                        obj.IdModulo = request.IdModulo;
                        obj.Tipo = request.Tipo;
                        obj.Nombre = request.Nombre;
                        obj.Descripcion = request.Descripcion;
                        obj.Duracion = request.Duracion;
                        obj.Url = request.Url;
                        obj.Orden = request.Orden;
                        obj.Peso = request.Peso;
                        obj.Estado = EstadoRegistro.Activo;
                        obj.UsuarioCreacion = user.IdUsuario;
                        obj.FechaCreacion = DateTime.Now;

                        context.Leccion.Add(obj);

                        context.SaveChanges();

                        var cuestionario = new Cuestionario
                        {
                            IdLeccion = obj.IdLeccion,
                            Nombre = request.Nombre,
                            Descripcion = request.Descripcion,
                            Peso = request.Peso,
                            Estado = EstadoRegistro.Activo,
                            UsuarioCreacion = user.IdUsuario,
                            FechaCreacion = DateTime.Now
                        };

                        context.Cuestionario.Add(cuestionario);

                        if (context.SaveChanges() > 0)
                        {
                            tx.Commit();
                            result.IsSuccess = true;
                            result.Message = Message.ExitoGuardar;
                        }
                        else
                        {
                            tx.Rollback();
                            result.IsSuccess = false;
                            result.Message = Message.ErrorGuardar;
                        };

                        return result;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        result.IsSuccess = false;
                        result.Message = Message.ErrorGuardar;
                        return result;
                    }
                }
            }
        }

        public Result UpdateLeccion(LeccionRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = context.Leccion.Find(request.IdLeccion);
                obj.Nombre = request.Nombre;
                obj.Descripcion = request.Descripcion;
                obj.Duracion = request.Duracion;
                obj.TipoUrl = request.TipoUrl;
                obj.Url = request.Url;
                obj.Orden = request.Orden;
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

        public Result DeleteLeccion(LeccionRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                //TODO: Validar si no existtte en cuestionario
                //var query = (from i in context.Inscripcion
                //             where i.IdEvento == request.IdEvento
                //             select i).ToList();
                //if (query.Count > 0)
                //{
                //    result.IsSuccess = false;
                //    result.Message = Message.ExistePersonas;
                //}

                var obj = context.Leccion.Find(request.IdLeccion);
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

        public LeccionResponse GetLeccion(LeccionRequest request)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = (from l in context.Leccion
                           where l.Estado == EstadoRegistro.Activo
                           && l.IdLeccion == request.IdLeccion
                           select new LeccionResponse
                           {
                               IdLeccion = l.IdLeccion,
                               IdModulo = l.IdModulo,
                               Tipo = l.Tipo,
                               Nombre = l.Nombre,
                               Duracion = l.Duracion ?? 0,
                               Descripcion = l.Descripcion,
                               TipoUrl = l.TipoUrl,
                               UrlVideo = l.Url,
                               Orden = l.Orden ?? 0
                           }).FirstOrDefault();

                return obj;
            }
        }

        //VALIDAR SI EL EVENTO SE ENCUENTRA ACTIVO
        public LeccionResponse GetLeccionByEventoActivo(LeccionRequest request)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = (from l in context.Leccion
                           join m in context.Modulo.Where(x=>x.Estado==EstadoActivo.Activo) on l.IdModulo equals m.IdModulo
                           join e in context.Evento.Where(x => x.Estado == EstadoActivo.Activo && x.Activo == true) on m.IdEvento equals e.IdEvento
                           where l.Estado == EstadoRegistro.Activo
                           && l.IdLeccion == request.IdLeccion
                           select new LeccionResponse
                           {
                               IdLeccion = l.IdLeccion,
                               IdModulo = l.IdModulo,
                               Tipo = l.Tipo,
                               Nombre = l.Nombre,
                               Duracion = l.Duracion ?? 0,
                               Descripcion = l.Descripcion,
                               TipoUrl = l.TipoUrl,
                               UrlVideo = l.Url,
                               Orden = l.Orden ?? 0,
                               NotaAprobatoria = e.NotaAprobatoria,
                               IdEvento = e.IdEvento
                           }).FirstOrDefault();              
                return obj;
            }
        }
        public LeccionResponse GetPrevLeccion(LeccionRequest request)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = (from l in context.Leccion
                           join m in context.Modulo.Where(x => x.Estado == EstadoActivo.Activo) on l.IdModulo equals m.IdModulo
                           join e in context.Evento.Where(x => x.Estado == EstadoActivo.Activo && x.Activo == true) on m.IdEvento equals e.IdEvento
                           where l.Estado == EstadoRegistro.Activo
                           && l.IdModulo == request.IdModulo
                           && l.Orden < request.Orden
                           orderby l.Orden descending
                           select new LeccionResponse
                           {
                               IdLeccion = l.IdLeccion,
                               IdModulo = l.IdModulo,
                               Tipo = l.Tipo,
                               Nombre = l.Nombre,
                               Duracion = l.Duracion ?? 0,
                               Descripcion = l.Descripcion,
                               UrlVideo = l.Url,
                               Orden = l.Orden ?? 0
                           }).FirstOrDefault();

                if (obj == null)
                {
                    obj = (from l in context.Leccion
                           join m in context.Modulo.Where(x => x.Estado == EstadoActivo.Activo) on l.IdModulo equals m.IdModulo
                           join e in context.Evento.Where(x => x.Estado == EstadoActivo.Activo && x.Activo == true) on m.IdEvento equals e.IdEvento
                           where l.Estado == EstadoRegistro.Activo
                           && m.IdEvento == request.IdEvento
                           && m.IdModulo < request.IdModulo
                           orderby m.IdModulo descending, l.Orden descending
                           select new LeccionResponse
                           {
                               IdLeccion = l.IdLeccion,
                               IdModulo = l.IdModulo,
                               Tipo = l.Tipo,
                               Nombre = l.Nombre,
                               Duracion = l.Duracion ?? 0,
                               Descripcion = l.Descripcion,
                               UrlVideo = l.Url,
                               Orden = l.Orden ?? 0
                           }).FirstOrDefault();
                }

                return obj;
            }
        }

        public LeccionResponse GetNextLeccion(LeccionRequest request)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = (from l in context.Leccion
                           join m in context.Modulo.Where(x => x.Estado == EstadoActivo.Activo) on l.IdModulo equals m.IdModulo
                           join e in context.Evento.Where(x => x.Estado == EstadoActivo.Activo && x.Activo == true) on m.IdEvento equals e.IdEvento
                           where l.Estado == EstadoRegistro.Activo
                           && l.IdModulo == request.IdModulo
                           && l.Orden > request.Orden
                           orderby l.Orden ascending
                           select new LeccionResponse
                           {
                               IdLeccion = l.IdLeccion,
                               IdModulo = l.IdModulo,
                               Tipo = l.Tipo,
                               Nombre = l.Nombre,
                               Duracion = l.Duracion ?? 0,
                               Descripcion = l.Descripcion,
                               UrlVideo = l.Url,
                               Orden = l.Orden ?? 0
                           }).FirstOrDefault();

                if (obj == null)
                {
                    obj = (from l in context.Leccion
                           join m in context.Modulo.Where(x => x.Estado == EstadoActivo.Activo) on l.IdModulo equals m.IdModulo
                           join e in context.Evento.Where(x => x.Estado == EstadoActivo.Activo && x.Activo == true) on m.IdEvento equals e.IdEvento
                           where l.Estado == EstadoRegistro.Activo
                           && m.IdEvento == request.IdEvento
                           && m.IdModulo > request.IdModulo
                           orderby m.IdModulo ascending, l.Orden ascending
                           select new LeccionResponse
                           {
                               IdLeccion = l.IdLeccion,
                               IdModulo = l.IdModulo,
                               Tipo = l.Tipo,
                               Nombre = l.Nombre,
                               Duracion = l.Duracion ?? 0,
                               Descripcion = l.Descripcion,
                               UrlVideo = l.Url,
                               Orden = l.Orden ?? 0
                           }).FirstOrDefault();
                }


                return obj;
            }
        }

        public List<LeccionResponse> ListLeccion(LeccionRequest request)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var query = (from l in context.Leccion
                             join m in context.Modulo.Where(x => x.Estado == EstadoActivo.Activo) on l.IdModulo equals m.IdModulo
                             join e in context.Evento.Where(x => x.Estado == EstadoActivo.Activo && x.Activo == true) on m.IdEvento equals e.IdEvento
                             where l.Estado == EstadoRegistro.Activo
                             && l.IdModulo == request.IdModulo
                             orderby l.Orden ascending
                             select new LeccionResponse
                             {
                                 IdLeccion = l.IdLeccion,
                                 IdModulo = l.IdModulo,
                                 Nombre = l.Nombre,
                                 Descripcion = l.Descripcion,
                                 NombreModulo = m.Nombre
                             }).ToList();

                return query;
            }
        }

        #endregion

        #region EventoInscripcion
        public Result ValidarFinalizacionEvento(InscripcionRequest request, UsuarioLogin user)
        {
            using (var context = new DBRContext())
            {
                Result result = new Result();
                List<CuestionarioTomatosValidar> ListValidar = new List<CuestionarioTomatosValidar>();
                var cuestionarios = (from e in context.Evento
                                     join m in context.Modulo on e.IdEvento equals m.IdEvento
                                     join l in context.Leccion on m.IdModulo equals l.IdModulo
                                     join c in context.Cuestionario on l.IdLeccion equals c.IdLeccion
                                     where e.Estado == EstadoRegistro.Activo
                                     && m.Estado == EstadoRegistro.Activo
                                     && l.Estado == EstadoRegistro.Activo
                                     && c.Estado == EstadoRegistro.Activo
                                     && l.Tipo == 3
                                     && e.IdEvento == request.IdEvento
                                     select new
                                     {
                                         c.IdCuestionario,
                                         m.IdModulo,
                                         PesoModulo = m.Peso,
                                         PesoLeccion = c.Peso,
                                         e.NotaAprobatoria
                                     }).ToList();

                //SI NO HAY EXAMENES CONFIGURADOS SOLO DEVUEVLE VERDADERO
                if (cuestionarios.Count() == 0)
                {
                    result.IsSuccess = true;
                    return result;
                }

                foreach (var item in cuestionarios)
                {
                    var cuestionariosTomados = (from e in context.Evento
                                                join m in context.Modulo on e.IdEvento equals m.IdEvento
                                                join l in context.Leccion on m.IdModulo equals l.IdModulo
                                                join c in context.Cuestionario on l.IdLeccion equals c.IdLeccion
                                                join ct in context.CuestionarioTomado on c.IdCuestionario equals ct.IdCuestionario
                                                where e.Estado == EstadoRegistro.Activo
                                                && m.Estado == EstadoRegistro.Activo
                                                && l.Estado == EstadoRegistro.Activo
                                                && c.Estado == EstadoRegistro.Activo
                                                && l.Tipo == 3
                                                && e.IdEvento == request.IdEvento
                                                && ct.IdUsuario == user.IdUsuario
                                                && ct.IdCuestionario == item.IdCuestionario
                                                orderby ct.Intento descending
                                                select ct).FirstOrDefault();
                    CuestionarioTomatosValidar validar = new CuestionarioTomatosValidar();
                    if (cuestionariosTomados == null)
                    {
                        result.IsSuccess = false;
                        result.Message = Message.FaltaExamentes;
                        return result;
                    }else
                    {
                        validar.IdModulo = item.IdModulo;
                        validar.PesoModulo = (int)item.PesoModulo;
                        validar.PesoLeccion = (int)item.PesoLeccion;
                        validar.Nota = cuestionariosTomados.Nota;
                        ListValidar.Add(validar);
                    }
                }
                decimal NotaModulo = 0;
                decimal PromedioFinal = 0;
                int PesoModulo = 0;
                var ListaModulos = ListValidar.Select(x => new { x.IdModulo, x.PesoModulo }).Distinct().ToList();
                foreach (var item0 in ListaModulos)
                {
                    var contador = ListValidar.Where(x => x.IdModulo == item0.IdModulo).ToList();
                    decimal Nota = 0;
                    decimal Promedio = 0;
                    int Peso = 0;
                    foreach (var item1 in contador)
                    {
                        Nota = Nota + item1.PesoLeccion * item1.Nota;
                        Peso = Peso + item1.PesoLeccion;
                    }
                    Promedio = Nota / Peso;
                    NotaModulo = NotaModulo + Promedio * item0.PesoModulo;
                    PesoModulo= PesoModulo+ item0.PesoModulo;
                }
                PromedioFinal = NotaModulo / PesoModulo;

                if (PromedioFinal >= cuestionarios[0].NotaAprobatoria)
                {
                    var Inscripcion = (from i in context.Inscripcion
                                       join e in context.Evento on i.IdEvento equals e.IdEvento
                                       join p in context.Persona on i.IdPersona equals p.IdPersona
                                       join u in context.Usuario on p.NumeroDocumento equals u.NumeroDocumento
                                       where i.Estado == EstadoRegistro.Activo
                                       && e.Estado == EstadoRegistro.Activo
                                       && p.Estado == EstadoRegistro.Activo
                                       && u.Estado == EstadoRegistro.Activo
                                       && e.IdEvento == request.IdEvento
                                       && u.IdUsuario == user.IdUsuario
                                       select i).FirstOrDefault();

                    Inscripcion.Nota = PromedioFinal;

                    var EventoUsuario = (from eu in context.EventoUsuario
                                         where eu.IdUsuario == user.IdUsuario
                                         && eu.IdEvento == request.IdEvento
                                         select eu).FirstOrDefault();

                    EventoUsuario.Abierto = false;
                    EventoUsuario.NotaFinal = PromedioFinal;

                    result.IsSuccess = context.SaveChanges() > 0;
                    result.ResultNota = EventoUsuario.NotaFinal;
                    return result;
                }else
                {
                    result.IsSuccess = false;
                    result.Message = Message.NoCumpleNota;
                    return result;
                }
            }
        }
        public Result CerrarEventoUsuario(InscripcionRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var EventoUsuario = (from eu in context.EventoUsuario
                                     where eu.IdUsuario == user.IdUsuario
                                     && eu.IdEvento == request.IdEvento
                                     && eu.Estado == EstadoRegistro.Activo
                                     select eu).FirstOrDefault();

                if (EventoUsuario.Abierto != null)
                {
                    if (!(bool)EventoUsuario.Abierto)
                    {
                        result.IsSuccess = false;
                        return result;
                    }
                }

                var Inscripcion = (from i in context.Inscripcion
                                   join e in context.Evento on i.IdEvento equals e.IdEvento
                                   join p in context.Persona on i.IdPersona equals p.IdPersona
                                   join u in context.Usuario on p.NumeroDocumento equals u.NumeroDocumento
                                   where i.Estado == EstadoRegistro.Activo
                                   && e.Estado == EstadoRegistro.Activo
                                   && p.Estado == EstadoRegistro.Activo
                                   && u.Estado == EstadoRegistro.Activo
                                   && e.IdEvento == request.IdEvento
                                   && u.IdUsuario == user.IdUsuario
                                   select i).FirstOrDefault();
             

                EventoUsuario.Abierto = false;
                EventoUsuario.NotaFinal = Inscripcion.Nota;
                EventoUsuario.FechaModificacion = DateTime.Now;
                EventoUsuario.UsuarioModificacion = user.IdUsuario;

                result.IsSuccess = context.SaveChanges() > 0;

                return result;
            }
        }
        public InscripcionResponse GetEventoInscripcion(InscripcionRequest request, UsuarioLogin user)
        {
            using (var context = new DBRContext())
            {
                var query = (from i in context.Inscripcion
                             join est in context.Tipo on i.EstadoPago equals est.Valor
                             join p in context.Persona on i.IdPersona equals p.IdPersona
                             join u in context.Usuario on p.NumeroDocumento equals u.NumeroDocumento
                             join ocu in context.Tipo on p.TipoOcupacion equals ocu.Valor
                             join mod in context.Tipo on i.TipoModalidad equals mod.Valor
                             join e in context.Evento on i.IdEvento equals e.IdEvento
                             join pf in context.Profesion on p.IdProfesion equals pf.IdProfesion
                             into left
                             from lefts in left.DefaultIfEmpty()
                             where i.Estado == EstadoRegistro.Activo
                             && p.Estado == EstadoRegistro.Activo
                             && u.Estado == EstadoRegistro.Activo
                             && est.Grupo == "ESTADO PAG"
                             && ocu.Grupo == "OCUPACION"
                             && mod.Grupo == "MODALIDAD"
                             && i.IdEvento == request.IdEvento
                             && u.IdUsuario == user.IdUsuario
                             select new InscripcionResponse
                             {
                                 IdInscripcion = i.IdInscripcion,
                                 IdEvento = (int)i.IdEvento,
                                 IdPersona = (int)i.IdPersona,
                                 EstadoPago = (int)i.EstadoPago,
                                 NombreEstadoPago = est.NombreTipo,
                                 TipoPago = (int)i.TipoPago,
                                 EntregaCertificado = (bool)i.EntregaCertificado,
                                 rowguid = i.rowguid.ToString(),
                                 TipoModalidad = i.TipoModalidad,
                                 Modalidad = mod.NombreTipo,
                                 Monto = i.Monto,
                                 NombreBanco = i.NombreBanco,
                                 FechaOperacion = i.FechaOperacion,
                                 NumeroOperacion = i.NumeroOperacion,
                                 NumeroCertificado = i.NumeroCertificado,
                                 Certificado = i.Certificado,
                                 Nota = i.Nota,
                                 TipoInscripcion = i.TipoInscripcion,
                                 //Persona
                                 Nombre = p.ApellidoPaterno + " " + p.ApellidoMaterno + ", " + p.Nombres,
                                 Nombres = p.Nombres,
                                 ApellidoPaterno = p.ApellidoPaterno,
                                 ApellidoMaterno = p.ApellidoMaterno,
                                 NumeroDocumento = p.NumeroDocumento,
                                 TipoOcupacion = (int)p.TipoOcupacion,
                                 DescripcionOcupacion = p.DescripcionOcupacion,
                                 TipoOcupacionAbreviatura = ocu.Abreviatura,
                                 CIP = p.CIP,
                                 Celular = p.Celular,
                                 Correo = p.Correo,
                                 IdProfesion = lefts.IdProfesion,
                                 DescripcionProfesion = lefts.Descripcion,
                                 //Evento
                                 NombreCertificado = e.DocumentoCertificado,
                                 NombreCertificadoImprimir = e.DocumentoCertificadoImprimir,
                                 NombreCertificadoExpositor = e.DocumentoCertificadoExpositor,
                                 DetallarCertificado = e.DetallarCertificado ?? false,
                                 GenerarCertificado = e.GenerarCertificado ?? false
                             });

                return query.FirstOrDefault();
            }

        }
        #endregion

        #region METODO AVAS CONSULTORES
        public List<EventoResponse> ListEventoActivoAvas()
        {
            using (var context = new DBRContext())
            {
                var query = (from e in context.Evento
                             where e.Estado == EstadoRegistro.Activo
                             && e.Activo == true
                             orderby e.Fecha ascending
                             select new EventoResponse
                             {
                                 IdEvento = e.IdEvento,
                                 NombreEvento = e.NombreEvento,
                                 Expositor = e.Expositor,
                                 Fecha = (DateTime)e.Fecha,
                                 Horas = (int)e.Horas,
                                 ImagenEvento = e.ImagenEvento,
                                 DocumentoFotocheck = e.DocumentoFotocheck,
                                 DocumentoCertificado = e.DocumentoCertificado,
                                 DocumentoCertificadoImprimir = e.DocumentoCertificadoImprimir,
                                 Activo = (bool)e.Activo
                             });

                return query.ToList();
            }
        }
        public EventoResponse GetEventoDetalleByRowIdAvast(EventoRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from e in context.Evento
                             join t in context.Tipo
                                 on new { Tipo = e.Tipo, TipoCurso = "TIPO CURSO" } equals new { Tipo = t.Valor, TipoCurso = t.Grupo } into ts
                             from t in ts.DefaultIfEmpty()
                             join t2 in context.Tipo.Where(x => x.Grupo == "MODALIDAD") on e.Modalidad equals t2.Valor
                             into ts2
                             from t2 in ts2.DefaultIfEmpty()
                             where e.Estado == EstadoRegistro.Activo
                             && e.rowid == request.rowid
                             && e.Activo == true
                             select new EventoResponse
                             {
                                 IdEvento = e.IdEvento,
                                 NombreEvento = e.NombreEvento,
                                 TipoCursoNombre = t != null ? t.NombreTipo : "",
                                 NombreModalidad = t2 != null ? t2.NombreTipo : "",
                                 Descripcion = e.Descripcion,
                                 Expositor = e.Expositor,
                                 Fecha = (DateTime)e.Fecha,
                                 Horas = (int)e.Horas,
                                 ImagenEvento = e.ImagenEvento,
                                 DocumentoFotocheck = e.DocumentoFotocheck,
                                 DocumentoCertificado = e.DocumentoCertificado,
                                 DocumentoCertificadoImprimir = e.DocumentoCertificadoImprimir,
                                 Costo = e.Costo,
                                 CostoValor = e.CostoValor,
                                 CostoValorPromocional = e.CostoValorPromocional,
                                 rowid = e.rowid.ToString()
                             }).FirstOrDefault();

                return query;

            }
        }
        #endregion

        #region METODOS 100% INGENIEROS
        public List<EventoResponse> ListUltimosEvento()
        {
            using (var context = new DBRContext())
            {
                var query = (from e in context.Evento
                             join t in context.Tipo
                                 on new { Tipo = e.Tipo, TipoCurso = "TIPO CURSO" } equals new { Tipo = t.Valor, TipoCurso = t.Grupo } into ts
                             from t in ts.DefaultIfEmpty()
                             where e.Estado == EstadoRegistro.Activo
                             && e.Activo == true
                             orderby e.Fecha descending
                             select new EventoResponse
                             {
                                 rowid = e.rowid.ToString(),
                                 IdEvento = e.IdEvento,
                                 NombreEvento = e.NombreEvento,
                                 TipoCursoNombre = t != null ? t.NombreTipo : "",
                                 Expositor = e.Expositor,
                                 Fecha = (DateTime)e.Fecha,
                                 Horas = (int)e.Horas,
                                 ImagenEvento = e.ImagenEvento,
                                 Activo = (bool)e.Activo,
                                 Costo = e.Costo,
                                 CostoValor = e.CostoValor,
                                 CantidadInscritos = (from i in context.Inscripcion where i.IdEvento == e.IdEvento && i.Estado == EstadoActivo.Activo select 1).Count()
                             }).Take(6).ToList();

                query = query.OrderByDescending(x => x.Fecha).ToList();

                return query;
            }
        }
        public EventoResponse GetEventoDetalleByRowId(EventoRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from e in context.Evento
                             join t in context.Tipo
                                 on new { Tipo = e.Tipo, TipoCurso = "TIPO CURSO" } equals new { Tipo = t.Valor, TipoCurso = t.Grupo } into ts
                             from t in ts.DefaultIfEmpty()
                             join t2 in context.Tipo.Where(x => x.Grupo == "MODALIDAD") on e.Modalidad equals t2.Valor
                             into ts2
                             from t2 in ts2.DefaultIfEmpty()
                             where e.Estado == EstadoRegistro.Activo
                             && e.rowid == request.rowid
                             && e.Activo == true
                             select new EventoResponse
                             {
                                 IdEvento = e.IdEvento,
                                 NombreEvento = e.NombreEvento,
                                 TipoCursoNombre = t != null ? t.NombreTipo : "",
                                 NombreModalidad = t2 != null ? t2.NombreTipo : "",
                                 Descripcion = e.Descripcion,
                                 Expositor = e.Expositor,
                                 Fecha = (DateTime)e.Fecha,
                                 Horas = (int)e.Horas,
                                 ImagenEvento = e.ImagenEvento,
                                 DocumentoFotocheck = e.DocumentoFotocheck,
                                 DocumentoCertificado = e.DocumentoCertificado,
                                 DocumentoCertificadoImprimir = e.DocumentoCertificadoImprimir,
                                 Costo = e.Costo,
                                 CostoValor = e.CostoValor,
                                 CostoValorPromocional = e.CostoValorPromocional,
                                 CantidadInscritos = (from i in context.Inscripcion where i.IdEvento == e.IdEvento && i.Estado == EstadoActivo.Activo select 1).Count(),
                                 CantidadLeciones = (from l in context.Leccion
                                                     join m in context.Modulo on l.IdModulo equals m.IdModulo
                                                     where l.Estado == EstadoActivo.Activo
                                                     && m.Estado == EstadoActivo.Activo
                                                     && m.IdEvento == e.IdEvento
                                                     select 1).Count(),
                                 rowid = e.rowid.ToString()
                             }).FirstOrDefault();

                return query;

            }
        }
        public List<ModuloWebResponse> GetModuloLeccionByIdEvento(EventoRequest request)
        {
            using (var context = new DBRContext())
            {
                var Modulos = (from m in context.Modulo
                               where m.Estado == EstadoRegistro.Activo
                               && m.IdEvento == request.IdEvento
                               select new ModuloWebResponse
                               {
                                   IdModulo = m.IdModulo,
                                   Nombre = m.Nombre
                               }).ToList();

                var Lecciones = (from l in context.Leccion
                                 join m in context.Modulo on l.IdModulo equals m.IdModulo
                                 join t in context.Tipo on l.Tipo equals t.Valor
                                 where l.Estado == EstadoActivo.Activo
                                 && m.Estado == EstadoActivo.Activo
                                 && m.IdEvento == request.IdEvento
                                 && t.Grupo == "TIPO LECCION"
                                 select new LeccionWebResponse
                                 {
                                     IdLeccion=l.IdLeccion,
                                     IdModulo=m.IdModulo,
                                     Nombre =l.Nombre,
                                     Duracion = l.Duracion,
                                     NombreTipo=t.NombreTipo,
                                     Orden = (int)l.Orden
                                 }).ToList();

                for (int i = 0; i < Modulos.Count(); i++)
                {
                    Modulos[i].Lecciones = Lecciones.Where(x => x.IdModulo == Modulos[i].IdModulo).OrderBy(x => x.Orden).ToList();
                }

                return Modulos;

            }
        }
        public List<DocenteResponse> LisDocentes()
        {
            using (var context = new DBRContext())
            {
                var query = (from m in context.Docente
                               where m.Estado == EstadoRegistro.Activo
                               select new DocenteResponse
                               {
                                   IdDocente = m.IdDocuente,
                                   Nombre = m.Nombre,
                                   NombreFoto = m.NombreFoto,
                                   Profesion=m.Profesion,
                                   RowId = m.rowid.ToString()
                               }).ToList();

                return query;
            }
        }
        public DocenteResponse GetDocenteByRowId(DocenteRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from m in context.Docente
                             where m.Estado == EstadoRegistro.Activo
                             && m.rowid == request.RowId
                             select new DocenteResponse
                             {
                                 IdDocente = m.IdDocuente,
                                 Nombre = m.Nombre,
                                 NombreFoto = m.NombreFoto,
                                 Profesion = m.Profesion,
                                 Especialista = m.Especialista,
                                 Detalle = m.Detalle,
                                 RowId = m.rowid.ToString()
                             }).FirstOrDefault();

                return query;
            }
        }
        //Cursos
        public List<EventoResponse> ListEventoFiltrado(List<int> IdsCategoria, List<int> IdsTema)
        {
            IdsCategoria = IdsCategoria ?? new List<int>();
            IdsTema = IdsTema ?? new List<int>();
            using (var context = new DBRContext())
            {
                var query = (from e in context.Evento
                             join tt in context.EventoTema on e.IdEvento  equals tt.IdEvento into tts
                             from tt in tts.DefaultIfEmpty()
                             join t in context.Tipo
                                 on new { e.Tipo, TipoCurso = "TIPO CURSO" } equals new { Tipo = t.Valor, TipoCurso = t.Grupo } into ts
                             from t in ts.DefaultIfEmpty()
                             join t2 in context.Tipo.Where(x => x.Grupo == "MODALIDAD") on e.Modalidad equals t2.Valor into ts2
                                from t2 in ts2.DefaultIfEmpty()
                             where e.Estado == EstadoRegistro.Activo
                             && e.Activo == true
                             && (IdsCategoria.Contains((int)e.Tipo) || IdsCategoria.Count == 0)
                             && (IdsTema.Contains((int)tt.TipoTema) || IdsTema.Count == 0)
                             orderby e.Fecha descending
                             select new EventoResponse
                             {
                                 rowid = e.rowid.ToString(),
                                 IdEvento = e.IdEvento,
                                 NombreEvento = e.NombreEvento,
                                 TipoCursoNombre = t != null ? t.NombreTipo : "",
                                 NombreModalidad = t2 != null ? t2.NombreTipo : "",
                                 Expositor = e.Expositor,
                                 Fecha = (DateTime)e.Fecha,
                                 Horas = (int)e.Horas,
                                 ImagenEvento = e.ImagenEvento,
                                 Activo = (bool)e.Activo,
                                 Costo = e.Costo,
                                 CostoValor = e.CostoValor,
                                 CostoValorPromocional = e.CostoValorPromocional,
                                 CantidadInscritos = (from i in context.Inscripcion where i.IdEvento == e.IdEvento && i.Estado == EstadoActivo.Activo select 1).Count(),
                             }).Distinct().ToList();

                query = query.OrderByDescending(x => x.Fecha).ToList();

                return query;
            }
        }
        //Participacion
        public List<EventoResponse> ListEventosPorPersona(PersonaRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from i in context.Inscripcion
                             join e in context.Evento on i.IdEvento equals e.IdEvento
                             join p in context.Persona on i.IdPersona equals p.IdPersona
                             where e.Estado == EstadoRegistro.Activo
                             && i.Estado == EstadoRegistro.Activo
                             && p.NumeroDocumento == request.NumeroDocumento
                             && i.EstadoPago == 1//Pagado
                             && i.Certificado != null
                             orderby e.Fecha ascending
                             select new EventoResponse
                             {
                                 IdEvento = e.IdEvento,
                                 IdInscripcion = i.IdInscripcion,
                                 IdPersona = p.IdPersona,
                                 NombreEvento = e.NombreEvento,
                                 Expositor = e.Expositor,
                                 Fecha = (DateTime)e.Fecha,
                                 Horas = (int)e.Horas,
                                 ImagenEvento = e.ImagenEvento,
                                 DocumentoFotocheck = e.DocumentoFotocheck,
                                 DocumentoCertificado = e.DocumentoCertificado,
                                 DocumentoCertificadoImprimir = e.DocumentoCertificadoImprimir,
                                 Activo = (bool)e.Activo,
                                 NombrePersona = p.Nombres + " " + p.ApellidoPaterno + " " + p.ApellidoMaterno,
                                 CetificadoFirmado = i.Certificado
                             }).ToList();

                return query;
            }
        }
        public List<EventoResponse> ListEventosPorCodigo(PersonaRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from i in context.Inscripcion
                             join e in context.Evento on i.IdEvento equals e.IdEvento
                             join p in context.Persona on i.IdPersona equals p.IdPersona
                             where e.Estado == EstadoRegistro.Activo
                             && i.Estado == EstadoRegistro.Activo
                             &&
                             (
                                (i.NumeroCertificado == 0 && (i.rowguid.ToString().Substring(0, 5) + i.IdInscripcion.ToString()).ToUpper() == request.NumeroDocumento) ||
                                (i.NumeroCertificado > 0)
                             )
                             && i.EstadoPago == 1//Pagado
                             && i.Certificado != null
                             orderby e.Fecha ascending
                             select new EventoResponse
                             {
                                 IdEvento = e.IdEvento,
                                 IdInscripcion = i.IdInscripcion,
                                 IdPersona = p.IdPersona,
                                 NombreEvento = e.NombreEvento,
                                 Expositor = e.Expositor,
                                 Fecha = (DateTime)e.Fecha,
                                 Horas = (int)e.Horas,
                                 ImagenEvento = e.ImagenEvento,
                                 DocumentoFotocheck = i.IdInscripcion.ToString() + i.IdEvento.ToString() + i.IdPersona.ToString(),
                                 DocumentoCertificado = e.DocumentoCertificado,
                                 DocumentoCertificadoImprimir = e.DocumentoCertificadoImprimir,
                                 Activo = (bool)e.Activo,
                                 NombrePersona = p.Nombres + " " + p.ApellidoPaterno + " " + p.ApellidoMaterno,
                                 CetificadoFirmado = i.Certificado
                             });
                var queryList = query.ToList();

                queryList = queryList.Where(x => x.DocumentoFotocheck.PadRight(16, '0') == request.NumeroDocumento).ToList();

                return queryList;
            }
        }
        #endregion
    }
}
