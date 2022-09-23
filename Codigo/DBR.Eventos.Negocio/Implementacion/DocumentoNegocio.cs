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
    public class DocumentoNegocio
    {
        DocumentoDatos _DocumentoDatos = new DocumentoDatos(new DbContext("db_ingenierosEntities"));
        LogError error = new LogError();
        public DocumentoNegocio()
        {
            
        }
        public List<DocumentoResponse> ListDocumentoByIdDocumento(DocumentoRequest request)
        {
            var response = _DocumentoDatos.ListDocumentoByIdDocumento(request);
            return response;
        }
        public List<DocumentoResponse> ListDocumentoByIdDocumentoEliminados(DocumentoRequest request)
        {
            var response = _DocumentoDatos.ListDocumentoByIdDocumentoEliminados(request);
            return response;
        }
        public Paged<DocumentoResponse> ListDocumentoPaginado(DocumentoRequest request)
        {
            var response = _DocumentoDatos.ListDocumentoPaginado(request);
            return response;
        }
        public Result SaveDocumento(DocumentoRequest request, UsuarioLogin user)
        {
            try
            {
                var response = _DocumentoDatos.SaveDocumento(request, user);
                return response;
            }
            catch (Exception ex)
            {
                error.debugError(ex);
                return new Result() { IsSuccess = false, Message = Message.ErrorGuardar };
            }
        }
        public Result UpdateDocumento(DocumentoRequest request, UsuarioLogin user)
        {
            try
            {
                var response = _DocumentoDatos.UpdateDocumento(request, user);
                return response;
            }
            catch (Exception ex)
            {
                error.debugError(ex);
                return new Result() { IsSuccess = false, Message = Message.ErrorActualizar };
            }
        }
        public Result DeleteDocumento(DocumentoRequest request, UsuarioLogin user)
        {
            try
            {
                var response = _DocumentoDatos.DeleteDocumento(request, user);
                return response;
            }
            catch (Exception ex)
            {
                error.debugError(ex);
                return new Result() { IsSuccess = false, Message = Message.ErrorEliminar };
            }
        }
        public DocumentoResponse GetDocumento(DocumentoRequest request)
        {
            var response = _DocumentoDatos.GetDocumento(request);
            return response;
        }
        public Result DeleteFisicoDocumento(DocumentoRequest request, UsuarioLogin user, string pathDocumentos)
        {
            try
            {
                var response = _DocumentoDatos.DeleteFisicoDocumento(request, user, pathDocumentos);
                return response;
            }
            catch (Exception ex)
            {
                error.debugError(ex);
                return new Result() { IsSuccess = false, Message = Message.ErrorEliminar };
            }
            
        }
        public Result RestoreDocumento(DocumentoRequest request, UsuarioLogin user)
        {
            try
            {
                var response = _DocumentoDatos.RestoreDocumento(request, user);
                return response;
            }
            catch (Exception ex)
            {
                error.debugError(ex);
                return new Result() { IsSuccess = false, Message = Message.ErrorActualizar };
            }

        }
        public long GetSizeFolder()
        {
            var response = _DocumentoDatos.GetSizeFolder();
            return response;
        }
        //VISTA DE USUARIO
        public Paged<DocumentoResponse> ListDocumentoUsuarioPaginado(DocumentoRequest request, UsuarioLogin user)
        {
            var response = _DocumentoDatos.ListDocumentoUsuarioPaginado(request, user);
            return response;
        }
        public DocumentoResponse GetDocumentoUsuario(DocumentoRequest request, UsuarioLogin user)
        {
            var response = _DocumentoDatos.GetDocumentoUsuario(request, user);
            return response;
        }
        public Result UpdateEstadoDocumento(DocumentoRequest request)
        {
            var response = _DocumentoDatos.UpdateEstadoDocumento(request);
            return response;
        }
    }
}
