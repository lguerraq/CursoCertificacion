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
        GrillaModulo: function () {
            $('#gridModulos').DataTable({
                "searching": true,
                "ordering": false,
                "processing": true,
                "serverSide": true,
                "destroy": true,
                "responsive": true,
                "autoWidth": false,
                "language": {
                    url: '//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json'
                },
                "ajax": {
                    "url": urlListModuloPaged,
                    "type": 'POST',
                    "contentType": "application/json; charset=utf-8",
                    "dataType": "json",
                    "data": function (oPaged) {
                        var request = new Object();
                        request.IdEvento = $("#txtIdEvento2").val();
                        return JSON.stringify({ "page": oPaged, "request": request });
                    },
                },
                "columns": [
                    { "data": "Nombre", "width": "40%" },
                    { "data": "Expositor", "width": "40%" },
                    { "data": "Horas", "sClass": 'text-center', "width": "10%" },
                    {
                        "data": function (row, type, set, meta) {
                            var editar = '<a data-toggle="tooltip1" data-placement="bottom" title="Editar" onclick="Eventos.GetModulo(' + row.IdModulo + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></a>';
                            var eliminar = '<a data-toggle="tooltip1" data-placement="bottom" title="Eliminar" onclick="Eventos.DeleteModulo(' + row.IdModulo + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-trash" aria-hidden="true"></span></a>';
                            return editar + " " + eliminar;
                        }, "sClass": 'text-center', "width": "10%"
                    }
                ],
                "drawCallback": function (settings) {
                    $('[data-toggle="tooltip1"]').tooltip();
                }
            });
        },
        GrillaLeccion: function (modulo) {
            $('.grid-lecciones[data-modulo="'+modulo+'"]').DataTable({
                "searching": true,
                "ordering": false,
                "processing": true,
                "serverSide": true,
                "destroy": true,
                "responsive": true,
                "autoWidth": false,
                "language": {
                    url: '//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json'
                },
                "ajax": {
                    "url": urlListLeccionPaged,
                    "type": 'POST',
                    "contentType": "application/json; charset=utf-8",
                    "dataType": "json",
                    "data": function (oPaged) {
                        var request = new Object();
                        request.IdModulo = modulo;
                        return JSON.stringify({ "page": oPaged, "request": request });
                    },
                },
                "columns": [
                    { "data": "NombreTipo", "width": "10%" },
                    { "data": "Nombre", "width": "70%" },
                    { "data": "Duracion", "sClass": 'text-center', "width": "10%" },
                    {
                        "data": function (row, type, set, meta) {
                            var editar = '<a data-toggle="tooltip1" data-placement="bottom" title="Editar" onclick="Eventos.GetLeccion(' + row.IdLeccion + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></a>';
                            var eliminar = '<a data-toggle="tooltip1" data-placement="bottom" title="Eliminar" onclick="Eventos.DeleteLeccion(' + row.IdLeccion + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-trash" aria-hidden="true"></span></a>';
                            return editar + " " + eliminar;
                        }, "sClass": 'text-center', "width": "10%"
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
    ValoresIniciales: function () {

        DBR.blockUIStar();

        $.ajax({
            url: urlValoresIniciales,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                DBR.LlenarCombo(response.TipoLeccion, "#ddlTipo", false, false, "Seleccione");
                DBR.blockUIStop();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log(textStatus);
            }
        });
    },
    SaveModulo: function () {

        var request = new Object();
        request.IdModulo = $("#txtIdModulo").val();
        request.IdEvento = $("#txtIdEvento").val();
        request.Nombre = $("#txtModuloNombre").val();
        request.Descripcion = $("#txtModuloDescripcion").val();
        request.Expositor = $("#txtModuloExpositor").val();
        request.Horas = $("#txtModuloHoras").val();
        request.Peso = $("#ddlModuloPeso").val();

        DBR.blockUIStar();

        $.ajax({
            url: urlSaveModulo,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();
                if (response.IsSuccess) {
                    //$('#gridModulos').DataTable().ajax.reload();
                    $("#modalModulo").modal("hide");
                    DBR.ToastSuccess(response.Message);
                    window.location.reload();
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
    GetModulo: function (IdModulo) {

        $("#lblTitleModal").html("Editar");
        Funciones.LimpiarCamposModulo();

        var request = new Object();
        request.IdModulo = IdModulo;

        DBR.blockUIStar();
        $.ajax({
            url: urlGetModulo,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();

                $("#txtIdModulo").val(response.IdModulo);
                $("#txtModuloNombre").val(response.Nombre);
                $("#txtModuloDescripcion").val(response.Descripcion);
                $("#txtModuloExpositor").val(response.Expositor);
                $("#txtModuloHoras").val(response.Horas);
                $("#ddlModuloPeso").val(response.Peso);

                $("#modalModulo").modal("show");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                DBR.blockUIStop();
                console.log(textStatus);
            }
        });


    },
    DeleteModulo: function (IdModulo) {
        var request = new Object();
        request.IdModulo = IdModulo;

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
                            url: urlDeleteModulo,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ "request": request }),
                            success: function (response) {
                                DBR.blockUIStop();
                                dialogItself.close();
                                if (response.IsSuccess) {
                                    //$('#gridModulos').DataTable().ajax.reload();
                                    DBR.ToastSuccess(response.Message);
                                    window.location.reload();
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
    SaveLeccion: function () {

        var request = new Object();
        request.IdLeccion = $("#txtIdLeccion").val();
        request.IdModulo = $("#txtIdModuloLeccion").val();
        request.Tipo = $("#ddlTipo").val();
        request.Nombre = $("#txtLeccionNombre").val();
        request.Duracion = $("#txtDuracion").val();
        request.Descripcion = contenidoHtml;
        request.TipoUrl = $("input[name='rTipoUrl']:checked").val();

        if (request.TipoUrl == 1) {
            request.Url = $("#txtUrlVideo").val();
        } else {
            request.Url = $("#txtIframeVideo").val();
        }
        
        request.Orden = $("#txtOrden").val();
        request.Peso = $("#ddlLeccionPeso").val();

        DBR.blockUIStar();

        $.ajax({
            url: urlSaveLeccion,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();
                if (response.IsSuccess) {
                    $('.grid-lecciones[data-modulo="' + request.IdModulo + '"]').DataTable().ajax.reload();
                    $("#modalLeccion").modal("hide");
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
    GetLeccion: function (IdLeccion) {

        $("#lblTitleLeccion").html("Editar");
        Funciones.LimpiarCamposLeccion();

        var request = new Object();
        request.IdLeccion = IdLeccion;

        DBR.blockUIStar();
        $.ajax({
            url: urlGetLeccion,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();

                $("#txtIdLeccion").val(response.IdLeccion);
                $("#ddlTipo").val(response.Tipo);
                $("#txtLeccionNombre").val(response.Nombre);              
                $("#txtDuracion").val(response.Duracion);
                $("input[name=rTipoUrl][value=" + response.TipoUrl + "]").iCheck('check');
                if (response.TipoUrl == 1) {
                    $("#txtUrlVideo").val(response.UrlVideo);
                    $("#txtIframeVideo").val("");
                } else {
                    $("#txtUrlVideo").val("");
                    $("#txtIframeVideo").val(response.UrlVideo);
                }
                $("#txtOrden").val(response.Orden);

                if (response.Tipo == 3) { //Tipo cuestionario
                    var idCurso = $("#txtIdEvento").val();
                    window.location.href = urlCuestionario + '?idLeccion=' + response.IdLeccion + '&idCurso=' + idCurso;
                }
                else {
                    if (response.Descripcion == null) {
                        response.Descripcion = "<p></p>";
                    }
                    response.Descripcion = response.Descripcion.replace(/div/g, "p");
                    CKEDITOR.instances.txtMensajeCompleto.setData(response.Descripcion);
                    contenidoHtml = response.Descripcion;
                    $("#modalLeccion").modal("show");
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                DBR.blockUIStop();
                console.log(textStatus);
            }
        });


    },
    DeleteLeccion: function (IdLeccion) {
        var request = new Object();
        request.IdLeccion = IdLeccion;

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
                            url: urlDeleteLeccion,
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            data: JSON.stringify({ "request": request }),
                            success: function (response) {
                                DBR.blockUIStop();
                                dialogItself.close();
                                if (response.IsSuccess) {
                                    $('#gridLeccion').DataTable().ajax.reload();
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
    }
}
var Funciones = {
    LimpiarCamposModulo: function () {
        DBR.limpiarCampo("#txtModuloNombre");
        DBR.limpiarCampo("#txtModuloDescripcion");
        DBR.limpiarCampo("#txtModuloExpositor");
        DBR.limpiarCampo("#txtModuloHoras");
        DBR.limpiarCampo("#ddlModuloPeso");
    },
    LimpiarCamposLeccion: function () {
        DBR.limpiarCampo("#ddlTipo");
        DBR.limpiarCampo("#txtLeccionNombre");
        DBR.limpiarCampo("#txtDuracion");
        DBR.limpiarCampo("#txtUrlVideo");
        DBR.limpiarCampo("#txtOrden");
        DBR.limpiarCampo("#ddlLeccionPeso");

        CKEDITOR.instances.txtMensajeCompleto.setData("");
        contenidoHtml = "";

        $("input[name=rTipoUrl][value=1]").iCheck('check');
        $("#txtUrlVideo").val("");
        $("#txtIframeVideo").val("");
        $("#conteinerUrl").removeClass("hide");
        $("#conteinerIframa").addClass("hide");
    }
}
$(document).ready(function () {
    //config.ini();
    $.fn.modal.Constructor.prototype.enforceFocus = function () { };
    /** add active class and stay opened when selected */
    var url = window.location.origin + "/Mantenimiento/Curso";
    $('ul.sidebar-menu a').filter(function () {
        return this.href == url;
    }).parent().siblings().removeClass('active').end().addClass('active');
    $('ul.treeview-menu a').filter(function () {
        return this.href == url;
    }).parentsUntil(".sidebar-menu > .treeview-menu").siblings().removeClass('active').end().addClass('active');

    config.iniGrids.GrillaModulo();
    Eventos.ValoresIniciales();
    $.each($(".grid-lecciones"), function (i, v) {
        config.iniGrids.GrillaLeccion($(v).data("modulo"));
    });
    $('.minimal').iCheck({
        checkboxClass: 'icheckbox_square-blue',
        radioClass: 'iradio_square-blue',
    });

    $('#txtFecha').datepicker({
        autoclose: true,
        language: 'es',
        format: "dd/mm/yyyy",
        startDate: new Date()
    });
    $("#txtOrden").numericInput({ allowFloat: false, allowNegative: false });
    $("#txtModuloHoras").numericInput({ allowFloat: false, allowNegative: false });
    $("#txtDuracion").numericInput({ allowFloat: false, allowNegative: false });

    $("input[name='rTipoUrl']").on("ifChanged", function () {
        if ($(this).val() == 1) {
            $("#conteinerUrl").removeClass("hide");
            $("#conteinerIframa").addClass("hide");
        } else {
            $("#conteinerUrl").addClass("hide");
            $("#conteinerIframa").removeClass("hide");
        }
    });

    //Modulo
    $("#btnAgregarModulo").on("click", function () {
        $("#lblTitleModal").html("Agregar");
        $("#txtIdModulo").val(0);
        Funciones.LimpiarCamposModulo();
        $("#modalModulo").modal("show");
    });

    $(".btn-edit-modulo").on("click", function () {
        var modulo = $(this).data("modulo");
        Eventos.GetModulo(modulo);
    });

    $("#btnGuardarModulo").on("click", function () {
        var validar = true;
        if (!DBR.validarCampo("#txtModuloNombre")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtModuloDescripcion")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtModuloExpositor")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtModuloHoras")) {
            validar = false;
        }
        if (!DBR.validarCampo("#ddlModuloPeso")) {
            validar = false;
        }
        if (validar) {
            Eventos.SaveModulo();
        }
    });

    $("button.btn-delete-modulo").on("click", function () {
        var modulo = $(this).data("modulo");
        Eventos.DeleteModulo(modulo);
    });

    //#region Leccion

    $(".btn-agregar-leccion").on("click", function () {
        $("#lblTitleLeccion").html("Agregar");
        $("#txtIdModuloLeccion").val($(this).data("modulo"));
        $("#txtIdLeccion").val(0);
        $("div.div-leccion").show();
        Funciones.LimpiarCamposLeccion();
        $("#modalLeccion").modal("show");
    });

    $("#ddlTipo").change(function () {
        if ($(this).val() == 3) { //Tipo Evaluación
            $("div.div-leccion").hide();
            $("div.div-peso").show();
        } else {
            $("div.div-leccion").show();
            $("div.div-peso").hide();
        }
    });
    
    $("#btnGuardarLeccion").on("click", function () {
        var validar = true;
        if (!DBR.validarCampo("#ddlTipo")) {
            validar = false;
        }

        if (validar && $("#ddlTipo").val() != 3) { //Tipo Evaluación
            if (!DBR.validarCampo("#txtDuracion")) {
                validar = false;
            }
        } else {
            if (!DBR.validarCampo("#ddlLeccionPeso")) {
                validar = false;
            }
        }

        if (!DBR.validarCampo("#txtLeccionNombre")) {
            validar = false;
        }

        //if (!DBR.validarCampo("#txtUrlVideo")) {
        //    validar = false;
        //}
        if (!DBR.validarCampo("#txtOrden")) {
            validar = false;
        }
        if (validar) {
            Eventos.SaveLeccion();
        }
    });
    //#endregion

    //#region Editor
    var controlMensComp = CKEDITOR.replace('txtMensajeCompleto', {
        height: 350,
        filebrowserBrowseUrl: urlFileServerBrowse,
        filebrowserUploadUrl: urlSaveFileServerCkEditor
    });
    controlMensComp.on('change', function (evt) {
        contenidoHtml = evt.editor.getData();
    });
    //#endregion
});