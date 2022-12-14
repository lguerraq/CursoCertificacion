using System;
using System.Collections.Generic;

namespace DBR.Evento.Modelo.Request
{
    public class EventoRequest
    {
        public int IdEvento { get; set; }

        public int Tipo { get; set; }
        public int Modalidad { get; set; }
        public string NombreEvento { get; set; }

        public string Descripcion { get; set; }

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
        public string Costo { get; set; }
        public decimal NotaAprobatoria { get; set; }
        public decimal CostoValor { get; set; }
        public decimal? CostoValorPromocional { get; set; }
        public bool DetallarCertificado { get; set; }
        public bool GenerarCertificado { get; set; }
        public List<int> ListTemas{ get; set; }
        public Guid rowid { get; set; }
        public string rowidString { get; set; }
    }
}
