using DBR.Evento.Modelo;
using DBR.Evento.Modelo.Request;
using DBR.Evento.Modelo.Response;
using DBR.Eventos.Datos;
using DBR.Eventos.Datos.Implementacion;
using System.Collections.Generic;
using System.Data.Entity;

namespace DBR.Eventos.Negocio.Implementacion
{
    public class UsuarioNegocio
    {
        UsuarioDatos _usuarioDatos = new UsuarioDatos(new DbContext("db_ingenierosEntities"));
        public UsuarioNegocio()
        {

        }

        public List<UsuarioResponse> BuscarUsuarioXlogin(UsuarioRequest request)
        {
            var response= _usuarioDatos.BuscarUsuarioXlogin(request);
            return response;
        }
        public List<UsuarioResponse> BuscarUsuarioParticipanteXlogin(UsuarioRequest request)
        {
            var response = _usuarioDatos.BuscarUsuarioParticipanteXlogin(request);
            return response;
        }
        public List<UsuarioResponse> ListUsuario()
        {
            var response = _usuarioDatos.ListUsuario();
            return response;
        }
        public Paged<UsuarioResponse> ListUsuarioPaged(PageRequest page)
        {
            var response = _usuarioDatos.ListUsuarioPaged(page);
            return response;
        }
        public Result SaveUsuario(UsuarioRequest request, UsuarioLogin user)
        {
            var response = _usuarioDatos.SaveUsuario(request, user);
            return response;
        }
        public Result UpdateUsuario(UsuarioRequest request, UsuarioLogin user)
        {
            var response = _usuarioDatos.UpdateUsuario(request, user);
            return response;
        }
        public Result DeleteUsuario(UsuarioRequest request, UsuarioLogin user)
        {
            var response = _usuarioDatos.DeleteUsuario(request, user);
            return response;
        }
        public UsuarioResponse GetUsuario(UsuarioRequest request)
        {
            var response = _usuarioDatos.GetUsuario(request);
            return response;
        }
        public List<OpcionResponse> ListOpcionesByRol(OpcionRequest request)
        {
            var response = _usuarioDatos.ListOpcionesByRol(request);
            return response;
        }
        public Result UpdatePasswordUsuario(UsuarioRequest request, UsuarioLogin user)
        {
            var response = _usuarioDatos.UpdatePasswordUsuario(request, user);
            return response;
        }
        //Usuario
        public List<ComboResponse> ListUsuarioSinAsignar(EventoUsuarioRequest request)
        {
            var response = _usuarioDatos.ListUsuarioSinAsignar(request);
            return response;
        }
        //UsuarioHistorico
        public Result SaveUsuarioHistorico(UsuarioRequest request)
        {
            var response = _usuarioDatos.SaveUsuarioHistorico(request);
            return response;
        }
        public Result SaveUsuarioAcceso(UsuarioLogin user)
        {
            var response = _usuarioDatos.SaveUsuarioAcceso(user);
            return response;
        }
        public UsuarioActividadResponse GetUsuarioAcceso(UsuarioLogin user)
        {
            var response = _usuarioDatos.GetUsuarioAcceso(user);
            return response;
        }
        public Result DeleteUsuarioAcceso(UsuarioLogin user)
        {
            var response = _usuarioDatos.DeleteUsuarioAcceso(user);
            return response;
        }
    }
}
