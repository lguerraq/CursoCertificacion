using System.Collections.Generic;

namespace DBR.Evento.Modelo.Response
{
    public class ModuloResponse
    {
        public int TotalRegistros { get; set; }
        public int IdModulo { get; set; }
        public int? IdEvento { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Expositor { get; set; }
        public int? Horas { get; set; }

        public int? Peso { get; set; }
        //Evento
        public string NombreEvento { get; set; }

        public List<LeccionResponse>  Lecciones { get; set; }
    }
}
