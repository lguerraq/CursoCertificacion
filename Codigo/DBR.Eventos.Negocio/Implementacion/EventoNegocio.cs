using DBR.Evento.Modelo;
using DBR.Evento.Modelo.Request;
using DBR.Evento.Modelo.Response;
using DBR.Eventos.Datos.Implementacion;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DBR.Eventos.Negocio.Implementacion
{
    public class EventoNegocio
    {
        EventoDatos _eventoDatos = new EventoDatos(new DbContext("db_ingenierosEntities"));
        public EventoNegocio()
        {

        }
        public Paged<EventoResponse> ListEvento(PageRequest page)
        {
            var response = _eventoDatos.ListEvento(page);
            return response;
        }
        public List<ComboResponse> ListEventoCombo()
        {
            var response = _eventoDatos.ListEventoCombo();
            return response;
        }
        public List<ComboResponse> ListAllEventoCombo()
        {
            var response = _eventoDatos.ListAllEventoCombo();
            return response;
        }
        public List<EventoResponse> ListEventoActivos()
        {
            var response = _eventoDatos.ListEventoActivos();
            return response;
        }
        public Result SaveEvento(EventoRequest request, UsuarioLogin user)
        {
            var response = _eventoDatos.SaveEvento(request,user);
            return response;
        }
        public Result UpdateEvento(EventoRequest request, UsuarioLogin user)
        {
            var response = _eventoDatos.UpdateEvento(request,user);
            return response;
        }
        public Result UpdateCursoFiles(EventoRequest request, UsuarioLogin user)
        {
            var response = _eventoDatos.UpdateCursoFiles(request, user);
            return response;
        }
        public Result DeleteEvento(EventoRequest request, UsuarioLogin user)
        {
            var response = _eventoDatos.DeleteEvento(request,user);
            return response;
        }
        public Result UpdateEstadoEvento(EventoRequest request, UsuarioLogin user)
        {
            var response = _eventoDatos.UpdateEstadoEvento(request,user);
            return response;
        }
        public EventoResponse GetEvento(EventoRequest request)
        {
            var response = _eventoDatos.GetEvento(request);
            return response;
        }
        //Virtual
        public EventoResponse GetEventoByRowId(EventoRequest request,UsuarioLogin user)
        {
            var response = _eventoDatos.GetEventoByRowId(request, user);
            return response;
        }
        public Paged<EventoResponse> ListEventoAsignacionPaged(PageRequest page)
        {
            var response = _eventoDatos.ListEventoAsignacionPaged(page);
            return response;
        }
        public List<EventoResponse> ListEventoUsuario(UsuarioLogin user)
        {
            var response = _eventoDatos.ListEventoUsuario(user);
            return response;
        }
        public Paged<EventoResponse> ListEventoUsuarioPaged(PageRequest page, UsuarioLogin user)
        {
            var response = _eventoDatos.ListEventoUsuarioPaged(page, user);
            return response;
        }
        //EventoUsuario
        public Paged<EventoUsuarioResponse> ListEventoUsuarioAsignadoPaged(PageRequest page, EventoUsuarioRequest request)
        {
            var response = _eventoDatos.ListEventoUsuarioAsignadoPaged(page, request);
            return response;
        }
        public List<EventoUsuarioResponse> ListEventoUsuarioAsignado(EventoUsuarioRequest request)
        {
            var response = _eventoDatos.ListEventoUsuarioAsignado(request);
            return response;
        }
        public EventoUsuarioResponse GetEventoUsuario(EventoUsuarioRequest request)
        {
            var response = _eventoDatos.GetEventoUsuario(request);
            return response;
        }
        public Result SaveEventoUsuario(EventoUsuarioRequest request, UsuarioLogin user, List<int> requestVideos)
        {
            var response = _eventoDatos.SaveEventoUsuario(request, user, requestVideos);
            return response;
        }
        public Result UpdateEventoUsuario(EventoUsuarioRequest request, UsuarioLogin user, List<int> requestVideos)
        {
            var response = _eventoDatos.UpdateEventoUsuario(request, user, requestVideos);
            return response;
        }
        public Result UpdateEventoAccedidoUsuario(EventoUsuarioRequest request, UsuarioLogin user)
        {
            var response = _eventoDatos.UpdateEventoAccedidoUsuario(request, user);
            return response;
        }
        public Result DeleteEventoUsuario(EventoUsuarioRequest request, UsuarioLogin user)
        {
            var response = _eventoDatos.DeleteEventoUsuario(request, user);
            return response;
        }
        //Participacion
        public Paged<EventoResponse> ListEventosPorPersonaPaged(PageRequest page, PersonaRequest request)
        {
            var response = _eventoDatos.ListEventosPorPersonaPaged(page, request);
            return response;
        }
        public Paged<EventoResponse> ListEventosPorCodigoPaged(PageRequest page, PersonaRequest request)
        {
            var response = _eventoDatos.ListEventosPorCodigoPaged(page,request);
            return response;
        }
        //EventoCorroe
        public Result SaveEventoCorreo(EventoCorreoRequest request, UsuarioLogin user)
        {
            var response = _eventoDatos.SaveEventoCorreo(request, user);
            return response;
        }
        public Result UpdateCorreo(EventoCorreoRequest request, UsuarioLogin user)
        {
            var response = _eventoDatos.UpdateCorreo(request, user);
            return response;
        }
        public EventoCorreoResponse GeEventoCorreoByIdEvento(EventoCorreoRequest request)
        {
            var response = _eventoDatos.GeEventoCorreoByIdEvento(request);
            return response;
        }
        //Modulo
        public Paged<ModuloResponse> ListModuloPaged(PageRequest page, ModuloRequest request)
        {
            var response = _eventoDatos.ListModuloPaged(page, request);
            return response;
        }
        public Result SaveModulo(ModuloRequest request, UsuarioLogin user)
        {
            var response = _eventoDatos.SaveModulo(request, user);
            return response;
        }
        public Result UpdateModulo(ModuloRequest request, UsuarioLogin user)
        {
            var response = _eventoDatos.UpdateModulo(request, user);
            return response;
        }
        public Result DeleteModulo(ModuloRequest request, UsuarioLogin user)
        {
            var response = _eventoDatos.DeleteModulo(request, user);
            return response;
        }
        public ModuloResponse GetModulo(ModuloRequest request)
        {
            var response = _eventoDatos.GetModulo(request);
            return response;
        }
        public List<ModuloResponse> ListModulo(ModuloRequest request)
        {
            var response = _eventoDatos.ListModulo(request);
            return response;
        }

        public List<ModuloResponse> ListModuloWithLecciones(ModuloRequest request)
        {
            var result = _eventoDatos.ListModuloWithLecciones(request);

            var Modulos = result.Select(item => new
            {
                IdModulo = item.IdModulo,
                Nombre = item.Nombre,
                Expositor = item.Expositor,
                Peso = item.Peso
            }).Distinct().OrderBy(x => x.IdModulo).Select(x => new ModuloResponse
            {
                IdModulo = x.IdModulo,
                Nombre = x.Nombre,
                Expositor = x.Expositor,
                Peso = x.Peso
            }).ToList();

            for (int i = 0; i < Modulos.Count(); i++)
            {
                Modulos[i].Lecciones = result.Where(x => x.IdModulo == Modulos[i].IdModulo).OrderBy(x => x.Orden).Select(item => new LeccionResponse { IdModulo = item.IdModulo, IdLeccion = item.IdLeccion, Nombre = item.NombreLeccion, Tipo = item.Tipo, Duracion = item.Duracion }).ToList();
            }
            return Modulos;
        }


        //Leccion
        public Paged<LeccionResponse> ListLeccionPaged(PageRequest page, LeccionRequest request)
        {
            var response = _eventoDatos.ListLeccionPaged(page, request);
            return response;
        }

        public Result SaveLeccion(LeccionRequest request, UsuarioLogin user)
        {
            Result response;
            if (request.Tipo == 3) //Tipo cuestionario
            {
                response = _eventoDatos.SaveLeccionCuestionario(request, user);
            }
            else
            {
                response = _eventoDatos.SaveLeccion(request, user);
            }
            return response;
        }

        public Result UpdateLeccion(LeccionRequest request, UsuarioLogin user)
        {
            var response = _eventoDatos.UpdateLeccion(request, user);
            return response;
        }

        public Result DeleteLeccion(LeccionRequest request, UsuarioLogin user)
        {
            var response = _eventoDatos.DeleteLeccion(request, user);
            return response;
        }

        public LeccionResponse GetLeccion(LeccionRequest request)
        {
            var response = _eventoDatos.GetLeccion(request);
            return response;
        }
        public LeccionResponse GetLeccionByEventoActivo(LeccionRequest request)
        {
            var response = _eventoDatos.GetLeccionByEventoActivo(request);
            return response;
        }
        public LeccionResponse GetPrevLeccion(LeccionRequest request)
        {
            var response = _eventoDatos.GetPrevLeccion(request);
            return response;
        }

        public LeccionResponse GetNextLeccion(LeccionRequest request)
        {
            var response = _eventoDatos.GetNextLeccion(request);
            return response;
        }

        public List<LeccionResponse> ListLeccion(LeccionRequest request)
        {
            var response = _eventoDatos.ListLeccion(request);
            return response;
        }

        #region EventoInscripcion
        public Result ValidarFinalizacionEvento(InscripcionRequest request, UsuarioLogin user)
        {
            var response = _eventoDatos.ValidarFinalizacionEvento(request, user);
            return response;
        }
        public Result CerrarEventoUsuario(InscripcionRequest request, UsuarioLogin user)
        {
            var response = _eventoDatos.CerrarEventoUsuario(request, user);
            return response;
        }
        public InscripcionResponse GetEventoInscripcion(InscripcionRequest request, UsuarioLogin user)
        {
            var response = _eventoDatos.GetEventoInscripcion(request, user);
            return response;
        }
        #endregion


        #region METODO AVAS CONSULTORES
        public List<EventoResponse> ListEventoActivoAvas()
        {
            var response = _eventoDatos.ListEventoActivoAvas();
            return response;
        }
        public EventoResponse GetEventoDetalleByRowIdAvast(EventoRequest request)
        {
            var response = _eventoDatos.GetEventoDetalleByRowIdAvast(request);
            return response;
        }
        #endregion
        #region METODOS 100% INGENIEROS
        public List<EventoResponse> ListUltimosEvento()
        {
            var response = _eventoDatos.ListUltimosEvento();
            return response;
        }
        public EventoResponse GetEventoDetalleByRowId(EventoRequest request)
        {
            var response = _eventoDatos.GetEventoDetalleByRowId(request);
            return response;
        }
        public List<ModuloWebResponse> GetModuloLeccionByIdEvento(EventoRequest request)
        {
            var response = _eventoDatos.GetModuloLeccionByIdEvento(request);
            return response;
        }
        public List<DocenteResponse> LisDocentes()
        {
            var response = _eventoDatos.LisDocentes();
            return response;
        }
        public DocenteResponse GetDocenteByRowId(DocenteRequest request)
        {
            var response = _eventoDatos.GetDocenteByRowId(request);
            return response;
        }
        //Cursos
        public List<EventoResponse> ListEventoFiltrado(List<int> IdsCategoria, List<int> IdsTema)
        {
            var response = _eventoDatos.ListEventoFiltrado(IdsCategoria, IdsTema);
            return response;
        }
        //Participante
        public List<EventoResponse> ListEventosPorPersona(PersonaRequest request)
        {
            var response = _eventoDatos.ListEventosPorPersona(request);
            return response;
        }
        public List<EventoResponse> ListEventosPorCodigo(PersonaRequest request)
        {
            var response = _eventoDatos.ListEventosPorCodigo(request);
            return response;
        }
        #endregion
    }
}
