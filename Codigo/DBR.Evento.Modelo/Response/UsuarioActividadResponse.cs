using System;

namespace DBR.Evento.Modelo.Response
{
    public class UsuarioActividadResponse
    {
        public int IdUsuarioActividad { get; set; }
        public int? IdUsuario { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string Token { get; set; }
    }
}
