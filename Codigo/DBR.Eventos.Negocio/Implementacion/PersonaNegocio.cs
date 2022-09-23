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
    public class PersonaNegocio
    {
        PersonaDatos _personaDatos = new PersonaDatos(new DbContext("db_ingenierosEntities"));
        public PersonaNegocio()
        {

        }
        public Paged<PersonaResponse> ListPersonaPaged(PageRequest page, PersonaRequest request)
        {
            var response = _personaDatos.ListPersonaPaged(page, request);
            return response;
        }
        public List<PersonaResponse> ListAllPersona()
        {
            var response = _personaDatos.ListAllPersona();
            return response;
        }
        public Result SavePersona(PersonaRequest request, UsuarioLogin user)
        {
            var response = _personaDatos.SavePersona(request,user);
            return response;
        }
        public Result SaveUpdatePersonaMasivo(List<PersonaRequest> request, UsuarioLogin user)
        {
            var response = new Result();
            try
            {
                List<PersonaRequest> ListNewPersonas = new List<PersonaRequest>();
                List<PersonaRequest> ListUpdatePersonas = new List<PersonaRequest>();
                foreach (var item in request)
                {
                    var persona = _personaDatos.GetPersonaXdni(item);
                    if (persona.Count > 0)
                    {
                        item.IdPersona = persona[0].IdPersona;
                        ListUpdatePersonas.Add(item);
                    }
                    else
                    {
                        ListNewPersonas.Add(item);
                    }
                }
                response = _personaDatos.SavePersonaMasivo(ListNewPersonas, user);
                var response2 = _personaDatos.UpdatePersonaXdniMasivo(ListUpdatePersonas, user);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = Message.ErrorNoControlado;
                response.MessageExeption = ex.Message;
                response.StackTrace = ex.StackTrace;
            }
            
            return response;
        }
        public Result SavePersonaMasivo(List<PersonaRequest> request, UsuarioLogin user)
        {
            var response = _personaDatos.SavePersonaMasivo(request,user);
            return response;
        }
        public Result UpdatePersonaXdniMasivo(List<PersonaRequest> request, UsuarioLogin user)
        {
            var response = _personaDatos.UpdatePersonaXdniMasivo(request,user);
            return response;
        }
        public Result UpdatePersona(PersonaRequest request, UsuarioLogin user)
        {
            var response = _personaDatos.UpdatePersona(request,user);
            return response;
        }
        public Result UpdatePersonaXdni(PersonaRequest request, UsuarioLogin user)
        {
            try
            {
                var response = _personaDatos.UpdatePersonaXdni(request, user);
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
        public Result DeletePersona(PersonaRequest request, UsuarioLogin user)
        {
            var response = _personaDatos.DeletePersona(request,user);
            return response;
        }
        public PersonaResponse GetPersona(PersonaRequest request)
        {
            var response = _personaDatos.GetPersona(request);
            return response;
        }
        public List<PersonaResponse> GetPersonaXdni(PersonaRequest request)
        {
            var response = _personaDatos.GetPersonaXdni(request);
            return response;
        }
        public List<PersonaResponse> GetPersonaXcip(PersonaRequest request)
        {
            var response = _personaDatos.GetPersonaXcip(request);
            return response;
        }
        public List<PersonaResponse> ListAllPersonaCorreos()
        {
            var response = _personaDatos.ListAllPersonaCorreos();
            return response;
        }
        public List<PersonaResponse> ListAllPersonaCorreosFaltantes(CorreoRequest request)
        {
            var response = _personaDatos.ListAllPersonaCorreosFaltantes(request);
            return response;
        }
        public List<PersonaResponse> ListAllPersonaCorreosPorProfesion(CorreoRequest request, List<int?> IdsProfesion)
        {
            var response = _personaDatos.ListAllPersonaCorreosPorProfesion(request, IdsProfesion);
            return response;
        }
    }
}
