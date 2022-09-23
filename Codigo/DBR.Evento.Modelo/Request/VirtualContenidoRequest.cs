namespace DBR.Evento.Modelo.Request
{
    public class VirtualContenidoRequest
    {
        public int IdVirtualContenido { get; set; }
        public int IdEvento { get; set; }
        public string Contenido { get; set; }
        public int IdEventoUsuario { get; set; }
    }
}
