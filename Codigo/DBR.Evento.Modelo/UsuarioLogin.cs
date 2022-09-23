namespace DBR.Evento.Modelo
{
    public class UsuarioLogin
    {
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public int IdUsuarioTipo { get; set; }
        public string Token { get; set; }
        //DatosPersona
        public int IdPersona { get; set; }
        public string NumeroDocumento { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Correo { get; set; }
    }
}
