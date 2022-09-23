namespace DBR.Evento.Modelo.Request
{
    public class PortadaRequest
    {
        public int IdPortada { get; set; }
        public string NombreImagen { get; set; }
        public string Descripcion { get; set; }
        public string SubTitulo1 { get; set; }
        public string SubTitulo2 { get; set; }
        public string TextoEnlace { get; set; }
        public string UrlEnlace { get; set; }

        //Data area seleccionadad
        public decimal left { get; set; }
        public decimal top { get; set; }
        public decimal width { get; set; }
        public decimal height { get; set; }
        public decimal widthCropper { get; set; }
        public decimal heightCropper { get; set; }
        public int naturalWidth { get; set; }
        public int naturalHeight { get; set; }
    }
}
