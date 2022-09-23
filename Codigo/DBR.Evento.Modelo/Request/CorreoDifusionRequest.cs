using System;

namespace DBR.Evento.Modelo.Request
{
    public class CorreoDifusionRequest
    {
        public int IdCorreoDifusion { get; set; }
        public int? IdCorreo { get; set; }
        public int? IdEvento { get; set; }
        public int IdPersona { get; set; }
        public string Correo { get; set; }
        public int Estado { get; set; }
        public string ErrorMensaje { get; set; }
        public string ErrorStackTrace { get; set; }
        public int UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int NumeroEnvio { get; set; }
    }
}
