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
    
    public partial class Respuesta
    {
        public int IdRespuesta { get; set; }
        public int IdPregunta { get; set; }
        public string Descripcion { get; set; }
        public bool EsCorrecta { get; set; }
        public bool Estado { get; set; }
        public int UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public Nullable<int> UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
    
        public virtual Pregunta Pregunta { get; set; }
    }
}
