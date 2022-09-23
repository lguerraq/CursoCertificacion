namespace DBR.Evento.Modelo.Response
{
    public class CorreoReporteResponse
    {
        public int Row { get; set; }
        public int Año { get; set; }
        public int Mes { get; set; }
        public int TotalEnviados { get; set; }
        public int EnviadosGratis { get; set; }
        public int EnviadosAdicionales { get; set; }
        public decimal CostoEnvioAdicional { get; set; }
        public decimal TotalCostoAdicional { get; set; }
    }
}
