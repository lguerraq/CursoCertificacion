using DBR.Evento.Modelo.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBR.Eventos.Presentacion.Models
{
    public class CursoDashboardViewModel
    {
        public int TotalCursos { get; set; }

        public int NuevosCursos { get; set; }

        public List<EventoResponse> UltimosCursos { get; set; }
    }
}