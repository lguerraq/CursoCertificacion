using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBR.Evento.Modelo.Request
{
    public class LeccionRequest
    {
        public int IdLeccion { get; set; }

        public int IdModulo { get; set; }
        public int IdEvento { get; set; }

        public int Tipo { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public int Duracion { get; set; }

        public int TipoUrl { get; set; }
        public string Url { get; set; }

        public int Orden { get; set; }

        public int Peso { get; set; }
    }
}
