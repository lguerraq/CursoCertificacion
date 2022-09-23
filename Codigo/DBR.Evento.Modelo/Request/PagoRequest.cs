namespace DBR.Evento.Modelo.Request
{
    public class PagoRequest
    {
        public int amount { get; set; }
        public int customer_id { get; set; }
        public string address { get; set; }
        public string address_city { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public int installments { get; set; }
        public string source_id { get; set; }
        public string ruc { get; set; }
    }
}
