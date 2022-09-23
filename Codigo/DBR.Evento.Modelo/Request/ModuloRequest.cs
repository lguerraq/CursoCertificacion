namespace DBR.Evento.Modelo.Request
{
    public class ModuloRequest
    {
        public int IdModulo { get; set; }
        public int IdEvento { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Expositor { get; set; }
        public int? Horas { get; set; }

        public int Peso { get; set; }
    }
}
