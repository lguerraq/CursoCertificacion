using System;

namespace DBR.Evento.Modelo.Response
{
    public class UsuarioResponse
    {
        public int TotalRegistros { get; set; }
        public int IdUsuario { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }        
        public int? IdUsuarioTipo { get; set; }
        public string UsuarioTipo { get; set; }
        public DateTime? UltimoAcceso { get; set; }
        public DateTime? UltimaActividad { get; set; }
        public string Token { get; set; }
        public string Correo { get; set; }
        //DatosPersona
        public int? IdPersona { get; set; }
        public string NumeroDocumento { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }       
    }
}
