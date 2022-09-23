using DBR.Evento.Modelo;
using DBR.Evento.Modelo.Request;
using DBR.Evento.Modelo.Response;
using DBR.Eventos.Datos.Implementacion;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBR.Eventos.Negocio.Implementacion
{
    public class CuestionarioNegocio
    {
        CuestionarioData _cuestionarioDatos = new CuestionarioData(new DbContext("db_ingenierosEntities"));

        public CuestionarioResponse GetCuestionario(CuestionarioRequest request)
        {
            return _cuestionarioDatos.GetCuestionario(request);
        }

        public CuestionarioResponse GetCuestionarioCompleto(CuestionarioRequest request,UsuarioLogin user)
        {
            return _cuestionarioDatos.GetCuestionarioCompleto(request, user);
        }

        #region Preguntas

        public Paged<PreguntaResponse> ListPreguntaPaged(PageRequest page, PreguntaRequest request)
        {
            var response = _cuestionarioDatos.ListPreguntaPaged(page, request);
            return response;
        }
        public Result SavePregunta(PreguntaRequest request, UsuarioLogin user)
        {
            var response = _cuestionarioDatos.SavePregunta(request, user);
            return response;
        }
        public Result UpdatePregunta(PreguntaRequest request, UsuarioLogin user)
        {
            var response = _cuestionarioDatos.UpdatePregunta(request, user);
            return response;
        }
        public Result DeletePregunta(PreguntaRequest request, UsuarioLogin user)
        {
            var response = _cuestionarioDatos.DeletePregunta(request, user);
            return response;
        }
        public PreguntaResponse GetPregunta(PreguntaRequest request)
        {
            var response = _cuestionarioDatos.GetPregunta(request);
            return response;
        }

        public List<PreguntaResponse> ListPregunta(PreguntaRequest request)
        {
            return _cuestionarioDatos.ListPregunta(request);
        }

        #endregion

        public Result SaveEvaluacion(EvaluacionRequest request, UsuarioLogin user)
        {
            var cuestionario = _cuestionarioDatos.GetCuestionarioCompletoByCuestionario(new CuestionarioRequest { IdCuestionario = request.IdCuestionario });

            var nota = 0;
            foreach (var item in cuestionario.Preguntas)
            {
                var respuestaCorrecta = item.Respuestas.FirstOrDefault(r => r.EsCorrecta);
                var respuestaMarcada = request.Respuestas.FirstOrDefault(p => p.IdPregunta == item.IdPregunta);

                if (respuestaCorrecta.IdRespuesta == respuestaMarcada.IdRespuesta)
                {
                    nota += item.Puntaje;
                }
            }

            request.Nota = nota;

            var evaluacion = _cuestionarioDatos.GetEvaluacion(new EvaluacionRequest { IdCuestionario = request.IdCuestionario, IdUsuario = user.IdUsuario, IdEvento = request.IdEvento });

            if (evaluacion == null)
            {
                request.Intento = 1;
            }
            else
            {
                request.Intento = evaluacion.Intento + 1;
            }

            var response = _cuestionarioDatos.SaveEvaluacion(request, user);
            return response;
        }

        public EvaluacionResponse GetEvaluacion(EvaluacionRequest request)
        {
            var response = _cuestionarioDatos.GetEvaluacion(request);
            return response;
        }
    }
}
