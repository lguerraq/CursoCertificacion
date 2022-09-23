using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBR.Evento.Modelo.Request
{
    public class PreguntaRequest
    {
        public int IdPregunta { get; set; }

        public int IdCuestionario { get; set; }

        public int Tipo { get; set; }

        public string Nombre { get; set; }

        public string Explicacion { get; set; }

        public string Ayuda { get; set; }

        public int Puntaje { get; set; }

        public List<RespuestaRequest> Respuestas { get; set; }
    }
}
