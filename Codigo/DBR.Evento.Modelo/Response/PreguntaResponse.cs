using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBR.Evento.Modelo.Response
{
    public class PreguntaResponse
    {
        public int IdPregunta { get; set; }

        public int IdCuestionario { get; set; }

        public int Tipo { get; set; }

        public string Nombre { get; set; }

        public string Explicacion { get; set; }

        public string Ayuda { get; set; }

        public int Puntaje { get; set; }
        public bool EsRespondidaCorrecta { get; set; }
        public int TotalRegistros { get; set; }

        public List<RespuestaResponse> Respuestas { get; set; }
    }
}
