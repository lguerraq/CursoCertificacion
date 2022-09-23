namespace DBR.Eventos.Comun
{
    public class Message
    {
        //Inicio sesion
        public const string DatosIncorrectos = "Usuario y/o contraseña invalida";
        public const string DatosCorrectos = "Usuario y contraseña correcto";
        public const string TextoImagenIncorrecto = "El texto de la imagen incorrecto";
        public const string TextoImagenCaducado = "El texto de la imagen ha caducado, actualice";
        public const string SesionActiva = "Ya se encuentra una sesión iniciada, intentalo nuevamente en 5 minutos";
        public const string ErrorCapcha = "Google reCapcha: Comuníquese con el administrador del sistema";

        //Exepxiones
        public const string ErrorNoControlado = "Error no contrololado";
        public const string ExitoGuardar = "La información se guardo correctamente";
        public const string ErrorGuardar = "Hubo un error al guardar la información";
        public const string ExitoActualizar = "La información se actualizo correctamente";
        public const string ErrorActualizar = "Hubo un error al actualizar la información";
        public const string ExitoEliminar = "La información se elimino correctamente";
        public const string ErrorEliminar = "Hubo un error al eliminar la información";

        //Documentos
        public const string ErrorGuardarDocumentos = "Hubo un error al guardar los documentos";
        public const string ErrorDescargarDocumentos = "No se encontró el documento";
        public const string ExisteDocumento = "Ya existe un documento con el mismo nombre";
        public const string ExisteCarpeta = "Ya existe una carpeta con el mismo nombre";

        //Persona
        public const string ExistePersonaDni = "Ya existe una persona con el número de documento ingresado";
        public const string ExistePersonaInscripcion = "No se puede eliminar la persona, ya que esta inscrita un curso";

        //Inscripciones
        public const string ExistePersonaInscrita = "El numero de documento ya se encuentra registrado en el curso";
        public const string ExitoCargarCertificado = "El certificado se cargo correctamente";

        //Evento
        public const string ExistePersonas = "No se pudo eliminar, existe personas registradas";
        public const string ExisteEventoActivo = "Ya existe un curso activo, solo se permite uno activo";
        public const string GeneracionCertificado = "Aún no se encuentra habilitado la generación de certificados";

        //Correo
        public const string ErrorDeFormatoCorreo = "El correo no tiene el formato adecuado";
        public const string ExitoCorreoEnviado = "Se envio el correo correctamente";

        //Plantilla persona
        public const string ErrorGuardarPlantilla = "Hubo un error al procesar la plantilla";
        public const string ErrorProcesarPlantilla = "Hubo un error al procesar la plantilla";

        //Contraseña
        public const string ErrorOldPassword = "La contraseña antigua no es correcta.";
        public const string ExitoCambioPassword = "La contraseña se cambio correctamente.";
        public const string ErrorMismoPassword = "La nueva contraseña tiene que ser distinta a la actual";
        public const string ErrorConplejidadPassword = "La contraseña tiene que tener entre 8 y 15 caracteres, contener como minimo 1 Mayuscula, 1 Minuscula y Número";

        //Usuario
        public const string UsuarioExiste = "Ya existe un usuario con el los datos ingreado";
        public const string UsuarioExisteDni = "Ya existe un registro con el número de documento ingresado";
        public const string UsuarioPersona = "No se encontro correo valido, revisar si la persona existe";

        //EventoUsuario
        public const string EventoUsuarioExiste = "El usuario ya se encuentra asignado";
        public const string EventoUsuarioExisteIncripto = "El usuario no tiene una inscripción en el curso";


        public const string DocumentoNoPermitido = "El formato de archivo no está permitido o el archivo esta dañado.";

        //ValidarCuestionario
        public const string FaltaExamentes = "Completar todas las evaluaciones antes de generar el certificado";
        public const string NoCumpleNota = "No cumple la nota aprobatoria para el curso";

        //EventoCorreo
        public const string ErrorEnviarCorre = "No se encontro un correo configurado";

        //Empresa
        public const string ErrorRucRegistrado = "Ya se encuentra registrado una empresa con el ruc ingresado";
        public const string ErrorUsuarioConEmpresa = "El responsable ingresado ya se encuentra asociado a una empresa";
    }
    public class MessageExcel
    {
        public const string ErrorInformacionVacia = "Se encontró información vacía";
        public const string ErrorCorreo = "Correo no valido";
        public const string NumeroDocumentoIncorrecto = "Número de documento incorrecto";
    }

}
