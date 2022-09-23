using System.Data.Entity;
using DBR.Eventos.Datos.Base;
using System.Collections.Generic;
using DBR.Evento.Modelo.Response;
using System.Linq;
using DBR.Eventos.Comun;

namespace DBR.Eventos.Datos.Implementacion
{
    public class GeneralDatos : BaseAcceso
    {
        public GeneralDatos(DbContext context) : base(context)
        {
        }
        public List<ComboResponse> ListUniversidadCombo()
        {
            using (var context=new DBRContext())
            {
                var query = (from u in context.Universidad
                             where u.Estado==EstadoRegistro.Activo
                             select new ComboResponse
                             {
                                 Value = u.IdUniversidad.ToString(),
                                 Descripcion=u.Nombre
                             });
                return query.ToList();
            }
        }
        public List<ComboResponse> ListTipoCombo(string Grupo)
        {
            using (var context=new DBRContext())
            {
                var query = (from t in context.Tipo
                             where t.Grupo == Grupo
                             select new ComboResponse
                             {
                                 Value=t.Valor.ToString(),
                                 Descripcion=t.NombreTipo
                             });
                return query.ToList();
            }
        }
        public List<ComboResponse> ListProfesion()
        {
            using (var context = new DBRContext())
            {
                var query = (from t in context.Profesion
                             select new ComboResponse
                             {
                                 Value = t.IdProfesion.ToString(),
                                 Descripcion = t.Descripcion
                             });
                return query.ToList();
            }
        }
        public List<ComboResponse> ListTipoUsuarioCombo()
        {
            using (var context = new DBRContext())
            {
                var query = (from t in context.UsuarioTipo
                             select new ComboResponse
                             {
                                 Value = t.IdUsuarioTipo.ToString(),
                                 Descripcion = t.Descripcion
                             });
                return query.ToList();
            }
        }
        public List<ComboResponse> ListPaisCombo()
        {
            using (var context = new DBRContext())
            {
                var query = (from t in context.Pais
                             select new ComboResponse
                             {
                                 Value = t.IdPais.ToString(),
                                 Descripcion = t.NombrePais
                             });
                return query.ToList();
            }
        }
    }
}
