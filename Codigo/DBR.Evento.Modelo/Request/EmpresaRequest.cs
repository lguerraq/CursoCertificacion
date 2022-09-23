namespace DBR.Evento.Modelo.Request
{
    public class EmpresaRequest
    {
        public int IdEmpresa { get; set; }
        public int? IdUsuario { get; set; }
        public string Ruc { get; set; }
        public string RazonSocial { get; set; }
        public string NombreComercial { get; set; }
        public string DireccionFiscal { get; set; }
        public byte[] Logo { get; set; }
        public int? Frecuencia { get; set; }
    }
}
