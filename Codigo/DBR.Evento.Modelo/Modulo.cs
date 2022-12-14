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
    
    public partial class Modulo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Modulo()
        {
            this.Leccion = new HashSet<Leccion>();
        }
    
        public int IdModulo { get; set; }
        public Nullable<int> IdEvento { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Expositor { get; set; }
        public Nullable<int> Horas { get; set; }
        public Nullable<int> Peso { get; set; }
        public Nullable<bool> Estado { get; set; }
        public Nullable<System.Guid> rowid { get; set; }
        public int UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public Nullable<int> UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Leccion> Leccion { get; set; }
        public virtual Evento Evento { get; set; }
    }
}
