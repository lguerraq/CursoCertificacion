namespace DBR.Evento.Modelo.Response
{
    public class VirtualVideoResponse
    {
        public int IdVirtualVideo { get; set; }
        public int? IdEvento { get; set; }
        public string Url { get; set; }
        public int? IdEventoUsuarioVirtualVideo { get; set; }
        public bool MostrarVideo { get; set; }
    }
}
