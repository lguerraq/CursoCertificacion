using System.Data.Entity;
using DBR.Eventos.Datos.Base;
using DBR.Evento.Modelo;
using DBR.Evento.Modelo.Request;
using DBR.Eventos.Comun;
using System;
using DBR.Evento.Modelo.Response;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using Tanneryd.BulkOperations.EF6;
using Tanneryd.BulkOperations.EF6.Model;
using System.Data;

namespace DBR.Eventos.Datos.Implementacion
{
    public class PersonaDatos : BaseAcceso
    {
        public PersonaDatos(DbContext context) : base(context)
        {
        }
        public Paged<PersonaResponse> ListPersonaPaged(PageRequest page, PersonaRequest request)
        {
            Paged<PersonaResponse> obj = new Paged<PersonaResponse>();

            using (var context = new DBRContext())
            {               
                var p1 = new SqlParameter { ParameterName = "search", Value = page.search.value, SqlDbType = SqlDbType.VarChar };
                var p2 = new SqlParameter { ParameterName = "start", Value = page.start, SqlDbType = SqlDbType.Int };
                var p3 = new SqlParameter { ParameterName = "length", Value = page.length, SqlDbType = SqlDbType.Int };

                var response = context.Database.SqlQuery<PersonaResponse>("dbo.UspListPersonaPaged @search, @start, @length", p1, p2, p3).ToList();
                var Total = response.Count() == 0 ? 0 : response[0].TotalRegistros;

                obj.data = response;
                obj.recordsTotal = Total;
                obj.recordsFiltered = Total;
                return obj;
            }

        }
        public List<PersonaResponse> ListAllPersona()
        {
            using (var context = new DBRContext())
            {
                var query = (from p in context.Persona
                             join ocu in context.Tipo on p.TipoOcupacion equals ocu.Valor
                             join pr in context.Profesion on p.IdProfesion equals pr.IdProfesion into left
                             from lefts in left.DefaultIfEmpty()
                             join ps in context.Pais on p.IdPais equals ps.IdPais into left2
                             from lefts2 in left2.DefaultIfEmpty()
                             where p.Estado == EstadoRegistro.Activo
                             && ocu.Grupo == "OCUPACION"
                             select new PersonaResponse
                             {
                                 IdPersona = p.IdPersona,
                                 Nombres = p.Nombres,
                                 ApellidoPaterno = p.ApellidoPaterno,
                                 ApellidoMaterno = p.ApellidoMaterno,
                                 NumeroDocumento = p.NumeroDocumento == null ? "" : p.NumeroDocumento,
                                 TipoOcupacion = (int)p.TipoOcupacion,
                                 DescripcionOcupacion = p.DescripcionOcupacion,
                                 TipoOcupacionNombre = ocu.NombreTipo,
                                 TipoOcupacionAbreviatura = ocu.Abreviatura,
                                 CIP = p.CIP == null ? "" : p.CIP,
                                 Celular = p.Celular,
                                 Correo = p.Correo == null ? "" : p.Correo,
                                 IdProfesion = lefts.IdProfesion,
                                 DescripcionProfesion = lefts.Descripcion,
                                 NombrePais=lefts2.NombrePais,
                                 Ciudad=p.Ciudad
                             });

                return query.ToList();
            }

        }
        public Result SavePersona(PersonaRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var personaDni = (from p in context.Persona
                                  where p.NumeroDocumento == request.NumeroDocumento
                                  && p.Estado == EstadoRegistro.Activo
                                  select p.IdPersona).ToList();

                if (personaDni.Count > 0)
                {
                    result.IsSuccess = false;
                    result.Message = Message.ExistePersonaDni;
                    return result;
                }

                var obj = new Persona();
                obj.Nombres = request.Nombres;
                obj.ApellidoPaterno = request.ApellidoPaterno;
                obj.ApellidoMaterno = request.ApellidoMaterno;
                obj.NumeroDocumento = request.NumeroDocumento;
                obj.TipoOcupacion = request.TipoOcupacion;
                obj.DescripcionOcupacion = request.DescripcionOcupacion;
                obj.CIP = request.CIP;
                obj.Celular = request.Celular;
                obj.Correo = request.Correo;
                obj.IdProfesion = request.IdProfesion;
                obj.IdPais = request.IdPais;
                obj.Ciudad = request.Ciudad;
                obj.Estado = EstadoRegistro.Activo;
                obj.UsuarioCreacion = user.IdUsuario;
                obj.FechaCreacion = DateTime.Now;

                context.Persona.Add(obj);

                if (context.SaveChanges() > 0)
                {
                    result.IsSuccess = true;
                    result.Message = Message.ExitoGuardar;
                    result.Codigo = obj.IdPersona;
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = Message.ErrorGuardar;
                };
                return result;
            }
        }
        public Result SavePersonaMasivo(List<PersonaRequest> request, UsuarioLogin user)
        {
            Result result = new Result();
            var context = new DBRContext();
            using (var transac = context.Database.BeginTransaction())
            {
                try
                {
                    var entitiesBulkInsertPersonas = new List<Persona>();

                    foreach (var item in request)
                    {
                        Persona obj = new Persona();
                        obj.Nombres = item.Nombres;
                        obj.ApellidoPaterno = item.ApellidoPaterno;
                        obj.ApellidoMaterno = item.ApellidoMaterno;
                        obj.NumeroDocumento = item.NumeroDocumento;
                        obj.TipoOcupacion = item.TipoOcupacion;
                        obj.DescripcionOcupacion = item.DescripcionOcupacion;
                        obj.CIP = item.CIP;
                        obj.Celular = item.Celular;
                        obj.Correo = item.Correo;
                        obj.IdProfesion = item.IdProfesion;
                        obj.Estado = EstadoRegistro.Activo;
                        obj.UsuarioCreacion = user.IdUsuario;
                        obj.FechaCreacion = DateTime.Now;

                        entitiesBulkInsertPersonas.Add(obj);
                    }

                    var t = context.Database.CurrentTransaction.UnderlyingTransaction as SqlTransaction;

                    var responsePersonas = context.BulkInsertAll(
                        new BulkInsertRequest<Persona>
                        {
                            Entities = entitiesBulkInsertPersonas,
                            Recursive = true,
                            Transaction = t
                        });

                    transac.Commit();
                    result.IsSuccess = true;
                    result.Message = Message.ExitoGuardar;
                }
                catch (Exception ex)
                {
                    transac.Rollback();
                    result.IsSuccess = false;
                    result.Message = Message.ErrorNoControlado;
                    result.MessageExeption = ex.Message;
                    result.StackTrace = ex.StackTrace;
                }
                return result;
            }
        }
        public Result UpdatePersona(PersonaRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var personaDni = (from p in context.Persona
                                  where p.NumeroDocumento == request.NumeroDocumento
                                  && p.Estado == EstadoRegistro.Activo
                                  && p.IdPersona != request.IdPersona
                                  select p.IdPersona).ToList();
                if (personaDni.Count > 0)
                {
                    result.IsSuccess = false;
                    result.Message = Message.ExistePersonaDni;
                    return result;
                }

                var obj = context.Persona.Find(request.IdPersona);
                obj.Nombres = request.Nombres;
                obj.ApellidoPaterno = request.ApellidoPaterno;
                obj.ApellidoMaterno = request.ApellidoMaterno;
                obj.NumeroDocumento = request.NumeroDocumento;
                obj.TipoOcupacion = request.TipoOcupacion;
                obj.DescripcionOcupacion = request.DescripcionOcupacion;
                obj.CIP = request.CIP;
                obj.Celular = request.Celular;
                obj.Correo = request.Correo;
                obj.IdProfesion = request.IdProfesion;
                obj.IdPais = request.IdPais;
                obj.Ciudad = request.Ciudad;
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
        public Result UpdatePersonaXdni(PersonaRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {

                var obj = (from p in context.Persona
                           where p.NumeroDocumento == request.NumeroDocumento
                           && p.Estado == EstadoRegistro.Activo
                           select p).FirstOrDefault();

                obj.Nombres = request.Nombres;
                obj.ApellidoPaterno = request.ApellidoPaterno;
                obj.ApellidoMaterno = request.ApellidoMaterno;
                obj.NumeroDocumento = request.NumeroDocumento;
                obj.TipoOcupacion = request.TipoOcupacion;
                obj.DescripcionOcupacion = request.DescripcionOcupacion;
                obj.CIP = request.CIP;
                obj.Celular = request.Celular;
                obj.Correo = request.Correo;
                obj.IdProfesion = request.IdProfesion;
                obj.IdPais = request.IdPais;
                obj.Ciudad = request.Ciudad;
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
        public Result UpdatePersonaXdniMasivo(List<PersonaRequest> request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                foreach (var item in request)
                {
                    var obj = (from p in context.Persona
                               where p.NumeroDocumento == item.NumeroDocumento
                               && p.Estado == EstadoRegistro.Activo
                               select p).FirstOrDefault();

                    obj.Nombres = item.Nombres;
                    obj.ApellidoPaterno = item.ApellidoPaterno;
                    obj.ApellidoMaterno = item.ApellidoMaterno;
                    obj.NumeroDocumento = item.NumeroDocumento;
                    obj.TipoOcupacion = item.TipoOcupacion;
                    obj.DescripcionOcupacion = item.DescripcionOcupacion;
                    obj.CIP = item.CIP;
                    obj.Celular = item.Celular;
                    obj.Correo = item.Correo;
                    obj.IdProfesion = item.IdProfesion;
                    obj.UsuarioModificacion = user.IdUsuario;
                    obj.FechaModificacion = DateTime.Now;
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
        public Result DeletePersona(PersonaRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {

                var ins = (from i in context.Inscripcion
                           where i.IdPersona == request.IdPersona
                           && i.Estado == EstadoRegistro.Activo
                           select i.IdInscripcion).ToList();

                if (ins.Count() > 0)
                {
                    result.IsSuccess = false;
                    result.Message = Message.ExistePersonaInscripcion;
                    return result;
                }

                var obj = context.Persona.Find(request.IdPersona);
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
        public PersonaResponse GetPersona(PersonaRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from p in context.Persona
                             where p.Estado == EstadoRegistro.Activo
                             && p.IdPersona == request.IdPersona
                             select new PersonaResponse
                             {
                                 Nombres = p.Nombres,
                                 NumeroDocumento = p.NumeroDocumento,
                                 ApellidoPaterno = p.ApellidoPaterno,
                                 ApellidoMaterno = p.ApellidoMaterno,
                                 TipoOcupacion = (int)p.TipoOcupacion,
                                 DescripcionOcupacion = p.DescripcionOcupacion,
                                 CIP = p.CIP,
                                 Celular = p.Celular,
                                 Correo = p.Correo,
                                 IdProfesion = p.IdProfesion
                             });
                return query.FirstOrDefault();
            }
        }
        public List<PersonaResponse> GetPersonaXdni(PersonaRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from p in context.Persona
                             where p.Estado == EstadoRegistro.Activo
                             && p.NumeroDocumento == request.NumeroDocumento
                             select new PersonaResponse
                             {
                                 IdPersona = p.IdPersona,
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
                                 Ciudad = p.Ciudad
                             });
                return query.ToList();
            }
        }
        public List<PersonaResponse> GetPersonaXcip(PersonaRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from p in context.Persona
                             where p.Estado == EstadoRegistro.Activo
                             && p.CIP == request.CIP
                             select new PersonaResponse
                             {
                                 IdPersona = p.IdPersona,
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
                                 Ciudad = p.Ciudad
                             });
                return query.ToList();
            }
        }
        public List<PersonaResponse> ListAllPersonaCorreos()
        {
            using (var context = new DBRContext())
            {
                var desafiliador = (from c in context.Desafiliado
                                    where c.Estado == EstadoActivo.Activo
                                    select c.Correo).ToList();


                var query = (from p in context.Persona
                             where p.Estado == EstadoRegistro.Activo
                             && p.Correo != null && p.Correo.Trim() != "-" && p.Correo.Trim() != ""
                             && !desafiliador.Contains(p.Correo)
                             select new PersonaResponse
                             {
                                 IdPersona = p.IdPersona,
                                 Nombres = p.Nombres,
                                 ApellidoPaterno = p.ApellidoPaterno,
                                 ApellidoMaterno = p.ApellidoMaterno,
                                 NumeroDocumento = p.NumeroDocumento,
                                 TipoOcupacion = (int)p.TipoOcupacion,
                                 CIP = p.CIP,
                                 Celular = p.Celular,
                                 Correo = p.Correo
                             }).ToList();
                return query.ToList().OrderBy(x => x.ApellidoPaterno).ToList();
            }
        }
        public List<PersonaResponse> ListAllPersonaCorreosFaltantes(CorreoRequest request)
        {
            using (var context = new DBRContext())
            {
                var desafiliador = (from c in context.Desafiliado
                                    where c.Estado == EstadoActivo.Activo
                                    select c.Correo).ToList();

                var CorreosEnviados = (from cd in context.CorreoDifusion
                                       join c in context.Correo.Where(x => x.NumeroEnvio == x.NumeroEnvio) on cd.IdCorreo equals c.IdCorreo
                                       where c.Estado == EstadoRegistro.Activo
                                       && c.IdCorreo == request.IdCorreo
                                       select cd.Correo).ToList();

                var query = (from p in context.Persona
                             where p.Estado == EstadoRegistro.Activo
                             && p.Correo != null && p.Correo.Trim() != "-" && p.Correo.Trim() != ""
                             && !desafiliador.Contains(p.Correo)
                             select new PersonaResponse
                             {
                                 IdPersona = p.IdPersona,
                                 Nombres = p.Nombres,
                                 ApellidoPaterno = p.ApellidoPaterno,
                                 ApellidoMaterno = p.ApellidoMaterno,
                                 NumeroDocumento = p.NumeroDocumento,
                                 TipoOcupacion = (int)p.TipoOcupacion,
                                 CIP = p.CIP,
                                 Celular = p.Celular,
                                 Correo = p.Correo
                             }).ToList();

                var response = query.Where(x => !CorreosEnviados.Contains(x.Correo)).ToList();

                return response.ToList().OrderBy(x => x.ApellidoPaterno).ToList();
            }
        }
        public List<PersonaResponse> ListAllPersonaCorreosPorProfesion(CorreoRequest request, List<int?> IdsProfesion)
        {
            using (var context = new DBRContext())
            {
                var desafiliador = (from c in context.Desafiliado
                                    where c.Estado == EstadoActivo.Activo
                                    select c.Correo).ToList();

                var query = (from p in context.Persona
                             where p.Estado == EstadoRegistro.Activo
                             && p.Correo != null && p.Correo.Trim() != "-" && p.Correo.Trim() != ""
                             && !desafiliador.Contains(p.Correo)
                             && IdsProfesion.Contains(p.IdProfesion)
                             select new PersonaResponse
                             {
                                 IdPersona = p.IdPersona,
                                 Nombres = p.Nombres,
                                 ApellidoPaterno = p.ApellidoPaterno,
                                 ApellidoMaterno = p.ApellidoMaterno,
                                 NumeroDocumento = p.NumeroDocumento,
                                 TipoOcupacion = (int)p.TipoOcupacion,
                                 CIP = p.CIP,
                                 Celular = p.Celular,
                                 Correo = p.Correo
                             }).ToList();

                return query.ToList().OrderBy(x => x.ApellidoPaterno).ToList();
            }
        }
    }
}
