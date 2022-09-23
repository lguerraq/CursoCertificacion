using System.Data.Entity;
using DBR.Eventos.Datos.Base;
using DBR.Evento.Modelo;
using DBR.Evento.Modelo.Request;
using System;
using DBR.Eventos.Comun;
using System.Linq;
using System.Collections.Generic;
using DBR.Evento.Modelo.Response;

namespace DBR.Eventos.Datos.Implementacion
{
    public class InscripcionDatos : BaseAcceso
    {
        public InscripcionDatos(DbContext context) : base(context)
        {
        }

        #region METODOS DEL SISTEMA ELERNIG
        public Result SaveInscripcion(InscripcionRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context=new DBRContext())
            {
                var insc = (from i in context.Inscripcion
                            where i.IdPersona == request.IdPersona
                            && i.Estado == EstadoRegistro.Activo
                            && i.IdEvento == request.IdEvento
                            select i.IdInscripcion).ToList();

                if (insc.Count > 0)
                {
                    result.IsSuccess = false;
                    result.Message = Message.ExistePersonaInscrita;
                    return result;
                }

                var evento = (from e in context.Evento
                              where e.IdEvento == request.IdEvento
                              && e.Estado == EstadoRegistro.Activo
                              select e.Costo).FirstOrDefault();

                var obj = new Inscripcion();
                obj.IdEvento = request.IdEvento;
                obj.IdPersona = request.IdPersona;
                obj.EstadoPago = request.EstadoPago;
                obj.TipoPago = request.TipoPago;
                obj.EntregaCertificado = false;
                obj.TipoModalidad = request.TipoModalidad;
                obj.Monto = request.Monto;
                obj.NombreBanco = request.NombreBanco;
                obj.FechaOperacion = request.FechaOperacion;
                obj.NumeroOperacion = request.NumeroOperacion;
                obj.NumeroCertificado = 1;//IdInscripcionIdEventoIdPersona
                obj.Nota = request.Nota;
                obj.TipoInscripcion = request.TipoInscripcion;
                obj.Ruc = request.Ruc;
                obj.Estado = EstadoRegistro.Activo;
                obj.UsuarioCreacion = user.IdUsuario;
                obj.FechaCreacion = DateTime.Now;
                obj.rowguid = Guid.NewGuid();

                context.Inscripcion.Add(obj);

                if (context.SaveChanges() > 0)
                {
                    result.IsSuccess = true;
                    result.Informacion = evento;
                    result.Codigo = obj.IdInscripcion;
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
        public Result UpdateInscripcion(InscripcionRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var insc = (from i in context.Inscripcion
                            where i.IdPersona == request.IdPersona
                            && i.Estado == EstadoRegistro.Activo
                            && i.IdEvento == request.IdEvento
                            && i.IdInscripcion != request.IdInscripcion
                            select i.IdInscripcion).ToList();

                if (insc.Count > 0)
                {
                    result.IsSuccess = false;
                    result.Message = Message.ExistePersonaInscrita;
                    return result;
                }

                var obj = context.Inscripcion.Find(request.IdInscripcion);
                obj.IdPersona = request.IdPersona;
                obj.EstadoPago = request.EstadoPago;
                obj.TipoPago = request.TipoPago;
                obj.TipoModalidad = request.TipoModalidad;
                obj.Monto = request.Monto;
                obj.NombreBanco = request.NombreBanco;
                obj.FechaOperacion = request.FechaOperacion;
                obj.NumeroOperacion = request.NumeroOperacion;
                obj.Nota = request.Nota;
                obj.TipoInscripcion = request.TipoInscripcion;
                obj.Ruc = request.Ruc;
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
        public Result DeleteInscripcion(InscripcionRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = context.Inscripcion.Find(request.IdInscripcion);

                obj.Estado = EstadoRegistro.Inactivo;
                obj.UsuarioModificacion = user.IdUsuario;
                obj.FechaModificacion = DateTime.Now;

                var EventoUsuaro = (from eu in context.EventoUsuario
                                    where eu.IdEvento == request.IdEvento
                                    && eu.IdUsuario == user.IdUsuario
                                    && eu.Estado == EstadoRegistro.Activo
                                    select eu).ToList();

                for (int i = 0; i < EventoUsuaro.Count; i++)
                {
                    EventoUsuaro[i].Estado = EstadoRegistro.Inactivo;
                    EventoUsuaro[i].UsuarioModificacion = user.IdUsuario;
                    EventoUsuaro[i].FechaModificacion = DateTime.Now;
                }

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
        public Paged<InscripcionResponse> ListInscripcion(PageRequest page, InscripcionRequest request)
        {
            using (var context=new DBRContext())
            {
                var query = (from i in context.Inscripcion
                             join est in context.Tipo on i.EstadoPago equals est.Valor
                             join tpa in context.Tipo on i.TipoPago equals tpa.Valor
                             join p in context.Persona on i.IdPersona equals p.IdPersona
                             join ocu in context.Tipo on p.TipoOcupacion equals ocu.Valor
                             join mod in context.Tipo on i.TipoModalidad equals mod.Valor
                             join e in context.Evento on i.IdEvento equals e.IdEvento
                             join pf in context.Profesion on p.IdProfesion equals pf.IdProfesion
                                into left from lefts in left.DefaultIfEmpty()
                             join u in context.Usuario.Where(x => x.Estado == EstadoRegistro.Activo) on p.NumeroDocumento equals u.NumeroDocumento
                                into ut from uts in ut.DefaultIfEmpty()
                             where i.Estado == EstadoRegistro.Activo
                             && p.Estado == EstadoRegistro.Activo
                             && est.Grupo == "ESTADO PAG"
                             && tpa.Grupo == "TIPO PAGO"
                             && ocu.Grupo == "OCUPACION"
                             && mod.Grupo == "MODALIDAD"
                             && i.IdEvento == request.IdEvento
                             && (
                                p.Nombres.Contains(page.search.value) ||
                                p.ApellidoPaterno.Contains(page.search.value) ||
                                p.ApellidoMaterno.Contains(page.search.value) ||
                                p.NumeroDocumento.Contains(page.search.value) ||
                                est.NombreTipo.Contains(page.search.value) ||
                                tpa.NombreTipo.Contains(page.search.value)
                             )
                             select new InscripcionResponse
                             {
                                 IdInscripcion=i.IdInscripcion,
                                 IdEvento=(int)i.IdEvento,
                                 IdPersona=(int)i.IdPersona,
                                 EstadoPago=(int)i.EstadoPago,
                                 NombreEstadoPago=est.NombreTipo,
                                 TipoPago = (int)i.TipoPago,
                                 NombreTipoPago = tpa.NombreTipo,
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
                                 GenerarCertificado = e.GenerarCertificado ?? false,
                                 //Usuario
                                 IdUsuario = uts.IdUsuario
                             });
                return PaginateAlt(page, query);
            }
            
        }
        public List<InscripcionResponse> ListAllInscripcion(InscripcionRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from i in context.Inscripcion
                             join est in context.Tipo on i.EstadoPago equals est.Valor
                             join tpa in context.Tipo on i.TipoPago equals tpa.Valor
                             join p in context.Persona on i.IdPersona equals p.IdPersona
                             join ocu in context.Tipo on p.TipoOcupacion equals ocu.Valor
                             join mod in context.Tipo on i.TipoModalidad equals mod.Valor
                             join e in context.Evento on i.IdEvento equals e.IdEvento
                             join pf in context.Profesion on p.IdProfesion equals pf.IdProfesion
                             into left
                             from lefts in left.DefaultIfEmpty()
                             join ps in context.Pais on p.IdPais equals ps.IdPais
                             into left2
                             from lefts2 in left2.DefaultIfEmpty()
                             where i.Estado == EstadoRegistro.Activo
                             && p.Estado == EstadoRegistro.Activo
                             && est.Grupo == "ESTADO PAG"
                             && tpa.Grupo == "TIPO PAGO"
                             && ocu.Grupo == "OCUPACION"
                             && mod.Grupo == "MODALIDAD"
                             && i.IdEvento == request.IdEvento
                             orderby p.ApellidoPaterno ascending
                             select new InscripcionResponse
                             {
                                 IdInscripcion = i.IdInscripcion,
                                 IdEvento = (int)i.IdEvento,
                                 IdPersona = (int)i.IdPersona,
                                 EstadoPago = (int)i.EstadoPago,
                                 NombreEstadoPago = est.NombreTipo,
                                 TipoPago = (int)i.TipoPago,
                                 NombreTipoPago = tpa.NombreTipo,
                                 EntregaCertificado = (bool)i.EntregaCertificado,
                                 rowguid = i.rowguid.ToString(),
                                 TipoModalidad = i.TipoModalidad,
                                 Modalidad = mod.NombreTipo,
                                 Monto = i.Monto,
                                 NombreBanco = i.NombreBanco,
                                 FechaOperacion = i.FechaOperacion,
                                 NumeroOperacion = i.NumeroOperacion,
                                 //Persona
                                 Nombres = p.Nombres,
                                 ApellidoPaterno = p.ApellidoPaterno,
                                 ApellidoMaterno = p.ApellidoMaterno,
                                 NumeroDocumento = p.NumeroDocumento,
                                 TipoOcupacion = (int)p.TipoOcupacion,
                                 TipoOcupacionNombre = ocu.NombreTipo,
                                 DescripcionOcupacion = p.DescripcionOcupacion,
                                 TipoOcupacionAbreviatura = ocu.Abreviatura,
                                 CIP = p.CIP,
                                 Celular = p.Celular,
                                 Correo = p.Correo,
                                 IdProfesion = lefts.IdProfesion,
                                 DescripcionProfesion = lefts.Descripcion,
                                 NombrePais=lefts2.NombrePais,
                                 Ciudad = p.Ciudad,
                                 //Evento
                                 FechaRegistro = i.FechaCreacion,
                                 NombreCertificado = e.DocumentoCertificado,
                                 NombreCertificadoImprimir = e.DocumentoCertificadoImprimir,
                                 NombreFotocheck = e.DocumentoFotocheck
                             });
                return query.ToList();
            }

        }
        public List<InscripcionResponse> GetInscripcion(InscripcionRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from i in context.Inscripcion
                             join est in context.Tipo on i.EstadoPago equals est.Valor
                             join tpa in context.Tipo on i.TipoPago equals tpa.Valor
                             join p in context.Persona on i.IdPersona equals p.IdPersona
                             join e in context.Evento on i.IdEvento equals e.IdEvento
                             join mod in context.Tipo on i.TipoModalidad equals mod.Valor
                             where i.Estado == EstadoRegistro.Activo
                             && est.Grupo == "ESTADO PAG"
                             && tpa.Grupo == "TIPO PAGO"
                             && mod.Grupo == "MODALIDAD"
                             && i.IdInscripcion == request.IdInscripcion
                             select new InscripcionResponse
                             {
                                 IdInscripcion = i.IdInscripcion,
                                 IdEvento = (int)i.IdEvento,
                                 IdPersona = (int)i.IdPersona,
                                 EstadoPago = (int)i.EstadoPago,
                                 NombreEstadoPago = est.NombreTipo,
                                 TipoPago = (int)i.TipoPago,
                                 NombreTipoPago = tpa.NombreTipo,
                                 EntregaCertificado = (bool)i.EntregaCertificado,
                                 TipoModalidad = i.TipoModalidad,
                                 Modalidad = mod.NombreTipo,
                                 Monto = i.Monto,
                                 NombreBanco = i.NombreBanco,
                                 FechaOperacion = i.FechaOperacion,
                                 NumeroOperacion = i.NumeroOperacion,
                                 Nota = i.Nota,
                                 TipoInscripcion = i.TipoInscripcion,
                                 Ruc = i.Ruc,
                                 //Persona
                                 Nombres = p.Nombres,
                                 ApellidoPaterno = p.ApellidoPaterno,
                                 ApellidoMaterno = p.ApellidoMaterno,
                                 NumeroDocumento = p.NumeroDocumento,
                                 TipoOcupacion = (int)p.TipoOcupacion,
                                 DescripcionOcupacion = p.DescripcionOcupacion,
                                 CIP = p.CIP,
                                 Celular = p.Celular,
                                 Correo = p.Correo,
                                 IdProfesion = p.IdProfesion,
                                 IdPais = p.IdPais,
                                 Ciudad = p.Ciudad,
                                 //Evento
                                 NombreCertificado = e.DocumentoCertificado,
                                 NombreFotocheck = e.DocumentoFotocheck
                             });
                return query.ToList();
            }

        }
        public Result UpdateEntregaCertificadoInscripcion(InscripcionRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = context.Inscripcion.Find(request.IdInscripcion);
                obj.EntregaCertificado = true;
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
        public Result UpdateInscripcionCertificado(InscripcionRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {

                var obj = context.Inscripcion.Find(request.IdInscripcion);
                obj.Certificado = request.Certificado;
                obj.UsuarioModificacion = user.IdUsuario;
                obj.FechaModificacion = DateTime.Now;

                if (context.SaveChanges() > 0)
                {
                    result.IsSuccess = true;
                    result.Message = Message.ExitoCargarCertificado;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = Message.ErrorActualizar;
                };
                return result;
            }
        }
        #endregion

        #region METODOS 100% INGENIEROS
        public EstadisticaResponse GetEstadisticas()
        {
            using (var context = new DBRContext())
            {
                var resultado = new EstadisticaResponse();
                resultado.Inscripciones = (from i in context.Inscripcion where i.Estado == EstadoRegistro.Activo select 1).Count();
                resultado.Certificaciones = (from i in context.Inscripcion where i.Estado == EstadoRegistro.Activo && i.Certificado != null select 1).Count();
                resultado.Eventos = (from i in context.Evento where i.Estado == EstadoRegistro.Activo select 1).Count();
                resultado.Estudiantes = (from i in context.Persona where i.Estado == EstadoRegistro.Activo  select 1).Count() - (from i in context.Desafiliado where i.Estado == EstadoRegistro.Activo select 1).Count(); ;
                return resultado;
            }

        }
        public Result SaveInscripcionWeb(InscripcionRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {

                var insc = (from i in context.Inscripcion
                            where i.IdPersona == request.IdPersona
                            && i.Estado == EstadoRegistro.Activo
                            && i.IdEvento == request.IdEvento
                            select i.IdInscripcion).ToList();

                if (insc.Count > 0)
                {
                    result.IsSuccess = false;
                    result.Message = Message.ExistePersonaInscrita;
                    return result;
                }

                var evento = (from e in context.Evento
                              where e.IdEvento == request.IdEvento
                              && e.Estado == EstadoRegistro.Activo
                              select e.Costo).FirstOrDefault();

                var obj = new Inscripcion();
                obj.IdEvento = request.IdEvento;
                obj.IdPersona = request.IdPersona;
                obj.EstadoPago = request.EstadoPago;
                obj.TipoPago = request.TipoPago;
                obj.EntregaCertificado = false;
                obj.TipoModalidad = request.TipoModalidad;
                obj.Monto = request.Monto;
                obj.NombreBanco = request.NombreBanco;
                obj.FechaOperacion = request.FechaOperacion;
                obj.NumeroOperacion = request.NumeroOperacion;
                obj.NumeroCertificado = 1;//IdInscripcionIdEventoIdPersona
                obj.Nota = request.Nota;
                obj.TipoInscripcion = request.TipoInscripcion;
                obj.Ruc = request.Ruc;
                obj.Estado = EstadoRegistro.Activo;
                obj.UsuarioCreacion = user.IdUsuario;
                obj.FechaCreacion = DateTime.Now;
                obj.rowguid = Guid.NewGuid();

                context.Inscripcion.Add(obj);

                var obj1 = new EventoUsuario();
                obj1.IdEvento = request.IdEvento;
                obj1.IdUsuario = user.IdUsuario;
                obj1.FechaInicio = DateTime.Now;
                obj1.FechaFin = DateTime.Now.AddMonths(6);
                obj1.Estado = EstadoRegistro.Activo;
                obj1.UsuarioCreacion = user.IdUsuario;
                obj1.FechaCreacion = DateTime.Now;

                context.EventoUsuario.Add(obj1);

                if (context.SaveChanges() > 0)
                {
                    result.IsSuccess = true;
                    result.Informacion = evento;
                    result.Codigo = obj.IdInscripcion;
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
        public Result SavePreInscripcion(InscripcionRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {

                var insc = (from i in context.Inscripcion
                            where i.IdPersona == request.IdPersona
                            && i.Estado == EstadoRegistro.Activo
                            && i.IdEvento == request.IdEvento
                            select i.IdInscripcion).ToList();

                if (insc.Count > 0)
                {
                    result.IsSuccess = false;
                    result.Message = Message.ExistePersonaInscrita;
                    return result;
                }

                var evento = (from e in context.Evento
                              where e.IdEvento == request.IdEvento
                              && e.Estado == EstadoRegistro.Activo
                              select e.Costo).FirstOrDefault();

                var obj = new Inscripcion();
                obj.IdEvento = request.IdEvento;
                obj.IdPersona = request.IdPersona;
                obj.EstadoPago = request.EstadoPago;
                obj.TipoPago = request.TipoPago;
                obj.EntregaCertificado = false;
                obj.TipoModalidad = request.TipoModalidad;
                obj.Monto = request.Monto;
                obj.NombreBanco = request.NombreBanco;
                obj.FechaOperacion = request.FechaOperacion;
                obj.NumeroOperacion = request.NumeroOperacion;
                obj.NumeroCertificado = 1;//IdInscripcionIdEventoIdPersona
                obj.Nota = request.Nota;
                obj.TipoInscripcion = request.TipoInscripcion;
                obj.Ruc = request.Ruc;
                obj.Estado = EstadoRegistro.Activo;
                obj.UsuarioCreacion = user.IdUsuario;
                obj.FechaCreacion = DateTime.Now;
                obj.rowguid = Guid.NewGuid();

                context.Inscripcion.Add(obj);

                if (context.SaveChanges() > 0)
                {
                    result.IsSuccess = true;
                    result.Informacion = evento;
                    result.Codigo = obj.IdInscripcion;
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
        public Result UpdateInscripcionWeb(InscripcionRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var insc = (from i in context.Inscripcion
                            where i.IdPersona == request.IdPersona
                            && i.Estado == EstadoRegistro.Activo
                            && i.IdEvento == request.IdEvento
                            && i.IdInscripcion != request.IdInscripcion
                            select i.IdInscripcion).ToList();

                if (insc.Count > 0)
                {
                    result.IsSuccess = false;
                    result.Message = Message.ExistePersonaInscrita;
                    return result;
                }

                var obj = context.Inscripcion.Find(request.IdInscripcion);
                obj.IdPersona = request.IdPersona;
                obj.EstadoPago = request.EstadoPago;
                obj.TipoPago = request.TipoPago;
                obj.TipoModalidad = request.TipoModalidad;
                obj.Monto = request.Monto;
                obj.NombreBanco = request.NombreBanco;
                obj.FechaOperacion = request.FechaOperacion;
                obj.NumeroOperacion = request.NumeroOperacion;
                obj.Nota = request.Nota;
                obj.TipoInscripcion = request.TipoInscripcion;
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
        public bool ExisteInscripcionByIdUsuario(InscripcionRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from i in context.Inscripcion
                             where i.Estado == EstadoRegistro.Activo
                             && i.IdEvento == request.IdEvento
                             && i.IdPersona==request.IdPersona
                             select i).FirstOrDefault();
                return query != null;
            }
        }
        #endregion
    }
}
