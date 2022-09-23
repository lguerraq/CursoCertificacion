using DBR.Evento.Modelo;
using DBR.Evento.Modelo.Request;
using DBR.Evento.Modelo.Response;
using DBR.Eventos.Comun;
using DBR.Eventos.Datos.Implementacion;
using DBR.Eventos.Negocio.BaseError;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace DBR.Eventos.Negocio.Implementacion
{
    public class EmpresaNegocio
    {
        EmpresaDatos _EmpresaDatos = new EmpresaDatos(new DbContext("db_ingenierosEntities"));
        UsuarioDatos _UsuarioDatos = new UsuarioDatos(new DbContext("db_ingenierosEntities"));
        LogError error = new LogError();
        public EmpresaNegocio()
        {

        }
        public List<ComboResponse> ListEmpresaCombo()
        {
            var response = _EmpresaDatos.ListEmpresaCombo();
            return response;
        }
        public Paged<EmpresaResponse> ListEmpresaPaginado(PageRequest page)
        {
            var response = _EmpresaDatos.ListEmpresaPaginado(page);
            return response;
        }
        public Result SaveEmpresa(EmpresaRequest request,UsuarioRequest requestuser, UsuarioLogin user)
        {
            try
            {
                var responseusurio = new Result();
                var usurio = _UsuarioDatos.BuscarUsuarioXdni(requestuser);
                if (usurio == null)
                {
                    responseusurio = _UsuarioDatos.SaveUsuario(requestuser, user);
                }
                else
                {
                    requestuser.IdUsuario = usurio.IdUsuario;
                    requestuser.Password = usurio.Password;
                    responseusurio = _UsuarioDatos.UpdateUsuario(requestuser, user);
                }
                if (!responseusurio.IsSuccess)
                {
                    return responseusurio;
                }
                request.IdUsuario = responseusurio.Codigo;
                var response = _EmpresaDatos.SaveEmpresa(request, user);
                return response;           
            }
            catch (Exception ex)
            {
                error.debugError(ex);
                return new Result() { IsSuccess = false, Message = Message.ErrorGuardar };
            }
        }
        public Result UpdateEmpresa(EmpresaRequest request, UsuarioRequest requestuser, UsuarioLogin user)
        {
            try
            {
                var responseusurio = new Result();
                var usurio = _UsuarioDatos.BuscarUsuarioXdni(requestuser);
                if (usurio == null)
                {
                    responseusurio = _UsuarioDatos.SaveUsuario(requestuser, user);
                }
                else
                {
                    requestuser.IdUsuario = usurio.IdUsuario;
                    requestuser.Password = usurio.Password;
                    responseusurio = _UsuarioDatos.UpdateUsuario(requestuser, user);
                }
                if (!responseusurio.IsSuccess)
                {
                    return responseusurio;
                }
                request.IdUsuario = responseusurio.Codigo;
                var response = _EmpresaDatos.UpdateEmpresa(request, user);
                return response;
            }
            catch (Exception ex)
            {
                error.debugError(ex);
                return new Result() { IsSuccess = false, Message = Message.ErrorGuardar };
            }
        }
        public Result DeleteEmpresa(EmpresaRequest request, UsuarioLogin user)
        {
            try
            {
                var response = _EmpresaDatos.DeleteEmpresa(request, user);
                return response;
            }
            catch (Exception ex)
            {
                error.debugError(ex);
                return new Result() { IsSuccess = false, Message = Message.ErrorGuardar };
            }
        }
        public EmpresaResponse GetEmpresa(EmpresaRequest request)
        {
            var response = _EmpresaDatos.GetEmpresa(request);
            return response;
        }
        public EmpresaResponse GetEmpresaByUsuario(UsuarioLogin user)
        {
            var response = _EmpresaDatos.GetEmpresaByUsuario(user);
            return response;
        }
    }
}
