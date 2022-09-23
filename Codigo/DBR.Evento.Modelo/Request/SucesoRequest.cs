using System;

namespace DBR.Evento.Modelo.Request
{
    public class SucesoRequest
    {
        public int IdSuceso { get; set; }
        public string NombreSuceso { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public string ImagenSuceso { get; set; }
        public int Horas { get; set; }
        public bool Activo { get; set; }
    }
}
