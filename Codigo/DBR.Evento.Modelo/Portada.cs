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
    
    public partial class Portada
    {
        public int IdPortada { get; set; }
        public string NombreImagen { get; set; }
        public string Descripcion { get; set; }
        public string SubTitulo1 { get; set; }
        public string SubTitulo2 { get; set; }
        public string TextoEnlace { get; set; }
        public string UrlEnlace { get; set; }
        public Nullable<bool> Estado { get; set; }
        public Nullable<System.DateTime> FechaCreacion { get; set; }
        public Nullable<int> UsuarioCreacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
        public Nullable<int> UsuarioModificacion { get; set; }
    }
}
