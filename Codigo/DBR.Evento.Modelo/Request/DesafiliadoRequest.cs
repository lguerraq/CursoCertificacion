namespace DBR.Evento.Modelo.Request
{
    public class DesafiliadoRequest
    {
        public int IdDesafiliado { get; set; }
        public string Correo { get; set; }
        public int? Valor { get; set; }
        public string Observacion { get; set; }
    }
}
