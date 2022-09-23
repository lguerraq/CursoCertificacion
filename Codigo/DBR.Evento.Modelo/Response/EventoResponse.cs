using System;
using System.Collections.Generic;

namespace DBR.Evento.Modelo.Response
{
    public class EventoResponse
    {
        public int IdEvento { get; set; }
        public int? Tipo { get; set; }
        public int? Modalidad { get; set; }
        public string NombreModalidad { get; set; }
        public string NombreEvento { get; set; }
        public DateTime Fecha { get; set; }
        public string Expositor { get; set; }
        public string ImagenEvento { get; set; }
        public string DocumentoFotocheck { get; set; }
        public string DocumentoCertificado { get; set; }
        public string DocumentoCertificadoImprimir { get; set; }
        public string DocumentoCertificadoExpositor { get; set; }
        public int EstadoEvento { get; set; }
        public int Horas { get; set; }
        public bool Activo { get; set; }
        public string rowid { get; set; }
        public int CantidadAsignados { get; set; }
        public int CantidadInscritos { get; set; }
        public int CantidadLeciones { get; set; }
        public decimal? NotaAprobatoria { get; set; }
        public string Costo { get; set; }
        public decimal? CostoValor { get; set; }
        public decimal? CostoValorPromocional { get; set; }
        public bool? DetallarCertificado { get; set; }
        public bool? GenerarCertificado { get; set; }
        public List<int> IdsTemas { get; set; }
        //Persona
        public int IdPersona { get; set; }
        public string NombrePersona { get; set; }
        //Incripcion
        public string CetificadoFirmado { get; set; }
        public int IdInscripcion { get; set; }

        public string TipoCursoNombre { get; set; }

        public string Descripcion { get; set; }

        public DateTime? FechaUltimoAcceso { get; set; }

        public string UrlVideo { get; set; }
    }
}
