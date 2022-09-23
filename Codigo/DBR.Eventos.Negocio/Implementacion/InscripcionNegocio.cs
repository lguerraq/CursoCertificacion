using DBR.Evento.Modelo;
using DBR.Evento.Modelo.Request;
using DBR.Evento.Modelo.Response;
using DBR.Eventos.Comun;
using DBR.Eventos.Datos.Implementacion;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace DBR.Eventos.Negocio.Implementacion
{
    public class InscripcionNegocio
    {

        InscripcionDatos _inscripcionDatos = new InscripcionDatos(new DbContext("db_ingenierosEntities"));
        PersonaDatos _personaDatos = new PersonaDatos(new DbContext("db_ingenierosEntities"));
        public InscripcionNegocio()
        {

        }
        #region METODOS DEL SISTEMA ELERNIG
        public Result SaveInscripcion(InscripcionRequest request1, PersonaRequest request2, UsuarioLogin user)
        {
            try
            {
                var persona = _personaDatos.GetPersonaXdni(request2);
                if (persona.Count == 0)
                {
                    var result1 = _personaDatos.SavePersona(request2, user);
                    request1.IdPersona = result1.Codigo;
                }
                else
                {
                    var result2 = _personaDatos.UpdatePersonaXdni(request2, user);
                    request1.IdPersona = persona[0].IdPersona;
                }

                var response = _inscripcionDatos.SaveInscripcion(request1, user);
                return response;
            }
            catch (Exception ex)
            {
                Result result = new Result();
                result.Message = Message.ErrorNoControlado;
                result.MessageExeption = ex.Message;
                return result;
            }
            
        }       
        public Result UpdateInscripcion(InscripcionRequest request1, PersonaRequest request2, UsuarioLogin user)
        {
            try
            {
                var persona = _personaDatos.GetPersonaXdni(request2);
                if (persona.Count == 0)
                {
                    var result1 = _personaDatos.SavePersona(request2, user);
                    request1.IdPersona = result1.Codigo;
                }
                else
                {
                    var result2 = _personaDatos.UpdatePersonaXdni(request2, user);
                    request1.IdPersona = persona[0].IdPersona;
                }

                var response = _inscripcionDatos.UpdateInscripcion(request1, user);
                return response;
            }
            catch (Exception ex)
            {
                Result result = new Result();
                result.Message = Message.ErrorNoControlado;
                result.MessageExeption = ex.Message;
                return result;
            }
            
        }
        public Result DeleteInscripcion(InscripcionRequest request, UsuarioLogin user)
        {
            try
            {
                var response = _inscripcionDatos.DeleteInscripcion(request, user);
                return response;
            }
            catch (Exception ex)
            {
                Result result = new Result();
                result.Message = Message.ErrorNoControlado;
                result.MessageExeption = ex.Message;
                return result;
            }
            
        }
        public Paged<InscripcionResponse> ListInscripcion(PageRequest page, InscripcionRequest request)
        {
            var response = _inscripcionDatos.ListInscripcion(page, request);
            return response;
        }
        public List<InscripcionResponse> ListAllInscripcion(InscripcionRequest request)
        {
            var response = _inscripcionDatos.ListAllInscripcion(request);
            return response;
        }
        public List<InscripcionResponse> GetInscripcion(InscripcionRequest request)
        {
            var response = _inscripcionDatos.GetInscripcion(request);
            return response;
        }
        public Result UpdateEntregaCertificadoInscripcion(InscripcionRequest request, UsuarioLogin user)
        {
            try
            {
                var response = _inscripcionDatos.UpdateEntregaCertificadoInscripcion(request, user);
                return response;
            }
            catch (Exception ex)
            {
                Result result = new Result();
                result.Message = Message.ErrorNoControlado;
                result.MessageExeption = ex.Message;
                return result;
            }
        }
        public Result UpdateInscripcionCertificado(InscripcionRequest request, UsuarioLogin user)
        {           
            try
            {
                var response = _inscripcionDatos.UpdateInscripcionCertificado(request, user);
                return response;
            }
            catch (Exception ex)
            {
                Result result = new Result();
                result.Message = Message.ErrorNoControlado;
                result.MessageExeption = ex.Message;
                return result;
            }
        }
        #endregion

        #region METODOS 100% INGENIEROS
        public EstadisticaResponse GetEstadisticas()
        {
            var response = _inscripcionDatos.GetEstadisticas();
            return response;
        }
        public Result SaveInscripcionWeb(InscripcionRequest request1, UsuarioLogin user)
        {
            try
            {
                var response = _inscripcionDatos.SaveInscripcionWeb(request1, user);
                return response;
            }
            catch (Exception ex)
            {
                Result result = new Result();
                result.Message = Message.ErrorNoControlado;
                result.MessageExeption = ex.Message;
                return result;
            }

        }
        public Result SavePreInscripcion(InscripcionRequest request, UsuarioLogin user)
        {
            try
            {
                var response = _inscripcionDatos.SavePreInscripcion(request, user);
                return response;
            }
            catch (Exception ex)
            {
                Result result = new Result();
                result.Message = Message.ErrorNoControlado;
                result.MessageExeption = ex.Message;
                return result;
            }
        }
        public Result UpdateInscripcionWeb(InscripcionRequest request1, UsuarioLogin user)
        {
            try
            {
                var response = _inscripcionDatos.UpdateInscripcionWeb(request1, user);
                return response;
            }
            catch (Exception ex)
            {
                Result result = new Result();
                result.Message = Message.ErrorNoControlado;
                result.MessageExeption = ex.Message;
                return result;
            }

        }
        public bool ExisteInscripcionByIdUsuario(InscripcionRequest request)
        {
            var response = _inscripcionDatos.ExisteInscripcionByIdUsuario(request);
            return response;
        }
        #endregion
    }
}
