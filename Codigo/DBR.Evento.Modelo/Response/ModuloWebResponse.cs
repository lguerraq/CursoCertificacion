using System.Collections.Generic;

namespace DBR.Evento.Modelo.Response
{
    public class ModuloWebResponse
    {
        public int IdModulo { get; set; }
        public string Nombre { get; set; }
        public List<LeccionWebResponse> Lecciones { get; set; }
    }
    public class LeccionWebResponse
    {
        public int IdLeccion { get; set; }
        public int IdModulo { get; set; }
        public string Nombre { get; set; }
        public int? Duracion { get; set; }
        public string NombreTipo { get; set; }
        public int Orden { get; set; }
    }
}
