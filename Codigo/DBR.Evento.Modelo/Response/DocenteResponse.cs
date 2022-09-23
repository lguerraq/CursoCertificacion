using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBR.Evento.Modelo.Response
{
    public class DocenteResponse
    {
        public int IdDocente { get; set; }
        public string Nombre { get; set; }
        public string NombreFoto { get; set; }
        public string Profesion { get; set; }
        public string Especialista { get; set; }
        public string Detalle { get; set; }
        public string RowId { get; set; }
    }
}
