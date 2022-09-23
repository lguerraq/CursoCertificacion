namespace DBR.Evento.Modelo.Response
{
    public class PersonaResponse
    {
        public int TotalRegistros { get; set; }
        public int IdPersona { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string NumeroDocumento { get; set; }
        public int TipoOcupacion { get; set; }
        public string DescripcionOcupacion { get; set; }
        public string CIP { get; set; }
        public string Celular { get; set; }
        public string Correo { get; set; }
        public string TipoOcupacionNombre { get; set; }
        public string TipoOcupacionAbreviatura { get; set; }
        public int? IdProfesion { get; set; }
        public string DescripcionProfesion { get; set; }
        public int? IdPais { get; set; }
        public string NombrePais { get; set; }
        public string Ciudad { get; set; }
    }
}
