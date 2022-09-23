using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBR.Evento.Modelo.Response
{
    public class RespuestaResponse
    {
        public int IdRespuesta { get; set; }

        public int IdPregunta { get; set; }

        public string Descripcion { get; set; }

        public bool EsCorrecta { get; set; }

        public string NombrePregunta { get; set; }
        public bool EsRespondida { get; set; }
    }
}
