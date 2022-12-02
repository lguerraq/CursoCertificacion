var contenidoHtml = "";
var respuestas = [];
var respuestasTrueFalse = [];
var respuestasMulti = [];

function GetUUIDv4() {
    let Cadena = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx';
    return Cadena.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

respuestasTrueFalse.push({
    RowId: GetUUIDv4(),
    IdRespuesta: 0,
    Descripcion: 'VERDADERO',
    Selected: true
});

respuestasTrueFalse.push({
    RowId: GetUUIDv4(),
    IdRespuesta: 0,
    Descripcion: 'FALSO',
    Selected: false
});

respuestasMulti.push({
    RowId: GetUUIDv4(),
    IdRespuesta: 0,
    Descripcion: 'OPCIÓN 01',
    Selected: true
});

respuestasMulti.push({
    RowId: GetUUIDv4(),
    IdRespuesta: 0,
    Descripcion: 'OPCIÓN 02',
    Selected: false
});

respuestasMulti.push({
    RowId: GetUUIDv4(),
    IdRespuesta: 0,
    Descripcion: 'OPCIÓN 03',
    Selected: false
});

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
        GrillaLeccion: function (modulo) {
            $('.grid-lecciones[data-modulo="' + modulo + '"]').DataTable({
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
                    { "data": "Tipo", "width": "10%" },
                    { "data": "Nombre", "width": "70%" },
                    { "data": "Duracion", "sClass": 'text-center', "width": "10%" },
                    {
                        "data": function (row, type, set, meta) {
                            var editar = '<a data-toggle="tooltip1" data-placement="bottom" title="Editar" onclick="Cuestionario.GetRespuesta(' + row.IdLeccion + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></a>';
                            var eliminar = '<a data-toggle="tooltip1" data-placement="bottom" title="Eliminar" onclick="Cuestionario.DeleteRespuesta(' + row.IdLeccion + ')" class="btn btn-primary btn-xs"><span class="glyphicon glyphicon-trash" aria-hidden="true"></span></a>';
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
var Cuestionario = {
    SavePregunta: function () {

        var request = new Object();
        request.IdPregunta = $("#txtIdPregunta").val();
        request.IdCuestionario = $("#txtIdCuestionario").val();
        request.Tipo = $("#ddlPreguntaTipo").val();
        request.Nombre = $("#txtPreguntaNombre").val();
        request.Explicacion = $("#txtPreguntaExplicacion").val();
        request.Ayuda = $("#txtPreguntaAyuda").val();
        request.Puntaje = $("#txtPreguntPuntaje").val();
        request.Respuestas = respuestas;

        DBR.blockUIStar();

        $.ajax({
            url: urlSavePregunta,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();
                if (response.IsSuccess) {
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
    GetPregunta: function (IdPregunta) {

        $("#lblTitleModal").html("Editar");
        Funciones.LimpiarCamposPregunta();

        var request = new Object();
        request.IdPregunta = IdPregunta;

        DBR.blockUIStar();

        $.ajax({
            url: urlGetPregunta,
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ "request": request }),
            success: function (response) {
                DBR.blockUIStop();

                $("#ddlPreguntaTipo").prop("disabled", true);

                $("#txtIdPregunta").val(response.IdPregunta);
                $("#ddlPreguntaTipo").val(response.Tipo);
                $("#txtPreguntaNombre").val(response.Nombre);
                $("#txtPreguntaExplicacion").val(response.Explicacion);
                $("#txtPreguntaAyuda").val(response.Ayuda);
                $("#txtPreguntPuntaje").val(response.Puntaje);

                respuestas = [];
                $.each(response.Respuestas, function (i, v) {
                    respuestas.push({
                        RowId: GetUUIDv4(),
                        IdRespuesta: v.IdRespuesta,
                        Descripcion: v.Descripcion,
                        Selected: v.EsCorrecta
                    });
                });

                Funciones.CreateTable(response.Tipo, 2, respuestas);

                //Show answers
                $("#modalPregunta").modal("show");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                DBR.blockUIStop();
                console.log(textStatus);
            }
        });


    },
    DeletePregunta: function (IdPregunta) {
        var request = new Object();
        request.IdPregunta = IdPregunta;

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
                            url: urlDeletePregunta,
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
    SaveRespuesta: function () {

        var request = new Object();
        request.IdLeccion = $("#txtIdLeccion").val();
        request.IdModulo = $("#txtIdModuloLeccion").val();
        request.Tipo = $("#ddlTipo").val();
        request.Nombre = $("#txtLeccionNombre").val();
        request.Duracion = $("#txtDuracion").val();
        request.Descripcion = contenidoHtml;
        request.Url = $("#txtUrlVideo").val();
        request.Orden = $("#txtOrden").val();

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
    GetRespuesta: function (IdLeccion) {

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
                $("#txtMensajeCompleto").val(response.Descripcion);
                $("#txtDuracion").val(response.Duracion);
                $("#txtUrlVideo").val(response.UrlVideo);
                $("#txtOrden").val(response.Orden);

                $("#modalLeccion").modal("show");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                DBR.blockUIStop();
                console.log(textStatus);
            }
        });


    },
    DeleteRespuesta: function (IdLeccion) {
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
    LimpiarCamposPregunta: function () {
        DBR.limpiarCampo("#ddlPreguntaTipo");
        DBR.limpiarCampo("#txtPreguntaNombre");
        DBR.limpiarCampo("#txtPreguntaExplicacion");
        DBR.limpiarCampo("#txtPreguntaAyuda");
        DBR.limpiarCampo("#txtPreguntPuntaje");
        $("#bodyTable").html("");
    },
    LimpiarCamposLeccion: function () {
        DBR.limpiarCampo("#ddlTipo");
        DBR.limpiarCampo("#txtLeccionNombre");
        DBR.limpiarCampo("#txtDuracion");
        DBR.limpiarCampo("#txtMensajeCompleto");
        DBR.limpiarCampo("#txtUrlVideo");
        DBR.limpiarCampo("#txtOrden");
    },
    CreateTable: function (tipo, mode, items) {
        var rowHeader = $('<tr><th width="50">Item</th><th width = "270">Respuesta</th><th width="150" class="text-center">¿Es opción correcta?</th></tr>');

        if (tipo == 1) {
            $("#headTable").empty();
            $("#headTable").append(rowHeader);

            $("#btnAgregarRespuesta").hide();

            respuestas = mode == 1 ? respuestasTrueFalse : items;
            Funciones.FillTable(respuestas);

            $("#tableRespuestas").SetEditable({
                columnsEd: "1",
                allowDelete: false,
                onEdit: function (row) {
                    var currentRow = row[0];
                    var currentIndex = respuestas.findIndex(r => r.RowId == $(currentRow).data("rowid"));
                    respuestas[currentIndex].Descripcion = currentRow.cells[1].innerText;
                }
            });
        } else if (tipo == 2) {
            $("#headTable").empty();
            $("#headTable").append(rowHeader);

            $("#btnAgregarRespuesta").show();

            respuestas = mode == 1 ? respuestasMulti : items;
            Funciones.FillTable(respuestas);

            $("#tableRespuestas").SetEditable({
                columnsEd: "1",
                onEdit: function (row) {
                    var currentRow = row[0];
                    var currentIndex = respuestas.findIndex(r => r.RowId == $(currentRow).data("rowid"));
                    respuestas[currentIndex].Descripcion = currentRow.cells[1].innerText;
                },
                onDelete: function () { },
                onBeforeDelete: function (row) {
                    if (respuestas.length > 2) {
                        var currentRow = row[0];
                        var currentIndex = respuestas.findIndex(r => r.RowId == $(currentRow).data("rowid"));
                        if (currentIndex >= 0) {
                            respuestas.splice(currentIndex, 1);
                        }
                        return true;
                    } else {
                        DBR.ToastError("No se puede eliminar la respuesta, deben haber como mínimo 2.");
                        return false;
                    }
                },
                onAdd: function () {
                    var lastRow = $("#bodyTable").find("tr:last");
                    let contador = 0;
                    $("#bodyTable .form-control").each(function () {
                        contador++;
                        if (contador == 1) {
                            $(this).focus();
                        }                       
                    });
                    if (contador > 0) {
                        DBR.ToastError("Hay respuestas en modo edición, guardelas para poder continuar");
                        lastRow.remove();
                    }                   
                    lastRow[0].cells[2].innerHTML = '<input type="radio" name="chkRespuesta">';
                    lastRow.find("td:nth(2)").find("input").click(function () {
                        var rowid = $(this).closest("tr").data("rowid");
                        var currentIndex = respuestas.findIndex(r => r.RowId == rowid);
                        for (var i = 0; i < respuestas.length; i++) {
                            if (i == currentIndex) {
                                respuestas[i].Selected = true;
                            } else {
                                respuestas[i].Selected = false;
                            }
                        }
                    });
                    respuestas.push({
                        RowId: lastRow.data("rowid"),
                        IdRespuesta: 0,
                        Descripcion: '',
                        Selected: false
                    });

                    $.each($("#bodyTable").find("tr").find("td:first"), function (i, v) { v.innerText = i + 1; });                 
                },
                $addButton: $('#btnAgregarRespuesta')
            });
        } else {

        }

        $("input[name=chkRespuesta]").click(function () {
            var rowid = $(this).closest("tr").data("rowid");
            var currentIndex = respuestas.findIndex(r => r.RowId == rowid);
            for (var i = 0; i < respuestas.length; i++) {
                if (i == currentIndex) {
                    respuestas[i].Selected = true;
                } else {
                    respuestas[i].Selected = false;
                }
            }
        });
    },
    FillTable: function (respuestas) {
        $("#bodyTable").empty();
        $.each(respuestas, function (i, v) {
            var row = $('<tr data-id="' + v.IdRespuesta + '" data-rowid="' + v.RowId + '"><td>' + (i+1) + '</td><td>' + v.Descripcion + '</td><td class="text-center"><input type="radio" name="chkRespuesta" ' + (v.Selected ? 'checked' : '') + ' /></td></tr>');
            $("#bodyTable").append(row);
        });
    }
}
$(document).ready(function () {
    //config.ini();

    /** add active class and stay opened when selected */
    var url = window.location.origin + "/Mantenimiento/Evento";
    $('ul.sidebar-menu a').filter(function () {
        return this.href == url;
    }).parent().siblings().removeClass('active').end().addClass('active');
    $('ul.treeview-menu a').filter(function () {
        return this.href == url;
    }).parentsUntil(".sidebar-menu > .treeview-menu").siblings().removeClass('active').end().addClass('active');

    $.each($(".grid-lecciones"), function (i, v) {
        config.iniGrids.GrillaLeccion($(v).data("modulo"));
    });

    $('#txtFecha').datepicker({
        autoclose: true,
        language: 'es',
        format: "dd/mm/yyyy",
        startDate: new Date()
    });

    $("#txtPreguntPuntaje").numericInput({ allowFloat: false, allowNegative: false });

    //Pregunta
    $("#btnAgregarPregunta").on("click", function () {
        $("#lblTitleModal").html("Agregar");
        $("#txtIdPregunta").val(0);
        $("#ddlPreguntaTipo").prop("disabled", false);
        Funciones.LimpiarCamposPregunta();
        $("#modalPregunta").modal("show");
    });

    $("#ddlPreguntaTipo").change(function () {
        if ($("#txtIdPregunta").val() != 0) {
            return;
        }

        Funciones.CreateTable($(this).val(), 1, []);
    });

    $(".btn-edit-pregunta").on("click", function () {
        var pregunta = $(this).data("pregunta");
        Cuestionario.GetPregunta(pregunta);
    });

    $("#btnGuardarPregunta").on("click", function () {
        var validar = true;
        if (!DBR.validarCampo("#txtPreguntaNombre")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtPreguntPuntaje")) {
            validar = false;
        }

        if (validar) {

            $.each(respuestas, function (i, v) {
                if (v.Descripcion == null || v.Descripcion == '') {
                    validar = false;
                    DBR.ToastError("Ingrese una descripción a sus respuestas.");
                    return;
                }
            });

            if(validar) Cuestionario.SavePregunta();
        }
    });

    $("button.btn-delete-pregunta").on("click", function () {
        var pregunta = $(this).data("pregunta");
        Cuestionario.DeletePregunta(pregunta);
    });

    //Eventos
    $("#modalPregunta").on("hidden.bs.modal", function () {
        //$("body").addClass("modal-open");
    });

    //#region Leccion

    $(".btn-agregar-leccion").on("click", function () {
        $("#lblTitleLeccion").html("Agregar");
        $("#txtIdModuloLeccion").val($(this).data("modulo"));
        $("#txtIdLeccion").val(0);
        Funciones.LimpiarCamposLeccion();
        $("#modalLeccion").modal("show");
    });


    $("#btnGuardarLeccion").on("click", function () {
        var validar = true;
        if (!DBR.validarCampo("#ddlTipo")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtLeccionNombre")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtDuracion")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtUrlVideo")) {
            validar = false;
        }
        if (!DBR.validarCampo("#txtOrden")) {
            validar = false;
        }
        if (validar) {
            Cuestionario.SaveRespuesta();
        }
    });
    //#endregion

    //#region Editor

    $("#modalLeccion").on('shown.bs.modal', function () {
        var controlMensComp = CKEDITOR.replace('txtMensajeCompleto', { height: 500 });

        controlMensComp.on('change', function (evt) {
            contenidoHtml = evt.editor.getData();
        });

        controlMensComp.on('instanceReady', function (ev) {
            editor = ev.editor;
            editor.setReadOnly(false);
        });
    });

    //#endregion
});