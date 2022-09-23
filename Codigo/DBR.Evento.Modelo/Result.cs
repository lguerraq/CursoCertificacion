namespace DBR.Evento.Modelo
{
    public class Result
    {
        public int Codigo { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string MessageExeption { get; set; }
        public string StackTrace { get; set; }
        public string InnerException { get; set; }
        public string Informacion { get; set; }
        public decimal? ResultNota { get; set; }
    }
}
