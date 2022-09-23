using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBR.Evento.Modelo.Response
{
    public class EvaluacionResponse
    {
        public int IdCuestionario { get; set; }

        public decimal Nota { get; set; }

        public int Intento { get; set; }
        public bool? Abierto { get; set; }
    }
}
