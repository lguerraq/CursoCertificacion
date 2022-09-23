using System;

namespace DBR.Evento.Modelo.Response
{
    public class InscripcionResponse
    {
        public int IdInscripcion { get; set; }
        public int IdEvento { get; set; }
        public int IdPersona { get; set; }
        public int EstadoPago { get; set; }
        public string NombreEstadoPago { get; set; }
        public int TipoPago { get; set; }
        public string NombreTipoPago { get; set; }
        public bool EntregaCertificado { get; set; }
        public string rowguid { get; set; }
        public int? TipoModalidad { get; set; }
        public string Modalidad { get; set; }
        public decimal? Monto { get; set; }
        public string NombreBanco { get; set; }
        public DateTime? FechaOperacion { get; set; }
        public string NumeroOperacion { get; set; }
        public int? NumeroCertificado { get; set; }
        public string Certificado { get; set; }
        public decimal? Nota { get; set; }
        public string Ruc { get; set; }
        public int? TipoInscripcion { get; set; }
        //Persona
        public string Nombre { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string NumeroDocumento { get; set; }
        public int TipoOcupacion { get; set; }
        public string TipoOcupacionNombre { get; set; }
        public string DescripcionOcupacion { get; set; }
        public string TipoOcupacionAbreviatura { get; set; }
        public string CIP { get; set; }
        public string Celular { get; set; }
        public string Correo { get; set; }
        public int? IdProfesion { get; set; }
        public string DescripcionProfesion { get; set; }
        public int? IdPais { get; set; }
        public string NombrePais { get; set; }
        public string Ciudad { get; set; }
        //Evento
        public DateTime FechaRegistro { get; set; }
        public string NombreCertificado { get; set; }
        public string NombreCertificadoImprimir { get; set; }
        public string NombreFotocheck { get; set; }
        public string NombreCertificadoExpositor { get; set; }
        public bool? DetallarCertificado { get; set; }
        public bool? GenerarCertificado { get; set; }
        //Usuario
        public int? IdUsuario { get; set; }
    }
}
