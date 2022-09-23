using DBR.Evento.Modelo.Response;
using DBR.Eventos.Datos.Implementacion;
using System.Collections.Generic;
using System.Data.Entity;

namespace DBR.Eventos.Negocio.Implementacion
{
    public class GeneralNegocio
    {
        GeneralDatos _generalDatos = new GeneralDatos(new DbContext("db_ingenierosEntities"));
        public GeneralNegocio()
        {

        }
        public List<ComboResponse> ListUniversidadCombo()
        {
            var response = _generalDatos.ListUniversidadCombo();
            return response;
        }
        public List<ComboResponse> ListTipoCombo(string Grupo)
        {
            var response = _generalDatos.ListTipoCombo(Grupo);
            return response;
        }
        public List<ComboResponse> ListProfesion()
        {
            var response = _generalDatos.ListProfesion();
            return response;
        }
        public List<ComboResponse> ListTipoUsuarioCombo()
        {
            var response = _generalDatos.ListTipoUsuarioCombo();
            return response;
        }
        public List<ComboResponse> ListPaisCombo()
        {
            var response = _generalDatos.ListPaisCombo();
            return response;
        }
    }
}
