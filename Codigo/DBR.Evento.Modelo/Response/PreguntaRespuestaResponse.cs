namespace DBR.Evento.Modelo.Response
{
    public class PreguntaRespuestaResponse
    {
        public int IdPregunta { get; set; }
        public string Nombre { get; set; }
        public string Ayuda { get; set; }
        public int Puntaje { get; set; }
        public int Tipo { get; set; }
        public int IdRespuesta { get; set; }
        public string Descripcion { get; set; }
        public bool EsCorrecta { get; set; }        
    }
}
