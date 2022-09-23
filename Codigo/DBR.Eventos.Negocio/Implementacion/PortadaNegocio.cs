using DBR.Evento.Modelo;
using DBR.Evento.Modelo.Request;
using DBR.Evento.Modelo.Response;
using DBR.Eventos.Datos.Implementacion;
using System.Collections.Generic;
using System.Data.Entity;

namespace DBR.Eventos.Negocio.Implementacion
{
    public class PortadaNegocio
    {
        PortadaDatos _PortadaDatos = new PortadaDatos(new DbContext("db_ingenierosEntities"));
        public Paged<PortadaResponse> ListPortadaPaginado(PageRequest page)
        {
            return _PortadaDatos.ListPortadaPaginado(page);
        }
        public List<PortadaResponse> ListPortada()
        {
            return _PortadaDatos.ListPortada();
        }
        public Result SavePortada(PortadaRequest request, UsuarioLogin user)
        {
            return _PortadaDatos.SavePortada(request, user);
        }
        public Result DeletePortada(PortadaRequest request, UsuarioLogin user)
        {
            return _PortadaDatos.DeletePortada(request, user);
        }
        public Result UpdatePortada(PortadaRequest request, UsuarioLogin user)
        {
            return _PortadaDatos.UpdatePortada(request, user);
        }
        public PortadaResponse GetPortada(PortadaRequest request)
        {
            return _PortadaDatos.GetPortada(request);
        }
    }
}
