using DBR.Evento.Modelo.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBR.Eventos.Presentacion.Models
{
    public class CursoDetalleViewModel
    {
        public string UrlVideo { get; set; }

        public int CantidadLecciones { get; set; }

        public int CantidadHoras { get; set; }

        public List<ModuloResponse> Modulos { get; set; }
    }
}