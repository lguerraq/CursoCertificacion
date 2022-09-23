using DBR.Evento.Modelo;
using DBR.Evento.Modelo.Request;
using DBR.Evento.Modelo.Response;
using DBR.Eventos.Datos.Implementacion;
using System.Collections.Generic;
using System.Data.Entity;

namespace DBR.Eventos.Negocio.Implementacion
{
    public class VirtualNegocio
    {
        VirtualDatos _virtualDatos = new VirtualDatos(new DbContext("db_ingenierosEntities"));
        public VirtualNegocio()
        {

        }
        //VirtualContenido
        public VirtualContenidoResponse GetContenidoVirtualByEvento(VirtualContenidoRequest request)
        {
            var response = _virtualDatos.GetContenidoVirtualByEvento(request);
            return response;
        }
        public Result SaveVirtualContenido(VirtualContenidoRequest request, UsuarioLogin user)
        {
            var response = _virtualDatos.SaveVirtualContenido(request, user);
            return response;
        }
        public Result UpdateVirtualContenido(VirtualContenidoRequest request, UsuarioLogin user)
        {
            var response = _virtualDatos.UpdateVirtualContenido(request, user);
            return response;
        }
        //VirtualVideo
        public List<VirtualVideoResponse> ListVirtualVideoByEvento(VirtualContenidoRequest request)
        {
            var response = _virtualDatos.ListVirtualVideoByEvento(request);
            return response;
        }
        public Result SaveVirtualVideo(VirtualVideoRequest request, UsuarioLogin user)
        {
            var response = _virtualDatos.SaveVirtualVideo(request, user);
            return response;
        }
        public VirtualVideoResponse GetVirtualVideo(VirtualVideoRequest request)
        {
            var response = _virtualDatos.GetVirtualVideo(request);
            return response;
        }
        public Result UpdateVirtualVideo(VirtualVideoRequest request, UsuarioLogin user)
        {
            var response = _virtualDatos.UpdateVirtualVideo(request, user);
            return response;
        }
        public Result DeleteVirtualVideo(VirtualVideoRequest request, UsuarioLogin user)
        {
            var response = _virtualDatos.DeleteVirtualVideo(request, user);
            return response;
        }
        //EventousuarioEventoVideo
        public List<VirtualVideoResponse> ListEventoUsuarioVirtualVideoByEvento(VirtualContenidoRequest request)
        {
            var response = _virtualDatos.ListEventoUsuarioVirtualVideoByEvento(request);
            return response;
        }
        public List<VirtualVideoResponse> ListVirtualVideoByUsuarioByEvento(VirtualContenidoRequest request, UsuarioLogin user)
        {
            var response = _virtualDatos.ListVirtualVideoByUsuarioByEvento(request, user);
            return response;
        }
    }
}
