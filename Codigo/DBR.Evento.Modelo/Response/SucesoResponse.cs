using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBR.Evento.Modelo.Response
{
    public class SucesoResponse
    {
        public int IdSuceso { get; set; }
        public string NombreSuceso { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public string ImagenSuceso { get; set; }
        public int Horas { get; set; }
        public bool Activo { get; set; }
        public string rowid { get; set; }      
    }
}
