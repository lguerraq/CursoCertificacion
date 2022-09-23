var Eventos = {
    ListUsuarioSinAsignar: function () {
        var request = new Object();
        request.IdEvento = $("#txtIdEvento").val();

        $.ajax({
            url: urlListUsuarioSinAsignar,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.LlenarCombo(response, "#cboUsuario", true, false, "Seleccionar");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
            }
        });
    },
    ModalListaEventoUsuario: function (IdEvento) {
        $("#txtIdEvento").val(IdEvento);
        config.iniGrids.GrillaUsuarioEventos();
        $("#modalListaUsuarioEvento").modal("show");
    },
    SaveEventoUsuario: function () {

        var request = new Object();
        request.IdEventoUsuario = $("#txtIdEventoUsuario").val();
        request.IdEvento = $("#txtIdEvento").val();
        request.IdUsuario = $("#cboUsuario").val();
        request.FechaInicio = $("#txtFechaInicio").val();
        request.FechaFin = $("#txtFechaFin").val();

        var requestVideos = [];

        DBR.blockUIStar();

        $.ajax({
            url: urlSaveEventoUsuario,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request, "requestVideos": requestVideos }),
            success: function (response) {
                DBR.blockUIStop();
                if (response.IsSuccess) {
                    $('#gridEventoUsuario').DataTable().ajax.reload(null, false);
                    $("#modalUsuarioEvento").modal("hide");
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
    EditEventoUsuario: function (IdEventoUsuario, IdUsuario, FechaInicio, FechaFin) {
        Funciones.LimpiarCamposEventoUsuario();
        $("#txtIdEventoUsuario").val(IdEventoUsuario);
        $("#cboUsuario").prop("disabled",true);
        $("#cboUsuario").val(IdUsuario).trigger("change");
        $('#txtFechaInicio').datepicker("setDate", FechaInicio);
        $('#txtFechaFin').datepicker("setDate", FechaFin);
        Eventos.ListVirtualVideoByEvento();
        $("#modalUsuarioEvento").modal("show");
    },
    DeleteEventoUsuario: function (IdEventoUsuario) {
        var request = new Object();
        request.IdEventoUsuario = IdEventoUsuario;

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
                            url: urlDeleteEventoUsuario,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ "request": request }),
                            success: function (response) {
                                DBR.blockUIStop();
                                dialogItself.close();
                                if (response.IsSuccess) {
                                    $('#gridEventoUsuario').DataTable().ajax.reload(null, false);
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
    ListVirtualVideoByEvento: function () {

        var request = new Object();
        request.IdEvento = $("#txtIdEvento").val();
        request.IdEventoUsuario = $("#txtIdEventoUsuario").val();

        $.ajax({
            url: urlListEventoUsuarioVirtualVideoByEvento,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                Funciones.LlenarVirtualVideo(response);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
            }
        });
    },
}
var config = {
    iniGrids: {
        GrillaEventos: function () {
            var width = $("#roNombre").width();
            $('#gridEventos').DataTable({
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
                    "url": urlListEventoAsignacionPaged,
                    "type": 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    "data": function (oPaged) {
                        return JSON.stringify({ "page": oPaged });
                    },
                },
                "columns": [
                            {
                                "data": function (row, type, set, meta) {
                                    return '<div title="' + row.NombreEvento + '" style="max-width:400px; overflow:hidden;white-space:nowrap;text-overflow: ellipsis;">' + row.NombreEvento + '</div>';
                                }, "bSortable": false, "sClass": 'text-left', "width": "30%"
                            },
                            {
                                "data": function (row, type, set, meta) {
                                    return parseJsonDate(row.Fecha);
                                }, "bSortable": false, "sClass": 'text-center'
                            },
                            {
                                "data": function (row, type, set, meta) {
                                    return '<div title="' + row.Expositor + '" style="max-width:300px; overflow:hidden;white-space:nowrap;text-overflow: ellipsis;">' + row.Expositor + '</div>';
                                }, "bSortable": false, "sClass": 'text-left', "width": "22%"
                            },
                            { "data": "CantidadAsignados", "sClass": 'text-center' },
                            {
                                "data": function (row, type, set, meta) {
                                    var Usuarios = '<a data-toggle="tooltip1" data-placement="bottom" title="Usuarios" onclick="Eventos.ModalListaEventoUsuario(' + row.IdEvento + ')" class="btn btn-primary btn-xs"><span class="fa fa-list-alt" aria-hidden="true"></span></a>';
                                    return Usuarios;
                                }, "sClass": 'text-center'
                            }
                ],
                "drawCallback": function (settings) {
                    $(".rowNombreEvento").width(width);
                    $('[data-toggle="tooltip1"]').tooltip();
                }
            });
        },
        GrillaUsuarioEventos: function () {
            $('#gridEventoUsuario').DataTable({
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
                    "url": urlListEventoUsuarioAsignadoPaged,
                    "type": 'POST',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    "data": function (oPaged) {
                        var request = new Object();
                        request.IdEvento = $("#txtIdEvento").val();
                        return JSON.stringify({ "page": oPaged, "request": request });
                    },
                },
                "columns": [
                            { "data": "NombreUsuario" },
                            {
                               "data": function (row, type, set, meta) {
                                   return parseJsonDate(row.FechaInicio);
                               }, "bSortable": false, "sClass": 'text-center'
                            },
                            {
                                "data": function (row, type, set, meta) {
                                    return parseJsonDate(row.FechaFin);
                                }, "bSortable": false, "sClass": 'text-center'
                            },
                            {
                                "data": function (row, type, set, meta) {
                                    var Editar = '<a data-toggle="tooltip1" data-placement="bottom" title="Editar" onclick="Eventos.EditEventoUsuario(' + row.IdEventoUsuario + ',' +row.IdUsuario + ',\'' + parseJsonDate(row.FechaInicio) + '\',\'' + parseJsonDate(row.FechaFin) + '\')" class="btn btn-primary btn-xs"><span class="fa fa-pencil" aria-hidden="true"></span></a>';
                                    var Eliminar = '<a data-toggle="tooltip1" data-placement="bottom" title="Eliminar" onclick="Eventos.DeleteEventoUsuario(' + row.IdEventoUsuario + ')" class="btn btn-primary btn-xs"><span class="fa fa-trash-o" aria-hidden="true"></span></a>';
                                    return Editar + " " + Eliminar;
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
var Funciones = {
    LimpiarCamposEventoUsuario: function () {
        $("#cboUsuario").prop("disabled", false);
        DBR.limpiarCampo("#cboUsuario");
        DBR.limpiarCampo("#txtFechaInicio");
        DBR.limpiarCampo("#txtFechaFin");
        $('#txtFechaInicio').datepicker("setDate", "");
        $('#txtFechaFin').datepicker("setDate", "");
        $(".labelCheck").removeClass("videosLabel");
        $("#sinVideos").addClass("hide");
    },
    LlenarVirtualVideo: function (data) {
        var row = "";
        $.each(data, function (e, i) {
            let checked = "";
            if (i.IdEventoUsuarioVirtualVideo != null) {
                checked = "checked";
            }
            var check = '<div class="col-lg-2 col-md-3 col-sm-4 col-xs-4 videosCon"><label class="labelCheck" for="videos[' + e + ']">VIDEO ' + (e + 1) + ' </label> <input value="' + i.IdVirtualVideo + '" id="videos[' + e + ']" ' + checked + ' type="checkbox" class="chkVideos"></div>';
            row = row + check;
        });
        if (row == "") {
            row = row + '<div class="col-md-12 videosCon"><label class="videosLabel">No se encontraron videos</label></div>';
        }
        $("#containerVideos").html(row);
        $('.chkVideos').iCheck({
            checkboxClass: 'icheckbox_minimal-blue',
            radioClass: 'iradio_minimal-blue',
        });
    }
}

$(document).ready(function () {
    config.iniGrids.GrillaEventos();
    Eventos.ListUsuarioSinAsignar();
    
    $('#txtFechaInicio').datepicker({
        autoclose: true,
        language: 'es',
        format: "dd/mm/yyyy"
    });
    $('#txtFechaFin').datepicker({
        autoclose: true,
        language: 'es',
        format: "dd/mm/yyyy"
    });
    $("#btnAgregarEventoUsuario").on("click", function () {
        $("#txtIdEventoUsuario").val(0);
        Funciones.LimpiarCamposEventoUsuario();
        //Eventos.ListVirtualVideoByEvento();
        $("#modalUsuarioEvento").modal("show");
    });
    $("#btnGuardarEventoUsuario").on("click", function () {
        var validar = true;
        if (!DBR.validarCampo("#cboUsuario")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtFechaInicio")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtFechaFin")) {
            validar = false;
        }
        if (validar) {
            Eventos.SaveEventoUsuario();
        }
    });
    $('#modalUsuarioEvento').on('hidden.bs.modal', function (event) {
        $('body').addClass('modal-open');
    });
});