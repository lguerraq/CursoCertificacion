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
    
    public partial class Correo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Correo()
        {
            this.CorreoDifusion = new HashSet<CorreoDifusion>();
        }
    
        public int IdCorreo { get; set; }
        public string Asunto { get; set; }
        public string Origen { get; set; }
        public string NombreOrigen { get; set; }
        public string Mensaje { get; set; }
        public Nullable<int> EstadoCorreo { get; set; }
        public Nullable<System.DateTime> FechaEnvio { get; set; }
        public Nullable<int> NumeroEnvio { get; set; }
        public bool Estado { get; set; }
        public int UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public Nullable<int> UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CorreoDifusion> CorreoDifusion { get; set; }
    }
}