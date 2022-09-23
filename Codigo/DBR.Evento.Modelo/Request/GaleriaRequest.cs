using System;

namespace DBR.Evento.Modelo.Request
{
    public class GaleriaRequest
    {
        public int IdGaleria { get; set; }
        public string Descripcion { get; set; }
        public string Nombre { get; set; }
        public Nullable<bool> Activo { get; set; }
    }
}
