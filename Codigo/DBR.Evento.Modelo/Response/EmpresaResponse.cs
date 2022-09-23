namespace DBR.Evento.Modelo.Response
{
    public class EmpresaResponse
    {
        public int IdEmpresa { get; set; }
        public int? IdUsuario { get; set; }
        public string Ruc { get; set; }
        public string RazonSocial { get; set; }
        public string NombreComercial { get; set; }
        public string DireccionFiscal { get; set; }
        public byte[] Logo { get; set; }
        public int? Frecuencia { get; set; }
        public string Responsable { get; set; }       
        //Usuarios
        public string Usuario { get; set; }
        public string NumeroDocumento { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Correo { get; set; }
    }
}
