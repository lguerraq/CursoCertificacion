using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBR.Evento.Modelo.Request
{
    public class EvaluacionRequest
    {
        public int IdCuestionario { get; set; }

        public int Nota { get; set; }

        public int Intento { get; set; }

        public int IdUsuario { get; set; }
        public int IdEvento { get; set; }

        public List<RespuestaRequest> Respuestas { get; set; }
    }
}
