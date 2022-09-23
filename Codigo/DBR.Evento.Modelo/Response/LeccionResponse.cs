using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBR.Evento.Modelo.Response
{
    public class LeccionResponse
    {
        public int IdLeccion { get; set; }

        public int? IdModulo { get; set; }

        public string NombreModulo { get; set; }

        public int? Tipo { get; set; }

        public string NombreTipo { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }
        public int? TipoUrl { get; set; }
        public string UrlVideo { get; set; }

        public int Duracion { get; set; }

        public int Orden { get; set; }

        public int TotalRegistros { get; set; }
        public int IdEvento { get; set; }
        //Evento
        public decimal? NotaAprobatoria { get; set; }
    }
}
