namespace DBR.Evento.Modelo.Request
{
    public class DocumentoRequest
    {
        public int IdDocumento { get; set; }
        public int? IdDocumentoPadre { get; set; }
        public int? IdEmpresa { get; set; }
        public int? Tipo { get; set; }
        public string Nombre { get; set; }
        public string NombreOriginal { get; set; }
        public string Extension { get; set; }
        public int? Tamaño { get; set; }
        public int? EstadoDocumento { get; set; }
        public bool Estado { get; set; }
    }
}
