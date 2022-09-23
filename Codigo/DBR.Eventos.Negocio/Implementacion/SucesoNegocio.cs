using DBR.Evento.Modelo;
using DBR.Evento.Modelo.Request;
using DBR.Evento.Modelo.Response;
using DBR.Eventos.Datos.Implementacion;
using System.Collections.Generic;
using System.Data.Entity;

namespace DBR.Eventos.Negocio.Implementacion
{
    public class SucesoNegocio
    {
        SucesoDatos _SucesoDatos = new SucesoDatos(new DbContext("db_ingenierosEntities"));
        public SucesoNegocio()
        {

        }
        public Paged<SucesoResponse> ListSucesoPaginado(PageRequest page)
        {
            var response = _SucesoDatos.ListSucesoPaginado(page);
            return response;
        }
        public Result SaveSuceso(SucesoRequest request, UsuarioLogin user)
        {
            var response = _SucesoDatos.SaveSuceso(request, user);
            return response;
        }
        public Result UpdateSuceso(SucesoRequest request, UsuarioLogin user)
        {
            var response = _SucesoDatos.UpdateSuceso(request, user);
            return response;
        }
        public Result DeleteSuceso(SucesoRequest request, UsuarioLogin user)
        {
            var response = _SucesoDatos.DeleteSuceso(request, user);
            return response;
        }
        public Result UpdateEstadoSuceso(SucesoRequest request, UsuarioLogin user)
        {
            var response = _SucesoDatos.UpdateEstadoSuceso(request, user);
            return response;
        }
        public SucesoResponse GetSuceso(SucesoRequest request)
        {
            var response = _SucesoDatos.GetSuceso(request);
            return response;
        }

        #region METODOS 100% INGENIEROS
        public List<SucesoResponse> ListUltimosSucesos()
        {
            var response = _SucesoDatos.ListUltimosSucesos();
            return response;
        }
        public List<SucesoResponse> ListAllSucesos()
        {
            var response = _SucesoDatos.ListAllSucesos();
            return response;
        }
        #endregion
    }
}
