using DBR.Evento.Modelo;
using DBR.Evento.Modelo.Request;
using DBR.Evento.Modelo.Response;
using DBR.Eventos.Datos.Implementacion;
using System.Collections.Generic;
using System.Data.Entity;

namespace DBR.Eventos.Negocio.Implementacion
{
    public class GaleriaNegocio
    {
        GaleriaDatos _galeriaDatos = new GaleriaDatos(new DbContext("db_ingenierosEntities"));
        public GaleriaNegocio()
        {

        }
        public Paged<GaleriaResponse> ListGaleria(PageRequest page)
        {
            var response = _galeriaDatos.ListGaleria(page);
            return response;
        }
        public List<GaleriaResponse> ListGaleriaActivos()
        {
            var response = _galeriaDatos.ListGaleriaActivos();
            return response;
        }
        public Result SaveGaleria(GaleriaRequest request, UsuarioLogin user)
        {
            var response = _galeriaDatos.SaveGaleria(request,user);
            return response;
        }
        public Result DeleteGaleria(GaleriaRequest request, UsuarioLogin user)
        {
            var response = _galeriaDatos.DeleteGaleria(request,user);
            return response;
        }
        public Result UpdateActivoGaleria(GaleriaRequest request, UsuarioLogin user)
        {
            var response = _galeriaDatos.UpdateActivoGaleria(request,user);
            return response;
        }

        #region METODOS AVAS CONSULTORES
        public List<GaleriaResponse> ListGaleriaActivosAvas()
        {
            var response = _galeriaDatos.ListGaleriaActivosAvas();
            return response;
        }
        #endregion

        #region METODOS 100% INGENIEROS
        public List<GaleriaResponse> ListUltimosGaleriaActivos()
        {
            var response = _galeriaDatos.ListUltimosGaleriaActivos();
            return response;
        }
        public List<GaleriaResponse> ListAllGaleriaActivos()
        {
            var response = _galeriaDatos.ListAllGaleriaActivos();
            return response;
        }
        #endregion
    }
}
