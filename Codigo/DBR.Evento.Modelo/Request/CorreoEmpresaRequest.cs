namespace DBR.Evento.Modelo.Request
{
    public class CorreoEmpresaRequest
    {
        public string Asunto { get; set; }
        public string CorreoOrigen { get; set; }
        public string NombreOrigen { get; set; }
        public string CorreoDestino { get; set; }
        public string Mensaje { get; set; }
    }
}
