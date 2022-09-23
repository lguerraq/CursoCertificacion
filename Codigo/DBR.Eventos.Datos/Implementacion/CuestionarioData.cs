using DBR.Evento.Modelo;
using DBR.Evento.Modelo.Request;
using DBR.Evento.Modelo.Response;
using DBR.Eventos.Comun;
using DBR.Eventos.Datos.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBR.Eventos.Datos.Implementacion
{
    public class CuestionarioData : BaseAcceso
    {
        public CuestionarioData(DbContext context) : base(context)
        {
        }

        //public Paged<EventoResponse> ListEvento(PageRequest page)
        //{
        //    using (var context = new DBRContext())
        //    {
        //        var query = (from e in context.Evento
        //                     where e.Estado == EstadoRegistro.Activo
        //                     select new EventoResponse
        //                     {
        //                         IdEvento = e.IdEvento,
        //                         NombreEvento = e.NombreEvento,
        //                         Expositor = e.Expositor,
        //                         Fecha = (DateTime)e.Fecha,
        //                         Horas = (int)e.Horas,
        //                         ImagenEvento = e.ImagenEvento,
        //                         DocumentoFotocheck = e.DocumentoFotocheck,
        //                         DocumentoCertificado = e.DocumentoCertificado,
        //                         DocumentoCertificadoImprimir = e.DocumentoCertificadoImprimir,
        //                         DocumentoCertificadoExpositor = e.DocumentoCertificadoExpositor,
        //                         Activo = (bool)e.Activo,
        //                         Costo = e.Costo
        //                     });

        //        return Paginate(page, query, q => q.Fecha);
        //    }
        //}
        //public List<ComboResponse> ListEventoCombo()
        //{
        //    using (var context = new DBRContext())
        //    {
        //        var query = (from e in context.Evento
        //                     where e.Estado == EstadoRegistro.Activo
        //                     && e.Activo == true
        //                     orderby e.Fecha ascending
        //                     select new ComboResponse
        //                     {
        //                         Value = e.IdEvento.ToString(),
        //                         Descripcion = e.NombreEvento
        //                     });

        //        return query.ToList();
        //    }
        //}
        //public List<ComboResponse> ListAllEventoCombo()
        //{
        //    using (var context = new DBRContext())
        //    {
        //        var query = (from e in context.Evento
        //                     where e.Estado == EstadoRegistro.Activo
        //                     orderby e.Fecha ascending
        //                     select new ComboResponse
        //                     {
        //                         Value = e.IdEvento.ToString(),
        //                         Descripcion = e.NombreEvento
        //                     });

        //        return query.ToList();
        //    }
        //}
        //public List<EventoResponse> ListEventoActivos()
        //{
        //    using (var context = new DBRContext())
        //    {
        //        var query = (from e in context.Evento
        //                     where e.Estado == EstadoRegistro.Activo
        //                     && e.Activo == true
        //                     orderby e.Fecha ascending
        //                     select new EventoResponse
        //                     {
        //                         IdEvento = e.IdEvento,
        //                         NombreEvento = e.NombreEvento,
        //                         Expositor = e.Expositor,
        //                         Fecha = (DateTime)e.Fecha,
        //                         Horas = (int)e.Horas,
        //                         ImagenEvento = e.ImagenEvento,
        //                         DocumentoFotocheck = e.DocumentoFotocheck,
        //                         DocumentoCertificado = e.DocumentoCertificado,
        //                         DocumentoCertificadoImprimir = e.DocumentoCertificadoImprimir,
        //                         Activo = (bool)e.Activo
        //                     });

        //        return query.ToList();
        //    }
        //}
        //public List<EventoResponse> ListAllEvento()
        //{
        //    using (var context = new DBRContext())
        //    {
        //        var query = (from e in context.Evento
        //                     where e.Estado == EstadoRegistro.Activo
        //                     orderby e.Fecha ascending
        //                     select new EventoResponse
        //                     {
        //                         IdEvento = e.IdEvento,
        //                         NombreEvento = e.NombreEvento,
        //                         Expositor = e.Expositor,
        //                         Fecha = (DateTime)e.Fecha,
        //                         Horas = (int)e.Horas,
        //                         ImagenEvento = e.ImagenEvento,
        //                         DocumentoFotocheck = e.DocumentoFotocheck,
        //                         DocumentoCertificado = e.DocumentoCertificado,
        //                         DocumentoCertificadoImprimir = e.DocumentoCertificadoImprimir,
        //                         Activo = (bool)e.Activo
        //                     });

        //        return query.ToList();
        //    }
        //}
        public Result SaveCuestionario(CuestionarioRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = new Cuestionario();
                obj.Nombre = request.Nombre;
                obj.Descripcion = request.Descripcion;
                obj.Estado = EstadoRegistro.Activo;
                obj.UsuarioCreacion = user.IdUsuario;
                obj.FechaCreacion = DateTime.Now;

                context.Cuestionario.Add(obj);

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
        public Result UpdateCuestionario(CuestionarioRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = context.Cuestionario.Find(request.IdCuestionario);
                obj.Nombre = request.Nombre;
                obj.Descripcion = request.Descripcion;
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
        //public Result UpdateCursoFiles(EventoRequest request, UsuarioLogin user)
        //{
        //    Result result = new Result();
        //    using (var context = new DBRContext())
        //    {
        //        var obj = context.Evento.Find(request.IdEvento);

        //        if (request.ImagenEvento != null && request.ImagenEvento != "")
        //            obj.ImagenEvento = request.ImagenEvento;
        //        if (request.DocumentoFotocheck != null && request.DocumentoFotocheck != "")
        //            obj.DocumentoFotocheck = request.DocumentoFotocheck;
        //        if (request.DocumentoCertificado != null && request.DocumentoCertificado != "")
        //            obj.DocumentoCertificado = request.DocumentoCertificado;
        //        if (request.DocumentoCertificadoImprimir != null && request.DocumentoCertificadoImprimir != "")
        //            obj.DocumentoCertificadoImprimir = request.DocumentoCertificadoImprimir;
        //        if (request.DocumentoCertificadoExpositor != null && request.DocumentoCertificadoExpositor != "")
        //            obj.DocumentoCertificadoExpositor = request.DocumentoCertificadoExpositor;

        //        obj.UsuarioModificacion = user.IdUsuario;
        //        obj.FechaModificacion = DateTime.Now;

        //        if (context.SaveChanges() > 0)
        //        {
        //            result.IsSuccess = true;
        //            result.Message = Message.ExitoActualizar;
        //        }
        //        else
        //        {
        //            result.IsSuccess = false;
        //            result.Message = Message.ErrorActualizar;
        //        };
        //        return result;
        //    }
        //}
        public Result DeleteCuestionario(CuestionarioRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                //TODO: Validar que no tenga preguntas
                //var query = (from i in context.Inscripcion
                //             where i.IdEvento == request.IdEvento
                //             select i).ToList();
                //if (query.Count > 0)
                //{
                //    result.IsSuccess = false;
                //    result.Message = Message.ExistePersonas;
                //}

                var obj = context.Cuestionario.Find(request.IdCuestionario);
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
        //public Result UpdateEstadoEvento(EventoRequest request, UsuarioLogin user)
        //{
        //    Result result = new Result();
        //    using (var context = new DBRContext())
        //    {
        //        var obj = context.Evento.Find(request.IdEvento);

        //        obj.Activo = request.Activo;
        //        obj.UsuarioModificacion = user.IdUsuario;
        //        obj.FechaModificacion = DateTime.Now;

        //        if (context.SaveChanges() > 0)
        //        {
        //            result.IsSuccess = true;
        //            result.Message = Message.ExitoActualizar;
        //        }
        //        else
        //        {
        //            result.IsSuccess = false;
        //            result.Message = Message.ErrorActualizar;
        //        };
        //        return result;
        //    }
        //}
        public CuestionarioResponse GetCuestionario(CuestionarioRequest request)
        {
            using (var context = new DBRContext())
            {
                var query = (from e in context.Cuestionario
                             where e.Estado == EstadoRegistro.Activo
                             && e.IdLeccion == request.IdLeccion
                             select new CuestionarioResponse
                             {
                                 IdCuestionario = e.IdCuestionario,
                                 Nombre = e.Nombre,
                                 Descripcion = e.Descripcion,
                                 Peso = e.Peso ?? 0
                             }).FirstOrDefault();

                return query;
            }
        }

        public CuestionarioResponse GetCuestionarioCompleto(CuestionarioRequest request, UsuarioLogin user)
        {
            using (var context = new DBRContext())
            {               

                var cuestionario = (from e in context.Cuestionario
                             where e.Estado == EstadoRegistro.Activo
                             && e.IdLeccion == request.IdLeccion
                             select new CuestionarioResponse
                             {
                                 IdCuestionario = e.IdCuestionario,
                                 Nombre = e.Nombre,
                                 Descripcion = e.Descripcion,
                                 Peso = e.Peso ?? 0
                             }).FirstOrDefault();

                var cuestionariosTomados = (from l in context.Leccion
                                            join c in context.Cuestionario on l.IdLeccion equals c.IdLeccion
                                            join ct in context.CuestionarioTomado on c.IdCuestionario equals ct.IdCuestionario
                                            where l.Estado == EstadoRegistro.Activo
                                            && c.Estado == EstadoRegistro.Activo
                                            && l.Tipo == 3
                                            && l.IdLeccion == request.IdLeccion
                                            && ct.IdUsuario == user.IdUsuario
                                            && ct.IdCuestionario == cuestionario.IdCuestionario
                                            orderby ct.Intento descending
                                            select ct).FirstOrDefault();
                
                var UltimoCuestionario = 0;
                if (cuestionariosTomados != null)
                {
                    UltimoCuestionario = cuestionariosTomados.IdCuestionarioTomado;
                }

                var cuestionariosTomadoRestuesta = (from l in context.Leccion
                                                    join c in context.Cuestionario on l.IdLeccion equals c.IdLeccion
                                                    join ct in context.CuestionarioTomado on c.IdCuestionario equals ct.IdCuestionario
                                                    join ctr in context.CuestionarioRespuesta on ct.IdCuestionarioTomado equals ctr.IdCuestionarioTomado
                                                    where l.Estado == EstadoRegistro.Activo
                                                    && c.Estado == EstadoRegistro.Activo
                                                    && l.Tipo == 3
                                                    && l.IdLeccion == request.IdLeccion
                                                    && ct.IdUsuario == user.IdUsuario
                                                    && ct.IdCuestionario == cuestionario.IdCuestionario
                                                    && ct.IdCuestionarioTomado == UltimoCuestionario
                                                    select ctr).ToList();

                var preguntasRespuestas = (from p in context.Pregunta
                                           join r in context.Respuesta
                                           on p.IdPregunta equals r.IdPregunta
                                           where p.IdCuestionario == cuestionario.IdCuestionario
                                           && p.Estado == EstadoActivo.Activo
                                           orderby p.IdPregunta
                                           select new PreguntaRespuestaResponse
                                           {
                                               IdPregunta = p.IdPregunta,
                                               Nombre = p.Nombre,
                                               Ayuda = p.Ayuda,
                                               Puntaje = p.Puntaje,
                                               Tipo = p.Tipo,
                                               IdRespuesta = r.IdRespuesta,
                                               Descripcion = r.Descripcion,
                                               EsCorrecta = r.EsCorrecta
                                           }).ToList();

                var listaPreguntas = new List<PreguntaResponse>();

                foreach (var item in preguntasRespuestas)
                {
                    if (!listaPreguntas.Any(p => p.IdPregunta == item.IdPregunta))
                    {
                        var pregunta = new PreguntaResponse
                        {
                            IdPregunta = item.IdPregunta,
                            Nombre = item.Nombre,
                            Ayuda = item.Ayuda,
                            Tipo = item.Tipo
                        };

                        pregunta.Respuestas = preguntasRespuestas.Where(p => p.IdPregunta == pregunta.IdPregunta).Select(r => new RespuestaResponse { 
                            IdPregunta = r.IdPregunta, 
                            IdRespuesta = r.IdRespuesta, 
                            Descripcion = r.Descripcion, 
                            EsCorrecta = r.EsCorrecta,
                            EsRespondida = cuestionariosTomadoRestuesta.Where(x => x.IdPregunta == r.IdPregunta && x.IdRespuesta == r.IdRespuesta).Count() > 0
                        }).ToList();

                        var respuestaCorrect = pregunta.Respuestas.Where(x => x.EsCorrecta == true).FirstOrDefault();
                        pregunta.EsRespondidaCorrecta = cuestionariosTomadoRestuesta.Where(x => x.IdPregunta == respuestaCorrect.IdPregunta && x.IdRespuesta == respuestaCorrect.IdRespuesta).Count() > 0;

                        listaPreguntas.Add(pregunta);
                    }
                }

                cuestionario.Preguntas = listaPreguntas;

                return cuestionario;
            }
        }

        public CuestionarioResponse GetCuestionarioCompletoByCuestionario(CuestionarioRequest request)
        {
            using (var context = new DBRContext())
            {
                var cuestionario = (from e in context.Cuestionario
                                    where e.Estado == EstadoRegistro.Activo
                                    && e.IdCuestionario == request.IdCuestionario
                                    select new CuestionarioResponse
                                    {
                                        IdCuestionario = e.IdCuestionario,
                                        Nombre = e.Nombre,
                                        Descripcion = e.Descripcion,
                                        Peso = e.Peso ?? 0
                                    }).FirstOrDefault();

                var preguntasRespuestas = (from p in context.Pregunta
                                           join r in context.Respuesta
                                           on p.IdPregunta equals r.IdPregunta
                                           where p.IdCuestionario == cuestionario.IdCuestionario
                                           && p.Estado == true
                                           orderby p.IdPregunta
                                           select new
                                           {
                                               p.IdPregunta,
                                               p.Nombre,
                                               p.Ayuda,
                                               p.Puntaje,
                                               p.Tipo,
                                               r.IdRespuesta,
                                               r.Descripcion,
                                               r.EsCorrecta
                                           }).ToList();

                var listaPreguntas = new List<PreguntaResponse>();

                foreach (var item in preguntasRespuestas)
                {
                    if (!listaPreguntas.Any(p => p.IdPregunta == item.IdPregunta))
                    {
                        var pregunta = new PreguntaResponse
                        {
                            IdPregunta = item.IdPregunta,
                            Nombre = item.Nombre,
                            Ayuda = item.Ayuda,
                            Tipo = item.Tipo,
                            Puntaje = item.Puntaje
                        };

                        pregunta.Respuestas = preguntasRespuestas.Where(p => p.IdPregunta == pregunta.IdPregunta).Select(r => new RespuestaResponse { IdRespuesta = r.IdRespuesta, Descripcion = r.Descripcion, EsCorrecta = r.EsCorrecta }).ToList();

                        listaPreguntas.Add(pregunta);
                    }
                }

                cuestionario.Preguntas = listaPreguntas;

                return cuestionario;
            }
        }

        public Paged<PreguntaResponse> ListPreguntaPaged(PageRequest page, PreguntaRequest request)
        {
            Paged<PreguntaResponse> obj = new Paged<PreguntaResponse>();
            using (var context = new DBRContext())
            {
                page.search.value = page.search.value == null ? "" : page.search.value;

                var p0 = new SqlParameter { ParameterName = "IdCuestionario", Value = request.IdCuestionario, SqlDbType = SqlDbType.Int };
                var p1 = new SqlParameter { ParameterName = "search", Value = page.search.value, SqlDbType = SqlDbType.VarChar };
                var p2 = new SqlParameter { ParameterName = "start", Value = page.start, SqlDbType = SqlDbType.Int };
                var p3 = new SqlParameter { ParameterName = "length", Value = page.length, SqlDbType = SqlDbType.Int };

                var response = context.Database.SqlQuery<PreguntaResponse>("dbo.UspListPreguntaByCuestionarioPaged @IdEvento, @search, @start, @length", p0, p1, p2, p3).ToList();
                var Total = response.Count() == 0 ? 0 : response[0].TotalRegistros;

                obj.data = response;
                obj.recordsTotal = Total;
                obj.recordsFiltered = Total;
                return obj;
            }
        }
        public Result SavePregunta(PreguntaRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = new Pregunta();
                obj.IdCuestionario = request.IdCuestionario;
                obj.Tipo = request.Tipo;
                obj.Nombre = request.Nombre;
                obj.Explicacion = request.Explicacion;
                obj.Ayuda = request.Ayuda;
                obj.Puntaje = request.Puntaje;
                obj.Estado = EstadoRegistro.Activo;
                obj.UsuarioCreacion = user.IdUsuario;
                obj.FechaCreacion = DateTime.Now;

                if (request.Respuestas != null)
                {
                    foreach (var item in request.Respuestas)
                    {
                        obj.Respuesta.Add(new Respuesta
                        {
                            Descripcion = item.Descripcion,
                            EsCorrecta = item.Selected,
                            Estado = EstadoRegistro.Activo,
                            UsuarioCreacion = user.IdUsuario,
                            FechaCreacion = DateTime.Now
                        });
                    }
                }

                context.Pregunta.Add(obj);

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoGuardar;

                return result;
            }
        }
        public Result UpdatePregunta(PreguntaRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = (from m in context.Pregunta
                           where m.Estado == EstadoRegistro.Activo
                           && m.IdPregunta == request.IdPregunta
                           select m).FirstOrDefault();

                obj.Nombre = request.Nombre;
                obj.Explicacion = request.Explicacion;
                obj.Ayuda = request.Ayuda;
                obj.Puntaje = request.Puntaje;
                obj.UsuarioModificacion = user.IdUsuario;
                obj.FechaModificacion = DateTime.Now;

                if (request.Respuestas != null)
                {
                    var respuestas = context.Respuesta.Where(r => r.IdPregunta == request.IdPregunta);

                    foreach (var item in respuestas)
                    {
                        if (request.Respuestas.Find(r => r.IdRespuesta == item.IdRespuesta) == null)
                        {
                            item.Estado = EstadoRegistro.Inactivo;
                            context.Respuesta.AddOrUpdate(item);
                        }
                    }

                    foreach (var item in request.Respuestas)
                    {
                        if (item.IdRespuesta == 0)
                        {
                            var respuesta = new Respuesta
                            {
                                IdPregunta = request.IdPregunta,
                                Descripcion = item.Descripcion,
                                EsCorrecta = item.Selected,
                                Estado = EstadoRegistro.Activo,
                                UsuarioCreacion = user.IdUsuario,
                                FechaCreacion = DateTime.Now
                            };

                            context.Respuesta.Add(respuesta);
                        }
                        else
                        {
                            var respuesta = context.Respuesta.Find(item.IdRespuesta);
                            respuesta.Descripcion = item.Descripcion;
                            respuesta.EsCorrecta = item.Selected;
                            respuesta.UsuarioModificacion = user.IdUsuario;
                            respuesta.FechaModificacion = DateTime.Now;

                            context.Respuesta.AddOrUpdate(respuesta);
                        }
                    }

                }

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoActualizar;

                return result;
            }
        }
        public Result DeletePregunta(PreguntaRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = (from m in context.Pregunta
                           where m.Estado == EstadoRegistro.Activo
                           && m.IdPregunta == request.IdPregunta
                           select m).FirstOrDefault();

                obj.Estado = EstadoRegistro.Inactivo;
                obj.UsuarioModificacion = user.IdUsuario;
                obj.FechaModificacion = DateTime.Now;

                result.IsSuccess = context.SaveChanges() > 0;
                result.Message = Message.ExitoEliminar;

                return result;
            }
        }
        public PreguntaResponse GetPregunta(PreguntaRequest request)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = (from m in context.Pregunta
                           where m.Estado == EstadoRegistro.Activo
                           && m.IdPregunta == request.IdPregunta
                           select new PreguntaResponse
                           {
                               IdPregunta = m.IdPregunta,
                               IdCuestionario = m.IdCuestionario,
                               Tipo = m.Tipo,
                               Nombre = m.Nombre,
                               Explicacion = m.Explicacion,
                               Ayuda = m.Ayuda,
                               Puntaje = m.Puntaje
                           }).FirstOrDefault(); ;

                var respuestas = (from r in context.Respuesta
                                  where r.IdPregunta == request.IdPregunta
                                  && r.Estado == EstadoRegistro.Activo
                                  select new RespuestaResponse
                                  {
                                      IdRespuesta = r.IdRespuesta,
                                      Descripcion = r.Descripcion,
                                      EsCorrecta = r.EsCorrecta
                                  }).ToList();

                obj.Respuestas = respuestas;
                return obj;
            }
        }
        public List<PreguntaResponse> ListPregunta(PreguntaRequest request)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var query = (from p in context.Pregunta
                             join c in context.Cuestionario on p.IdCuestionario equals c.IdCuestionario
                             where p.Estado == EstadoRegistro.Activo
                             && p.IdCuestionario == request.IdCuestionario
                             orderby p.IdPregunta ascending
                             select new PreguntaResponse
                             {
                                 IdPregunta = p.IdPregunta,
                                 IdCuestionario = p.IdCuestionario,
                                 Nombre = p.Nombre,
                                 Explicacion = p.Explicacion,
                                 Ayuda = p.Ayuda,
                                 Puntaje = p.Puntaje,
                             }).ToList();

                return query;
            }
        }

        //Respuesta
        //public Paged<RespuestaResponse> ListRespuestaPaged(PageRequest page, RespuestaRequest request)
        //{
        //    Paged<LeccionResponse> obj = new Paged<LeccionResponse>();
        //    using (var context = new DBRContext())
        //    {
        //        page.search.value = page.search.value == null ? "" : page.search.value;

        //        var p0 = new SqlParameter { ParameterName = "IdModulo", Value = request.IdModulo, SqlDbType = SqlDbType.Int };
        //        var p1 = new SqlParameter { ParameterName = "search", Value = page.search.value, SqlDbType = SqlDbType.VarChar };
        //        var p2 = new SqlParameter { ParameterName = "start", Value = page.start, SqlDbType = SqlDbType.Int };
        //        var p3 = new SqlParameter { ParameterName = "length", Value = page.length, SqlDbType = SqlDbType.Int };

        //        var response = context.Database.SqlQuery<LeccionResponse>("dbo.UspListLeccionByModuloPaged @IdModulo, @search, @start, @length", p0, p1, p2, p3).ToList();
        //        var Total = response.Count() == 0 ? 0 : response[0].TotalRegistros;

        //        obj.data = response;
        //        obj.recordsTotal = Total;
        //        obj.recordsFiltered = Total;
        //        return obj;
        //    }
        //}

        public Result SaveRespuesta(RespuestaRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = new Respuesta();
                obj.IdPregunta = request.IdPregunta;
                obj.Descripcion = request.Descripcion;
                obj.EsCorrecta = request.Selected;
                obj.Estado = EstadoRegistro.Activo;
                obj.UsuarioCreacion = user.IdUsuario;
                obj.FechaCreacion = DateTime.Now;

                context.Respuesta.Add(obj);

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

        public Result UpdateRespuesta(RespuestaRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = context.Respuesta.Find(request.IdRespuesta);
                obj.Descripcion = request.Descripcion;
                obj.EsCorrecta = request.Selected;
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

        public Result DeleteRespuesta(RespuestaRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = context.Respuesta.Find(request.IdRespuesta);
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

        public RespuestaResponse GetRespuesta(RespuestaRequest request)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var obj = (from l in context.Respuesta
                           where l.Estado == EstadoRegistro.Activo
                           && l.IdRespuesta == request.IdRespuesta
                           select new RespuestaResponse
                           {
                               IdRespuesta = l.IdRespuesta,
                               IdPregunta = l.IdPregunta,
                               EsCorrecta = l.EsCorrecta,
                               Descripcion = l.Descripcion,
                           }).FirstOrDefault();

                return obj;
            }
        }

        public List<RespuestaResponse> ListRespuesta(RespuestaRequest request)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var query = (from r in context.Respuesta
                             join p in context.Pregunta on r.IdPregunta equals p.IdPregunta
                             where r.Estado == EstadoRegistro.Activo
                             && r.IdPregunta == request.IdPregunta
                             orderby r.IdPregunta ascending
                             select new RespuestaResponse
                             {
                                 IdRespuesta = r.IdRespuesta,
                                 IdPregunta = r.IdPregunta,
                                 Descripcion = r.Descripcion,
                                 EsCorrecta = r.EsCorrecta,
                                 NombrePregunta = p.Nombre
                             }).ToList();

                return query;
            }
        }

        public Result SaveEvaluacion(EvaluacionRequest request, UsuarioLogin user)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var Inscripcion = (from i in context.Inscripcion
                                   where i.Estado == EstadoRegistro.Activo
                                   && i.IdPersona == user.IdPersona
                                   && i.IdEvento == request.IdEvento
                                   select i).FirstOrDefault();

                if (Inscripcion != null)
                {
                    Inscripcion.Nota = request.Nota;
                    Inscripcion.FechaModificacion = DateTime.Now;
                    Inscripcion.UsuarioCreacion = user.IdUsuario;

                    context.SaveChanges();
                }

                var evaluacion = new CuestionarioTomado
                {
                    IdCuestionario = request.IdCuestionario,
                    IdUsuario = user.IdUsuario,
                    Nota = request.Nota,
                    Intento = request.Intento,
                    Estado = EstadoRegistro.Activo,
                    FechaCreacion = DateTime.Now,
                    UsuarioCreacion = user.IdUsuario
                };

                context.CuestionarioTomado.Add(evaluacion);

                if (context.SaveChanges() > 0)
                {
                    foreach (var item in request.Respuestas)
                    {
                        var cuestionarioRespuesta = new CuestionarioRespuesta
                        {
                            IdCuestionarioTomado = evaluacion.IdCuestionarioTomado,
                            IdPregunta = item.IdPregunta,
                            IdRespuesta = item.IdRespuesta,
                            Estado = EstadoRegistro.Activo,
                            FechaCreacion = DateTime.Now,
                            UsuarioCreacion = user.IdUsuario
                        };

                        context.CuestionarioRespuesta.Add(cuestionarioRespuesta);
                    }
                 
                    context.SaveChanges();
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

        public EvaluacionResponse GetEvaluacion(EvaluacionRequest request)
        {
            Result result = new Result();
            using (var context = new DBRContext())
            {
                var EventoUsuario = (from eu in context.EventoUsuario
                                     where eu.IdEvento == request.IdEvento
                                     && eu.IdUsuario == request.IdUsuario
                                     && eu.Estado == EstadoRegistro.Activo
                                     select eu).FirstOrDefault();

                var obj = (from c in context.CuestionarioTomado
                           where c.Estado == EstadoRegistro.Activo
                           && c.IdCuestionario == request.IdCuestionario
                           && c.IdUsuario == request.IdUsuario
                           orderby c.Intento descending
                           select new EvaluacionResponse
                           {
                               IdCuestionario = c.IdCuestionario,
                               Nota = c.Nota,
                               Intento = c.Intento,
                               Abierto = EventoUsuario.Abierto ?? true
                           }).FirstOrDefault();

                return obj;
            }
        }
    }
}
