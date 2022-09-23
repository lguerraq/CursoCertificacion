namespace DBR.Eventos.Comun
{
    public class Enumeraciones
    {
    }
    public class EstadoRegistro
    {
        public const bool Activo = true;
        public const bool Inactivo = false;
    }
    public class EstadoActivo
    {
        public const bool Activo = true;
        public const bool Inactivo = false;
    }
    public class EstadoCorreo
    {
        public const int Creado = 1;
        public const int Ejecutandose = 2;
        public const int Finalizado = 3;
    }

    public class Errores
    {
        public string Mensaje { get; set; }
        public int TipoError { get; set; }
        public int Posicion { get; set; }
    }
    public enum Perfil
    {
        Administrador = 1,
        SubAdministrador = 2,
        Inscripcion = 3,
        Participante = 4,
        EditorContenido = 5
    }
    public enum FormatoColumnPersona
    {
        Numero = 1,
        NumeroDocumento = 2,
        Cip = 3,
        Nombres = 4,
        ApellidoPaterno = 5,
        ApellidoMaterno = 6,
        Celular = 7,
        Correo = 8,
        Profesion = 9,
        OficioOcupacion = 10,
        OficioOcupacionDescripcion = 11
    }
    public class NombreCarpeta
    {
        public const string Galeria = "Galeria";
        public const string Portada = "Portada";
        public const string Evento = "Evento";
        public const string Suceso = "Suceso";
        public const string EventoCertificado = "Evento/Certificado";
        public const string EventoCertificadoFirmado = "Evento/CertificadoFirmado";
        public const string Documento = "Documento";
    }
}
