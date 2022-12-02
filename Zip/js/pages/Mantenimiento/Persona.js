var config = {
    iniGrids: {
        GrillaPersonas: function () {
            $('#gridPersonas').DataTable({
                "searching": true,
                "ordering": false,
                "processing": true,
                "serverSide": true,
                "destroy": true,
                "stateSave": true,
                "language": {
                    url: '//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json'
                },
                "ajax": {
                    "url": UrlListPersonaPaged,
                    "type": 'POST',
                    "datatype": "json",
                    "data": {},
                },
                "columns": [
                            { "data": "ApellidoPaterno" },
                            { "data": "ApellidoMaterno" },
                            { "data": "Nombres" },
                            { "data": "NumeroDocumento" },
                            { "data": "Celular" },
                            { "data": "Correo" },
                            { "data": "CIP" },
                            { "data": "TipoOcupacionNombre" },
                            {
                                "data": function (row, type, set, meta) {
                                    var editar = '<a data-toggle="tooltip1" data-placement="bottom" title="Editar evento" onclick="Eventos.GetPersona(' +
                                        row.IdPersona +
                                        ',\'' + row.NumeroDocumento +
                                        '\',\'' + row.CIP +
                                        '\',\'' + row.Nombres +
                                        '\',\'' + row.ApellidoPaterno +
                                        '\',\'' + row.ApellidoMaterno +
                                        '\',\'' + row.Celular +
                                        '\',\'' + row.Correo +
                                        '\',' + row.TipoOcupacion +
                                        ',\'' + row.DescripcionOcupacion +
                                        '\',' + row.IdProfesion +
                                        ',' + row.IdPais +
                                        ',\'' + row.Ciudad + '\'' +
                                        ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></a>';
                                    var eliminar = '<a data-toggle="tooltip1" data-placement="bottom" title="Eliminar evento" onclick="Eventos.DeletePersona(' + row.IdPersona + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-trash" aria-hidden="true"></span></a>';
                                    return editar + " " + eliminar;
                                }, "sClass": 'text-center'
                            }
                ],
                "drawCallback": function (settings) {
                    $('[data-toggle="tooltip1"]').tooltip();
                }
            });
        },
    }
}
var Eventos = {
    ValoresInicialesPersona: function () {
        $.ajax({
            url: UrlValoresInicialesPersona,
            type: "POST",
            dataType: "json",
            success: function (response) {
                DBR.LlenarCombo(response.Ocupacion, "#cboTipoOcupacion", false, false, "Seleccione");
                DBR.LlenarCombo(response.Profesion, "#cboProfesion", false, false, "Seleccione");
                DBR.LlenarCombo(response.Pais, "#cboPais", true, false, "Seleccione");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
            }
        });
    },
    SavePersona: function () {

        var request = new Object();
        request.IdPersona = $("#txtIdPersona").val();
        request.Nombres = $.trim($("#txtNombres").val().toUpperCase());
        request.ApellidoPaterno = $.trim($("#txtApellidoPaterno").val().toUpperCase());
        request.ApellidoMaterno = $.trim($("#txtApellidoMaterno").val().toUpperCase());
        request.NumeroDocumento = $.trim($("#txtDni").val());
        request.TipoOcupacion = $("#cboTipoOcupacion").val();
        request.CIP = $.trim($("#txtCip").val());
        request.Celular = $.trim($("#txtCelular").val());
        request.Correo = $.trim($("#txtCorreo").val().toUpperCase());
        request.DescripcionOcupacion = $.trim($("#txtDescripcionOcupacion").val().toUpperCase());
        request.IdProfesion = $("#cboProfesion").val();
        request.IdPais = $("#cboPais").val();
        request.Ciudad = $.trim($("#txtCiudad").val().toUpperCase());

        DBR.blockUIStar();
        $.ajax({
            url: UrlSavePersona,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();
                if (response.IsSuccess) {
                    $('#gridPersonas').DataTable().ajax.reload(null, false);
                    $("#modalPersona").modal("hide");
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
    GetPersona: function (IdPersona, NumeroDocumento, CIP, Nombres, ApellidoPaterno, ApellidoMaterno, Celular, Correo, TipoOcupacion, DescripcionOcupacion, IdProfesion, IdPais, Ciudad) {
        Funciones.LimpiarCamposPersona();
        $("#lblTitleAgregarPersona").html("Editar");
        $("#txtIdPersona").val(IdPersona);
        $("#txtNombres").val(Nombres);
        $("#txtApellidoPaterno").val(ApellidoPaterno);
        $("#txtApellidoMaterno").val(ApellidoMaterno);
        $("#txtDni").val(NumeroDocumento);
        $("#cboTipoOcupacion").val(TipoOcupacion);       
        if (CIP != "null") {
            $("#txtCip").val(CIP);
        }
        $("#txtCelular").val(Celular);
        $("#txtCorreo").val(Correo);
        if (DescripcionOcupacion != "null") {
            $("#txtDescripcionOcupacion").val(DescripcionOcupacion);
        }        
        if (IdProfesion != null) {
            $("#cboProfesion").val(IdProfesion).trigger("change");
        }
        if (IdPais != null) {
            $("#cboPais").val(IdPais).trigger("change");
        }
        if (Ciudad != "null") {
            $("#txtCiudad").val(Ciudad);
        }
        $("#modalPersona").modal("show");
    },
    DeletePersona: function (IdPersona) {
        var request = new Object();
        request.IdPersona = IdPersona;

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
                            url: UrlDeletePersona,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ "request": request }),
                            success: function (response) {
                                DBR.blockUIStop();
                                dialogItself.close();
                                if (response.IsSuccess) {
                                    $('#gridPersonas').DataTable().ajax.reload(null, false);
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
    SavePersonaMasiva: function () {
        var input = document.getElementById("filePlantilla");

        var formData = new FormData();
        formData.append("documento", input.files[0]);

        DBR.blockUIStar();

        $.ajax({
            url: UrlSavePersonaMasivo,
            type: "POST",
            dataType: "json",
            cache: false,
            contentType: false,
            processData: false,
            data: formData
        }).done(function (response) {
            DBR.blockUIStop();
            if (response.IsSuccess) {
                $("#filePlantilla").filestyle('clear');
                $("#modalCargaMasiva").modal("hide");
                var Descargar = '<a class="btn btn-default btn-sm" onclick="Eventos.DescargarPlanillaErrores()" style="cursor:pointer;"><i class="fa fa-cloud-download"></i> Planilla con errores</a>';
                if (response.Codigo == 1) {
                    $('#gridPersonas').DataTable().ajax.reload(null, false);
                    DBR.ToastSuccess(response.Message);
                } else {
                    DBR.MensajeInformativoNormal(response.Message + "\n" + Descargar, "Resultado de validación");
                }
            } else {
                DBR.ToastError(response.Message);
            }
        }).fail(function (XMLHttpRequest) {
            DBR.blockUIStop();
            console.log(XMLHttpRequest);
        });
    },
    DescargarPlanillaErrores: function () {
        window.location.href = UrlDescargarPlantillaErrores;
    }
}
var Funciones = {
    LimpiarCamposPersona: function () {
        $("#formPersona .validar").each(function () {
            DBR.limpiarCampo(this);
        });
        $("#txtCip").val("");
        $("#txtDescripcionOcupacion").val("");
        $("#txtCiudad").val("");
    },
}
$(document).ready(function () {
    Eventos.ValoresInicialesPersona();
    config.iniGrids.GrillaPersonas();

    $("#txtDni").numericInput({ allowFloat: false, allowNegative: false });
    $("#txtCelular").numericInput({ allowFloat: false, allowNegative: false });
    $("#txtCip").numericInput({ allowFloat: false, allowNegative: false });
    $("#cboProfesion").select2();

    $("#btnAgregarPersona").on("click", function () {
        Funciones.LimpiarCamposPersona();
        $("#txtIdPersona").val(0);
        $("#lblTitleAgregarPersona").html("Agregar");
        $("#modalPersona").modal("show");
    });
    $("#btnGuardarPersona").on("click", function () {
        var validar = true;
        $("#formPersona .validar").each(function () {
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
            Eventos.SavePersona();
        }
    });
    $("#btnAgregarCargaMasiva").on("click", function () {
        $("#filePlantilla").filestyle('clear');
        $("#filePlantilla").parent().removeClass("has-error");
        $("#filePlantilla").next().next().addClass("hide");
        $("#modalCargaMasiva").modal("show");
    });
    $("#btnGuardarCargaMasiva").on("click", function () {
        var validar = true;
        var input = document.getElementById("filePlantilla");
        if (input.files.length == 0) {
            $("#filePlantilla").parent().addClass("has-error");
            $("#filePlantilla").next().next().removeClass("hide");
            validar = false;
        } else {
            $("#filePlantilla").parent().removeClass("has-error");
            $("#filePlantilla").next().next().addClass("hide");
            if (input.files[0].size > 4194304) {
                DBR.ToastWarning("Verifique que el archivo adjunto no supere el 4MB");
                validar = false;
            }
        }
        if (validar) {
            Eventos.SavePersonaMasiva();
        }
    });
});