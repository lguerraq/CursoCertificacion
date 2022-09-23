namespace DBR.Evento.Modelo.Request
{
    public class PersonaRequest
    {
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
        public string TipoOcupacionAbreviatura { get; set; }
        public string Buscar { get; set; }
        public int IdProfesion { get; set; }
        public string NombreCompleto { get; set; }
        public int IdPais { get; set; }
        public string Ciudad { get; set; }
    }
}
