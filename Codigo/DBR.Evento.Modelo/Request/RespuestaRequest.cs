using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBR.Evento.Modelo.Request
{
    public class RespuestaRequest
    {
        public int IdRespuesta { get; set; }

        public int IdPregunta { get; set; }

        public string Descripcion { get; set; }

        public bool Selected { get; set; }
    }
}
