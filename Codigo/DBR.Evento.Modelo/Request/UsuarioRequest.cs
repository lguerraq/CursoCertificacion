namespace DBR.Evento.Modelo.Request
{
    public class UsuarioRequest
    {
        public int IdUsuario { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string NumeroDocumento { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public int IdUsuarioTipo { get; set; }
        public string Capcha { get; set; }
        public string Correo { get; set; }
    }
}
