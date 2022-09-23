using System;

namespace DBR.Evento.Modelo.Request
{
    public class InscripcionRequest
    {
        public int IdInscripcion { get; set; }
        public int IdEvento { get; set; }
        public int IdPersona { get; set; }
        public int EstadoPago { get; set; }
        public int TipoPago { get; set; }       
        public int EntregaCertificado { get; set; }
        public int? TipoModalidad { get; set; }
        public decimal? Monto { get; set; }
        public string NombreBanco { get; set; }
        public DateTime? FechaOperacion { get; set; }
        public string NumeroOperacion { get; set; }
        public int NumeroCertificado { get; set; }
        public string Certificado { get; set; }
        public decimal? Nota { get; set; }
        public int? TipoInscripcion { get; set; }
        public string Ruc { get; set; }
        public string rowguid { get; set; }
    }
}
