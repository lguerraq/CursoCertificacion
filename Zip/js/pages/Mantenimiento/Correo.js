var Enviados = 0;
var Total = 0;
var Contador = 0;
var IniciarValidacion = false;
var contenidoHtml = "";

var config = {
    ini: function () {
        $.ajaxSetup({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: {}
        });
    },
    iniGrids: {
        GrillaCorreos: function () {
            $('#gridCorreo').DataTable({
                "searching": false,
                "ordering": false,
                "processing": true,
                "serverSide": true,
                "destroy": true,
                "responsive": true,
                "stateSave": true,
                "language": {
                    url: '//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json'
                },
                "ajax": {
                    "url": urlListCorreoPaged,
                    "type": 'POST',
                    "datatype": "json",
                    "data": {},
                },
                "columns": [
                            {
                                "data": function (row, type, set, meta) {
                                    return '<div style="min-width: 200px;white-space: normal;">' + row.Asunto + '</div>';
                                }, "sClass": 'text-left'
                            },
                            { "data": "Origen", "sClass": 'text-left' },
                            { "data": "NombreOrigen", "sClass": 'text-left' },
                            {
                                "data": function (row, type, set, meta) {
                                    return parseJsonDate(row.FechaEnvio);
                                }, "sClass": 'text-center'
                            },
                            { "data": "NumeroEnvio", "sClass": 'text-center' },
                            { "data": "Cantidad", "sClass": 'text-center' },
                            {
                                "data": function (row, type, set, meta) {
                                    
                                    var editar = '<a data-toggle="tooltip1" data-placement="bottom" title="Editar" onclick="Eventos.GetCorreo(' + row.IdCorreo + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></a>';
                                    var eliminar = '<a data-toggle="tooltip1" data-placement="bottom" title="Eliminar" onclick="Eventos.DeleteCorreo(' + row.IdCorreo + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-trash" aria-hidden="true"></span></a>';
                                    var ver = '<a data-toggle="tooltip1" data-placement="bottom" title="Vista previa" onclick="Eventos.VerCorreo(' + row.IdCorreo + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-eye-open" aria-hidden="true"></span></a>';
                                    var enviar = '<a data-toggle="tooltip1" data-placement="bottom" title="Envio masivo" onclick="Eventos.EnviarCorreo(' + row.IdCorreo + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-envelope" aria-hidden="true"></span></a>';
                                    var enviarFaltantes = '<a data-toggle="tooltip1" data-placement="bottom" title="Enviar faltantes" onclick="Eventos.EnviarCorreoFaltantes(' + row.IdCorreo + ')" class="btn btn-primary btn-xs"><span class="fa fa-external-link" aria-hidden="true"></span></a>';
                                    var EnvioIndividual = '<a data-toggle="tooltip1" data-placement="bottom" title="Envio individual" onclick="Eventos.ModalCorreoIndividual(' + row.IdCorreo + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-share-alt" aria-hidden="true"></span></a>';
                                    var EnvioPorProfesion = '<a data-toggle="tooltip1" data-placement="bottom" title="Envio profesión" onclick="Eventos.ModalCorreoProfesion(' + row.IdCorreo + ')" class="btn btn-primary btn-xs"><span class="fa fa-group" aria-hidden="true"></span></a>';
                                    
                                    return ver + " " + enviar + " " + enviarFaltantes + " " + EnvioIndividual + " " + EnvioPorProfesion + " " + editar + " " + eliminar;
                                }, "sClass": 'text-center'
                            }
                ],
                "drawCallback": function (settings) {
                    $('[data-toggle="tooltip1"]').tooltip({ container: "body" });
                }
            });
        },
    }
}

