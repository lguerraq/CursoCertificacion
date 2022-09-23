using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBR.Evento.Modelo.Response
{
    public class CuestionarioResponse
    {
        public int IdCuestionario { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public int Peso { get; set; }

        public List<PreguntaResponse> Preguntas { get; set; }
    }
}
