using System;

namespace DBR.Evento.Modelo.Response
{
    public class EventoCorreoResponse
    {
        public int IdEventoCorreo { get; set; }
        public int IdEvento { get; set; }
        public string Asunto { get; set; }
        public string Origen { get; set; }
        public string NombreOrigen { get; set; }
        public string Mensaje { get; set; }
        public int EstadoCorreo { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public int NumeroEnvio { get; set; }
        public int Cantidad { get; set; }
    }
}