var Eventos = {
    ValoresIniciales: function () {

        DBR.blockUIStar();

        $.ajax({
            url: urlValoresInicialesCorreo,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                DBR.LlenarCombo(response.Profesion, "#cboProfesion", true, true, "Seleccione");
                DBR.blockUIStop();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
            }
        });
    },
    SaveCorreo: function () {
        var request = new Object();
        request.IdCorreo = $("#txtIdCorreo").val();
        request.Asunto = $.trim($("#txtAsuntoMensaje").val().toUpperCase());
        request.Origen = $.trim($("#txtCorreoOrigen").val().toLowerCase());
        request.NombreOrigen = $.trim($("#txtNombreOrigen").val().toUpperCase());
        request.Mensaje = contenidoHtml;

        DBR.blockUIStar();

        $.ajax({
            url: urlSaveCorreo,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();
                if (response.IsSuccess) {
                    $('#gridCorreo').DataTable().ajax.reload();
                    DBR.ToastSuccess(response.Message);
                    $("#modalAgregarCorreo").modal("hide");
                } else {
                    DBR.ToastError(response.Message);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                DBR.blockUIStop();
                console.log(textStatus);
            }
        });
    },
    DeleteCorreo: function (IdCorreo) {
        var request = new Object();
        request.IdCorreo = IdCorreo;

        BootstrapDialog.show({
            title: 'Confirmación',
            closeByBackdrop: false,
            message: '¿Esta seguro de eliminar el registro?',
            size: BootstrapDialog.SIZE_SMALL,
            buttons: [
                {
                    label: 'Si',
                    cssClass: 'btn-primary btn-sm',
                    action: function (dialogItself) {

                        DBR.blockUIStar();
                        $.ajax({
                            url: urlDeleteCorreo,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ "request": request }),
                            success: function (response) {
                                DBR.blockUIStop();

                                dialogItself.close();
                                if (response.IsSuccess) {
                                    $('#gridCorreo').DataTable().ajax.reload();
                                    DBR.ToastSuccess(response.Message);
                                } else {
                                    DBR.ToastError(response.Message);
                                }
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                DBR.blockUIStop();
                                console.log(textStatus);
                            }
                        });
                    }
                },
                {
                    label: 'No',
                    cssClass: 'btn-default btn-sm',
                    action: function (dialogItself) {
                        dialogItself.close();
                    }
                }]
        });
    },
    GetCorreo: function (IdCorreo) {
        var request = new Object();
        request.IdCorreo = IdCorreo;
        DBR.blockUIStar();
        $.ajax({
            url: urlGetCorreo,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();
                Funciones.LimpiarControlesEditar();
                $("#txtIdCorreo").val(response.IdCorreo);
                $("#txtAsuntoMensaje").val(response.Asunto);
                $("#txtCorreoOrigen").val(response.Origen);
                $("#txtNombreOrigen").val(response.NombreOrigen);
                response.Mensaje = response.Mensaje.replace(/div/g, "p");
                CKEDITOR.instances.txtMensajeCompleto.setData(response.Mensaje);
                contenidoHtml = response.Mensaje;
                $("#modalAgregarCorreo").modal("show");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                DBR.blockUIStop();
                console.log(textStatus);
            }
        });
    },
    EnviarCorreo: function (IdCorreo) {
        var request = new Object();
        request.IdCorreo = IdCorreo;

        BootstrapDialog.show({
            title: 'Confirmación',
            closeByBackdrop: false,
            message: 'Una vez iniciado el envio no podra detenerlo </br></br> ¿Esta seguro de continuar?',
            size: BootstrapDialog.SIZE_SMALL,
            buttons: [
                {
                    label: 'Si',
                    cssClass: 'btn-primary btn-sm',
                    action: function (dialogItself) {
                        dialogItself.close();

                        $("#modalProcesoEnvio").modal("show");

                        $.ajax({
                            url: urlEnviarCorreoMasivo,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ "request": request }),
                            timeout: 3600000,
                            success: function (response) {
                                $("#modalProcesoEnvio").modal("hide");
                                if (response.IsSuccess) {
                                    DBR.MensajeInformativoSmall(response.Message, "Resultado");
                                    $('#gridCorreo').DataTable().ajax.reload();
                                } else {
                                    DBR.ToastError(response.Message);
                                }
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                console.log(textStatus);
                            }
                        });                                               
                        
                    }
                },
                {
                    label: 'No',
                    cssClass: 'btn-default btn-sm',
                    action: function (dialogItself) {
                        dialogItself.close();
                    }
                }]
        });
    },
    EnviarCorreoFaltantes: function (IdCorreo) {
        var request = new Object();
        request.IdCorreo = IdCorreo;

        BootstrapDialog.show({
            title: 'Confirmación',
            closeByBackdrop: false,
            message: 'Una vez iniciado el envio no podra detenerlo </br></br> ¿Esta seguro de continuar?',
            size: BootstrapDialog.SIZE_SMALL,
            buttons: [
                {
                    label: 'Si',
                    cssClass: 'btn-primary btn-sm',
                    action: function (dialogItself) {
                        dialogItself.close();

                        $("#modalProcesoEnvio").modal("show");

                        $.ajax({
                            url: urlEnviarCorreoMasivoFaltantes,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ "request": request }),
                            timeout: 3600000,
                            success: function (response) {
                                $("#modalProcesoEnvio").modal("hide");
                                if (response.IsSuccess) {
                                    DBR.MensajeInformativoSmall(response.Message, "Resultado");
                                    $('#gridCorreo').DataTable().ajax.reload();
                                } else {
                                    DBR.ToastError(response.Message);
                                }
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                console.log(textStatus);
                            }
                        });

                    }
                },
                {
                    label: 'No',
                    cssClass: 'btn-default btn-sm',
                    action: function (dialogItself) {
                        dialogItself.close();
                    }
                }]
        });
    },
    VerCorreo: function (IdCorreo) {
        var request = new Object();
        request.IdCorreo = IdCorreo;


        DBR.blockUIStar();

        $.ajax({
            url: urlGetCorreo,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();
                $("#mensajePreview").html(response.Mensaje);
                $("#modalVerCorreo").modal("show");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                DBR.blockUIStop();
                console.log(textStatus);
            }
        });
    },
    SaveFileServer: function () {

        var input = document.getElementById("inputFileServer");

        var formData = new FormData();
        formData.append("documento", input.files[0]);

        DBR.blockUIStar();

        $.ajax({
            url: urlSaveFileServer,
            type: "POST",
            dataType: "json",
            cache: false,
            contentType: false,
            processData: false,
            data: formData
        }).done(function (response) {
            DBR.blockUIStop();
            $("#inputFileServer").filestyle('clear');
            if (response.IsSuccess) {
                $("#txtLink").val(response.Message);
            } else {
                $("#txtLink").val("");
                GISSAT.ToastError(response.Message);
            }
        }).fail(function (XMLHttpRequest) {
            DBR.blockUIStop();
            console.log(XMLHttpRequest);
        });
    },
    ModalCorreoIndividual: function (IdCorreo) {
        Funciones.LimpiarControlesPersona();
        $("#txtIdCorreoEnvioIndividual").val(IdCorreo);
        $("#modalEnvioIndividual").modal("show");
    },
    ModalCorreoProfesion: function (IdCorreo) {
        DBR.limpiarCampo("#cboProfesion");
        $("#txtIdCorreo2").val(IdCorreo);
        $("#modalEnvioPorProfesion").modal("show");
    },
    EnviarCorreoPorProfesion: function () {
        var request = new Object();
        request.IdCorreo = $("#txtIdCorreo2").val();

        var IdsProfesion = new Object();
        IdsProfesion = $("#cboProfesion").val();
      
        BootstrapDialog.show({
            title: 'Confirmación',
            closeByBackdrop: false,
            message: 'Una vez iniciado el envio no podra detenerlo </br></br> ¿Esta seguro de continuar?',
            size: BootstrapDialog.SIZE_SMALL,
            buttons: [
                {
                    label: 'Si',
                    cssClass: 'btn-primary btn-sm',
                    action: function (dialogItself) {
                        dialogItself.close();

                        $("#modalProcesoEnvio").modal("show");

                        $.ajax({
                            url: urlEnviarCorreoMasivoPorProfesion,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ "request": request, "IdsProfesion": IdsProfesion }),
                            timeout: 3600000,
                            success: function (response) {
                                $("#modalEnvioPorProfesion").modal("hide");
                                $("#modalProcesoEnvio").modal("hide");
                                if (response.IsSuccess) {
                                    DBR.MensajeInformativoSmall(response.Message, "Resultado");
                                    $('#gridCorreo').DataTable().ajax.reload();
                                } else {
                                    DBR.ToastError(response.Message);
                                }
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                console.log(textStatus);
                            }
                        });

                    }
                },
                {
                    label: 'No',
                    cssClass: 'btn-default btn-sm',
                    action: function (dialogItself) {
                        dialogItself.close();
                    }
                }]
        });
    },
    GetPersona: function () {
        var request = new Object();
        request.NumeroDocumento = $("#txtDni").val();

        $.ajax({
            url: urlGetPersonaXdni,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                if (response.length > 0) {
                    $("#txtNombres").val(response[0].Nombres);
                    $("#txtApellidoPaterno").val(response[0].ApellidoPaterno);
                    $("#txtApellidoMaterno").val(response[0].ApellidoMaterno);
                    $("#txtCip").val(response[0].CIP);
                    $("#txtCorreo").val(response[0].Correo);
                    DBR.ToastSuccess("Persona encontrada");
                } else {
                    $("#txtNombres").val("");
                    $("#txtApellidoPaterno").val("");
                    $("#txtApellidoMaterno").val("");
                    $("#txtCip").val("");
                    $("#txtCorreo").val("");
                    DBR.ToastWarning("No se encontro persona");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
            }
        });
    },
    GetPersonaXcip: function () {
        var request = new Object();
        request.Cip = $("#txtCip").val();

        $.ajax({
            url: urlGetPersonaXcip,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                if (response.length > 0) {
                    $("#txtDni").val(response[0].NumeroDocumento);
                    $("#txtNombres").val(response[0].Nombres);
                    $("#txtApellidoPaterno").val(response[0].ApellidoPaterno);
                    $("#txtApellidoMaterno").val(response[0].ApellidoMaterno);
                    $("#txtCip").val(response[0].CIP);
                    $("#txtCorreo").val(response[0].Correo);
                    DBR.ToastSuccess("Persona encontrada");
                } else {
                    $("#txtDni").val("");
                    $("#txtNombres").val("");
                    $("#txtApellidoPaterno").val("");
                    $("#txtApellidoMaterno").val("");
                    $("#txtCorreo").val("");
                    DBR.ToastWarning("No se encontro persona");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
            }
        });
    },
    EnviarCorreoIndividual: function () {
        var request = new Object();
        request.IdCorreo = $("#txtIdCorreoEnvioIndividual").val();

        var Persona = new Object();
        Persona.NumeroDocumento = $.trim($("#txtDni").val());
        Persona.Nombres = $.trim($("#txtNombres").val().toUpperCase());
        Persona.ApellidoPaterno = $.trim($("#txtApellidoPaterno").val().toUpperCase());
        Persona.ApellidoMaterno = $.trim($("#txtApellidoMaterno").val().toUpperCase());
        Persona.CIP = $.trim($("#txtCip").val());
        Persona.Correo = $.trim($("#txtCorreo").val().toUpperCase());

        DBR.blockUIStar();
        $.ajax({
            url: urlEnviarCorreoIndividual,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request, "Persona": Persona }),
            success: function (response) {
                DBR.blockUIStop();
                if (response.IsSuccess) {
                    $('#gridCorreo').DataTable().ajax.reload(null, false);
                    $("#modalEnvioIndividual").modal("hide");
                    DBR.ToastSuccess(response.Message);
                } else {
                    DBR.ToastError(response.Message);
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                DBR.blockUIStop();
                console.log(textStatus);
            }
        });
    }
}
var Funciones = {
    LimpiarControles: function () {
        DBR.limpiarCampo("#txtAsuntoMensaje");
        DBR.limpiarCampo("#txtCorreoOrigen");
        DBR.limpiarCampo("#txtNombreOrigen");
        $("#inputFileServer").filestyle('clear');
        $("#txtLink").val("");
        CKEDITOR.instances.txtMensajeCompleto.setData("");
        contenidoHtml = "";
    },
    LimpiarControlesEditar: function () {
        DBR.limpiarCampo("#txtAsuntoMensaje");
        DBR.limpiarCampo("#txtCorreoOrigen");
        DBR.limpiarCampo("#txtNombreOrigen");
        $("#inputFileServer").filestyle('clear');
        $("#txtLink").val("");
    },
    LimpiarControlesPersona: function () {
        $("#formEnviarIndividual .validar").each(function () {
            DBR.limpiarCampo(this);
        });
        $("#txtCip").val("");
    }
}
$(document).ready(function () {
    $.fn.modal.Constructor.prototype.enforceFocus = function () { };

    config.iniGrids.GrillaCorreos();
    Eventos.ValoresIniciales();

    var controlMensComp = CKEDITOR.replace('txtMensajeCompleto', {
        height: 500,
        filebrowserBrowseUrl : urlFileServerBrowse,
        filebrowserUploadUrl: urlSaveFileServerCkEditor
    });

    controlMensComp.on('change', function (evt) {
        contenidoHtml = evt.editor.getData();
    });
    $("#btnAgregarCorreo").on("click", function () {
        Funciones.LimpiarControles();
        $("#txtCorreoOrigen").val(CorreoOrigen);
        $("#txtNombreOrigen").val(NombreCorreoOrigen);
        $("#modalAgregarCorreo").modal("show");
    });
    $("#btnGuardarCorreo").on("click", function () {
        var validar = true;
        if (!DBR.validarCampo("#txtAsuntoMensaje")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtCorreoOrigen")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtNombreOrigen")) {
            validar = false;
        }
        if ($.trim(contenidoHtml).length == 0) {
            contenidoHtml = "<p></p>";
        }
        if (validar) {
            Eventos.SaveCorreo();
        }
    });
    $("#inputFileServer").on("change", function () {

        var input = document.getElementById("inputFileServer");
        if (input.files.length == 0) {
            GISSAT.ToastWarning("No se selecciono ningún archivo");
            return false;
        }
        if (input.files[0].size > 4194304) {
            GISSAT.ToastWarning("Verifique que el archivo no supere los 4MB");
            $("#inputFileServer").filestyle('clear');
            $("#txtLink").val("");
            return false;
        }
        Eventos.SaveFileServer();
    });
    $("#txtDni").keypress(function (e) {
        if (e.which == 13) {
            Eventos.GetPersona();
        }
    });
    $("#txtCip").keypress(function (e) {
        if (e.which == 13) {
            Eventos.GetPersonaXcip();
        }
    });
    $("#btnBuscarPorDni").on("click", function () {
        var valor = $("#txtDni").val().length;
        if (valor >= 8) {
            Eventos.GetPersona();
        } else {
            DBR.ToastWarning("El dni no puede ser menos a 8 dígitos");
        }
    });
    $("#btnBuscarPorCip").on("click", function () {
        var valor = $("#txtCip").val().length;
        if (valor > 4) {
            Eventos.GetPersonaXcip();
        } else {
            DBR.ToastWarning("El CIP no puede ser menos a 4 dígitos");
        }
    });
    $("#btnEnviarIndividual").on("click", function () {
        var validar = true;
        $("#formEnviarIndividual .validar").each(function () {
            if (!DBR.validarCampo(this)) {
                validar = false;
            }
        });
        if ($.trim($("#txtDni").val()) != "") {
            if ($.trim($("#txtDni").val()).length < 8) {
                DBR.ToastWarning("El dni no puede ser menos a 8 dígitos");
                validar = false;
            }
        }

        if (validar) {
            Eventos.EnviarCorreoIndividual();
        }
    });
    $("#btnEnviarPorProfesion").on("click", function () {
        if (DBR.validarCampo("#cboProfesion")) {
            Eventos.EnviarCorreoPorProfesion();
        }
    });
});