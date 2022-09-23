using System;

namespace DBR.Evento.Modelo.Response
{
    public class EventoUsuarioResponse
    {
        public int IdEventoUsuario { get; set; }
        public int? IdEvento { get; set; }
        public int? IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
