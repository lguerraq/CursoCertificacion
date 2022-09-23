using DBR.Evento.Modelo;
using DBR.Evento.Modelo.Request;
using DBR.Evento.Modelo.Response;
using DBR.Eventos.Datos.Implementacion;
using System.Data.Entity;

namespace DBR.Eventos.Negocio.Implementacion
{
    public class CorreoNegocio
    {
        CorreoDatos _correoDatos = new CorreoDatos(new DbContext("db_ingenierosEntities"));
        public CorreoNegocio()
        {

        }
        //Correo
        public Paged<CorreoResponse> ListCorreoPaged(PageRequest page)
        {
            var response = _correoDatos.ListCorreoPaged(page);
            return response;
        }
        public Result SaveCorreo(CorreoRequest request, UsuarioLogin user)
        {
            var response = _correoDatos.SaveCorreo(request, user);
            return response;
        }
        public Result UpdateCorreo(CorreoRequest request, UsuarioLogin user)
        {
            var response = _correoDatos.UpdateCorreo(request, user);
            return response;
        }
        public Result DeleteCorreo(CorreoRequest request, UsuarioLogin user)
        {
            var response = _correoDatos.DeleteCorreo(request, user);
            return response;
        }
        public Result UpdateCorreoEnvio(CorreoRequest request, UsuarioLogin user)
        {
            var response = _correoDatos.UpdateCorreoEnvio(request,user);
            return response;
        }
        public CorreoResponse GetCorreo(CorreoRequest request)
        {
            var response = _correoDatos.GetCorreo(request);
            return response;
        }
        public int ConsultarEnvios(CorreoRequest request)
        {
            var response = _correoDatos.ConsultarEnvios(request);
            return response;
        }
        //CorreoResponse
        public Result SaveCorreoDifusion(CorreoDifusionRequest request, UsuarioLogin user)
        {
            var response = _correoDatos.SaveCorreoDifusion(request, user);
            return response;
        }

        //Reporte Correo
        public Paged<CorreoReporteResponse> ListCorreoReportePaged(PageRequest page)
        {
            var response = _correoDatos.ListCorreoReportePaged(page);
            return response;
        }

        //Desafiliado
        public Result SaveDesafiliado(DesafiliadoRequest request, UsuarioLogin user)
        {
            var response = _correoDatos.SaveDesafiliado(request, user);
            return response;
        }
    }
}
