//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBR.Evento.Modelo
{
    using System;
    using System.Collections.Generic;
    
    public partial class DocumentoHistorico
    {
        public int IdDocumentoHistorico { get; set; }
        public int IdDocumento { get; set; }
        public Nullable<int> IdDocumentoPadre { get; set; }
        public Nullable<int> IdEmpresa { get; set; }
        public Nullable<int> Tipo { get; set; }
        public string Nombre { get; set; }
        public string NombreOriginal { get; set; }
        public string Extension { get; set; }
        public Nullable<int> Tamaño { get; set; }
        public Nullable<int> EstadoDocumento { get; set; }
        public Nullable<System.DateTime> FechaDescarga { get; set; }
        public bool Estado { get; set; }
        public System.Guid rowid { get; set; }
        public int UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public Nullable<int> UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
    }
}
