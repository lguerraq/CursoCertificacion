using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBR.Evento.Modelo.Response
{
    public class ModuloLeccionResponse : ModuloResponse
    {
        public int IdLeccion { get; set; }

        public int Tipo { get; set; }

        public string NombreLeccion { get; set; }

        public int Duracion { get; set; }
        public int Orden { get; set; }
    }
}
