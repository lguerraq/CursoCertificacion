namespace DBR.Eventos.Datos
{
    using Evento.Modelo;
    using System.Data.Entity;
    public class DBRContext : DbContext
    {
        public DBRContext() : base("name=db_ingenierosEntities")
        {
            Database.SetInitializer<DBRContext>(null);
            this.Configuration.ValidateOnSaveEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<Configuracion> Configuracion { get; set; }
        public virtual DbSet<Conversacion> Conversacion { get; set; }
        public virtual DbSet<ConversacionDetalle> ConversacionDetalle { get; set; }
        public virtual DbSet<CorreoDifusion> CorreoDifusion { get; set; }
        public virtual DbSet<Cuestionario> Cuestionario { get; set; }
        public virtual DbSet<CuestionarioRespuesta> CuestionarioRespuesta { get; set; }
        public virtual DbSet<CuestionarioTomado> CuestionarioTomado { get; set; }
        public virtual DbSet<Desafiliado> Desafiliado { get; set; }
        public virtual DbSet<Docente> Docente { get; set; }
        public virtual DbSet<EventoTema> EventoTema { get; set; }
        public virtual DbSet<EventoUsuario> EventoUsuario { get; set; }
        public virtual DbSet<EventoUsuarioVirtualVideo> EventoUsuarioVirtualVideo { get; set; }
        public virtual DbSet<Expositor> Expositor { get; set; }
        public virtual DbSet<Galeria> Galeria { get; set; }
        public virtual DbSet<Inscripcion> Inscripcion { get; set; }
        public virtual DbSet<Modulo> Modulo { get; set; }
        public virtual DbSet<Opcion> Opcion { get; set; }
        public virtual DbSet<OpcionUsuarioTipo> OpcionUsuarioTipo { get; set; }
        public virtual DbSet<Pais> Pais { get; set; }
        public virtual DbSet<Persona> Persona { get; set; }
        public virtual DbSet<Pregunta> Pregunta { get; set; }
        public virtual DbSet<Profesion> Profesion { get; set; }
        public virtual DbSet<Respuesta> Respuesta { get; set; }
        public virtual DbSet<Suceso> Suceso { get; set; }
        public virtual DbSet<Tipo> Tipo { get; set; }
        public virtual DbSet<Universidad> Universidad { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<UsuarioActividad> UsuarioActividad { get; set; }
        public virtual DbSet<UsuarioHistorico> UsuarioHistorico { get; set; }
        public virtual DbSet<UsuarioTipo> UsuarioTipo { get; set; }
        public virtual DbSet<VirtualVideo> VirtualVideo { get; set; }
        public virtual DbSet<UsuarioActividadHistorico> UsuarioActividadHistorico { get; set; }
        public virtual DbSet<Correo> Correo { get; set; }
        public virtual DbSet<VirtualContenido> VirtualContenido { get; set; }
        public virtual DbSet<Portada> Portada { get; set; }
        public virtual DbSet<EventoCorreo> EventoCorreo { get; set; }
        public virtual DbSet<Leccion> Leccion { get; set; }
        public virtual DbSet<Evento> Evento { get; set; }
        public virtual DbSet<Documento> Documento { get; set; }
        public virtual DbSet<Empresa> Empresa { get; set; }
        public virtual DbSet<DocumentoHistorico> DocumentoHistorico { get; set; }
    }
}
