namespace DBR.Evento.Modelo.Response
{
    public class OpcionResponse
    {
        public int Id { get; set; }
        public int? IdUsuarioTipo { get; set; }
        public string Icono { get; set; }
        public int? IdPadre { get; set; }
        public string Descripcion { get; set; }
        public string UrlDescripcion { get; set; }
        public int? Orden { get; set; }
    }
}
