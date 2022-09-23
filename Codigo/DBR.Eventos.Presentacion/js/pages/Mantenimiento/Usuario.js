var config = {
    iniGrids: {
        GrillaUsuarios: function () {
            $('#gridUsuarios').DataTable({
                "searching": true,
                "ordering": false,
                "processing": true,
                "serverSide": true,
                "destroy": true,
                "responsive": true,
                "language": {
                    url: '//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json'
                },
                "ajax": {
                    "url": urlListUsuarioPaged,
                    "type": 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    "data": function (oPaged) {
                        return JSON.stringify({ "page": oPaged });
                    },
                },
                "columns": [
                    { "data": "Login" },
                    { "data": "Password" },
                    { "data": "Nombres" },
                    { "data": "ApellidoPaterno" },
                    { "data": "ApellidoMaterno" },
                    { "data": "UsuarioTipo" },
                    { "data": function (row, type, set, meta) {
                        
                        var editar = '<a data-toggle="tooltip1" data-placement="bottom" title="Editar" onclick="Eventos.GetUsuario(' + row.IdUsuario + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></a>';
                        var eliminar = '<a data-toggle="tooltip1" data-placement="bottom" title="Eliminar" onclick="Eventos.DeleteUsuario(' + row.IdUsuario + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-trash" aria-hidden="true"></span></a>';
                        var credenciales = '<a data-toggle="tooltip1" data-placement="bottom" title="Credenciales" onclick="Eventos.EnviarCredencialesUsuario(\'' + row.Login + '\')" class="btn btn-primary btn-xs"><i class="fa fa-key" aria-hidden="true"></i></a>';
                        return editar + " " + eliminar + " " + credenciales;
                    }, "sClass": 'text-center'}
                ],
                "drawCallback": function (settings) {
                    $('[data-toggle="tooltip1"]').tooltip();
                }
            });
        }
    }
}
var Eventos = {
    ListUsuarioTipo: function () {
        $.ajax({
            url: urlListUsuarioTipo,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                DBR.LlenarCombo(response, "#cboTipoUsuario", false, false, "Seleccione");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
            }
        });
    },
    SaveUsuario: function () {

        var request = new Object();
        request.IdUsuario = $("#txtIdUsuario").val();
        request.Login = $("#txtLogin").val();
        request.Password = $("#txtPassword").val();
        request.NumeroDocumento = $("#txtNumeroDocumento").val();
        request.Nombres = $("#txtNombres").val().toUpperCase();
        request.ApellidoPaterno = $("#txtApellidoPaterno").val().toUpperCase();
        request.ApellidoMaterno = $("#txtApellidoMaterno").val().toUpperCase();
        request.Correo = $("#txtCorreo").val().toUpperCase();
        request.IdUsuarioTipo = $("#cboTipoUsuario").val();

        DBR.blockUIStar()
        $.ajax({
            url: urlSaveUsuario,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();
                if (response.IsSuccess) {
                    $('#gridUsuarios').DataTable().ajax.reload();
                    $("#modalUsuario").modal("hide");
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
    },
    GetUsuario: function (IdUsuario) {

        $("#lblTitleAgregarUsuario").html("Editar");
        Funciones.LimpiarCamposUsuario();

        var request = new Object();
        request.IdUsuario = IdUsuario;

        DBR.blockUIStar()
        $.ajax({
            url: urlGetUsuario,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();
                $("#txtIdUsuario").val(response.IdUsuario);
                $("#txtLogin").val(response.Login);
                $("#txtPassword").val(response.Password);
                $("#txtNumeroDocumento").val(response.NumeroDocumento);
                $("#txtNombres").val(response.Nombres);
                $("#txtApellidoPaterno").val(response.ApellidoPaterno);
                $("#txtApellidoMaterno").val(response.ApellidoMaterno);
                $("#txtCorreo").val(response.Correo);
                $("#cboTipoUsuario").val(response.IdUsuarioTipo);
                $("#modalUsuario").modal("show");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                DBR.blockUIStop();
                console.log(textStatus);
            }
        });


    },
    DeleteUsuario: function (IdUsuario) {
        var request = new Object();
        request.IdUsuario = IdUsuario;

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

                        DBR.blockUIStar()
                        $.ajax({
                            url: urlDeleteUsuario,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ "request": request }),
                            success: function (response) {
                                DBR.blockUIStop();
                                dialogItself.close();
                                if (response.IsSuccess) {
                                    $('#gridUsuarios').DataTable().ajax.reload();
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
    EnviarCredencialesUsuario: function (Login) {
        var request = new Object();
        request.Login = Login;

        BootstrapDialog.show({
            title: 'Confirmación',
            closeByBackdrop: false,
            message: '¿Esta seguro de enviar los credenciales?',
            size: BootstrapDialog.SIZE_SMALL,
            buttons: [
                {
                    label: 'Si',
                    cssClass: 'btn-primary btn-sm',
                    action: function (dialogItself) {

                        DBR.blockUIStar()
                        $.ajax({
                            url: urlRenviarCredenciales,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ "request": request }),
                            success: function (response) {
                                DBR.blockUIStop();
                                dialogItself.close();
                                if (response.IsSuccess) {
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
    GetPersona: function () {
        var request = new Object();
        request.NumeroDocumento = $("#txtNumeroDocumento").val();

        DBR.blockUIStar();
        $.ajax({
            url: urlGetPersonaXdni,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();
                if (response.length > 0) {
                    $("#txtNombres").val(response[0].Nombres);
                    $("#txtApellidoPaterno").val(response[0].ApellidoPaterno);
                    $("#txtApellidoMaterno").val(response[0].ApellidoMaterno);
                    $("#txtLogin").val(response[0].NumeroDocumento);
                    $("#txtCorreo").val(response[0].Correo);
                    DBR.ToastSuccess("Persona encontrada");
                } else {
                    $("#txtLogin").val("");
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
}
var Funciones = {
    LimpiarCamposUsuario: function () {
        DBR.limpiarCampo("#txtLogin");
        DBR.limpiarCampo("#txtPassword");
        DBR.limpiarCampo("#txtNumeroDocumento");
        DBR.limpiarCampo("#txtNombres");
        DBR.limpiarCampo("#txtApellidoPaterno");
        DBR.limpiarCampo("#txtApellidoMaterno");
        DBR.limpiarCampo("#txtCorreo");
        DBR.limpiarCampo("#cboTipoUsuario");
    }
}
$(document).ready(function () {
    config.iniGrids.GrillaUsuarios();
    Eventos.ListUsuarioTipo();
    $("#btnAgregarUsuario").on("click", function () {
        $("#lblTitleAgregarUsuario").html("Agregar");
        $("#txtIdUsuario").val(0);
        Funciones.LimpiarCamposUsuario();
        $("#modalUsuario").modal("show");
    });
    $("#btnGuardarUsuario").on("click", function () {
        var validar = true;
        if (!DBR.validarCampo("#txtLogin")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtPassword")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtNumeroDocumento")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtNombres")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtApellidoPaterno")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtApellidoMaterno")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtCorreo")) {
            validar = false;
        }
        if (!DBR.validarCampo("#cboTipoUsuario")) {
            validar = false;
        }
        if (validar) {
            Eventos.SaveUsuario();
        }
    });
    $("#btnBuscarPorDni").on("click", function () {
        Eventos.GetPersona();
    });
});