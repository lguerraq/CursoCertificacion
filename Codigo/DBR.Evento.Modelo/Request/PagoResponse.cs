namespace DBR.Evento.Modelo.Request
{
    public class PagoResponse
    {
        public double creation_date { get; set; }
        public source source { get; set; }
        public outcome outcome { get; set; }
        public string reference_code { get; set; }
    }
    public class outcome
    {
        public string type { get; set; }
        public string code { get; set; }
        public string merchant_message { get; set; }
        public string user_message { get; set; }
    }
    public class source
    {
        public iin iin { get; set; }
    }
    public class iin
    {
        public string bin { get; set; }
        public string card_brand { get; set; }
        public string card_type { get; set; }
        public string card_category { get; set; }
        public issuer issuer { get; set; }
    }
    public class issuer
    {        
        public string name { get; set; }
    }
}
