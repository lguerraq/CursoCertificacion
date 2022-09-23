using System.Collections.Generic;

namespace DBR.Evento.Modelo.Response
{
    public class PagedResponse<T>
    {
        public List<T> data { get; set; }
        public int draw { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
    }
}
