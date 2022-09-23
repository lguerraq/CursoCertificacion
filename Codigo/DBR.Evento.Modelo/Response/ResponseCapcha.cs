using System;

namespace DBR.Evento.Modelo.Response
{
    public class ResponseCapcha
    {
        public bool success { get; set; }
        public DateTime? challenge_ts { get; set; }
        public string hostname { get; set; }
        public double? score { get; set; }
        public string action { get; set; }
    }
}
