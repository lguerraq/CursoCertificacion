using System;

namespace DBR.Evento.Modelo.Request
{
    public class EventoUsuarioRequest
    {
        public int IdEventoUsuario { get; set; }
        public int? IdEvento { get; set; }
        public int? IdUsuario { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
