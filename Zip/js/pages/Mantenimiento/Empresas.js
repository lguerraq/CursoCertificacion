var GrillaEmpresas;
var config = {
    iniGrids: {
        GridEmpresas: function () {
            GrillaEmpresas =
                $('#gridEmpresas').DataTable({
                    "searching": true,
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
                        "url": urlListEmpresaPaginado,
                        "type": 'POST',
                        "datatype": "json",
                        "contentType": 'application/json; charset=UTF-8',
                        "data": function (oPaged) {
                            return JSON.stringify({ "page": oPaged });
                        }
                    },
                    "columns": [
                        {
                            "data": "Ruc", "sClass": 'text-left'
                        },
                        {
                            "data": "RazonSocial", "sClass": 'text-left'
                        },
                        {
                            "data": "NombreComercial", "sClass": 'text-left'
                        },
                        {
                            "data": "Responsable", "sClass": 'text-left'
                        },
                        {
                            "data": function (row, type, set, meta) {
                                let result;
                                var eliminar = '<a data-toggle="tooltip1" data-placement="bottom" title="Eliminar" onclick="Eventos.DeleteEmpresa(' + row.IdEmpresa + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-trash" aria-hidden="true"></span></a>';
                                var editar = '<a data-toggle="tooltip1" data-placement="bottom" title="Editar" onclick="Eventos.GetEmpresa(' + row.IdEmpresa + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></a>';
                                return editar + " " + eliminar;
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
    SaveEmpresa: function () {
        DBR.blockUIStar();
        let request = {
            IdEmpresa: $("#txtIdEmpresa").val(),
            Ruc: $("#txtRuc").val(),
            RazonSocial: $("#txtRazonSocial").val(),
            NombreComercial: $("#txtNombreComercial").val(),
            DireccionFiscal: $("#txtDireccionFiscal").val(),
            Frecuencia: $("#cboFrecuencia").val()
        };

        let requestusurio = {
            Login: $("#txtLogin").val(),
            NumeroDocumento: $("#txtNumeroDocumento").val(),
            Nombres: $("#txtNombres").val().toUpperCase(),
            ApellidoPaterno: $("#txtApellidoPaterno").val().toUpperCase(),
            ApellidoMaterno: $("#txtApellidoMaterno").val().toUpperCase(),
            Correo: $("#txtCorreo").val().toUpperCase(),
            IdUsuarioTipo: 6
        };
        

        $.ajax({
            url: urlSaveEmpresa,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request, "requestuser": requestusurio}),
            success: function (response) {
                if (response.IsSuccess) {
                    DBR.ToastSuccess(response.Message);
                    GrillaEmpresas.ajax.reload();
                    $("#modalEmpresa").modal("hide");
                } else {
                    DBR.ToastError(response.Message);
                }
                DBR.blockUIStop();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
                DBR.blockUIStop();
            }
        });
    },
    GetEmpresa: function (IdEmpresa) {
        DBR.blockUIStar();
        let request = {
            IdEmpresa: IdEmpresa,
        };
        $.ajax({
            url: urlGetEmpresa,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();
                Funciones.LimpiarCapos();
                $("#lblTitleAgregarEmpresa").html("Editar");
                $("#txtIdEmpresa").val(response.IdEmpresa);

                $("#txtRuc").val(response.Ruc);
                $("#txtRazonSocial").val(response.RazonSocial);
                $("#txtNombreComercial").val(response.NombreComercial);
                $("#txtDireccionFiscal").val(response.DireccionFiscal);
                $("#cboFrecuencia").val(response.Frecuencia);

                $("#txtLogin").val(response.Usuario);
                $("#txtNumeroDocumento").val(response.NumeroDocumento);
                $("#txtNombres").val(response.Nombres);
                $("#txtApellidoPaterno").val(response.ApellidoPaterno);
                $("#txtApellidoMaterno").val(response.ApellidoMaterno);
                $("#txtCorreo").val(response.Correo);
                $("#modalEmpresa").modal("show");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
                DBR.blockUIStop();
            }
        });
    },
    DeleteEmpresa: function (IdEmpresa) {
        var request = new Object();
        request.IdEmpresa = IdEmpresa;

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
                            url: urlDeleteEmpresa,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ "request": request }),
                            success: function (response) {
                                DBR.blockUIStop();
                                dialogItself.close();
                                if (response.IsSuccess) {
                                    GrillaEmpresas.ajax.reload();
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
    LimpiarCapos: function () {

        DBR.limpiarCampo("#txtRuc");
        DBR.limpiarCampo("#txtRazonSocial");
        DBR.limpiarCampo("#txtNombreComercial");
        DBR.limpiarCampo("#txtDireccionFiscal");
        DBR.limpiarCampo("#cboFrecuencia");

        DBR.limpiarCampo("#txtLogin");
        DBR.limpiarCampo("#txtNumeroDocumento");
        DBR.limpiarCampo("#txtNombres");
        DBR.limpiarCampo("#txtApellidoPaterno");
        DBR.limpiarCampo("#txtApellidoMaterno");
        DBR.limpiarCampo("#txtCorreo");
    }
}
$(document).ready(function () {
    config.iniGrids.GridEmpresas();
    $("#txtRuc").numericInput({ allowFloat: false, allowNegative: false });
    $("#txtNumeroDocumento").numericInput({ allowFloat: false, allowNegative: false });
    $("#btnAgregar").on("click", function () {
        $("#lblTitleAgregarEmpresa").html("Agregar");
        $("#txtIdEmpresa").val(0);
        Funciones.LimpiarCapos();
        $("#modalEmpresa").modal("show");
    });
    $("#btnGuardarEmpresa").on("click", function () {
        let validar = true;
        if (!DBR.validarCampo("#txtRuc")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtRazonSocial")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtNombreComercial")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtDireccionFiscal")) {
            validar = false;
        }

        if (!DBR.validarCampo("#txtLogin")) {
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

        if (validar) {
            Eventos.SaveEmpresa();
        }
    });
    //SUBIR ARCHIVO
    $("#btnSubirArchivo").on("click", function () {
        $("#fileDocumento").filestyle('clear');
        $("#fileDocumento").parent().removeClass("has-error");
        $("#fileDocumento").next().next().addClass("hide");
        $("#lblTitleAgregarDocumento").html("Subir documento");
        $("#txtIdDocumento2").val(0);
        $("#modalDocumento").modal("show");
    });
    $("#btnGuardarDucumento").on("click", function () {
        let validar = true;
        var input = document.getElementById("fileDocumento");
        if (input.files.length == 0) {
            $("#fileDocumento").parent().addClass("has-error");
            $("#fileDocumento").next().next().removeClass("hide");
            validar = false;
        } else {
            $("#fileDocumento").parent().removeClass("has-error");
            $("#fileDocumento").next().next().addClass("hide");
            if (input.files[0].size > 10485760) {
                DBR.ToastWarning("Verifique que el archivo adjunto no supere el 10MB");
                validar = false;
            }
        }
        if (validar) {
            Eventos.SaveDocumento();
        }
    });
    //ACTUALIZAR DATOS
    $("#btnActualizar").on("click", function () {
        GrillaDocumentos.ajax.reload();
    });

    $("#btnBuscarPorDni").on("click", function () {
        Eventos.GetPersona();
    });
});