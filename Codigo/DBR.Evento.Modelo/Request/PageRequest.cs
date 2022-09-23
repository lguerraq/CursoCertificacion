namespace DBR.Evento.Modelo.Request
{
    public class PageRequest
    {
        public PageRequest()
        {
            search = new Search();
        }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string Column { get; set; }
        public string Dir { get; set; }
        public string Order { get; set; }
        public Search search { get; set; }
        public int length { get; set; }
        public int start { get; set; } 
    }
    public class Search
    {
        public string value { get; set; }
    }
}
